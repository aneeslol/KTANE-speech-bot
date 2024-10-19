using SpeechRecognitionTest.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest
{
    public class BombGrammar
    {
        public static GrammarBuilder GetGrammar()
        {
            var choices = new Choices();

            choices.Add("hello");
            choices.Add("we did it");
            choices.Add("complete");
            choices.Add("next");
            choices.Add("stop module");
            choices.Add("yes");
            choices.Add("no");
            choices.Add("on");
            choices.Add("off");

            choices.Add("zero");
            choices.Add("one");
            choices.Add("two");
            choices.Add("three");
            choices.Add("four");
            choices.Add("five");
            choices.Add("six");

            choices.Add("red");
            choices.Add("white");
            choices.Add("yellow");
            choices.Add("blue");
            choices.Add("green");

            choices.Add("next word");
            choices.Add("repeat");
            choices.Add("cancel");
            choices.Add("restart");

            foreach (var name in ModuleNames)
            {
                choices.Add(name);
            }

            foreach (var command in SimpleWiresModule.Commands)
            {
                choices.Add(command);
            }

            foreach (var command in BigButtonModule.Commands)
            {
                choices.Add(command);
            }

            foreach (var command in KeyPadModule.Commands)
            {
                choices.Add(command);
            }

            foreach (var command in WhosOnFirstModule.Words.Values)
            {
                choices.Add(command);
            }

            foreach (var command in MorseCodeModule.Letters.Keys)
            {
                choices.Add(command);
            }

            foreach(var command in PasswordModule.Letters)
            {
                choices.Add(command);
            }

            foreach (var command in ComplicatedWiresModule.Commands)
            {
                choices.Add(command);
            }

            foreach (var command in WireSequencesModule.Commands)
            {
                choices.Add(command);
            }

            var gb = new GrammarBuilder(choices);
            return gb;
        }

        public static string SimpleWires = "simple wires";
        public static string ComplicatedWires = "complicated wires";
        public static string BigButton = "big button";
        public static string SimonSays = "simon says";
        public static string Keypad = "keypad";
        public static string WhosOnFirst = "whos on first";
        public static string Memory = "memory";
        public static string Mazes = "mazes";
        public static string MorseCode = "morse code";
        public static string WireSequences = "wire sequences";
        public static string Password = "password";
        public static string Knob = "needy knob";

        public static List<string> ModuleNames = new List<string> { SimpleWires, ComplicatedWires, BigButton, SimonSays, 
            Keypad, WhosOnFirst, Memory, Mazes, MorseCode, WireSequences, Password, Knob };

    }
}
