using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber.Audio;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Cyber.AudioEngine
{
    public class AudioController
    {
        private AudioModel audio;
        private KeyboardState newState;
        private KeyboardState oldState;


        public AudioController(AudioModel audio)
        {
            this.audio = audio;
            audio.CueList = new List<Cue>();
        }

        public AudioModel Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        public void setAudio()
        {
            Debug.WriteLine("Inicjalizacja audio. Stan obecny: "+audio.CueList.Count);
            audio.CueList.Clear();
            Debug.WriteLine("Zerowanie audio. Stan obecny: " + audio.CueList.Count);

            audio.CueList.Add(audio.SoundBank.GetCue("vrag"));
            audio.CueList.Add(audio.SoundBank.GetCue("pierwaja"));
            audio.CueList.Add(audio.SoundBank.GetCue("edgeSound"));
            
            Debug.WriteLine("Audio ustawione. Stan obecny: " + audio.CueList.Count);
        }

        public void runAudio()
        {
            newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.NumPad1) && oldState.IsKeyUp(Keys.NumPad1))
            {
                Debug.WriteLine("Playing audio 1:" + audio.CueList[0].Name);
                audio.CueList[0].Play();
            }
            if (newState.IsKeyDown(Keys.NumPad2) && oldState.IsKeyUp(Keys.NumPad2))
            {
                Debug.WriteLine("Playing audio 2:" + audio.CueList[1].Name);
                audio.CueList[1].Play();
            }
            if (newState.IsKeyDown(Keys.NumPad3) && oldState.IsKeyUp(Keys.NumPad3))
            {
                Debug.WriteLine("Playing audio 3:" + audio.CueList[2].Name);
                audio.CueList[2].Play();
            }
            if (newState.IsKeyDown(Keys.NumPad0) && oldState.IsKeyUp(Keys.NumPad0))
            {
                resetAudio();
            }

            oldState = newState;
        }

        public void resetAudio()
        {
            foreach (Cue cue in audio.CueList)
            {
                if (cue.IsPlaying)
                {
                    cue.Stop(AudioStopOptions.Immediate);
                }
            }
            setAudio();
        }
    }
}
