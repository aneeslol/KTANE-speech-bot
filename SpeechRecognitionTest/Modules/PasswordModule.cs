using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class PasswordModule : BaseModule
    {
        public static List<string> Letters = new List<string>
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
        };

        public static List<string> Words = new List<string>
        {
            "about", "after", "again", "below", "could", "every", "first", "found", "great", "house", "large", "learn", "never", "other", "place",
            "plant", "point", "right", "small", "sound", "spell", "still", "study", "their", "there", "these", "thing", "think", "three", "water",
            "where", "which", "world", "would", "write"
        };

        public PasswordModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.Password;
        }

        List<List<string>> CurrentLetters;
        int CurrentList = 0;
        List<string> WrongWords;
        string CurrentWord;

        public override void Initialize()
        {
            CurrentLetters = new List<List<string>>
            {
                new List<string>(), new List<string>(), new List<string>(),
                new List<string>(), new List<string>(), new List<string>()
            };
            WrongWords = new List<string>();
            CurrentList = 1;
            Synth.Speak("ready");
        }

        public override void HandleSpeech(string speech)
        {
            if (Letters.Contains(speech))
            {
                CurrentLetters[CurrentList - 1].Add(speech);
                Synth.Speak("ok");
                var word = FindWord();
                CurrentWord = word;
                if (word != null)
                {
                    Synth.Speak("the word is " + word[0] + ", " + word[1] + ", " + word[2] + ", " + word[3] + ", " + word[4]);
                }
            }
            else if (speech == "one" || speech == "two" || speech == "three" || speech == "four" || speech == "five")
            {
                CurrentList = Utilities.GetIntFromSpeech(speech);
                Synth.Speak("ok, list " + CurrentList);
            }
            else if (speech == "next")
            {
                CurrentList++;
                Synth.Speak("ok, list " + CurrentList);
            }
            else if(speech == "cancel")
            {
                CurrentLetters[CurrentList].Clear();
                Synth.Speak("clearing list " + CurrentList);
            }
            else if(speech == "restart")
            {
                CurrentLetters = new List<List<string>>
                {
                    new List<string>(), new List<string>(), new List<string>(),
                    new List<string>(), new List<string>(), new List<string>()
                };
                CurrentList = 1;
                WrongWords = new List<string>();
                CurrentWord = null;
                Synth.Speak("starting over");
            }
            else if(speech == "next word")
            {
                WrongWords.Add(CurrentWord);
                Synth.Speak("ok");
                var word = FindWord();
                CurrentWord = word;
                if (word != null)
                {
                    Synth.Speak("the word is " + word[0] + ", " + word[1] + ", " + word[2] + ", " + word[3] + ", " + word[4]);
                }
            }
        }

        public string FindWord()
        {
            Dictionary<string, int> Matches = new Dictionary<string, int>();
            foreach (var word in Words.Where(w => !WrongWords.Contains(w)))
            {
                var count = 0;
                for (var i = 0; i < 5; i++)
                {
                    if (CurrentLetters[i].Contains(word[i].ToString()))
                        count++;
                }
                if (count >= 3)
                    Matches[word] = count;
            }

            if (Matches.Any())
            {
                return Matches.OrderByDescending(m => m.Value).First().Key;
            }

            return null;
        }
    }
}
