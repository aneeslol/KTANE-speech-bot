using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class KeyPadModule : BaseModule
    {
        string CurrentStep = "0";

        public static List<string> Commands = new List<string>
        {
            "balloon",
            "pyramid",
            "upside down y",
            "lightning bolt",
            "spider",
            "h",
            "backwards c",
            "backwards e",
            "slinky",
            "white star",
            "question mark",
            "copyright",
            "nose",
            "x",
            "paragraph",
            "t b",
            "smiley",
            "smiley face",
            "manora",
            "regular c",
            "snake",
            "black star",
            "dumbell",
            "a e",
            "backwards n",
            "omega",
            "three",
            "six"
        };

        List<List<string>> SymbolLists = new List<List<string>>
        {
            new List<string> { "balloon", "pyramid", "upside down y", "lightning bolt", "spider", "h", "backwards c" },
            new List<string> { "backwards e", "balloon", "backwards c", "slinky", "white star", "h", "question mark",  },
            new List<string> { "copyright", "nose", "slinky", "x", "three", "upside down y", "white star" },
            new List<string> { "six", "paragraph", "t b", "spider", "x", "question mark", "smiley" },
            new List<string> { "manora", "smiley", "t b", "regular c", "paragraph", "snake", "black star" },
            new List<string> { "six", "backwards e", "dumbell", "a e", "manora", "backwards n", "omega" },
        };

        List<string> CurrentSymbols = new List<string>();

        public KeyPadModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.Keypad;
        }

        public override void Initialize()
        {
            CurrentSymbols = new List<string>();
            Synth.Speak("Tell me the symbols");
            CurrentStep = "1";
        }

        public override void HandleSpeech(string speech)
        {
            if (CurrentStep != "1")
                return;

            if (CurrentSymbols.Count < 4)
            {
                if (Commands.Contains(speech))
                {
                    CurrentSymbols.Add(speech);
                    Synth.Speak("ok");
                }

                if (CurrentSymbols.Count == 4)
                {
                    var list = SymbolLists.FirstOrDefault(l => CurrentSymbols.All(l.Contains));
                    if (list == null)
                    {
                        Synth.Speak("I didn't get that");
                        Initialize();
                    }
                    else
                    {
                        for (var i = 0; i < list.Count(); i++)
                        {
                            if (CurrentSymbols.Contains(list[i]))
                            {
                                Synth.Speak(list[i]);
                            }
                        }
                    }
                }
            }
        }
    }
}
