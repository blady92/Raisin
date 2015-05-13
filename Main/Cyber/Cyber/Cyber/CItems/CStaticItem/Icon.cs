using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CItems.CStaticItem
{
    class Icon
    {
        private int x;
        private int y;
        private Sprite action;
        private Sprite danger;
        private Sprite onWatch;
        private StaticIcon iconState;

        #region ACCESSORS
        public Sprite Action
        {
            get { return action; }
            set { action = value; }
        }

        public Sprite Danger
        {
            get { return danger; }
            set { danger = value; }
        }

        public Sprite OnWatch
        {
            get { return onWatch; }
            set { onWatch = value; }
        }

        public StaticIcon IconState
        {
            get { return iconState; }
            set { iconState = value; }
        }
        #endregion

        public Icon(int x, int y, StaticIcon startState)
        {
            this.x = x;
            this.y = y;
            this.iconState = StaticIcon.none;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            action = new Sprite(x, y);
            action.LoadContent(theContentManager, "Assets/2D/buttonE");


            //danger = new Sprite(x, y);
            //danger.LoadContent(theContentManager, "Assets/2D/buttonE");

            //onWatch = new Sprite(x, y);
            //onWatch.LoadContent(theContentManager, "Assets/2D/buttonE");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (iconState == StaticIcon.action)
            {
                action.DrawByRectangle(spriteBatch);
            }
            else if (iconState == StaticIcon.danger)
            {
                //Debug.WriteLine("Ikona zagrożenia");
            }
            else if (iconState == StaticIcon.onWatch)
            {
                //Debug.WriteLine("Ikona onWatch");
            }
            else if (iconState == StaticIcon.none)
            {
                //Debug.WriteLine("Brak akcji do wykonania");
            }
        }
    }
}
