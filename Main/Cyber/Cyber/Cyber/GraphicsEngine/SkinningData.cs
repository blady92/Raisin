using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Cyber.GraphicsEngine
{
    //Klasa, która zawiera wszystkie dane potrzebne by wyrenderować i zanimować zeskinowany obiekt.
    //Zazwyczaj przechowywane we właściwości Tag Modelu, który jest animowany.
   public class SkinningData
    {
       //Tworzy nowy obiekt skinning data
       public SkinningData(Dictionary<string, AnimationClip> animationClips, List<Matrix> bindPose, List<Matrix> inverseBindPose, List<int> skeletonHierarchy)
       {
           AnimationClips = animationClips;
           BindPose = bindPose;
           InverseBindPose = inverseBindPose;
           SkeletonHierarchy = skeletonHierarchy;
       }

       //Prywatny konstruktor używany przez deserializer XNB
       private SkinningData()
       {
       }

       //Pobiera kolekcję animation clipów. Zazwyczaj przechowywane w dictionary, więc mogą się zawierać tam instancje takie jak "Walk", "Run", "Jump" etc
       [ContentSerializer]
       public Dictionary<string, AnimationClip> AnimationClips { get; private set; }

       //Macierze BindPose dla każdej kości w szkielecie, relatywne do ich kości-rodzica
       [ContentSerializer]
       public List<Matrix> BindPose { get; private set; }

       //Wierzchołki dla każdej zmiany kości w przestrzeni dla szkieletu
       [ContentSerializer]
       public List<Matrix> InverseBindPose { get; private set; }

       //Dla każdej kości w szkielecie, przechowuje indeks kości rodzica
       [ContentSerializer]
       public List<int> SkeletonHierarchy { get; private set; }
    }
}
