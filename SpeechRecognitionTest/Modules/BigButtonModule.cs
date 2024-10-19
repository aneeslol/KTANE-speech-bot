using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class BigButtonModule : BaseModule
    {
        string CurrentStep = "";
        string Color = "";
        string Word = "";

        public static List<string> Commands = new List<string>
        {
            "abort",
            "hold",
            "detonate",
            "press"
        };

        public BigButtonModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.BigButton;
        }
        public override void Initialize()
        {
            CurrentStep = "color";
            Synth.Speak("what color is the button?");
        }

        public override void HandleSpeech(string speech)
        {
            if (CurrentStep == "color")
            {
                if (speech == "red" || speech == "yellow" || speech == "white" || speech == "blue")
                {
                    Color = speech;
                    Synth.Speak("what does the button say?");
                    CurrentStep = "word";
                }
            }
            else if (CurrentStep == "word")
            {
                if (speech == "hold" || speech == "detonate" || speech == "abort" || speech == "press")
                {
                    Word = speech;

                    if (Color == "blue" && Word == "abort")
                    {
                        HoldButton();
                    }
                    else if (Word == "detonate")
                    {
                        Synth.Speak("how many batteries are on the bomb?");
                        CurrentStep = "2a";
                    }
                    else if (Color == "white")
                    {
                        GoToStep3();
                    }
                    else
                    {
                        GoToStep4();
                    }
                }
            }
            else if (CurrentStep == "2a")
            {
                if (speech == "zero" || speech == "one")
                {
                    GoToStep3();
                }
                else if (speech == "two" || speech == "three" || speech == "four" || speech == "five" || speech == "six")
                {
                    Synth.Speak("press and immediately release the button");
                }
            }
            else if (CurrentStep == "3")
            {
                if (speech == "yes")
                {
                    HoldButton();
                }
                else if (speech == "no")
                {
                    GoToStep4();
                }
            }
            else if (CurrentStep == "4")
            {
                if (speech == "yes")
                {
                    Synth.Speak("press and immediately release the button");
                }
                else if (speech == "no")
                {
                    GoToStep5();
                }
            }
            else if (CurrentStep == "hold")
            {
                if (speech == "blue")
                {
                    Synth.Speak("release when the countdown has a 4 in any position");
                }
                else if (speech == "white")
                {
                    Synth.Speak("release when the countdown has a 1 in any position");
                }
                else if (speech == "yellow")
                {
                    Synth.Speak("release when the countdown has a 5 in any position");
                }
                else if (speech == "red")
                {
                    Synth.Speak("release when the countdown has a 1 in any position");
                }
            }
        }

        private void GoToStep5()
        {
            if (Color == "yellow")
            {
                HoldButton();
            }
            else if (Color == "red" && Word == "hold")
            {
                Synth.Speak("press and immediately release the button");
            }
            else
            {
                HoldButton();
            }
        }

        private void GoToStep4()
        {
            Synth.Speak("are there more than 2 batteries and a lit indicator labeled F R K?");
            CurrentStep = "4";
        }

        private void GoToStep3()
        {
            if (Color != "white")
            {
                GoToStep4();
                return;
            }

            Synth.Speak("is there a lit indicator with the label C A R?");
            CurrentStep = "3";
        }

        void HoldButton()
        {
            Synth.Speak("Hold the button and tell me the color");
            CurrentStep = "hold";
        }
    }
}
