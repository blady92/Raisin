using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Cyber2O
{
    class AudioSong
    {
        private Song song;
        private float power;

        public void LoadContent(ContentManager theContentManager, string path)
        {
            song = theContentManager.Load<Song>(path);
        }

        public AudioSong(float incrementation)
        {
            this.power = power;
        }

        public float Power
        {
            get { return power; }
            set { power = value; }
        }

        public void IsRepeat(bool repeat)
        {
            MediaPlayer.IsRepeating = repeat;
        }
        public void AudioPlay()
        {
            MediaPlayer.Play(song);
        }

        public void AudioPause()
        {
            MediaPlayer.Pause();
        }

        public void AudioResume()
        {
            MediaPlayer.Resume();
        }
        public void AudioStop()
        {
            MediaPlayer.Stop();
        }

        public void VolumeDown()
        {
            if (MediaPlayer.Volume > 0)
                MediaPlayer.Volume -= power;
        }

        public void VolumeUp()
        {
            if (MediaPlayer.Volume < 1)
                MediaPlayer.Volume += power;
        }
    }
}
