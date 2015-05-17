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

        public List<StaticItem> staticItemList { get; set; }
        public List<StaticItem> npcItem { get; set; }
        private Action playAudio;
        private ConsoleSprites console;
        private Icon icon;
        private KeyboardState newState, oldstate;
        public StaticItemType CollisionItemType { get; set; }
        public bool ConsoleDetection { get; set; }

        public ColliderController(ConsoleSprites console, Icon icon)
        {
            this.console = console;
            this.icon = icon;
        }


        #region ACCESSORS

        public Action PlayAudio
        {
            get { return playAudio; }
            set { playAudio = value; }
        }

        #endregion

        //Sprawdza czy weszło w zasięg przeciwnika
        public bool EnemyCollision(StaticItem item)
        {
            foreach (StaticItem npc in npcItem)
            {
                if (item.ColliderInternal.AABB.Intersects(npc.ColliderExternal.AABB))
                    return true;
            }
            return false;
        }

        //Zwraca z czym się zderzyło z bliska. Od wykrywania zasięgu są metody powyżej
        public StaticItemType IsCollidedType(StaticItem item)
        {
            foreach (StaticItem staticItem in staticItemList)
            {
                if (staticItem.Type == StaticItemType.terminal)
                {
                    if (staticItem.ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB))
                    {
                        ConsoleDetection = true;
                        CollisionItemType = staticItem.Type;
                        return staticItem.Type;
                    }
                    else if (staticItem.ColliderExternal.AABB.Intersects(item.ColliderInternal.AABB))
                    {
                        ConsoleDetection = true;
                        CollisionItemType = StaticItemType.none;
                        return StaticItemType.none;
                    }
                    {
                        ConsoleDetection = false;
                    }
                }
                else if (staticItem.ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB))
                {
                    return staticItem.Type;
                }
            }

            foreach (StaticItem npc in npcItem)
            {
                if (npc.ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB))
                    return npc.Type;
            }

            return StaticItemType.none;
        }

        //Sprawdzenie czy zaszła jakakolwiek kolizja by się nie przenikać między sobą
        public void CheckCollision(StaticItem item, Vector3 move)
        {
            item.ColliderInternal.RecreateCage(move);
            if (IsCollidedType(item) == StaticItemType.none)
            {
                //Debug.WriteLine("Nie skolidowano");
                item.Position += move;
                icon.IconState = StaticIcon.none;
                if (ConsoleDetection)
                {
                    console.IsUsed = false;
                    //console.IsUsed = !ConsoleDetection;
                }
            }
            else
            {
                Debug.WriteLine(EnemyCollision(item).ToString());
                Debug.WriteLine(CollisionItemType.ToString());
                move = new Vector3(move.X * (-1), move.Y * (-1), move.Z * (-1));
                item.ColliderInternal.RecreateCage(move);
                playAudio();
            }
            
        }

        public void CallTerminalAfterCollision(StaticItem item)
        {
            if (ConsoleDetection)
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
