using CuttingEdge.Conditions;
using System;
using System.IO;

namespace MarkovBot
{
    public class Program
    {
        public static int Main(string[] args)
        {
            int ExitCode;
            string message;
            try
            {
                TestArguments(args, out int CountChains, out int CountTokens, out string FileContent);
                message = RunMarkovBot(CountChains, CountTokens, FileContent);
                ExitCode = 0;
            }
            catch(InputException Ex)
            {
                message = $"[InputException] {Ex.Message}";
                ExitCode = 1;
            }
            catch (MarkovBotException Ex)
            {
                message = $"[MarkovBotException] {Ex.Message}";
                ExitCode = 2;
            }
            catch (ProbabilityDistributionException Ex)
            {
                message = $"[ProbabilityDistributionException] {Ex.Message}";
                ExitCode = 2;
            }
            catch (CumulativeDistributionException Ex)
            {
                message = $"[CumulativeDistributionException] {Ex.Message}";
                ExitCode = 2;
            }
            catch(Exception Ex)
            {
                message = $"Unhandled Exception! {Ex.Message}";
                ExitCode = 3;
            }
            if(ExitCode == 0)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Error.WriteLine(message);
            }
            return ExitCode;
        }

        private static void TestArguments(string[] args, out int NumChains, out int NumTokens, out string FileContent)
        {
            try
            {
                if (args.Length != 3)
                {
                    throw new InputException(GetHelpString());
                }
                if (!Int32.TryParse(args[0], out NumChains))
                {
                    throw new InputException($"Failed to parse number of chains [{NumChains}] as an integer.");
                }
                Condition.Requires(NumChains, "Number of chains").IsGreaterOrEqual(1);
                if (!Int32.TryParse(args[1], out NumTokens))
                {
                    throw new InputException($"Failed to parse number of tokens [{NumTokens}] as an integer.");
                }
                Condition.Requires(NumTokens, "Number of tokens").IsGreaterOrEqual(1);
                string PathToInputFile = args[2];
                Condition.Requires(PathToInputFile, "Path to input file").IsNotNullOrEmpty();
                if (!File.Exists(PathToInputFile))
                {
                    throw new InputException($"Training file does not exist.");
                }
                FileContent = File.ReadAllText(PathToInputFile);
                Condition.Requires(FileContent).IsNotNullOrEmpty();
                Condition.Requires(FileContent.Split(new char[] { ' ', '\t' }).Length).IsGreaterThan(1);
            }
            catch (InputException)
            {
                throw;
            }
            catch (Exception Ex)
            {
                throw new InputException(Ex.Message);
            }
        }

        private static string RunMarkovBot(int CountChains, int CountTokens, string FileContent)
        {
            string OutputString = "";
            MarkovBot Bot = new MarkovBot(FileContent);
            for(int ChainNumber = 1; ChainNumber <= CountChains; ChainNumber++)
            {
                OutputString += $"[{ChainNumber}] {Bot.GenerateChain(CountTokens)}\n\n";
            }
            return OutputString;
        }

        public static string GetHelpString()
        {
            return "Wrong number of arguments!\n\n" +
                "You should have three arguments:\n" +
                "(1) The number of chains to generate,\n" +
                "(2) The number of tokens in each chain,\n" +
                "(3) The path to the training file.\n" +
                "MarkovBot <NUM CHAINS> <NUM TOKENS> <INPUT FILE>";
        }

        internal class InputException : Exception
        {
            internal InputException(string message) : base(message) { }
        }
    }
}
