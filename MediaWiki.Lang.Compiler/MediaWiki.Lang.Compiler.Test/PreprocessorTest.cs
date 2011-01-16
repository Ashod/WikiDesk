
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
        public void PreprocessMessages()
        {
            string code = File.ReadAllText("TestFiles/Messages.php");

            string output = Compiler.PreprocessText(code, "Test:::Space", "TestClass");
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

                Type globalArrayType = assembly.GetType("Test.Space.GlobalArray");
                Assert.NotNull(globalArrayType);

                FieldInfo wgLanguageNamesField = globalArrayType.GetField("wgLanguageNames");
                Assert.NotNull(wgLanguageNamesField);

                // Instantiate.
                object instance = Activator.CreateInstance(globalArrayType, ScriptContext.CurrentContext, true);
                Assert.NotNull(instance);
            }
            finally
            {
                // Delete the temporary source. We can't delete the loaded dll.
                File.Delete(sourceFilename);
            }
        }
    }
}
