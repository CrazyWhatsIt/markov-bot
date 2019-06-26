using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CuttingEdge.Conditions;

namespace MarkovBot
{
    public class MarkovBot
    {
        private Random RandNumGenerator = new Random();
        private Dictionary<string, ProbabilityDistribution> WordToProbabilityOfNextWord = new Dictionary<string, ProbabilityDistribution>();
        private string[] UniqueTokens = null;

        public MarkovBot(string PathToInput)
        {
            try
            {
                ProcessInputFile(PathToInput);
            }
            catch (CumulativeDistributionException)
            {
                throw;
            }
            catch (ProbabilityDistributionException)
            {
                throw;
            }
            catch(Exception Ex)
            {
                throw new MarkovBotException("Encountered an unhandled exception while creating a markov bot!", Ex);
            }
        }

        public string GenerateChain(int length)
        {
            try
            {
                Condition.Requires(RandNumGenerator).IsNotNull();
                Condition.Requires(WordToProbabilityOfNextWord).IsNotEmpty();
                Condition.Requires(UniqueTokens).IsNotNull();
                string MarkovChain;
                string CurrentToken = GetFirstToken();
                MarkovChain = CurrentToken;
                for (int TokenNumber = 2; TokenNumber <= length; TokenNumber++)
                {
                    CurrentToken = GetNextToken(CurrentToken);
                    MarkovChain += $" {CurrentToken}";
                }
                return MarkovChain;
            }
            catch (CumulativeDistributionException)
            {
                throw;
            }
            catch (ProbabilityDistributionException)
            {
                throw;
            }
            catch (Exception Ex)
            {
                throw new MarkovBotException("Could not generate a new markov chain.", Ex);
            }
        }

        private void ProcessInputFile(string path)
        {
            try
            {
                string[] FileContent = File.ReadAllLines(path);
                Condition.Requires(FileContent.Length).IsGreaterOrEqual(1);
                List<string> TokenList = TokenizeInputFile(FileContent);
                UniqueTokens = GenerateUniqueTokens(TokenList);
                List<Tuple<string, string>> WordPairs = GetAllWordPairs(TokenList);
                Dictionary<string, List<string>> WordPairBins = BinWordPairsByFirstWord(WordPairs);
                BuildProbabilityDistributions(WordPairBins);
            }
            catch (CumulativeDistributionException)
            {
                throw;
            }
            catch (ProbabilityDistributionException)
            {
                throw;
            }
            catch (Exception Ex)
            {
                throw new MarkovBotException("Failed to process the training file.", Ex);
            }
        }

        private List<string> TokenizeInputFile(string[] InputLines)
        {
            List<string> TokenList = new List<string>();
            foreach(string line in InputLines)
            {
                if(line != "")
                {
                    string[] TokenizedLine = line.Split(new char[] { ' ', '\t' });
                    foreach (string Token in TokenizedLine) if(Token != "") TokenList.Add(Token);
                }
            }
            Condition.Requires(TokenList).IsNotEmpty();
            return TokenList;
        }

        private string[] GenerateUniqueTokens(List<string> TokenList)
        {
            Condition.Requires(TokenList).IsNotNull();
            Condition.Requires(TokenList).IsNotEmpty();
            HashSet<string> UniqueTokens = new HashSet<string>();
            foreach(string Token in TokenList)
            {
                Condition.Requires(Token).IsNotNullOrEmpty();
                if (!UniqueTokens.Contains(Token))
                {
                    UniqueTokens.Add(Token);
                }
            }
            Condition.Requires(UniqueTokens.Count).IsGreaterOrEqual(1);
            string[] UniqueTokenArray = new string[UniqueTokens.Count];
            int TokenArrayIndex = 0;
            IEnumerator<string> TokenEnumerator = UniqueTokens.GetEnumerator();
            while (TokenEnumerator.MoveNext())
            {
                UniqueTokenArray[TokenArrayIndex] = TokenEnumerator.Current;
                TokenArrayIndex++;
            }
            Condition.Requires(TokenArrayIndex).IsEqualTo(UniqueTokens.Count);
            return UniqueTokenArray;
        }

        private List<Tuple<string, string>> GetAllWordPairs(List<string> TokenList)
        {
            List<Tuple<string, string>> WordPairs = new List<Tuple<string, string>>();
            string Previous = null;
            foreach(string Current in TokenList)
            {
                Condition.Requires(Current).IsNotNullOrEmpty();
                if (Current == null) throw new Exception("Tokens should never be null.");
                if(Previous != null)
                {
                    WordPairs.Add(new Tuple<string, string>(Previous, Current));
                }
                Previous = Current;
            }
            Condition.Requires(WordPairs).IsNotEmpty();
            return WordPairs;
        }

        private Dictionary<string, List<string>> BinWordPairsByFirstWord(List<Tuple<string, string>> WordPairs)
        {
            Dictionary<string, List<string>> WordPairBins = new Dictionary<string, List<string>>();
            Condition.Requires(WordPairs).IsNotEmpty();
            foreach(Tuple<string, string> Pair in WordPairs)
            {
                if (WordPairBins.ContainsKey(Pair.Item1))
                {
                    WordPairBins[Pair.Item1].Add(Pair.Item2);
                }
                else
                {
                    WordPairBins.Add(Pair.Item1, new List<string>() { Pair.Item2 });
                }
            }
            Condition.Requires(WordPairBins).IsNotEmpty();
            return WordPairBins;
        }

        private void BuildProbabilityDistributions(Dictionary<string, List<string>> Bins)
        {
            Condition.Requires(Bins).IsNotEmpty();
            foreach(string key in Bins.Keys)
            {
                CumulativeDistribution DistGivenThisToken = new CumulativeDistribution();
                List<string> ListOfSecondWords = Bins[key];
                foreach(string NextWord in ListOfSecondWords)
                {
                    DistGivenThisToken.AddLabelInstance(NextWord);
                }
                ProbabilityDistribution ProbsForThisToken = new ProbabilityDistribution(DistGivenThisToken);
                WordToProbabilityOfNextWord.Add(key, ProbsForThisToken);
            }
            Condition.Requires(WordToProbabilityOfNextWord).IsNotEmpty();
        }

        private string GetFirstToken()
        {
            Condition.Requires(UniqueTokens).IsNotNull();
            int RandomIndex = RandNumGenerator.Next(1, UniqueTokens.Length) - 1;
            Condition.Requires(RandomIndex).IsGreaterOrEqual(0);
            Condition.Requires(RandomIndex).IsLessThan(UniqueTokens.Length);
            return UniqueTokens[RandomIndex];
        }

        private string GetNextToken(string PreviousToken)
        {
            double Probability = RandNumGenerator.NextDouble();
            Condition.Requires(Probability).IsGreaterOrEqual(0);
            Condition.Requires(Probability).IsLessThan(1);
            string Next;
            if(WordToProbabilityOfNextWord.ContainsKey(PreviousToken))
            {
                Next = WordToProbabilityOfNextWord[PreviousToken].GetNextToken(Probability);
            }
            else
            {
                /// Imagine that the last word in an input file is the only instance of that word.
                /// No word follows it, so its not part of the dictionary. This handles that edge case.
                Next = GetFirstToken();
            }

            Condition.Requires(Next).IsNotNullOrEmpty();
            return Next;
        }
    }

    public class MarkovBotException : Exception
    {
        internal MarkovBotException(string message, Exception inner) : base(message, inner) { }
    }
}
