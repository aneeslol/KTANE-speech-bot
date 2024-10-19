using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest
{
    public class Utilities
    {
        public static int GetIntFromSpeech(string speech)
        {
            if (speech == "zero")
                return 0;
            else if (speech == "one")
                return 1;
            else if (speech == "two")
                return 2;
            else if (speech == "three")
                return 3;
            else if (speech == "four")
                return 4;
            else if (speech == "five")
                return 5;
            else if (speech == "six")
                return 6;
            else
                return -1;
        }
    }
}
