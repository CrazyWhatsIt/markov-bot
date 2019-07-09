using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MarkovBot;

namespace Tests
{
    [TestFixture]
    class TestProgram
    {
        private static readonly string _TestFilePath = @"D:\MarkovBot\Tests\TestCases\";

        [Test]
        public void TestZeroArgs()
        {
            int ExitCode = MarkovBot.Program.Main(new string[0]);
            Assert.AreEqual(1, ExitCode);
        }

        [Test]
        public void TestBadInputs()
        {
            int ExitCode = MarkovBot.Program.Main(new string[] { "-1", "1", _TestFilePath + "MultipleParagraph.txt" });
            Assert.AreEqual(1, ExitCode);
            ExitCode = MarkovBot.Program.Main(new string[] { "1", "Not a number", _TestFilePath + "MultipleParagraph.txt" });
            Assert.AreEqual(1, ExitCode);
            ExitCode = MarkovBot.Program.Main(new string[] { "1", "1", "Not a valid path." });
            Assert.AreEqual(1, ExitCode);
            ExitCode = MarkovBot.Program.Main(new string[] { "1", "1", _TestFilePath + "EmptyFile.txt" });
            Assert.AreEqual(1, ExitCode);
            ExitCode = MarkovBot.Program.Main(new string[] { "1", "1", _TestFilePath + "SingleToken.txt" });
        }

        [Test]
        public void TestOneTokenOneChain()
        {
            int ExitCode = MarkovBot.Program.Main(new string[] { "1", "1", _TestFilePath + "MultipleParagraph.txt" });
            Assert.AreEqual(0, ExitCode);
        }

        [Test]
        public void TestMultipleTokensOneChain()
        {
            int ExitCode = MarkovBot.Program.Main(new string[] { "10", "1", _TestFilePath + "MultipleParagraph.txt" });
            Assert.AreEqual(0, ExitCode);
        }

        [Test]
        public void TestMultipleTokensMultipleChains()
        {
            int ExitCode = MarkovBot.Program.Main(new string[] { "10", "10", _TestFilePath + "MultipleParagraph.txt" });
            Assert.AreEqual(0, ExitCode);
        }
    }
}
