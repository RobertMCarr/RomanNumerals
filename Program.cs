using System;
using System.Collections.Generic;
using System.Linq;

namespace RomanNumberals
{
    public struct Settings
    {
        public bool OutputArabicAsWell;
        public string Seperater;
    }

    class Program
    {
        // defaults
        static string defaultSeperater = ",";

        static void Main(string[] args)
        {
            int number = 0;
            var settings = ParseArguments(args, ref number);

            var RomanNumberalValues = new List<KeyValuePair<char, int>>();
            InitialiseRomanNumbericalValues(ref RomanNumberalValues);
            
            if(args.Length < 1 || number < 1)
            {
                for(int i = 1; i < 3001; i++)
                {
                    CalculateAndPrintOutput(settings, RomanNumberalValues, i);
                }
            }
            else
            {
                CalculateAndPrintOutput(settings, RomanNumberalValues, number);
            }
        }

        private static Settings ParseArguments(string[] args, ref int number)
        {
            var settings = new Settings();
            settings.Seperater = defaultSeperater; //default;

            if (args.Length > 0)
            {
                for(int i = 0; i< args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--a":
                            settings.OutputArabicAsWell = true;
                        break;
                        case "--s":
                            if((args.Length -1) >= (i + 1))
                            {
                                settings.Seperater = args[i+1];
                            }
                            else
                            {
                                Console.WriteLine("Error: seperater missing;");
                            }
                        break;
                        case "--n": 
                            if((args.Length -1) >= (i + 1))
                            {
                                int.TryParse(args[i + 1], out number); 
                            }
                            else
                            {
                                Console.WriteLine("Error: number missing;");
                            }
                            break;
                        default:break;
                    }
                }
            }
            else
            {
                // apply defaults
                settings.OutputArabicAsWell = true;
                settings.Seperater = defaultSeperater;
            }

            return settings;
        }

        private static void InitialiseRomanNumbericalValues(ref List<KeyValuePair<char, int>> dictionary)
        {
            dictionary.Add(new KeyValuePair<char, int>('I', 1));
            dictionary.Add(new KeyValuePair<char, int>('V', 5));
            dictionary.Add(new KeyValuePair<char, int>('X', 10));
            dictionary.Add(new KeyValuePair<char, int>('L', 50));
            dictionary.Add(new KeyValuePair<char, int>('C', 100));
            dictionary.Add(new KeyValuePair<char, int>('D', 500));
            dictionary.Add(new KeyValuePair<char, int>('M', 1000));
        }

        private static void CalculateAndPrintOutput(Settings settings, List<KeyValuePair<char, int>> romanNumberals, int number)
        {
            var result = ArabicToRomanNumberals(settings, romanNumberals, number);
            var rightHandSide = (settings.OutputArabicAsWell)? settings.Seperater  + number : string.Empty;
            Console.WriteLine(result + rightHandSide);
        }

        private static string ArabicToRomanNumberals(Settings settings, IList<KeyValuePair<char, int>> romanNumberals, int arabic)
        {
            if(arabic == 0) return string.Empty;

            int runningValue = arabic;
            string romanNumeralStr = string.Empty;
            var orderedList = romanNumberals.OrderByDescending(o => o.Value).ToList();

            foreach (var item in orderedList)
            {
                int result = runningValue / item.Value;
                if(result > 0)
                {
                    romanNumeralStr += new string(item.Key, (int)result);
                    runningValue -= (int)result * item.Value;
                    romanNumeralStr += ArabicToRomanNumberals(settings, romanNumberals, runningValue);
                    break;
                }
                else 
                {
                    float multiplier = (item.Value.ToString()[0] == '1')? 0.9f : (item.Value.ToString()[0] == '5')? 0.8f : 0.0f; 
                    int deductable = (int)(item.Value * multiplier);

                    if(runningValue >= deductable)
                    {
                        string deductableAsString = romanNumberals.First(x=>x.Value == item.Value - deductable).Key.ToString();
                        if (!string.IsNullOrEmpty(deductableAsString))
                        {
                            romanNumeralStr += deductableAsString + item.Key;
                            runningValue -= deductable;
                            romanNumeralStr += ArabicToRomanNumberals(settings, romanNumberals, runningValue);
                        }
                    }

                    if(!string.IsNullOrEmpty(romanNumeralStr)) break;
                }
            }

            return romanNumeralStr;
        }
    }
}
