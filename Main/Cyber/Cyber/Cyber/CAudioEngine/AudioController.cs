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
            audio.CueList.Clear();
            audio.CueList.Add(audio.SoundBank.GetCue("bgmusic 2"));
            audio.CueList.Add(audio.SoundBank.GetCue("menuhover"));
            audio.CueList.Add(audio.SoundBank.GetCue("terminalMove2"));
            audio.CueList.Add(audio.SoundBank.GetCue("terminalMoveDown2"));
            audio.CueList.Add(audio.SoundBank.GetCue("bgmusic_dramatic"));
            audio.CueList.Add(audio.SoundBank.GetCue("gateOpening"));
            audio.CueList.Add(audio.SoundBank.GetCue("walking"));
            audio.CueList.Add(audio.SoundBank.GetCue("clickedPositive"));
            audio.CueList.Add(audio.SoundBank.GetCue("alertSam"));
            audio.CueList.Add(audio.SoundBank.GetCue("cinematicFree"));
            audio.CueList.Add(audio.SoundBank.GetCue("cinematicExit")); 
        }

        public void runAudio()
        {
            //newState = Keyboard.GetState();
            //if (newState.IsKeyDown(Keys.NumPad1) && oldState.IsKeyUp(Keys.NumPad1))
            //{
            //    audio.CueList[0].Play();
            //}
            //if (newState.IsKeyDown(Keys.NumPad2) && oldState.IsKeyUp(Keys.NumPad2))
            //{
            //    audio.SoundBank.GetCue("menuhover").Play();
            //}
            //if (newState.IsKeyDown(Keys.NumPad3) && oldState.IsKeyUp(Keys.NumPad3))
            //{
            //    audio.CueList[2].Play();
            //}
            //if (newState.IsKeyDown(Keys.NumPad0) && oldState.IsKeyUp(Keys.NumPad0))
            //{
            //    resetAudio();
            //}

            //oldState = newState;
        }

        public void BGMusicController(string command)
        {
            if(command == "Play")
            {
                audio.CueList[0].Play();
            }
            else if(command == "Stop")
            {
                audio.CueList[0].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[0].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[0].Pause();
            }
        }
        public void BGMusicDramaticController(string command)
        {
            if (command == "Play")
            {
                audio.CueList[4].Play();
            }
            else if (command == "Stop")
            {
                audio.CueList[4].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[4].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[4].Pause();
            }
        }

        public void gateOpeningController(string command)
        {
            if (command == "Play")
            {
                audio.CueList[5].Play();
            }
            else if (command == "Stop")
            {
                audio.CueList[5].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[5].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[5].Pause();
            }
        }

        public void walkingController(string command)
        {
            if (command == "Play")
            {
                audio.CueList[6].Play();
            }
            else if (command == "Stop")
            {
                audio.CueList[6].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[6].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[6].Pause();
            }
        }

        public void clickedPositiveController(string command)
        {
            if (command == "Play")
            {
                audio.SoundBank.GetCue("clickedPositive").Play();
            }
            else if (command == "Stop")
            {
                audio.CueList[6].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[6].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[6].Pause();
            }
        }

        public void alertSamController(string command)
        {
            if (command == "Play")
            {
                audio.SoundBank.GetCue("alertSam").Play();
            }
            else if (command == "Stop")
            {
                audio.CueList[6].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[6].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[6].Pause();
            }
        }

        public void cinematicFreeController(string command)
        {
            if (command == "Play")
            {
                audio.CueList[7].Play();
            }
            else if (command == "Stop")
            {
                audio.CueList[7].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[7].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[7].Pause();
            }
        }
        public void cinematicExitController(string command)
        {
            if (command == "Play")
            {
                audio.SoundBank.GetCue("cinematicExit").Play();
            }
            else if (command == "Stop")
            {
                audio.CueList[8].Stop(AudioStopOptions.Immediate);
            }
            else if (command == "Resume")
            {
                audio.CueList[8].Resume();
            }
            else if (command == "Pause")
            {
                audio.CueList[8].Pause();
            }
        }


        public void menuHoverController(string command)
        {
            if (command == "Play")
            {
                audio.SoundBank.GetCue("menuhover").Play();
            }
            else
            {
                audio.SoundBank.GetCue("menuhover").Stop(AudioStopOptions.Immediate);
            }
        }

        public void terminalSoundEffect(string command)
        {
            if(command == "Play")
            {
                audio.SoundBank.GetCue("terminalMove2").Play();
            }
            else if (command == "PlayInvert")
            {
                audio.SoundBank.GetCue("terminalMoveDown2").Play();
            }
        }
       

        public void playAudio(int i)
        {
            if (audio.CueList[i].IsPlaying || audio.CueList[i].IsStopped || audio.CueList[i].IsPaused)
            {
                resetAudio();
            }
            audio.CueList[1].Play();
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

        //Wrappers
        public void Play0()
        {
            playAudio(1);
        }
        public void Play1()
        {
            playAudio(1);
        }
        public void Play2()
        {
            playAudio(2);
        }
    }
}
