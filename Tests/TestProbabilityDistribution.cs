using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MarkovBot;

namespace Tests
{
    [TestFixture]
    class TestProbabilityDistribution
    {
        [Test]
        public void _TestGetNextTokenFromUniformDistribution()
        {
            List<string> LabelList = new List<string>()
            {
                "Label A",
                "Label B",
                "Label C",
                "Label D"
            };
            Random RandGenerator = new Random();
            CumulativeDistribution cd = new CumulativeDistribution();
            foreach(string label in LabelList)
            {
                cd.AddLabelInstance(label);
            }
            ProbabilityDistribution pd = new ProbabilityDistribution(cd);
            const int NumberOfSamples = 100000;
            const double ProportionAreA = 0.25;
            int SamplesOfA = 0;
            for(int Counter = 1; Counter <= NumberOfSamples; Counter++)
            {
                string label = pd.GetNextToken(RandGenerator.NextDouble());
                if (label == "Label A")
                {
                    SamplesOfA++;
                }
            }
            double ProportionOfSamplesAreA = (double)SamplesOfA / (double)NumberOfSamples;
            Assert.AreEqual(ProportionAreA, Math.Round(ProportionOfSamplesAreA, 2));
        }

        [Test]
        public void TestGetNextTokenFromNotUniformDistribution()
        {
            List<string> LabelList = new List<string>()
            {
                "Label A",
                "Label A",
                "Label B",
                "Label C",
                "Label D"
            };
            Random RandGenerator = new Random();
            CumulativeDistribution cd = new CumulativeDistribution();
            foreach (string label in LabelList)
            {
                cd.AddLabelInstance(label);
            }
            ProbabilityDistribution pd = new ProbabilityDistribution(cd);
            const int NumberOfSamples = 100000;
            const double ProportionAreA = 0.40;
            int SamplesOfA = 0;
            for (int Counter = 1; Counter <= NumberOfSamples; Counter++)
            {
                string label = pd.GetNextToken(RandGenerator.NextDouble());
                if (label == "Label A")
                {
                    SamplesOfA++;
                }
            }
            double ProportionOfSamplesAreA = (double)SamplesOfA / (double)NumberOfSamples;
            double RoundedProportion = Math.Round(ProportionOfSamplesAreA, 2);
            Assert.AreEqual(ProportionAreA, RoundedProportion);
        }
    }
}
