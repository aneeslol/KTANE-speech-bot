using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class MemoryStep
    {
        public int Digit;
        public int Position;
    }

    public class MemoryModule : BaseModule
    {
        string CurrentStep = "";
        List<MemoryStep> Steps;
        MemoryStep CurrentMemoryStep;

        public MemoryModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.Memory;
        }

        public override void Initialize()
        {
            CurrentStep = "1";
            Steps = new List<MemoryStep>();
            Synth.Speak("what is the display?");
        }

        public override void HandleSpeech(string speech)
        {
            if (speech == "one" || speech == "two" || speech == "three" || speech == "four")
            {
                if (CurrentStep.EndsWith("a"))
                {
                    CollectPosition(speech);
                }
                else if (CurrentStep.EndsWith("b"))
                {
                    CollectDigit(speech);
                }
                else if (CurrentStep.StartsWith("1"))
                {
                    CurrentMemoryStep = new MemoryStep();
                    if (speech == "one" || speech == "two")
                    {
                        Synth.Speak("press the button in the second position and tell me the digit");
                        CurrentStep = "1b";
                        CurrentMemoryStep.Position = 2;
                    }
                    else if (speech == "three")
                    {
                        Synth.Speak("press the button in the third position and tell me the digit");
                        CurrentStep = "1b";
                        CurrentMemoryStep.Position = 3;
                    }
                    else if (speech == "four")
                    {
                        Synth.Speak("press the button in the fourth position and tell me the digit");
                        CurrentStep = "1b";
                        CurrentMemoryStep.Position = 4;
                    }
                }
                else if (CurrentStep.StartsWith("2"))
                {
                    if (speech == "one")
                    {
                        Synth.Speak("press the button labeled 4 and tell me the position");
                        CurrentStep = "2a";
                        CurrentMemoryStep.Digit = 4;
                    }
                    else if (speech == "two" || speech == "four")
                    {
                        Synth.Speak("press the button in the " + TranslatePosition(Steps[0].Position) + " position and tell me the digit");
                        CurrentStep = "2b";
                        CurrentMemoryStep.Position = Steps[0].Position;
                    }
                    else if (speech == "three")
                    {
                        Synth.Speak("press the button in the first position and tell me the digit");
                        CurrentStep = "2b";
                        CurrentMemoryStep.Position = 1;
                    }
                }
                else if (CurrentStep.StartsWith("3"))
                {
                    if (speech == "one")
                    {
                        Synth.Speak("press the button labeled " + Steps[1].Digit + " and tell me the position");
                        CurrentStep = "3a";
                        CurrentMemoryStep.Digit = Steps[1].Digit;
                    }
                    else if (speech == "two")
                    {
                        Synth.Speak("press the button labeled " + Steps[0].Digit + " and tell me the position");
                        CurrentStep = "3a";
                        CurrentMemoryStep.Digit = Steps[0].Digit;
                    }
                    else if (speech == "three")
                    {
                        Synth.Speak("press the button in the third position and tell me the digit");
                        CurrentStep = "3b";
                        CurrentMemoryStep.Position = 3;
                    }
                    else if (speech == "four")
                    {
                        Synth.Speak("press the button labeled 4 and tell me the position");
                        CurrentStep = "3a";
                        CurrentMemoryStep.Digit = 4;
                    }
                }
                else if (CurrentStep.StartsWith("4"))
                {
                    if (speech == "one")
                    {
                        Synth.Speak("press the button in the " + TranslatePosition(Steps[0].Position) + " position and tell me the digit");
                        CurrentStep = "4b";
                        CurrentMemoryStep.Position = Steps[0].Position;
                    }
                    else if (speech == "two")
                    {
                        Synth.Speak("press the button in the first position and tell me the digit");
                        CurrentStep = "4b";
                        CurrentMemoryStep.Position = 1;
                    }
                    else if (speech == "three" || speech == "four")
                    {
                        Synth.Speak("press the button in the " + TranslatePosition(Steps[1].Position) + " position and tell me the digit");
                        CurrentStep = "4b";
                        CurrentMemoryStep.Position = Steps[1].Position;
                    }
                }
                else if (CurrentStep.StartsWith("5"))
                {
                    if (speech == "one")
                    {
                        Synth.Speak("press the button labeled " + Steps[0].Digit);
                        CurrentStep = "6";
                    }
                    if (speech == "two")
                    {
                        Synth.Speak("press the button labeled " + Steps[1].Digit);
                        CurrentStep = "6";
                    }
                    if (speech == "three")
                    {
                        Synth.Speak("press the button labeled " + Steps[3].Digit);
                        CurrentStep = "6";
                    }
                    if (speech == "four")
                    {
                        Synth.Speak("press the button labeled " + Steps[2].Digit);
                        CurrentStep = "6";
                    }
                }
            }
        }

        void CollectPosition(string speech)
        {
            if (speech == "one" || speech == "two" || speech == "three" || speech == "four")
            {
                CurrentMemoryStep.Position = Utilities.GetIntFromSpeech(speech);
                NextStep();
            }
        }

        void CollectDigit(string speech)
        {
            if (speech == "one" || speech == "two" || speech == "three" || speech == "four")
            {
                CurrentMemoryStep.Digit = Utilities.GetIntFromSpeech(speech);
                NextStep();
            }
        }

        private void NextStep()
        {
            Steps.Add(CurrentMemoryStep);
            CurrentMemoryStep = new MemoryStep();
            var step = int.Parse(CurrentStep.Substring(0, 1));

            if (step < 5)
            {
                CurrentStep = (step + 1).ToString();
                Synth.Speak("ok, what is the display?");
            }
        }

        string TranslatePosition(int position)
        {
            switch (position)
            {
                case 1:
                    return "first";
                case 2:
                    return "second";
                case 3:
                    return "third";
                case 4:
                    return "fourth";
                default:
                    return "";
            }
        }
    }
}
