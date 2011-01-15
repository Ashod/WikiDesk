
namespace MediaWiki.Lang.Compiler.Test
{
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class PreprocessorTest
    {
        [Test]
        public void PreprocessGlobalArray()
        {
            string code = File.ReadAllText("TestFiles/GlobalArray.php");

            string output = Compiler.PreprocessText(code, "Test:::Space", "TestClass");
        }

        [Test]
        public void PreprocessClass()
        {
            string code = File.ReadAllText("TestFiles/Class.php");

            string output = Compiler.PreprocessText(code, "Test:::Space", "TestClass");
        }

        [Test]
        public void Compile()
        {
            string sourceFilename = Compiler.PreprocessFile("TestFiles/GlobalArray.php", "Test:::Space");

            System.Collections.Generic.List<string> units = new System.Collections.Generic.List<string>();
            units.Add(sourceFilename);

            Assert.True(Compiler.Compile(units, "Test.dll", true));
        }
    }
}
