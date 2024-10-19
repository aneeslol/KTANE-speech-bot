using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class SimonSaysModule : BaseModule
    {
        Dictionary<string, string> VowelNoStrikes = new Dictionary<string, string>
        {
            { "red", "blue" },
            { "blue", "red" },
            { "green", "yellow" },
            { "yellow", "green" }
        };

        Dictionary<string, string> VowelOneStrike = new Dictionary<string, string>
        {
            { "red", "yellow" },
            { "blue", "green" },
            { "green", "blue" },
            { "yellow", "red" }
        };

        Dictionary<string, string> VowelTwoStrikes = new Dictionary<string, string>
        {
            { "red", "green" },
            { "blue", "red" },
            { "green", "yellow" },
            { "yellow", "blue" }
        };
        Dictionary<string, string> NoVowelNoStrikes = new Dictionary<string, string>
        {
            { "red", "blue" },
            { "blue", "yellow" },
            { "green", "green" },
            { "yellow", "red" }
        };

        Dictionary<string, string> NoVowelOneStrike = new Dictionary<string, string>
        {
            { "red", "red" },
            { "blue", "blue" },
            { "green", "yellow" },
            { "yellow", "green" }
        };

        Dictionary<string, string> NoVowelTwoStrikes = new Dictionary<string, string>
        {
            { "red", "yellow" },
            { "blue", "green" },
            { "green", "blue" },
            { "yellow", "red" }
        };

        int CurrentSequenceLength = 0;
        List<string> CurrentSequence = new List<string>();
        string CurrentStep = "";
        Dictionary<string, string> CurrentDictionary;


        public SimonSaysModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.SimonSays;
        }

        public override void Initialize()
        {
            Synth.Speak("is there a vowel in the serial number?");
            CurrentStep = "vowel";
            CurrentSequenceLength = 1;
            CurrentSequence = new List<string>();
        }

        public override void HandleSpeech(string speech)
        {
            if (CurrentStep == "vowel")
            {
                if (speech == "yes")
                {
                    Synth.Speak("ok, how many strikes do we have?");
                    CurrentStep = "strikesV";
                }
                else if (speech == "no")
                {
                    Synth.Speak("ok, how many strikes do we have?");
                    CurrentStep = "strikesN";
                }
            }
            else if (CurrentStep.StartsWith("strikes"))
            {
                if (speech == "zero")
                {
                    if (CurrentStep.EndsWith("V"))
                    {
                        Synth.Speak("ok, tell me the sequence");
                        CurrentDictionary = VowelNoStrikes;
                        CurrentStep = "sequence";
                    }
                    else
                    {
                        Synth.Speak("ok, tell me the sequence");
                        CurrentDictionary = NoVowelNoStrikes;
                        CurrentStep = "sequence";
                    }
                }
                else if (speech == "one")
                {
                    if (CurrentStep.EndsWith("V"))
                    {
                        Synth.Speak("ok, tell me the sequence");
                        CurrentDictionary = VowelOneStrike;
                        CurrentStep = "sequence";
                    }
                    else
                    {
                        Synth.Speak("ok, tell me the sequence");
                        CurrentDictionary = NoVowelOneStrike;
                        CurrentStep = "sequence";
                    }
                }
                else if (speech == "two")
                {
                    if (CurrentStep.EndsWith("V"))
                    {
                        Synth.Speak("ok, tell me the sequence");
                        CurrentDictionary = VowelTwoStrikes;
                        CurrentStep = "sequence";
                    }
                    else
                    {
                        Synth.Speak("ok, tell me the sequence");
                        CurrentDictionary = NoVowelTwoStrikes;
                        CurrentStep = "sequence";
                    }
                }
            }
            else if (CurrentStep == "sequence")
            {
                if (speech == "red" || speech == "green" || speech == "yellow" || speech == "blue")
                {
                    CurrentSequence.Add(speech);
                    Synth.Speak("ok");

                    if (CurrentSequence.Count == CurrentSequenceLength)
                    {
                        Synth.Speak("the sequence is " + string.Join(" ", CurrentSequence.Select(s => CurrentDictionary[s])));
                        CurrentSequenceLength++;
                        CurrentSequence = new List<string>();
                    }
                }
            }
        }
    }
}
