using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public class MazeCoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MazeModule : BaseModule
    {
        List<string> Maze1 = new List<string>
            {
                "o o oXo o o",
                " XXX X XXXX",
                "*Xo oXo o o",
                " X  XXXXXX ",
                "oXo oXo o *",
                " XXX X XXX ",
                "oXo o oXo o",
                " XXXXXXXXX ",
                "o o oXo oXo",
                " XXX X XXX ",
                "o oXo oXo o",
            };

        List<string> Maze2 = new List<string>
            {
                "o o oXo o o",
                "XX XXX X XX",
                "o oXo oX* o",
                " XXX XXXXX ",
                "oXo oXo o o",
                " X XXX XXX ",
                "o *Xo oXoXo",
                " XXX XXX X ",
                "oXoXoXo oXo",
                " X X X XXX ",
                "oXo oXo o o",
            };

        List<string> Maze3 = new List<string>
            {
                "o o oXoXo o",
                " XXX X X X ",
                "oXoXoXo oXo",
                "XX X XXXXX ",
                "o oXoXo oXo",
                " X X X X X ",
                "oXoXoX*XoX*",
                " X X X X X ",
                "oXo oXoXoXo",
                " XXXXX X X ",
                "o o o oXo o",
            };

        List<string> Maze4 = new List<string>
            {
                "* oXo o o o",
                " X XXXXXXX ",
                "oXoXo o o o",
                " X X XXXXX ",
                "oXo oXo oXo",
                " XXXXX XXX ",
                "*Xo o o o o",
                " XXXXXXXXX ",
                "o o o o oXo",
                " XXXXXXX X ",
                "o o oXo oXo",
            };

        List<string> Maze5 = new List<string>
            {
                "o o o o o o",
                "XXXXXXXX X ",
                "o o o o oXo",
                " XXXXX XXXX",
                "o oXo oX* o",
                " X XXXXX X ",
                "oXo o oXoXo",
                " XXXXX XXX ",
                "oXo o o oXo",
                " X XXXXXXX ",
                "oXo o * o o",
            };

        List<string> Maze6 = new List<string>
            {
                "oXo oXo * o",
                " X X XXX X ",
                "oXoXoXo oXo",
                " X X X XXX ",
                "o oXoXoXo o",
                " XXXXX X XX",
                "o oXo oXoXo",
                "XX X X X   ",
                "o oX*XoXo o",
                " XXXXX XXX ",
                "o o o oXo o",
            };

        List<string> Maze7 = new List<string>
            {
                "o * o oXo o",
                " XXXXX X X ",
                "oXo oXo oXo",
                " X XXXXXXX ",
                "o oXo oXo o",
                "XXXX XXX XX",
                "o oXo o oXo",
                " X X XXXXX ",
                "oXoXo o oXo",
                " XXXXXXX X ",
                "o * o o o o",
            };

        List<string> Maze8 = new List<string>
            {
                "oXo o *Xo o",
                " X XXX X X ",
                "o o oXo oXo",
                " XXXXXXXXX ",
                "oXo o o oXo",
                " X XXXXX X ",
                "oXo *Xo o o",
                " XXX XXXXXX",
                "oXoXo o o o",
                " X XXXXXXXX",
                "o o o o o o",
            };

        List<string> Maze9 = new List<string>
            {
                "oXo o o o o",
                " X XXXXX X ",
                "oXoX* oXoXo",
                " X X XXX X ",
                "o o oXo oXo",
                " XXXXX XXX ",
                "oXoXo oXo o",
                " X X XXXXX ",
                "*XoXoXo oXo",
                " X X X X XX",
                "o oXo oXo o",
            };

        List<List<string>> Mazes;

        Pathfinder pathfinder = new Pathfinder();
        string CurrentStep = "";
        MazeCoordinate Circle1;
        MazeCoordinate Circle2;
        MazeCoordinate Start;
        MazeCoordinate Finish;
        List<string> CurrentMaze;

        public MazeModule(SpeechSynthesizer synth) : base(synth)
        {
            Name = BombGrammar.Mazes;
            Mazes = new List<List<string>> { Maze1, Maze2, Maze3, Maze4, Maze5, Maze6, Maze7, Maze8, Maze9 };
        }

        public override void Initialize()
        {
            pathfinder = new Pathfinder();
            CurrentStep = "circleX1";
            Circle1 = new MazeCoordinate();
            Circle2 = new MazeCoordinate();
            Start = new MazeCoordinate();
            Finish = new MazeCoordinate();
            Synth.Speak("what's the first circle?");
        }

        public override void HandleSpeech(string speech)
        {
            if (speech == "one" || speech == "two" || speech == "three" || speech == "four" || speech == "five" || speech == "six")
            {
                var coord = Utilities.GetIntFromSpeech(speech);
                if (CurrentStep == "circleX1")
                {
                    Circle1.X = coord;
                    CurrentStep = "circleY1";
                    Synth.Speak("ok");
                }
                else if (CurrentStep == "circleX2")
                {
                    Circle2.X = coord;
                    CurrentStep = "circleY2";
                    Synth.Speak("ok");
                }
                else if (CurrentStep == "circleY1")
                {
                    Circle1.Y = coord;
                    Synth.Speak("what's the second circle?");
                    CurrentStep = "circleX2";
                }
                else if (CurrentStep == "circleY2")
                {
                    Circle2.Y = coord;
                    CurrentMaze = GetMazeFromCircles();
                    if (CurrentMaze == null)
                    {
                        Synth.Speak("I didn't get that, what's the first circle?");
                        CurrentStep = "circleX1";
                    }
                    else
                    {
                        Synth.Speak("what's the starting point?");
                        CurrentStep = "startX";
                    }
                }
                else if (CurrentStep == "startX")
                {
                    Start.X = coord;
                    CurrentStep = "startY";
                    Synth.Speak("ok");
                }
                else if (CurrentStep == "startY")
                {
                    Start.Y = coord;
                    Synth.Speak("what's the end point?");
                    CurrentStep = "endX";
                }
                else if (CurrentStep == "endX")
                {
                    Finish.X = coord;
                    CurrentStep = "endY";
                    Synth.Speak("ok");
                }
                else if (CurrentStep == "endY")
                {
                    Finish.Y = coord;
                    var path = pathfinder.FindPath(CurrentMaze, Start.X, Start.Y, Finish.X, Finish.Y);
                    Synth.Speak("ok, here's the path, " + string.Join(", ", path));
                }
            }
            else if (speech == "repeat")
            {
                var path = pathfinder.FindPath(CurrentMaze, Start.X, Start.Y, Finish.X, Finish.Y);
                Synth.Speak("ok, here's the path, " + string.Join(", ", path));
            }
        }

        public List<string> GetMazeFromCircles()
        {
            return Mazes.FirstOrDefault(m =>
                m[(Circle1.Y - 1) * 2][(Circle1.X - 1) * 2] == '*' ||
                m[(Circle2.Y - 1) * 2][(Circle2.X - 1) * 2] == '*'
            );
        }
    }
}
