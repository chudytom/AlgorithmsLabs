using System;
using System.Collections.Generic;
using System.Text;

namespace ASD
{
    public class LZ77 : MarshalByRefObject
    {
        /// <summary>
        /// Odkodowywanie napisu zakodowanego algorytmem LZ77. Dane kodowanie jest poprawne (nie trzeba tego sprawdzać).
        /// </summary>
        public string Decode(List<EncodingTriple> encoding)
        {
            int N = 0;
            foreach (var triple in encoding)
            {
                N += triple.c + 1;
            }
            var text = new StringBuilder(N);
            foreach (var triple in encoding)
            {
                int characterCount = Math.Min(triple.p + 1, triple.c);
                int textBeginning = Math.Max(text.Length - 1 - triple.p, 0);
                int tempC = triple.c;
                while (tempC > 0)
                {
                    for (int i = 0; i < Math.Min(tempC, characterCount); i++)
                    {
                        text.Append(text[textBeginning + i]);
                    }
                    tempC -= Math.Min(tempC, characterCount);
                }
                text.Append(triple.s);
            }
            return text.ToString();
        }
        //Zmienić, żeby się nie alokował za każdym razem

        /// <summary>
        /// Kodowanie napisu s algorytmem LZ77
        /// </summary>
        /// <returns></returns>
        public List<EncodingTriple> Encode(string s, int maxP)
        {
            var code = new List<EncodingTriple>();
            var text = new StringBuilder(s.Length);
            int iterator = 0;
            while (s.Length >= text.Length)
            {
                var w = text.ToString().Substring(Math.Max(0, text.Length - 1 - (maxP + 1)),
                    Math.Min(text.Length, maxP + 1));

                var r = s.Substring(text.Length, s.Length - text.Length);

                int c = 0;
                int j = 0;
                code.Add(new EncodingTriple(w.Length - j, c, r[c + 1]));

                iterator++;
            }



            return code;
        }
    }

    [Serializable]
    public struct EncodingTriple
    {
        public int p, c;
        public char s;

        public EncodingTriple(int p, int c, char s)
        {
            this.p = p;
            this.c = c;
            this.s = s;
        }
    }
}
