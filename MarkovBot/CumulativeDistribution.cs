﻿using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkovBot
{
    public class CumulativeDistribution
    {
        private Dictionary<string, int> _LabelToFrequency = new Dictionary<string, int>();

        /// This could be inlined as 'public int Total { get; private set; } = 0'
        /// But I want class data members to have an underscore out front, and I
        /// want to avoid using underscores in the interface.
        private int _Total = 0;
        public int Total
        {
            get
            {
                return _Total;
            }
        }

        public void AddLabelInstance(string Label)
        {
            try
            {
                Condition.Requires(Label).IsNotNullOrEmpty();
                _Total++;
                if (_LabelToFrequency.ContainsKey(Label))
                {
                    _LabelToFrequency[Label]++;
                }
                else
                {
                    _LabelToFrequency.Add(Label, 1);
                }
                Condition.Requires(_LabelToFrequency.ContainsKey(Label)).IsTrue();
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
                Condition.Requires(_LabelToFrequency).IsNotEmpty();
                List<Occurrence> OccurrenceList = new List<Occurrence>();
                int CumFreq = 0;
                foreach (string key in _LabelToFrequency.Keys)
                {
                    CumFreq += _LabelToFrequency[key];
                    OccurrenceList.Add(new Occurrence()
                    {
                        Label = key,
                        Frequency = _LabelToFrequency[key],
                        CumulativeFrequency = CumFreq
                    });
                }
                Condition.Requires(CumFreq).IsEqualTo(Total);
                Condition.Requires(OccurrenceList).IsNotEmpty();
                Condition.Requires(OccurrenceList.Count).IsEqualTo(_LabelToFrequency.Count);
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
