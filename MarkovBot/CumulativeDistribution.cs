using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkovBot
{
    public class CumulativeDistribution
    {
        private Dictionary<string, int> LabelToFrequency = new Dictionary<string, int>();
        public int Total { get; private set; } = 0;

        public void AddLabelInstance(string Label)
        {
            try
            {
                Condition.Requires(Label).IsNotNullOrEmpty();
                Total++;
                if (LabelToFrequency.ContainsKey(Label))
                {
                    LabelToFrequency[Label]++;
                }
                else
                {
                    LabelToFrequency.Add(Label, 1);
                }
                Condition.Requires(LabelToFrequency.ContainsKey(Label)).IsTrue();
            }
            catch(Exception Ex)
            {
                throw new CumulativeDistributionException("Failed to add a new occurance.", Ex);
            }
        }

        public List<Occurrence> GetCumulativeDistribution()
        {
            try
            {
                Condition.Requires(LabelToFrequency).IsNotEmpty();
                List<Occurrence> OccurrenceList = new List<Occurrence>();
                int CumFreq = 0;
                foreach (string key in LabelToFrequency.Keys)
                {
                    CumFreq += LabelToFrequency[key];
                    Occurrence NextOccurrence = new Occurrence()
                    {
                        Label = key,
                        Frequency = LabelToFrequency[key],
                        CumulativeFrequency = CumFreq
                    };
                    OccurrenceList.Add(NextOccurrence);
                }
                Condition.Requires(CumFreq).IsEqualTo(Total);
                Condition.Requires(OccurrenceList).IsNotEmpty();
                Condition.Requires(OccurrenceList.Count).IsEqualTo(LabelToFrequency.Count);
                return OccurrenceList;
            }
            catch(Exception Ex)
            {
                throw new CumulativeDistributionException("Failed to generate a new cumulative distribution", Ex);
            }
        }
    }
    
    internal class CumulativeDistributionException : Exception
    {
        public CumulativeDistributionException(string message, Exception inner) : base(message, inner) { }
    }
}
