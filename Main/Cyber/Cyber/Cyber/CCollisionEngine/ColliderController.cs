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
        public StaticItem samantha { get; set; }
        private Action playAudio;
        private ConsoleSprites console;
        //private Icon icon;
        private KeyboardState newState, oldstate;
        public StaticItemType CollisionItemType { get; set; }
        public bool ConsoleDetection { get; set; }

        public ColliderController(ConsoleSprites console)
        {
            this.console = console;
            //this.icon = icon;
        }
        public int terminalNumber { get; set; }

        public PlotTwistClass plot { get; set; }

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
                {
                    npc.OnOffBilboard = true;
                    return true;
                }
                npc.OnOffBilboard = false;
            }
            return false;
        }

        //Zwraca z czym się zderzyło z bliska.
        public StaticItemType IsCollidedType(StaticItem item)
        {
            for(int i=0; i<staticItemList.Count; i++)
            {
                #region Obsługa konsoli
                if (staticItemList[i].Type == StaticItemType.terminal && item.Type == StaticItemType.samantha)
                {
                    if (staticItemList[i].ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB))
                    {
                        ConsoleDetection = true;
                        terminalNumber = i;
                        CollisionItemType = staticItemList[i].Type;
                        return staticItemList[i].Type;
                    }
                    else if (staticItemList[i].ColliderExternal.AABB.Intersects(item.ColliderInternal.AABB))
                    {
                        ConsoleDetection = true;
                        terminalNumber = i;
                        staticItemList[i].OnOffBilboard = true;
                        CollisionItemType = StaticItemType.none;
                        return StaticItemType.none;
                    }
                    else
                    {
                        terminalNumber = i;
                        staticItemList[i].OnOffBilboard = false;
                        ConsoleDetection = false;
                    }
                }
                #endregion

                if (staticItemList[i].ColliderInternal == null)
                {
                    continue;
                }

                if (staticItemList[i].Type == StaticItemType.gate && item.Type == StaticItemType.samantha && !plot.Gate1Opened)
                {
                    if (staticItemList[i].ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB))
                        return staticItemList[i].Type;
                    staticItemList[i].DrawID = (item.ColliderExternal.AABB.Intersects(staticItemList[i].ColliderInternal.AABB));
                }

                if (staticItemList[i].Type == StaticItemType.oxygenGenerator && item.Type == StaticItemType.samantha)
                {
                    staticItemList[i].DrawID = (item.ColliderExternal.AABB.Intersects(staticItemList[i].ColliderInternal.AABB));
                }
                if(staticItemList[i].ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB))
                {
                    return staticItemList[i].Type;
                }
            }

            foreach (StaticItem npc in npcItem)
            {
                if (npc != item && npc.ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB)) 
                    return npc.Type;
                npc.DrawID = (samantha.ColliderExternal.AABB.Intersects(npc.ColliderInternal.AABB));
            }

            if (samantha != item && samantha.ColliderInternal.AABB.Intersects(item.ColliderInternal.AABB))
                return samantha.Type;

            return StaticItemType.none;
        }

        //Sprawdzenie czy zaszła jakakolwiek kolizja by się nie przenikać między sobą
        public void CheckCollision(StaticItem item, Vector3 move)
        {
            item.ColliderInternal.RecreateCage(move);
            item.ColliderExternal.RecreateCage(move);
            if (IsCollidedType(item) == StaticItemType.none)
            {
                item.Position += move;
                #region Obracanie kolidera
                if (item.moveColliderExternal.X + move.X < 20 && item.moveColliderExternal.X + move.X > -20 &&
                    item.moveColliderExternal.Y + move.Y < 20 && item.moveColliderExternal.Y + move.Y > -20)
                {
                    if (item.moveColliderExternal.Y > 0 && move.X > 0)
                        move.Y = -move.X;
                    if (item.moveColliderExternal.Y > 0 && move.X < 0)
                        move.Y = move.X;
                    if (item.moveColliderExternal.Y < 0 && move.X > 0)
                        move.Y = move.X;
                    if (item.moveColliderExternal.Y < 0 && move.X < 0)
                        move.Y = -move.X;
                    if (item.moveColliderExternal.X > 0 && move.Y > 0) 
                        move.X = -move.Y;
                    if (item.moveColliderExternal.X > 0 && move.Y < 0)
                        move.X = move.Y;
                    if (item.moveColliderExternal.X < 0 && move.Y > 0)
                        move.X = move.Y;
                    if (item.moveColliderExternal.X < 0 && move.Y < 0)
                        move.X = -move.Y;

                    item.moveColliderExternal += new Vector2(move.X, move.Y);
                    item.ColliderExternal.RecreateCage(move + new Vector3(move.X, move.Y, 0));
                }
                #endregion

                if (ConsoleDetection)
                {
                    staticItemList[terminalNumber].OnOffBilboard = false;
                    console.IsUsed = false;
                }
            }
            
            else
            {
                move = new Vector3(move.X * (-1), move.Y * (-1), move.Z * (-1));
                item.ColliderInternal.RecreateCage(move);
                item.ColliderExternal.RecreateCage(move);
                //playAudio();
            }

        }

        public bool CallTerminalAfterCollision(StaticItem item)
        {
            if (ConsoleDetection)
            {
                KeyboardState newState = Keyboard.GetState();
                if (newState.IsKeyDown(Keys.Tab) && oldstate.IsKeyUp(Keys.Tab))
                {
                    console.ResetConsole();
                    console.SetDefault();
                    console.IsUsed = !console.IsUsed;
                }
                oldstate = newState;
                if (console.IsUsed)
                {
                    staticItemList[terminalNumber].OnOffBilboard = false;
                    return true;
                }
                else
                {
                    staticItemList[terminalNumber].OnOffBilboard = true;
                    return false;
                }
                
            }
            return false;
        }
    }
}
