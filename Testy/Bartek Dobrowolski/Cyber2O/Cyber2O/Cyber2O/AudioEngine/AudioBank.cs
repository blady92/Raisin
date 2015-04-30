using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Cyber2O
{
    class AudioBank
    {
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;

        public AudioBank(string setName)
        {
            string path = "../../../../Cyber2OContent/Assets/Music/";
            audioEngine = new AudioEngine(@path + setName + ".xgs");  //Path to .xgs
            waveBank = new WaveBank(audioEngine, @path + setName + ".xwb");   //Path to .xwb
            soundBank = new SoundBank(audioEngine, @path + setName + ".xsb");  //Path to .xsb
        }

        public AudioEngine AudioEngine
        {
            get { return audioEngine; }
            set { audioEngine = value; }
        }

        public WaveBank WaveBank
        {
            get { return waveBank; }
            set { waveBank = value; }
        }

        public SoundBank SoundBank
        {
            get { return soundBank; }
            set { soundBank = value; }
        }
    }
}
