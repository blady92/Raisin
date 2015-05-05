using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyber.GraphicsEngine
{

    //Klasa, która zawiera wszystkie keyframe'y potrzebne by opisać pojedynczą animację.
    //Ekwiwalent -  Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContent
    public class AnimationClip
    {
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
            Duration = duration;
            Keyframes = keyframes;
        }

        //Prywatny konstruktor na użytek deserializera XNB
        private AnimationClip()
        {
        }

        //Pobiera całkowitą długość animacji
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }

        //Pobiera listę wszystkich keyframe dla wszystkich kości, posortowanych po czasie
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }


    }
}
