using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая.Setting
{
    internal class CipherPassword
    {
        public string decode(string str) => encode(str);

        public string encode(string str)
        {
            string text = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 'a' && str[i] <= 'z')
                    text += Cipher(str[i], 'a', 'z');
                else if (str[i] >= 'A' && str[i] <= 'Z')
                    text += Cipher(str[i], 'A', 'Z');
                else if (str[i] >= '0' && str[i] <= '9')
                    text += Cipher(str[i], '0', '9');
            }
            return text;
        }

        private char Cipher(char letter, int firstLatter, int lastLatter) =>
            (char)(lastLatter - (letter - firstLatter));
    }
}
