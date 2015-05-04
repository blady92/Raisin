using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cyber.GraphicsEngine
{
    //Opisuje pozycje pojedynczej kości w pojedynczym punkcie w czasie
    public class Keyframe
    {
        //Tworzy nowy obiekt keyframe
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }

        //Prywatny konstruktor na użytek deserializera XNB
        private Keyframe()
        {
        }

        //Pobiera indeks kości animowanej przez obecnie wybraną klatkę/keyframe
        [ContentSerializer]
        public int Bone { get; private set; }

        //Pobiera offset czasowy od startu animacji do tej klatki/keyframe'u
        [ContentSerializer]
        public TimeSpan Time { get; private set; }

        //Pobiera przekształcenie kości dla tej klatki/keyframe'u
        [ContentSerializer]
        public Matrix Transform { get; private set; }


    }
}
