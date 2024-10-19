using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognitionTest.Modules
{
    public abstract class BaseModule
    {
        public string Name;
        public SpeechSynthesizer Synth;

        public BaseModule(SpeechSynthesizer synth)
        {
            Synth = synth;
        }

        public abstract void Initialize();

        public abstract void HandleSpeech(string speech);
    }
}
