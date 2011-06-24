// -----------------------------------------------------------------------------------------
// <copyright file="Compiler.cs" company="ashodnakashian.com">
//
// This file is part of WikiDesk.
// Copyright (C) 2010, 2011 Ashod Nakashian
// https://github.com/Ashod/WikiDesk
//
//  WikiDesk is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  WikiDesk is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with WikiDesk. If not, see http://www.gnu.org/licenses/.
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Compiles Language classes and Messages.
// </summary>
// -----------------------------------------------------------------------------------------

namespace MediaWiki.Lang.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    using PHP.Core;

    /// <summary>
    /// Compiles Language classes and Messages.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                // Error.
                Console.Error.WriteLine("Usage: RootPath RootNamespaceName [OutputFolder] <-Debug> <-Rebuild>");
                return 1;
            }

            string rootPath = args[0];
            string rootNamespace = args[1].Replace(".", ":::");
            string outputFolder = args.Length > 2 ? args[2] : string.Empty;

            bool debug = false;
            bool rebuild = false;
            for (int i = 2; i < args.Length; ++i)
            {
                if (string.Compare(args[i], "-Debug", true) == 0)
                {
                    debug = true;
                }
                else
                if (string.Compare(args[i], "-Rebuild", true) == 0)
                {
                    rebuild = true;
                }
            }

            if (rebuild)
            {
                Console.WriteLine("Deleting " + outputFolder);
                DeleteDirectory(outputFolder);
            }

            int rootPathLength = rootPath.Length + 1;
            string tempPath = Path.Combine(Path.GetTempPath(), "WikiDeskPhP");
            DeleteDirectory(tempPath);

            if (Compile(outputFolder, rootPath, tempPath, rootPathLength, rootNamespace, debug))
            {
                DeleteDirectory(tempPath);
            }

            return 0;
        }

        #region operations

        public static bool Compile(
                            IList<string> units,
                            string assemblyName,
                            bool debug)
        {
            List<string> commands = new List<string>(units.Count * 2);
            commands.Add("/encoding:UTF-8");
            commands.Add("/pure");
            commands.Add("/target:dll");
            commands.Add("/out:" + assemblyName);
            if (debug)
            {
                commands.Add("/debug");
            }

            foreach (string filename in units)
            {
                commands.Add(filename);
            }

            CommandLineParser commandLineParser = new CommandLineParser();
            commandLineParser.Parse(commands);

            TextWriter output = Console.Out;
            TextErrorSink errorSink = new TextErrorSink(Console.Error);

            ApplicationContext.DefineDefaultContext(false, true, false);
            ApplicationContext appContext = ApplicationContext.Default;

            CompilerConfiguration compilerConfig = ApplicationCompiler.LoadConfiguration(
                                                            appContext,
                                                            commandLineParser.Parameters.ConfigPaths,
                                                            output);

            commandLineParser.Parameters.ApplyToConfiguration(compilerConfig);

            try
            {
                new ApplicationCompiler().Compile(appContext, compilerConfig, errorSink, commandLineParser.Parameters);
            }
            catch (InvalidSourceException ex)
            {
                ex.Report(errorSink);
                return false;
            }
            catch (Exception ex)
            {
                errorSink.AddInternalError(ex);
                return false;
            }

            if (errorSink.ErrorCount + errorSink.FatalErrorCount + errorSink.WarningCount > 0)
            {
                output.WriteLine();
                output.WriteLine(
                    "Build complete -- {0} error{1}, {2} warning{3}.",
                    errorSink.ErrorCount + errorSink.FatalErrorCount,
                    (errorSink.ErrorCount + errorSink.FatalErrorCount == 1) ? string.Empty : "s",
                    errorSink.WarningCount,
                    (errorSink.WarningCount == 1) ? string.Empty : "s");
            }

            return !errorSink.AnyError;
        }

        /// <summary>
        /// Process a file into an output file.
        /// </summary>
        /// <param name="sourceFilename">The source file to process.</param>
        /// <param name="outputFilename">The file name where the processed code will be written.</param>
        /// <param name="namespaceName">The namespace to wrap the file in.</param>
        public static void PreprocessFile(string sourceFilename, string outputFilename, string namespaceName)
        {
            string code = File.ReadAllText(sourceFilename);

            // The class name is the filename in all lower-case.
            string className = Path.GetFileNameWithoutExtension(sourceFilename).ToLowerInvariant();
            string output = PreprocessText(code, namespaceName, className);

            File.WriteAllText(outputFilename, output);
        }

        /// <summary>
        /// Process a file in-place, overwriting the source.
        /// </summary>
        /// <param name="sourceFilename">The source file to process.</param>
        /// <param name="namespaceName">The namespace to wrap the file in.</param>
        public static void PreprocessFile(string sourceFilename, string namespaceName)
        {
            PreprocessFile(sourceFilename, sourceFilename, namespaceName);
        }

        /// <summary>
        /// Process a code string returning the result. Wraps globals in a new class.
        /// NOTE: If the file already contains any classes, no globals will be wrapped.
        /// </summary>
        /// <param name="code">The code string to process.</param>
        /// <param name="namespaceName">The namespace to wrap the file in.</param>
        /// <param name="className">The class name to wrap globals in.</param>
        /// <returns>The processed code string.</returns>
        public static string PreprocessText(string code, string namespaceName, string className)
        {
            List<string> lines = new List<string>(2048);

            code = code.Trim();

            if (code.EndsWith("?>"))
            {
                code.Substring(0, code.Length - 2);
            }

            // Escape Preg placeholders.
            int id;
            code = RegexReplace(
                    rexPregPlaceholder,
                    matchPreg => int.TryParse(matchPreg.Groups[3].Value, out id) ? "{" + (id - 1) + "}" :
                                    "\\$" + matchPreg.Groups[3].Value,
                    code);

            // If there are any classes in the file, skip wrapping.
            Match match = rexClass.Match(code);
            if (!match.Success)
            {
                code = RegexReplace(
                        rexVars,
                        matchVar => "public " + matchVar.Groups[1] + " = ",
                        code);

                String2Lines(code, lines);

                // Add a class engulfing all the code.
                string classDecl = string.Format("class {1}{0}{{", Environment.NewLine, className);
                lines.Insert(1, classDecl);
                lines.Insert(lines.Count, "}");
            }
            else
            {
                String2Lines(code, lines);
            }

            // Add namespace.
            string namespaceDecl = string.Format("namespace {0}{1}{{", namespaceName, Environment.NewLine);
            lines.Insert(1, namespaceDecl);
            lines.Insert(lines.Count, "}");

            lines.Add("?>");

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        #endregion // operations

        #region implementation

        private static bool Compile(string outputFolder, string rootPath, string tempPath, int rootPathLength, string rootNamespace, bool debug)
        {
            bool errors = false;

            foreach (string file in
                            Directory.GetFiles(rootPath, "*.php", SearchOption.AllDirectories))
            {
                string relFilename = file.Substring(rootPathLength);
                string filename = Path.Combine(tempPath, relFilename);
                string assemblyName = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(filename));

                // Check if a rebuild is necessary.
                FileInfo phpFileInfo = new FileInfo(file);
                FileInfo dllFileInfo = new FileInfo(assemblyName + ".dll");
                if (dllFileInfo.Exists && dllFileInfo.Length > 0 &&
                    dllFileInfo.LastAccessTimeUtc > phpFileInfo.LastWriteTimeUtc)
                {
                    continue;
                }

                Console.Write("Preprocessing " + file);
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                string namespaceName = rootNamespace;
                PreprocessFile(file, filename, namespaceName);

                Console.Write(". Compiling... ");
                List<string> units = new List<string>(1) { filename };
                if (Compile(units, assemblyName, debug))
                {
                    Console.WriteLine("Done.");
                }
                else
                {
                    errors = true;
                }
            }

            return !errors;
        }

        private static void String2Lines(string code, ICollection<string> lines)
        {
            lines.Clear();
            using (StringReader sr = new StringReader(code))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = sr.ReadLine();
                }
            }
        }

        private static string RegexReplace(Regex regex, RegexHandler handler, string input)
        {
            int lastIndex = 0;
            StringBuilder sb = new StringBuilder(input.Length * 2);

            Match match = regex.Match(input);
            while (match.Success && (lastIndex < input.Length))
            {
                // Copy the skipped part.
                sb.Append(input.Substring(lastIndex, match.Index - lastIndex));

                // Handle the match.
                string text = handler(match);

                // Either copy a replacement or the matched part as-is.
                sb.Append(text ?? match.Value);

                lastIndex = match.Index + match.Length;

                match = match.NextMatch();
            }

            // Copy the remaining bit.
            if (lastIndex == 0)
            {
                // There were no matches.
                Debug.Assert(sb.Length == 0, "Expected no matches.");
                return input;
            }

            sb.Append(input.Substring(lastIndex));
            return sb.ToString();
        }

        private static bool DeleteDirectory(string path)
        {
            try
            {
                Directory.Delete(path, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion // implementation

        #region representation

        /// <summary>
        /// Handles a matching regex.
        /// May return null to skip conversion of the matching block.
        /// </summary>
        /// <param name="match">The regex match instance.</param>
        /// <returns>A replacement string, or null to skip.</returns>
        private delegate string RegexHandler(Match match);

        private static readonly Regex rexClass = new Regex(@"class\s+\w+(\s+extends\s+\w+)?\s+\{", RegexOptions.Compiled);
        private static readonly Regex rexVars = new Regex(@"(\$\w+)(\s+)?=", RegexOptions.Compiled);
        private static readonly Regex rexPregPlaceholder = new Regex(@"(\\)?(\$(\d|\W))", RegexOptions.Compiled);

        #endregion // representation
    }
}
