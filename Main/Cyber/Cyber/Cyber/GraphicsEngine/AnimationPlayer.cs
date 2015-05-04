using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;

namespace Cyber.GraphicsEngine
{
    /* Klasa, której zadaniem jest odczytywać macierze pozycji kości z klipu animacji */

    class AnimationPlayer
    {
        #region FIELDS

        //Informacje o aktualnie odtwarzanym klipie animacji
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        int currentKeyFrame;

        //Aktualne macierze przekształceń animacji
        Matrix[] boneTransforms;
        Matrix[] worldTransforms;
        Matrix[] skinTransforms;

        //Backlink do pozycji bindowania i hierarchii szkieletu
        SkinningData skinningDataValue;


        #endregion

        // Tworzy nowy AnimationPlayer
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];

        }

        //Dekoder określonego AnimationClip
        public void StartClip(AnimationClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyFrame = 0;

            //Zainicjalizowanie przekształceń kości do pozycji bindowania
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        //Aktualizacja obecnej pozycji animacji
        public void Update(TimeSpan time, bool relativeToCurrentTime, Matrix rootTransform)
        {
            UpdateBoneTransforms(time, relativeToCurrentTime);
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
        }

        //Metoda-Helper używana przez metodę Update by odświeżać dane BoneTransforms
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException("AnimationPlayer.Update was called before StartClip");

            //Update pozycji animacji
            if(relativeToCurrentTime)
            {
                time += currentTimeValue;

                //Jeżeli doszliśmy do końca animacji, zaczynamy od początku
                while (time >= currentClipValue.Duration)
                    time -= currentClipValue.Duration;
            }

            if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            //Jeżeli pozycja cofnęła się, zresetuj indeks keyframe'ów
            if (time < currentTimeValue)
            {
                currentKeyFrame = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            //Odczytaj macierze keyframe
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyFrame < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyFrame];

                //Zatrzymaj, gdy odczytaliśmy do obecnej pozycji czasu
                if (keyframe.Time > currentTimeValue)
                    break;

                //Użyj tej klatki keyframe
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyFrame++;
            }
        }
        //Metoda-Helper używana przez metodę Update by odświeżać dane WorldTransforms
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            //Kość root
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            //Kości child
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];
                worldTransforms[bone] = boneTransforms[bone] * worldTransforms[parentBone];
            }
        }

        //Metoda-Helper używana przez metodę Update by odświeżać dane SkinTransforms
        public void UpdateSkinTransforms()
        {
            for(int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] * worldTransforms[bone];
            }
        }

        //Gettery:
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }

        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }

        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }

        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }

        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }

    }  
}
