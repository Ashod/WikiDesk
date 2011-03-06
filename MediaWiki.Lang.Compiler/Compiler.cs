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
                Console.Error.WriteLine("Usage: RootPath RootNamespaceName [OutputFolder]");
                return 1;
            }

            string rootPath = args[0];
            string rootNamespace = args[1].Replace(".", ":::");
            string outputFolder = args.Length > 2 ? args[2] : string.Empty;

            int rootPathLength = rootPath.Length + 1;
            string tempPath = Path.Combine(Path.GetTempPath(), "WikiDeskPhP");
            try
            {
                Directory.Delete(tempPath);
            }
            catch
            {
            }

            string[] filenames = Directory.GetFiles(rootPath, "*.php", SearchOption.AllDirectories);
            //List<string> units = new List<string>(filenames.Length);

            foreach (string file in filenames)
            {
                string relFilename = file.Substring(rootPathLength);
                string directories = Path.GetDirectoryName(relFilename);
                string relNamespace = directories.Replace("\\", ":::");

                string filename = Path.Combine(tempPath, relFilename);
                Directory.CreateDirectory(Path.GetDirectoryName(filename));

                Console.Write("Preprocessing " + file);
                string namespaceName = rootNamespace; //+
                    //(!string.IsNullOrEmpty(relNamespace) ? ":::" + relNamespace : string.Empty);
                PreprocessFile(file, filename, namespaceName);

                Console.WriteLine(", Compiling...");
                List<string> units = new List<string>(1);
                units.Add(filename);
                string assemblyName = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(filename));
                int ret = Compile(units, assemblyName, true) ? 0 : 1;
                Console.WriteLine(" Done.");
            }

//             Console.WriteLine("Compiling...");
//             int ret = Compile(units, assemblyName, true) ? 0 : 1;
//             Console.WriteLine(" Done.");

            return 0;
        }

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
            TextWriter errors = Console.Error;
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
//                 // initializes log:
//                 Debug.ConsoleInitialize(Path.GetDirectoryName(p.Parameters.OutPath));

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

            output.WriteLine();
            output.WriteLine("Build complete -- {0} error{1}, {2} warning{3}.",
                errorSink.ErrorCount + errorSink.FatalErrorCount, (errorSink.ErrorCount + errorSink.FatalErrorCount == 1) ? string.Empty : "s",
                errorSink.WarningCount, (errorSink.WarningCount == 1) ? string.Empty : "s");

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
    }
}
