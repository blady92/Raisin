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
        private ConsoleSprites console;
        private Icon icon;
        private KeyboardState newState, oldstate;

        public ColliderController(List<StaticItem> wallList, ConsoleSprites console, Icon icon)
        {
            this.wallList = wallList;
            this.console = console;
            this.icon = icon;
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
        
        public bool ConsoleCollided(StaticItem item)
        {
            if (item.ColliderInternal.AABB.Intersects(wallList[wallList.Count-1].ColliderExternal.AABB))
                return true;
            return false;
        }

        public StaticItemType IsCollidedType(StaticItem item)
        {
            foreach (StaticItem wall in wallList)
            {
                if (item.ColliderInternal.AABB.Intersects(wall.ColliderInternal.AABB))
                    return StaticItemType.wall;
                
            }
            return StaticItemType.none;
        }

        public void CheckCollision(StaticItem item, Vector3 move)
        {
            item.ColliderInternal.RecreateCage(move);
            if (IsCollidedType(item) == StaticItemType.none)
            {
                //Debug.WriteLine("Nie skolidowano");
                item.Position += move;
                icon.IconState = StaticIcon.none;
                if (!ConsoleCollided(item)) { 
                    console.IsUsed = false;
                }
            }
            else if (IsCollidedType(item) == StaticItemType.wall)
            {
                //Debug.WriteLine("Skolidowano ze ściano!");
                move = new Vector3(move.X * (-1), move.Y * (-1), move.Z * (-1));
                item.ColliderInternal.RecreateCage(move);
                playAudio();
            }
            
        }

        public void CallTerminalAfterCollision(StaticItem item)
        {
            if (ConsoleCollided(item))
            {
                KeyboardState newState = Keyboard.GetState();
                if (newState.IsKeyDown(Keys.Tab) && oldstate.IsKeyUp(Keys.Tab))
                {
                    console.ResetConsole();
                    console.IsUsed = !console.IsUsed;
                }
                oldstate = newState;
                if (console.IsUsed)
                {
                    icon.IconState = StaticIcon.none;
                }
                else
                {
                    icon.IconState = StaticIcon.action;
                }
            }
        }
    }
}
