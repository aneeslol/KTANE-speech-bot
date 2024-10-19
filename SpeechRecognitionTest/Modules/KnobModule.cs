using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class KnobModule : BaseModule
    {
        List<List<string>> UpSequences = new List<List<string>>
        {
            new List<string> {
                "off", "off", "on", "off", "on", "on",
                "on", "on", "on", "on", "off", "on"
            },
            new List<string> {
                "on", "off", "on", "off", "on", "off",
                "off", "on", "on", "off", "on", "on"
            }
        };

        List<List<string>> DownSequences = new List<List<string>>
        {
            new List<string> {
                "off", "on", "on", "off", "off", "on",
                "on", "on", "on", "on", "off", "on"
            },
            new List<string> {
                "on", "off", "on", "off", "on", "off",
                "off", "on", "off", "off", "off", "on"
            }
        };

        List<List<string>> LeftSequences = new List<List<string>>
        {
            new List<string> {
                "off", "off", "off", "off", "on", "off",
                "on", "off", "off", "on", "on", "on"
            },
            new List<string> {
                "off", "off", "off", "off", "on", "off",
                "off", "off", "off", "on", "on", "off"
            }
        };

        List<List<string>> RightSequences = new List<List<string>>
        {
            new List<string> {
                "on", "off", "on", "on", "on", "on",
                "on", "on", "on", "off", "on", "off"
            },
            new List<string> {
                "on", "off", "on", "on", "off", "off",
                "on", "on", "on", "off", "on", "off"
            }
        };

        public KnobModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.Knob;
        }

        List<string> Lights = new List<string>();

        List<List<string>> AllSequences = new List<List<string>>();

        public override void Initialize()
        {
            AllSequences = new List<List<string>>();
            AllSequences.AddRange(UpSequences);
            AllSequences.AddRange(DownSequences);
            AllSequences.AddRange(LeftSequences);
            AllSequences.AddRange(RightSequences);
            Synth.Speak("read the lights from left to right");
        }

        public override void HandleSpeech(string speech)
        {
            if (speech == "on" || speech == "off")
            {
                if (Lights.Count < 12)
                {
                    Lights.Add(speech);
                    Synth.SpeakAsync("ok");
                }

                var matchingSequences = AllSequences.Where(seq => CompareSequence(seq, Lights));
                if (matchingSequences.Count() == 1)
                {
                    if (UpSequences.Contains(matchingSequences.First()))
                        Synth.SpeakAsync("point the knob up");
                    else if (DownSequences.Contains(matchingSequences.First()))
                        Synth.SpeakAsync("point the knob down");
                    else if (LeftSequences.Contains(matchingSequences.First()))
                        Synth.SpeakAsync("point the knob left");
                    else if (RightSequences.Contains(matchingSequences.First()))
                        Synth.SpeakAsync("point the knob right");
                }
            }
            else if(speech == "restart")
            {
                Lights.Clear();
                Synth.Speak("restarting");
            }
        }

        bool CompareSequence(List<string> sequence1, List<string> sequence2)
        {
            for(var i = 0; i < sequence1.Count; i++)
            {
                if (i < sequence2.Count - 1 && sequence1[i] != sequence2[i])
                    return false;
            }

            return true;
        }

    }
}
