using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Cyber2O.Audio
{
    class AudioModel
    {
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private string setName;


        public string AudioEngineName
        {
            get { return setName; }
            set
            {
                setName = value;
                string path = "../../../../Cyber2OContent/Assets/Music/";
                audioEngine = new AudioEngine(@path + setName + ".xgs");  //Path to .xgs
                waveBank = new WaveBank(audioEngine, @path + setName + ".xwb");   //Path to .xwb
                soundBank = new SoundBank(audioEngine, @path + setName + ".xsb");  //Path to .xsb
            }
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
