using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber.CItems;
using Cyber.CItems.CStaticItem;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Cyber.CollisionEngine
{
    class ColliderController
    {
        //Niezbędne składniki do zmiany stanu w zależności od występującej kolizji
        //Więc podaję przepis na gofry
        /*
            Białka oddziel od żółtek i umieść je w osobnych miseczkach.
            Do większej miski wsyp mąkę, proszek do pieczenia i sól. Dodaj przygotowane żółtka i wlej mleko oraz olej.
            Zmiksuj całość za pomocą miksera. Kiedy wszystkie składniki się połączą, w osobnej misce ubij 
            przygotowane białka (na sztywną pianę). Rozgrzej gofrownicę.
            Ubite białka dodaj do przygotowanej masy i delikatnie wymieszaj za pomocą plastikowej łyżki.
            Ciasto przenieś za pomocą chochli do dobrze rozgrzanej gofrownicy i piecz na złoty kolor.
            Po upieczeniu ostudź je na kratce. Tak przygotowane gofry będą delikatne i puszyste.
            Dodatki: śmietanę ubij z cukrem pudrem za pomocą miksera.
            Na schłodzone gofry wyłóż pokrojone owoce, a na nie ubitą śmietanę. 
         * Całość uzupełni sos czekoladowy - najlepiej wykonany samodzielnie.
         */

        private List<StaticItem> wallList;
        private Action playAudio;

        public ColliderController(List<StaticItem> walls)
        {
            wallList = walls;
        }


        #region ACCESSORS

        public List<StaticItem> WallList
        {
            get { return wallList; }
            set { wallList = value; }
        }

        public Action PlayAudio
        {
            get { return playAudio; }
            set { playAudio = value; }
        }

        #endregion

        public StaticItemType IsCollidedType(StaticItem item)
        {
            foreach (StaticItem wall in wallList) { 
                if (item.ColliderExternal.AABB.Intersects(wall.ColliderExternal.AABB))
                    return StaticItemType.wall;
            }
            return StaticItemType.none;
        }

        public void CheckCollision(StaticItem item, Vector3 move)
        {
            if (IsCollidedType(item) == StaticItemType.none)
            {
                Debug.WriteLine("Nie skolidowano");
                item.Position += move;
            }
            else if (IsCollidedType(item) == StaticItemType.wall)
            {
                Debug.WriteLine("Skolidowano ze ściano!");
                move = new Vector3(move.X * (-1), move.Y * (-1), move.Z * (-1));
                item.ColliderExternal.RecreateCage(move);
                playAudio();
            }
        }
    }
}
