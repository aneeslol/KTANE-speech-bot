using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class WireSequencesModule : BaseModule
    {
        public static List<string> Commands = new List<string>()
        {
            "red to a",
            "red to b",
            "red to c",
            "black to a",
            "black to b",
            "black to c",
            "blue to a",
            "blue to b",
            "blue to c"
        };

        public List<string> RedSequence = new List<string>
        {
            "c", "b", "a", "ac", "b", "ac", "abc", "ab", "b"
        };

        public List<string> BlueSequence = new List<string>
        {
            "b", "ac", "b", "a", "b", "bc", "c", "ac", "a"
        };

        public List<string> BlackSequence = new List<string>()
        {
            "abc", "ac", "b", "ac", "b", "bc", "ab", "c", "c"
        };

        public WireSequencesModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.WireSequences;
        }

        int CurrentRed = 0;
        int CurrentBlue = 0;
        int CurrentBlack = 0;

        public override void Initialize()
        {
            CurrentRed = 0;
            CurrentBlue = 0;
            CurrentBlack = 0;
            Synth.Speak("ready");
        }

        public override void HandleSpeech(string speech)
        {
            if (speech.StartsWith("red to"))
            {
                HandleSequence(RedSequence, CurrentRed, speech.Last().ToString());
                CurrentRed++;
            }
            else if (speech.StartsWith("black to"))
            {
                HandleSequence(BlackSequence, CurrentBlack, speech.Last().ToString());
                CurrentBlack++;
            }
            else if (speech.StartsWith("blue to"))
            {
                HandleSequence(BlueSequence, CurrentBlue, speech.Last().ToString());
                CurrentBlue++;
            }
            else if(speech == "restart")
            {
                CurrentRed = 0;
                CurrentBlue = 0;
                CurrentBlack = 0;
                Synth.Speak("restarting");
            }
        }

        void HandleSequence(List<string> list, int index, string letter)
        {
            if (list[index].Contains(letter))
                Synth.Speak("cut the wire");
            else
                Synth.Speak("don't cut");
        }
    }
}
