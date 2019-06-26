using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkovBot
{
    public class ProbabilityDistribution
    {
        List<Occurrence> ProbabilityOccurrenceList = new List<Occurrence>();

        public ProbabilityDistribution(CumulativeDistribution cd)
        {
            try
            {
                Condition.Requires(cd).IsNotNull();
                Condition.Requires(cd.Total).IsGreaterThan(0);
                List<Occurrence> CumulativeDistributionList = cd.GetCumulativeDistribution();
                Condition.Requires(CumulativeDistributionList).IsNotEmpty();
                foreach (Occurrence CumulativeOccurrence in CumulativeDistributionList)
                {
                    Occurrence ProbabilityOccurrence = new Occurrence()
                    {
                        Label = CumulativeOccurrence.Label,
                        Frequency = CumulativeOccurrence.Frequency,
                        CumulativeFrequency = CumulativeOccurrence.CumulativeFrequency,
                        Probability = (double)CumulativeOccurrence.Frequency / (double)cd.Total,
                        CumulativeProbability = (double)CumulativeOccurrence.CumulativeFrequency / (double)cd.Total
                    };
                    Condition.Requires(ProbabilityOccurrence.Probability).IsGreaterThan(0);
                    Condition.Requires(ProbabilityOccurrence.Probability).IsLessOrEqual(1);
                    Condition.Requires(ProbabilityOccurrence.CumulativeProbability).IsGreaterThan(0);
                    Condition.Requires(ProbabilityOccurrence.CumulativeProbability).IsLessOrEqual(1);
                    ProbabilityOccurrenceList.Add(ProbabilityOccurrence);
                }
                Condition.Requires(ProbabilityOccurrenceList).IsNotEmpty();
                Condition.Requires(ProbabilityOccurrenceList.Count).IsEqualTo(CumulativeDistributionList.Count);
            }
            catch(Exception Ex)
            {
                throw new ProbabilityDistributionException("Failed to generate a probability distribution.", Ex);
            }
        }

        public string GetNextToken(double RandomNumber)
        {
            try
            {
                Condition.Requires(ProbabilityOccurrenceList).IsNotEmpty();
                Condition.Requires(RandomNumber).IsGreaterOrEqual(0);
                Condition.Requires(RandomNumber).IsLessOrEqual(1);
                string Token = null;
                foreach (Occurrence ProbabilityOccurrence in ProbabilityOccurrenceList)
                {
                    if (RandomNumber <= ProbabilityOccurrence.CumulativeProbability)
                    {
                        Token = ProbabilityOccurrence.Label;
                        break;
                    }
                }
                Condition.Requires(Token).IsNotNull();
                return Token;
            }
            catch(Exception Ex)
            {
                throw new ProbabilityDistributionException("Failed to get a next token.", Ex);
            }
        }
    }

    public class ProbabilityDistributionException : Exception
    {
        public ProbabilityDistributionException(string message, Exception inner) : base(message, inner) { }
    }
}
