using System;
using System.Collections.Generic;
using System.IO;

namespace generator
{
    ////////////////////////////////////////////////////////////////////////////////////////////
    //// Examle class
    ///////////////////////////////////////////////////////////////////////////////////////////
    class CharGenerator 
    {
        private string syms = "абвгдеёжзийклмнопрстуфхцчшщьыъэюя"; 
        private char[] data;
        private int size;
        private Random random = new Random();
        public CharGenerator() 
        {
           size = syms.Length;
           data = syms.ToCharArray(); 
        }
        public char getSym() 
        {
           return data[random.Next(0, size)]; 
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    //// sequence of two adjacent symbols, most often found together
    ////////////////////////////////////////////////////////////////////////////////////////////
    class Bigram
    {
        private string alphabet = "абвгдежзийклмнопрстуфхцчшщьыэюя";
        private char[] data;
        private int size;
        private Random rand = new Random();
        private int[,] newProb;
        private int sum = 0;

        public Bigram(int[,] prob)
        {
            size = alphabet.Length;
            data = alphabet.ToCharArray();
            newProb = new int[size, size];

            for (int i = 0; i < prob.GetLength(0); i++)
                for (int j = 0; j < prob.GetLength(1); j++)
                    sum += prob[i, j];
            
            int tempSum = 0;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    tempSum += prob[i, j];
                    newProb[i, j] = tempSum;
                }
        }
        public string getSymbol()
        {
            int m = rand.Next(0, sum);
            int i, j = 0;

            for (i = 0; i < size; i++)
                for (j = 0; j < size; j++)
                    if (m < newProb[i, j])
                        return data[i].ToString() + data[j].ToString();

            return data[i].ToString() + data[j].ToString();
        }
        public string getOutput(int steps)
        {
            string output = "";
            for (int i = 0; i < steps; i++)
            {
                output += getSymbol();
                output += ' ';
            }

            output.Trim();
            return output;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    //// most common words
    ////////////////////////////////////////////////////////////////////////////////////////////
    class WordFreq{
        private string[] data;
        private int size;
        private Random rand = new Random();
        private int[] newProb;
        private int sum = 0;

        public WordFreq(string[] words)
        {
            data = words;
            size = words.Length;
            newProb = new int[size];
            for (int i = 0; i < size; i++)
            {
                sum += i;
                newProb[i] = sum;
            }
        }
        public string getWord()
        {
            int temp = rand.Next(0, sum);
            int i;

            for (i = 0; i < size; i++)
                if (temp < newProb[i])
                    break;

            return data[i];
        }
        public string getOutput(int steps)
        {
            string output = "";
            for (int i = 0; i < steps; i++)
            {
                output += getWord();
                output += ' ';
            }

            output.Trim();
            return output;
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////
    //// most common pair words
    ////////////////////////////////////////////////////////////////////////////////////////////
    class PairFreq
    {
        private string[] data;
        private int size;
        private Random rand = new Random();
        private int[] newProb;
        private int sum = 0;

        public PairFreq(string[] words)
        {
            data = words;
            size = words.Length;
            newProb = new int[size];
            for (int i = 0; i < size; i++)
            {
                sum += i;
                newProb[i] = sum;
            }
        }
        public string getWords()
        {
            int m = rand.Next(0, sum);
            int i;

            for (i = 0; i < size; i++)
                if (m < newProb[i])
                    break;

            return data[i];
        }
        public string getOutput(int steps)
        {
            string output = "";
            for (int i = 0; i < steps; i++)
            {
                output += getWords();
                output += ' ';
            }

            output.Trim();
            return output;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            CharGenerator gen = new CharGenerator();
            SortedDictionary<char, int> stat = new SortedDictionary<char, int>();
            for(int i = 0; i < 1000; i++) 
            {
               char ch = gen.getSym(); 
               if (stat.ContainsKey(ch))
                  stat[ch]++;
               else
                  stat.Add(ch, 1); Console.Write(ch);
            }
            Console.Write('\n');
            foreach (KeyValuePair<char, int> entry in stat) 
            {
                 Console.WriteLine("{0} - {1}",entry.Key,entry.Value/1000.0); 
            }

            ///////////////////////////////////////////////////////
            
            string[] prob_temp = File.ReadAllLines("task1_bigram_input.txt");
            int[,] probability = new int[prob_temp.Length, prob_temp[0].Split(' ').Length];
            for (int i = 0; i < prob_temp.Length; i++)
            {
                string[] temp = prob_temp[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    probability[i, j] = Convert.ToInt32(temp[j]);
            }
            string[] words = File.ReadAllLines("task2_wordFreq_input.txt");
            string[] twowords = File.ReadAllLines("task3_pairFreq_input.txt");

            Bigram bigram = new Bigram(probability);
            string bigramOutput = bigram.getOutput(1000);

            WordFreq wordFrequency = new WordFreq(words);
            string wordOutput = wordFrequency.getOutput(1000);

            PairFreq pairFreq = new PairFreq(twowords);
            string pairOutput = pairFreq.getOutput(1000);

            File.WriteAllText("task1_bigram_output.txt", bigramOutput);
            File.WriteAllText("task2_wordFreq_output.txt", wordOutput);
            File.WriteAllText("task3_pairFreq_output.txt", pairOutput);
            
        }
    }
}

