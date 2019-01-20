using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkStatistics
{
    public static class FrequenciesCounter
    {
        static readonly string alphabet = "abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        public static Dictionary<char, double> GetFrequencies(string text)
        {
            Dictionary<char, int> letterCounts = new Dictionary<char, int>();
            Dictionary<char, double> letterFrequencies = new Dictionary<char, double>();

            foreach (var letter in alphabet)
            {
                letterCounts.Add(letter, 0);
                letterFrequencies.Add(letter, 0);
            }
            
            int lettersCount = 0;
            foreach (var letter in text)
            {
                if (letterCounts.ContainsKey(letter))
                {
                    letterCounts[letter]++;
                    lettersCount++;
                }
            }

            foreach (var letter in letterCounts.Keys)
            {
                if (lettersCount > 0)
                    letterFrequencies[letter] = Math.Round((double)letterCounts[letter] / lettersCount, 3);
            }
            return letterFrequencies;
        }
    }
}