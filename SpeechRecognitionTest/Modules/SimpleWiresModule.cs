using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class SimpleWiresModule : BaseModule
    {
        string CurrentStep;

        int NumRedWires;

        public static List<string> Commands = new List<string>
        {
            "zero wires",
            "one wire",
            "two wires",
            "three wires",
            "four wires",
            "five wires",
            "six wires"
        };

        public SimpleWiresModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.SimpleWires;
        }

        public override void Initialize()
        {
            CurrentStep = "1";
            Synth.Speak("How many wires?");
        }

        public override void HandleSpeech(string speech)
        {
            if (CurrentStep == "1")
            {
                HandleInitialStep(speech);
            }
            else if (CurrentStep.StartsWith("3"))
            {
                HandleThreeWires(speech);
            }
            else if (CurrentStep.StartsWith("4"))
            {
                HandleFourWires(speech);
            }
            else if (CurrentStep.StartsWith("5"))
            {
                HandleFiveWires(speech);
            }
            else if (CurrentStep.StartsWith("6"))
            {
                HandleSixWires(speech);
            }
        }

        void HandleInitialStep(string speech)
        {
            if (speech == "three wires" || speech == "three")
            {
                Synth.Speak("are there any red wires?");
                CurrentStep = "3a";
            }
            else if (speech == "four wires" || speech == "four")
            {
                Synth.Speak("how many red wires?");
                CurrentStep = "4a";
            }
            else if (speech == "five wires" || speech == "five")
            {
                Synth.Speak("is the last wire black?");
                CurrentStep = "5a";
            }
            else if (speech == "six wires" || speech == "six")
            {
                Synth.Speak("how many yellow wires?");
                CurrentStep = "6a";
            }
        }

        void HandleThreeWires(string speech)
        {
            if (CurrentStep == "3a")
            {
                if (speech == "no")
                    Synth.Speak("cut the second wire");
                else if (speech == "yes")
                {
                    Synth.Speak("is the last wire white?");
                    CurrentStep = "3b";
                }
            }
            else if (CurrentStep == "3b")
            {
                if (speech == "yes")
                    Synth.Speak("cut the last wire");
                else if (speech == "no")
                {
                    Synth.Speak("is there more than one blue wire?");
                    CurrentStep = "3c";
                }
            }
            else if (CurrentStep == "3c")
            {
                if (speech == "yes")
                    Synth.Speak("cut the last blue wire");
                else if (speech == "no")
                    Synth.Speak("cut the last wire");
            }
        }

        void HandleFourWires(string speech)
        {
            if (CurrentStep == "4a")
            {
                switch (speech)
                {
                    case "zero":
                    case "zero wires":
                        NumRedWires = 0;
                        CurrentStep = "4b";
                        Synth.Speak("is the last wire yellow?");
                        break;
                    case "one":
                    case "one wire":
                        NumRedWires = 1;
                        CurrentStep = "4b";
                        Synth.Speak("is the last wire yellow?");
                        break;
                    case "two":
                    case "three":
                    case "four":
                    case "two wires":
                    case "three wires":
                    case "four wires":
                        Synth.Speak("is the last digit of the serial number odd?");
                        NumRedWires = 2;
                        CurrentStep = "4aa";
                        break;
                }
            }
            else if (CurrentStep == "4aa")
            {
                if (speech == "yes")
                {
                    Synth.Speak("cut the last red wire");
                }
                else if (speech == "no")
                {
                    Synth.Speak("is the last wire yellow?");
                    CurrentStep = "4b";
                }
            }
            else if (CurrentStep == "4b")
            {
                if (speech == "yes")
                {
                    if (NumRedWires == 0)
                        Synth.Speak("cut the first wire");
                    else
                    {
                        Synth.Speak("is there exactly one blue wire?");
                        CurrentStep = "4c";
                    }
                }
                else if (speech == "no")
                {
                    Synth.Speak("is there exactly one blue wire?");
                    CurrentStep = "4c";
                }
            }
            else if (CurrentStep == "4c")
            {
                if (speech == "yes")
                    Synth.Speak("cut the first wire");
                else if (speech == "no")
                {
                    Synth.Speak("is there more than one yellow wire?");
                    CurrentStep = "4d";
                }
            }
            else if (CurrentStep == "4d")
            {
                if (speech == "yes")
                    Synth.Speak("cut the last wire");
                else if (speech == "no")
                    Synth.Speak("cut the second wire");
            }
        }

        void HandleFiveWires(string speech)
        {
            if (CurrentStep == "5a")
            {
                if (speech == "yes")
                {
                    Synth.Speak("is the last digit of the serial number odd?");
                    CurrentStep = "5aa";
                }
                else if (speech == "no")
                {
                    Synth.Speak("is there exactly one red wire?");
                    CurrentStep = "5b";
                }
            }
            else if (CurrentStep == "5aa")
            {
                if (speech == "yes")
                    Synth.Speak("cut the fourth wire");
                else if (speech == "no")
                {
                    Synth.Speak("is there exactly one red wire?");
                    CurrentStep = "5b";
                }
            }
            else if (CurrentStep == "5b")
            {
                if (speech == "yes")
                {
                    Synth.Speak("is there more than one yellow wire?");
                    CurrentStep = "5ba";
                }
                else if (speech == "no")
                {
                    Synth.Speak("are there no black wires?");
                    CurrentStep = "5c";
                }
            }
            else if (CurrentStep == "5ba")
            {
                if (speech == "yes")
                    Synth.Speak("cut the first wire");
                else if (speech == "no")
                {
                    Synth.Speak("are there no black wires?");
                    CurrentStep = "5c";
                }
            }
            else if (CurrentStep == "5c")
            {
                if (speech == "yes")
                    Synth.Speak("cut the second wire");
                else if (speech == "no")
                    Synth.Speak("cut the first wire");
            }
        }

        void HandleSixWires(string speech)
        {
            if (CurrentStep == "6a")
            {
                switch (speech)
                {
                    case "zero":
                    case "zero wires":
                        CurrentStep = "6aa";
                        Synth.Speak("is the last digit of the serial number odd?");
                        break;
                    case "one":
                    case "one wire":
                        CurrentStep = "6ba";
                        Synth.Speak("is there more than one white wire?");
                        break;
                    case "two":
                    case "three":
                    case "four":
                    case "five":
                    case "six":
                    case "two wires":
                    case "three wires":
                    case "four wires":
                    case "five wires":
                    case "six wires":
                        Synth.Speak("are there no red wires?");
                        CurrentStep = "6c";
                        break;
                }
            }
            else if (CurrentStep == "6aa")
            {
                if (speech == "yes")
                    Synth.Speak("cut the third wire");
                else if (speech == "no")
                {
                    Synth.Speak("are there no red wires?");
                    CurrentStep = "6c";
                }
            }
            else if (CurrentStep == "6ba")
            {
                if (speech == "yes")
                    Synth.Speak("cut the fourth wire");
                else if (speech == "no")
                {
                    Synth.Speak("are there no red wires?");
                    CurrentStep = "6c";
                }
            }
            else if (CurrentStep == "6c")
            {
                if (speech == "yes")
                    Synth.Speak("cut the last wire");
                else if (speech == "no")
                    Synth.Speak("cut the fourth wire");
            }
        }
    }
}
