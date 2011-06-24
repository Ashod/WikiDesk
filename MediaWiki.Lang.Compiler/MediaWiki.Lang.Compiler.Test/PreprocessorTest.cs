// -----------------------------------------------------------------------------------------
// <copyright file="PreprocessorTest.cs" company="ashodnakashian.com">
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
//   Defines the PreprocessorTest type.
// </summary>
// -----------------------------------------------------------------------------------------

namespace MediaWiki.Lang.Compiler.Test
{
    using System;
    using System.IO;
    using System.Reflection;

    using NUnit.Framework;

    using PHP.Core;

    [TestFixture]
    public class PreprocessorTest
    {
        [Test]
        public void PreprocessGlobalArray()
        {
            string code = File.ReadAllText("TestFiles/GlobalArray.php");

            string output = Compiler.PreprocessText(code, "Test:::Space", "TestClass");

            string expected = File.ReadAllText("TestFiles/GlobalArray.php.processed");
            Assert.AreEqual(expected, output);
        }

        [Test]
        public void PreprocessClass()
        {
            string code = File.ReadAllText("TestFiles/Class.php");

            string output = Compiler.PreprocessText(code, "Test:::Space", "TestClass");
        }

        [Test]
        public void PreprocessClass2()
        {
            const string CODE = @"<?php
/**
 * Base class for language conversion.
 * @ingroup Language
 *
 * @author Zhengzhu Feng <zhengzhu@gmail.com>
 * @maintainers fdcn <fdcn64@gmail.com>, shinjiman <shinjiman@gmail.com>, PhiLiP <philip.npc@gmail.com>
 */
class LanguageConverter {
	var $mMainLanguageCode;
	var $mVariants, $mVariantFallbacks, $mVariantNames;
}
";

            string output = Compiler.PreprocessText(CODE, "Test:::Space", "TestClass");
        }

        [Test]
        public void PreprocessMessages()
        {
            string code = File.ReadAllText("TestFiles/Messages.php");

            string output = Compiler.PreprocessText(code, "Test:::Space", "TestClass");

            string expected = File.ReadAllText("TestFiles/Messages.php.processed");
            Assert.AreEqual(expected, output);
        }

        [Test]
        public void Compile()
        {
            string sourceFilename = Path.GetTempFileName();
            string assemblyName = Path.GetTempFileName();

            try
            {
                Compiler.PreprocessFile("TestFiles/GlobalArray.php", sourceFilename, "Test:::Space");
                System.Collections.Generic.List<string> units = new System.Collections.Generic.List<string>();
                units.Add(sourceFilename);

                Assert.True(Compiler.Compile(units, assemblyName, true));

                // Introspect.
                Assembly assembly = Assembly.LoadFrom(assemblyName);
                Assert.NotNull(assembly);

                Type globalArrayType = assembly.GetType("Test.Space.globalarray");
                Assert.NotNull(globalArrayType);

                FieldInfo wgLanguageNamesField = globalArrayType.GetField("wgLanguageNames");
                Assert.NotNull(wgLanguageNamesField);

                // Instantiate.
                object instance = Activator.CreateInstance(globalArrayType, ScriptContext.CurrentContext, true);
                Assert.NotNull(instance);

                // Read a predetermined field.
                object value = wgLanguageNamesField.GetValue(instance);
                Assert.NotNull(value);

                PhpArray array = (PhpArray)((PhpReference)value).value;
                Assert.NotNull(array);

                Assert.AreEqual(16, array.Count);
            }
            finally
            {
                // Delete the temporary source. We can't delete the loaded dll.
                File.Delete(sourceFilename);
            }
        }
    }
}
