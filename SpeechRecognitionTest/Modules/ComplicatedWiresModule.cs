using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class ComplicatedWiresModule : BaseModule
    {
        public static List<string> Commands = new List<string>
        {
            "purple",
            "no l e d",
            "l e d",
            "star",
            "no star"
        };

        public static Dictionary<Tuple<string, string, string>, string> WireTable = new Dictionary<Tuple<string, string, string>, string>
        {
            //color led star
            { new Tuple<string, string, string>("white", "no", "no"), "C" },
            { new Tuple<string, string, string>("white", "yes", "no"), "D" },
            { new Tuple<string, string, string>("white", "no", "yes"), "C" },
            { new Tuple<string, string, string>("white", "yes", "yes"), "B" },
            { new Tuple<string, string, string>("red", "no", "no"), "S" },
            { new Tuple<string, string, string>("red", "yes", "no"), "B" },
            { new Tuple<string, string, string>("red", "no", "yes"), "C" },
            { new Tuple<string, string, string>("red", "yes", "yes"), "B" },
            { new Tuple<string, string, string>("blue", "no", "no"), "S" },
            { new Tuple<string, string, string>("blue", "yes", "no"), "P" },
            { new Tuple<string, string, string>("blue", "no", "yes"), "D" },
            { new Tuple<string, string, string>("blue", "yes", "yes"), "P" },
            { new Tuple<string, string, string>("purple", "no", "no"), "S" },
            { new Tuple<string, string, string>("purple", "yes", "no"), "S" },
            { new Tuple<string, string, string>("purple", "no", "yes"), "P" },
            { new Tuple<string, string, string>("purple", "yes", "yes"), "D" }
        };

        string CurrentColor;
        string CurrentLed;
        string CurrentStar;

        public ComplicatedWiresModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.ComplicatedWires;
        }

        public override void Initialize()
        {
            Synth.Speak("ready");
            CurrentColor = null;
            CurrentLed = null;
            CurrentStar = null;
        }

        public override void HandleSpeech(string speech)
        {
            if (speech == "red" || speech == "white" || speech == "blue" || speech == "purple")
                CurrentColor = speech;
            else if (speech == "l e d")
                CurrentLed = "yes";
            else if (speech == "no l e d")
                CurrentLed = "no";
            else if (speech == "star")
                CurrentStar = "yes";
            else if (speech == "no star")
                CurrentStar = "no";
            else if(speech == "cancel" || speech == "next")
            {
                CurrentColor = null;
                CurrentLed = null;
                CurrentStar = null;
                Synth.Speak("ready");
            }

            if (CurrentColor != null && CurrentLed != null && CurrentStar != null)
            {
                var result = WireTable[new Tuple<string, string, string>(CurrentColor, CurrentLed, CurrentStar)];
                Synth.Speak(TranslateResult(result));
            }
        }

        string TranslateResult(string result)
        {
            switch(result)
            {
                case "C":
                    return "cut the wire";
                case "D":
                    return "don't cut";
                case "B":
                    return "cut if there are two or more batteries";
                case "S":
                    return "cut if the last digit of the serial number is even";
                case "P":
                    return "cut if there's a parallel port";
                default:
                    return "";
            }
        }
    }
}
