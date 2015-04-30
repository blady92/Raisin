using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber
{
    class Sprite
    {
        //Położenie obrazka na podstawie jego wektora, tutaj zawsze od 0,0 aż do wymiarów obrazka
        public Vector2 Position = new Vector2(0,0);
        //Pole na załadowanie tektury
        private Texture2D sprite;
        //Koorynanty
        private int x;
        private int y;

        public Sprite(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2 PositionAccessor
        {
            get { return Position; }
            set { Position = value; }
        }

        public Texture2D SpriteAccessor
        {
            get { return sprite; }
            set { sprite = value; }
        }

        //Ładowanie tekstur do sprite'a
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            Debug.WriteLine(theAssetName);
            sprite = theContentManager.Load<Texture2D>(theAssetName);
            Debug.WriteLine("Loaded");
        }

        //Rysowanie statyczne - najczęściej wykorzystywane do nieruchomych elementów UI
        //Na podstawie punktu startowego aż do maksymalnych wymiarów obrazka
        public void DrawByRectangle(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, new Rectangle(x, y, sprite.Width, sprite.Height), Color.White);
            spriteBatch.End();
        }

        //Rysowanie na podstawie koordynatów myszy i wymiarów tekstury przypisanego do tego sprite'a
        public void DrawByVector(SpriteBatch spriteBatch, MouseState mouse)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, new Rectangle(mouse.X, mouse.Y, sprite.Width, sprite.Height), Color.White);
            spriteBatch.End();
        }

        //Rysowanie na podstawie dynamicznych koordynatów zmienianych przy pomocy accessorów, ale niezależnych od myszy etc
        //Przewidywane zastosowania to - rysowanie elementów 2D na podstawie zmienianego ekranu.
        public void DrawByVector(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, new Rectangle((int)Position.X, (int)Position.Y, sprite.Width, sprite.Height), Color.White);
            spriteBatch.End();
        }

        //Zwraca pole prostokąta
        public Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, sprite.Width, sprite.Height);
        }
    }
}
