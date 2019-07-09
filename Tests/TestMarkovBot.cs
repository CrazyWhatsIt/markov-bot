using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MarkovBot;
using System.IO;

namespace Tests
{
    [TestFixture]
    class TestMarkovBot
    {
        private static readonly string _TestFilePath = @"D:\MarkovBot\Tests\TestCases\";

        [Test]
        public void TestEmptyFile()
        {
            try
            {
                string path = _TestFilePath + "EmptyFile.txt";
                string content = File.ReadAllText(path);
                MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
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
                string path = _TestFilePath + "SingleToken.txt";
                string content = File.ReadAllText(path);
                MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            }
            catch (Exception Ex)
            {
                Assert.IsTrue(Ex is MarkovBotException);
            }
        }

        [Test]
        public void TestLastLineEmpty()
        {
            string path = _TestFilePath + "LastLineEmpty.txt";
            string content = File.ReadAllText(path);
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            string chain = mb.GenerateChain(2);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestOneParagraph()
        {
            string path = _TestFilePath + "OneParagraph.txt";
            string content = File.ReadAllText(path);
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestTwoParagraphs()
        {
            string path = _TestFilePath + "MultipleParagraph.txt";
            string content = File.ReadAllText(path);
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestMultipleEmptyLinesBetweenParagraphs()
        {
            string path = _TestFilePath + "MultipleLinesBetweenParagraphs.txt";
            string content = File.ReadAllText(path);
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestLargeInputFile()
        {
            string path = _TestFilePath + "AliceInWonderland.txt";
            string content = File.ReadAllText(path);
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            string chain = mb.GenerateChain(10);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestLargeInputFileAndLargeChain()
        {
            string path = _TestFilePath + "AliceInWonderland.txt";
            string content = File.ReadAllText(path);
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            string chain = mb.GenerateChain(10000);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }

        [Test]
        public void TestInputFileSmallerThanChain()
        {
            string path = _TestFilePath + "AliceInWonderland.txt";
            string content = File.ReadAllText(path);
            MarkovBot.MarkovBot mb = new MarkovBot.MarkovBot(content);
            string chain = mb.GenerateChain(30);
            Assert.IsNotNull(chain);
            Assert.IsNotEmpty(chain);
        }
    }
}
