using MarkovBot;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestFixture]
    class TestCumulativeDistribution
    {
        [Test]
        public void TestAddValidLabels()
        {
            CumulativeDistribution cd = new CumulativeDistribution();
            List<string> ListOfVaildLabels = new List<string>()
            {
                "Label A",
                "Label A",
                "Label A",
                "Label B",
                "Label C",
                "Label A",
                "Label B",
                "Label B",
                "Label B"
            };
            foreach(string label in ListOfVaildLabels)
            {
                cd.AddLabelInstance(label);
            }
        }

        [Test]
        public void TestAddASetOfLabelsHasCorrectTotal()
        {
            CumulativeDistribution cd = new CumulativeDistribution();
            List<string> ListOfVaildLabels = new List<string>()
            {
                "Label A",
                "Label A",
                "Label A",
                "Label B",
                "Label C",
                "Label A",
                "Label B",
                "Label B",
                "Label B"
            };
            foreach (string label in ListOfVaildLabels)
            {
                cd.AddLabelInstance(label);
            }
            Assert.AreEqual(ListOfVaildLabels.Count, cd.Total);
        }

        [Test]
        public void TestGetCumulativeDistributionReturnsCorrectList()
        {
            CumulativeDistribution cd = new CumulativeDistribution();
            List<string> ListOfVaildLabels = new List<string>()
            {
                "Label A",
                "Label A",
                "Label A",
                "Label B",
                "Label C",
                "Label A",
                "Label B",
                "Label B",
                "Label B"
            };
            foreach (string label in ListOfVaildLabels)
            {
                cd.AddLabelInstance(label);
            }
            List<Occurrence> CumulativeDistribution = cd.GetCumulativeDistribution();
            Assert.AreEqual(3, CumulativeDistribution.Count);
            foreach(Occurrence ThisOccurrence in CumulativeDistribution)
            {
                Assert.IsNotNull(ThisOccurrence.Label);
                Assert.IsNotNull(ThisOccurrence.Frequency);
                Assert.IsNotNull(ThisOccurrence.CumulativeFrequency);
                switch (ThisOccurrence.Label)
                {
                    case "Label A":
                        Assert.AreEqual(4, ThisOccurrence.Frequency);
                        break;
                    case "Label B":
                        Assert.AreEqual(4, ThisOccurrence.Frequency);
                        break;
                    case "Label C":
                        Assert.AreEqual(1, ThisOccurrence.Frequency);
                        break;
                    default:
                        throw new Exception("Unrecognzied label in occurrence list.");
                }
            }
        }
    }
}
