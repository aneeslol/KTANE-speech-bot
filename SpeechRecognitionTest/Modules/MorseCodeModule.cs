using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class MorseCodeModule : BaseModule
    {
        public static Dictionary<string, char> Letters = new Dictionary<string, char>
        {
            { "dot dash", 'a' },
            { "dash dot dot dot", 'b' },
            { "dash dot dash dot", 'c' },
            { "dash dot dot", 'd' },
            { "dot", 'e' },
            { "dot dot dash dot", 'f' },
            { "dash dash dot", 'g' },
            { "dot dot dot dot", 'h' },
            { "dot dot", 'i' },
            { "dot dash dash dash", 'j' },
            { "dash dot dash", 'k' },
            { "dot dash dot dot", 'l' },
            { "dash dash", 'm' },
            { "dash dot", 'n' },
            { "dash dash dash", 'o' },
            { "dot dash dash dot", 'p' },
            { "dash dash dot dash", 'q' },
            { "dot dash dot", 'r' },
            { "dot dot dot", 's' },
            { "dash", 't' },
            { "dot dot dash", 'u' },
            { "dot dot dot dash", 'v' },
            { "dot dash dash", 'w' },
            { "dash dot dot dash", 'x' },
            { "dash dot dash dash", 'y' },
            { "dash dash dot dot", 'z' },
        };

        Dictionary<string, decimal> Words = new Dictionary<string, decimal>
        {
            { "shell", 3.505m },
            { "halls", 3.515m },
            { "slick", 3.522m },
            { "trick", 3.532m },
            { "boxes", 3.535m },
            { "leaks", 3.542m },
            { "strobe", 3.545m },
            { "bistro", 3.552m },
            { "flick", 3.555m },
            { "bombs", 3.565m },
            { "break", 3.572m },
            { "brick", 3.575m },
            { "steak", 3.582m },
            { "sting", 3.592m },
            { "vector", 3.595m },
            { "beats", 3.6m }
        };

        List<char> CurrentLetters;

        public MorseCodeModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.MorseCode;
        }

        public override void Initialize()
        {
            CurrentLetters = new List<char>();
            Synth.Speak("ready");
        }

        public override void HandleSpeech(string speech)
        {
            if (Letters.Keys.Contains(speech))
            {
                CurrentLetters.Add(Letters[speech]);
                Synth.Speak("ok");

                if (CurrentLetters.Count > 3)
                {
                    var word = FindWord();
                    if (word != null)
                        Synth.Speak("ok, the frequency is " + Words[word].ToString());
                }
            }
            else if (speech == "cancel")
            {
                CurrentLetters.RemoveAt(CurrentLetters.Count - 1);
                Synth.Speak("cancelled");
            }
            else if(speech == "restart")
            {
                CurrentLetters = new List<char>();
                Synth.Speak("ready");
            }
        }

        public string FindWord()
        {
            var fourSequences = new List<string>();
            var threeSequences = new List<string>();

            for (int i = 0; i < CurrentLetters.Count; i++)
            {
                var index2 = i == CurrentLetters.Count - 1 ? 0 : i + 1;
                var index3 = i == CurrentLetters.Count - 2 ? 0 : index2 + 1;
                var index4 = i == CurrentLetters.Count - 3 ? 0 : index3 + 1;

                threeSequences.Add(CurrentLetters[i].ToString() + CurrentLetters[index2].ToString() + CurrentLetters[index3].ToString());
                fourSequences.Add(CurrentLetters[i].ToString() + CurrentLetters[index2].ToString() + CurrentLetters[index3].ToString() + CurrentLetters[index4].ToString());
            }

            var fourMatch = Words.Keys.FirstOrDefault(w => fourSequences.Any(w.Contains));
            if (fourMatch != null)
                return fourMatch;

            var threeMatches = Words.Keys.Where(w => threeSequences.Any(w.Contains))
                .OrderByDescending(m => m.Count(CurrentLetters.Contains));

            if (threeMatches.Any())
                return threeMatches.First();

            return null;
        }
    }
}
