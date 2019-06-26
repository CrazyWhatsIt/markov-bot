using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MarkovBot;

namespace Tests
{
    [TestFixture]
    class TestMarkovBot
    {
        private static readonly string TestFilePath = @"D:\MarkovBot\Tests\TestCases\";

        [Test]
        public void TestEmptyFile()
        {
            try
            {
                string path = TestFilePath + "EmptyFile.txt";
                MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            }
            catch(Exception Ex)
            {
                Assert.IsTrue(Ex is MarkovBotException);
            }
        }

        [Test]
        public void TestSingleTokenFile()
        {
            try
            {
                string path = TestFilePath + "SingleToken.txt";
                MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            }
            catch (Exception Ex)
            {
                Assert.IsTrue(Ex is MarkovBotException);
            }
        }

        [Test]
        public void TestLastLineEmpty()
        {
            string path = TestFilePath + "LastLineEmpty.txt";
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            string chain = mb.GenerateChain(2);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestOneParagraph()
        {
            string path = TestFilePath + "OneParagraph.txt";
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestTwoParagraphs()
        {
            string path = TestFilePath + "MultipleParagraph.txt";
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestMultipleEmptyLinesBetweenParagraphs()
        {
            string path = TestFilePath + "MultipleLinesBetweenParagraphs.txt";
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestLargeInputFile()
        {
            string path = TestFilePath + "AliceInWonderland.txt";
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestLargeInputFileAndLargeChain()
        {
            string path = TestFilePath + "AliceInWonderland.txt";
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            string chain = mb.GenerateChain(10000);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestInputFileSmallerThanChain()
        {
            string path = TestFilePath + "AliceInWonderland.txt";
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(path);
            string chain = mb.GenerateChain(30);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }
    }
}
