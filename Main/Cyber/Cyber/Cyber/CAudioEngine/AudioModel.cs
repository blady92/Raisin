using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Cyber.Audio
{
    public class AudioModel
    {
        private Microsoft.Xna.Framework.Audio.AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private string setName;

        private List<Cue> cueList;
        private Cue cue;

        public Cue Cue
        {
            get { return cue; }
            set { cue = value; }
        }

        public List<Cue> CueList
        {
            get { return cueList; }
            set { cueList = value; }
        }

        public AudioModel(string name)
        {
            AudioEngineName = name;
        }
        public string AudioEngineName
        {
            get { return setName; }
            set
            {
                cueList = new List<Cue>();
                setName = value;
                string path = "../../../../CyberContent/Assets/Music/Win/";
                audioEngine = new Microsoft.Xna.Framework.Audio.AudioEngine(@path + setName + ".xgs");  //Path to .xgs
                waveBank = new WaveBank(audioEngine, @path + setName + ".xwb");   //Path to .xwb
                soundBank = new SoundBank(audioEngine, @path + setName + ".xsb");  //Path to .xsb

            }
        }

        public Microsoft.Xna.Framework.Audio.AudioEngine AudioEngine
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
