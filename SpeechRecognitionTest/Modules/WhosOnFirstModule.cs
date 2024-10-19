using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class WhosOnFirstModule : BaseModule
    {
        string CurrentStep = "";

        public static Dictionary<string, List<string>> WordLists = new Dictionary<string, List<string>>()
        {
            {"ready",   new List<string>{ "yes", "okay", "what", "middle", "left", "press", "right", "blank", "ready", "no", "first", "uhhh", "nothing", "wait" } },
            {"first",   new List<string>{ "left", "okay", "yes", "middle", "no", "right", "nothing", "uhhh", "wait", "ready", "blank", "what", "press", "first" } },
            {"no",      new List<string>{ "blank", "uhhh", "wait", "first", "what", "ready", "right", "yes", "nothing", "left", "press", "okay", "no", "middle" } },
            {"blank",   new List<string>{ "wait", "right", "okay", "middle", "blank", "press", "ready", "nothing", "no", "what", "left", "uhhh", "yes", "first" } },
            {"nothing", new List<string>{ "uhhh", "right", "okay", "middle", "yes", "blank", "no", "press", "left", "what", "wait", "first", "nothing", "ready" } },
            {"yes",     new List<string>{ "okay", "right", "uhhh", "middle", "first", "what", "press", "ready", "nothing", "yes", "left", "blank", "no", "wait" } },
            {"what",    new List<string>{ "uhhh", "what", "left", "nothing", "ready", "blank", "middle", "no", "okay", "first", "wait", "yes", "press", "right" } },
            {"uhhh",    new List<string>{ "ready", "nothing", "left", "what", "okay", "yes", "right", "no", "press", "blank", "uhhh", "middle", "wait", "first" } },
            {"left",    new List<string>{ "right", "left", "first", "no", "middle", "yes", "blank", "what", "uhhh", "wait", "press", "ready", "okay", "nothing" } },
            {"right",   new List<string>{ "yes", "nothing", "ready", "press", "no", "wait", "what", "right", "middle", "left", "uhhh", "blank", "okay", "first" } },
            {"middle",  new List<string>{ "blank", "ready", "okay", "what", "nothing", "press", "no", "wait", "left", "middle", "right", "first", "uhhh", "yes" } },
            {"okay",    new List<string>{ "middle", "no", "first", "yes", "uhhh", "nothing", "wait", "okay", "left", "ready", "blank", "press", "what", "right" } },
            {"wait",    new List<string>{ "uhhh", "no", "blank", "okay", "yes", "left", "first", "press", "what", "wait", "nothing", "ready", "right", "middle" } },
            {"press",   new List<string>{ "right", "middle", "yes", "ready", "press", "okay", "nothing", "uhhh", "blank", "left", "first", "what", "no", "wait" } },
            {"you",     new List<string>{ "sure", "you are", "your", "you're", "next", "uh huh", "ur", "hold", "what?", "you", "uh uh", "like", "done", "u" } },
            {"you are", new List<string>{ "your", "next", "like", "uh huh", "what?", "done", "uh uh", "hold", "you", "u", "you're", "sure", "ur", "you are" } },
            {"your",    new List<string>{ "uh uh", "you are", "uh huh", "your", "next", "ur", "sure", "u", "you're", "you", "what?", "hold", "like", "done" } },
            {"you're",  new List<string>{ "you", "you're", "ur", "next", "uh uh", "you are", "u", "your", "what?", "uh huh", "sure", "done", "like", "hold" } },
            {"ur",      new List<string>{ "done", "u", "ur", "uh huh", "what?", "sure", "your", "hold", "you're", "like", "next", "uh uh", "you are", "you" } },
            {"u",       new List<string>{ "uh huh", "sure", "next", "what?", "you're", "ur", "uh uh", "done", "u", "you", "like", "hold", "you are", "your" } },
            {"uh huh",  new List<string>{ "uh huh", "your", "you are", "you", "done", "hold", "uh uh", "next", "sure", "like", "you're", "ur", "u", "what?" } },
            {"uh uh",   new List<string>{ "ur", "u", "you are", "you're", "next", "uh uh", "done", "you", "uh huh", "like", "your", "sure", "hold", "what?" } },
            {"what?",   new List<string>{ "you", "hold", "you're", "your", "u", "done", "uh uh", "like", "you are", "uh huh", "ur", "next", "what?", "sure" } },
            {"done",    new List<string>{ "sure", "uh huh", "next", "what?", "your", "ur", "you're", "hold", "like", "you", "u", "you are", "uh uh", "done" } },
            {"next",    new List<string>{ "what?", "uh huh", "uh uh", "your", "hold", "sure", "next", "like", "done", "you are", "ur", "you're", "u", "you" } },
            {"hold",    new List<string>{ "you are", "u", "done", "uh uh", "you", "ur", "sure", "what?", "you're", "next", "hold", "uh huh", "your", "like" } },
            {"sure",    new List<string>{ "you are", "done", "like", "you're", "you", "hold", "uh huh", "ur", "sure", "u", "what?", "next", "your", "uh uh" } },
            {"like",    new List<string>{ "you're", "next", "u", "ur", "hold", "done", "uh uh", "what?", "uh huh", "you", "like", "sure", "you are", "your" } }
        };

        public static Dictionary<string, string> WordLocations = new Dictionary<string, string>
        {
            { "yes", "middle left" },
            { "first", "top right" },
            { "display", "bottom right" },
            { "okay", "top right" },
            { "says", "bottom right" },
            { "nothing", "middle left" },
            { "", "bottom left" },
            { "blank", "middle right" },
            { "no", "bottom right" },
            { "led", "middle left" },
            { "lead", "bottom right" },
            { "read", "middle right" },
            { "red", "middle right" },
            { "reed", "bottom left" },
            { "leed", "bottom left" },
            { "hold on", "bottom right" },
            { "you", "middle right" },
            { "you are", "bottom right" },
            { "your", "middle right" },
            { "you're", "middle right" },
            { "ur", "top left" },
            { "there", "bottom right" },
            { "they're", "bottom left" },
            { "their", "middle right" },
            { "they are", "middle left" },
            { "see", "bottom right" },
            { "c", "top right" },
            { "cee", "bottom right" },
        };

        public static Dictionary<string, string> Words = new Dictionary<string, string>
        {
            { "yes", "yes" },
            { "first", "first" },
            { "display", "display" },
            { "okay", "o k a y" },
            { "says", "says" },
            { "nothing", "nothing" },
            { "", "empty" },
            { "blank", "blank" },
            { "no", "no" },
            { "led", "l e d" },
            { "lead", "led" },
            { "read", "r e a d" },
            { "red", "red" },
            { "reed", "r e e d" },
            { "leed", "leed" },
            { "hold on", "hold on" },
            { "you", "y o u" },
            { "you are", "you are six letters" },
            { "your", "your" },
            { "you're", "your apostrophe" },
            { "ur", "u r" },
            { "there", "there" },
            { "they're", "they're apostrophe" },
            { "their", "their possessive" },
            { "they are", "they are" },
            { "see", "s e e" },
            { "c", "the letter c" },
            { "cee", "see e e" },

            { "ready", "ready" },
            { "what", "what" },
            { "uhhh", "u h h h" },
            { "left", "left" },
            { "right", "right" },
            { "middle", "middle" },
            { "wait", "wait" },
            { "press", "press" },
            { "u", "the letter u" },
            { "uh huh", "u h h u h" },
            { "uh uh", "u h u h" },
            { "what?", "what question mark" },
            { "done", "done" },
            { "next", "next" },
            { "hold", "hold" },
            { "sure", "sure" },
            { "like", "like" }
        };

        public WhosOnFirstModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.WhosOnFirst;
        }

        public override void Initialize()
        {
            Synth.Speak("what's on the display?");
            CurrentStep = "display";
        }

        string CurrentWordList = "";

        public override void HandleSpeech(string speech)
        {
            if (CurrentStep == "display")
            {
                if (Words.Values.Contains(speech))
                {
                    var key = Words.First(w => w.Value == speech).Key;
                    if (WordLocations.ContainsKey(key))
                    {
                        var position = WordLocations[key];
                        Synth.Speak("what's in the " + position + "?");
                        CurrentStep = "word";
                    }
                }
            }
            else if (CurrentStep == "word")
            {
                if (Words.Values.Contains(speech))
                {
                    var key = Words.First(w => w.Value == speech).Key;
                    if (WordLists.ContainsKey(key))
                    {
                        var list = WordLists[key];
                        CurrentWordList = key;
                        CurrentStep = "list";
                        Synth.SpeakAsync(string.Join(", ,", list.Select(l => Words[l])));
                    }
                }
            }
            else if (CurrentStep == "list")
            {
                if (speech == "next word")
                {
                    Synth.SpeakAsyncCancelAll();
                    Synth.Speak("what's on the display?");
                    CurrentStep = "display";
                }
                else if (speech == "repeat")
                {
                    Synth.SpeakAsyncCancelAll();
                    var list = WordLists[CurrentWordList];
                    Synth.SpeakAsync(string.Join(", ,", list.Select(l => Words[l])));
                }
            }
        }
    }
}
