using System;
using System.Collections.Generic;
using System.Text;

namespace MarkovBot
{
    public class Occurrence
    {
        public string Label { get; set; }
        public int Frequency { get; set; }
        public int CumulativeFrequency { get; set; }
        public double Probability { get; set; }
        public double CumulativeProbability { get; set; }
    }
}
