using SpeechRecognitionTest.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeechRecognitionTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer synth = new SpeechSynthesizer();

        BaseModule CurrentModule = null;
        BaseModule NeedyModule = null;

        public MainWindow()
        {
            InitializeComponent();

            synth.SelectVoice("Microsoft Zira Desktop");
            synth.Rate = 2;

            _recognizer.LoadGrammar(new Grammar(BombGrammar.GetGrammar()));

            //_recognizer.SpeechHypothesized += _recognizer_SpeechHypothesized;
            _recognizer.SpeechRecognized += _recognizer_SpeechRecognized;
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void _recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {

        }

        private void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var speech = e.Result.Text;
            ResponseText.Content = speech;

            if (NeedyModule != null)
            {
                if (speech == "stop module" || speech == "complete")
                {
                    synth.SpeakAsyncCancelAll();
                    NeedyModule = null;
                    synth.Speak("needy module done");
                    if(CurrentModule != null)
                    {
                        synth.Speak("back to " + CurrentModule.Name);
                    }
                    
                    return;
                }
                else
                {
                    NeedyModule.HandleSpeech(speech);
                    return;
                }
            }
            else if (speech == BombGrammar.Knob)
            {
                NeedyModule = new KnobModule(synth);
                NeedyModule.Initialize();
                return;
            }

            if (CurrentModule != null)
            {
                if (speech == "stop module" || speech == "complete")
                {
                    synth.Speak("ok, next module");
                    CurrentModule = null;
                    return;
                }

                CurrentModule.HandleSpeech(speech);
                return;
            }
            else if (BombGrammar.ModuleNames.Contains(e.Result.Text, StringComparer.InvariantCultureIgnoreCase))
            {
                synth.Speak("OK, " + speech);
                StartNewModule(speech);
                return;
            }
            else
            {
                switch (e.Result.Text)
                {
                    case "hello":
                        synth.Speak("I hear you");
                        return;
                    case "we did it":
                        synth.Speak("let's go");
                        return;
                }
            }
        }

        private void StartNewModule(string moduleName)
        {
            BaseModule newModule = null;
            if (moduleName == BombGrammar.SimpleWires)
            {
                newModule = new SimpleWiresModule(synth);
            }
            else if (moduleName == BombGrammar.BigButton)
            {
                newModule = new BigButtonModule(synth);
            }
            else if (moduleName == BombGrammar.Keypad)
            {
                newModule = new KeyPadModule(synth);
            }
            else if (moduleName == BombGrammar.Memory)
            {
                newModule = new MemoryModule(synth);
            }
            else if (moduleName == BombGrammar.SimonSays)
            {
                newModule = new SimonSaysModule(synth);
            }
            else if (moduleName == BombGrammar.WhosOnFirst)
            {
                newModule = new WhosOnFirstModule(synth);
            }
            else if (moduleName == BombGrammar.Mazes)
            {
                newModule = new MazeModule(synth);
            }
            else if (moduleName == BombGrammar.MorseCode)
            {
                newModule = new MorseCodeModule(synth);
            }
            else if (moduleName == BombGrammar.Password)
            {
                newModule = new PasswordModule(synth);
            }
            else if (moduleName == BombGrammar.ComplicatedWires)
            {
                newModule = new ComplicatedWiresModule(synth);
            }
            else if (moduleName == BombGrammar.WireSequences)
            {
                newModule = new WireSequencesModule(synth);
            }

            if (newModule != null)
            {
                CurrentModule = newModule;
                newModule.Initialize();
            }
        }
    }
}
