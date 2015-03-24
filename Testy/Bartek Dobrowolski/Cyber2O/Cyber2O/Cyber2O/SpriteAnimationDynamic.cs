using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber2O
{
    public class SpriteAnimationDynamic
    {
        //Położenie obrazka
        private Vector2 spritePosition;
        
        //Obecna klatka standardowej animacji
        private int currentFrame;
        //Łączna ilość klatek standardowej animacji
        private int totalFrames;

        //Obecna klatka animacji po kliknięciu
        private int clickCurrentFrame;
        //Łączna ilość klatek animacji po kliknięciu
        private int clickTotalFrames;

        //Ścieżka do folderu z paczką do animacji na hover
        private string pathToDirectory;
        //Ścieżka do folderu z paczką do animacji po kliknięciu
        private string pathToClickAnimation;
        
        //Status czy został wciśnięty czy nie
        private bool clicked;
        //Status oznaczający, czy dany element jest klikalny
        private bool setToClick;

        public Texture2D[] TextureList { get; set; }
        public Texture2D[] ClickTextureList { get; set; }
        public int CurrentFrameAccessor
        {
            set { this.currentFrame = value;  }
            get { return this.currentFrame;  }
        }
        public Vector2 SpritePosition
        {
            get { return spritePosition; }
            set { spritePosition = value; }
        }

        public bool Clicked
        {
            get { return clicked;  }
            set { clicked = value; }
        }
        
        public SpriteAnimationDynamic(string pathToDirectory, bool click)
        {
            this.pathToDirectory = pathToDirectory; //.Replace(@"\", "/");
            this.TextureList = new Texture2D[CountFrames(pathToDirectory)];
            if (click)
            {
                pathToClickAnimation = pathToDirectory + "/clickFrames/";
                this.ClickTextureList = new Texture2D[CountFrames(pathToClickAnimation)];
                this.clickCurrentFrame = 0;
                setToClick = click;
            }
            else 
            { 
                this.setToClick = false;
            }
            this.currentFrame = 0;
            this.clicked = false;
        }
        public SpriteAnimationDynamic(string pathToDirectory, Vector2 position)
        {
            this.pathToDirectory = pathToDirectory; //.Replace(@"\", "/");
            this.spritePosition = position;
            this.TextureList = new Texture2D[CountFrames(pathToDirectory)];
            this.currentFrame = 0;
        }

        public SpriteAnimationDynamic(string pathToDirectory, int frames, Vector2 position)
        {
            this.pathToDirectory = pathToDirectory;
            this.spritePosition = position;
            this.TextureList = new Texture2D[frames];
        }

        //
        public void LoadAnimation(ContentManager theContentManager, int frames, Texture2D[] list, string path)
        {
            string[] sequences = Directory.GetFiles(@"..//..//..//..//Cyber2OContent/" + path);
            for (int i = 0; i < sequences.Length; i++)
            {
                string s = sequences[i];
                s = s.Remove(s.Length - 4);
                s = s.Replace(@"\", "/");
                sequences[i] = s.Substring(s.IndexOf("Assets"));
            }
            for (int i = 0; i < list.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(i+". load");
                list[i] = theContentManager.Load<Texture2D>(""+sequences[i]);
            }
        }

        //Ładowanie klatek animacji po najechaniu
        //oraz ustawienie ich maksymalnej ilości
        public void LoadAnimationHover(ContentManager theContentManager)
        {
            LoadAnimation(theContentManager, CountFrames(pathToDirectory), this.TextureList, pathToDirectory);
            totalFrames = TextureList.Length;
        }

        //Ładowanie klatek animacji po kliknięciu
        //oraz ustawienie ich maksymalnej ilości
        public void LoadClickAnimation(ContentManager theContentManager)
        {
            LoadAnimation(theContentManager, CountFrames(pathToClickAnimation), this.ClickTextureList, pathToClickAnimation);
            clickTotalFrames = ClickTextureList.Length;    
        }
        
        //Zliczanie ilości klatek w folderze
        public int CountFrames(string path)
        {
            int sequences = Directory.GetFiles(@"..//..//..//..//Cyber2OContent/"+path).Length;
            return sequences;
        }

        //Rysowanie
        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(
                (int)this.SpritePosition.X, 
                (int)this.SpritePosition.Y,
                TextureList[this.currentFrame].Width,
                TextureList[this.currentFrame].Height
            );

            spriteBatch.Begin();
            //Aby się wykonały animacjie sprite musi mieć status klikniętego i być "klikalnym" (ustawiane w konstruktorze)
            //PAMIĘTAJ O DODANIU KLATEK DO ODPOWIEDNICH FOLDERÓW!
            if (clicked && setToClick)
            {
                System.Diagnostics.Debug.WriteLine("Klasa Sprite: kliknięte i wciśniete");
                spriteBatch.Draw(ClickTextureList[clickCurrentFrame], sourceRectangle, Color.White);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Klasa Sprite: Nie kliknięte i nie wciśnięte");
                spriteBatch.Draw(TextureList[currentFrame], sourceRectangle, Color.White);
            }
            spriteBatch.End();
        }

        //Update w sposób ciągły i reset do zera
        public void Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
            }
        }

        //Update do ostatniej klatki i stop
        public void UpdateAnimation()
        {
            if (currentFrame < totalFrames-1)
            {
                currentFrame++;
            }
        }

        //Przeciwieństwo do UpdateAnimation
        public void UpdateReverse()
        {
            if (currentFrame > 0)
            {
                currentFrame--;
            }
        }

        //Reset klatki dla hovera
        public void UpdateResetFrame()
        {
            currentFrame = 0;
        }

        //Ustawienie czy ten sprite jest wciśniety
        public void UpdateClickAnimation(bool b)
        {
            clicked = b;
        }

        //Update klatek dla klikniętego sprite'a
        public void UpdateClickFrame()
        {
            if (clickCurrentFrame < clickTotalFrames - 1)
            {
                System.Diagnostics.Debug.WriteLine("Click frame: "+clickCurrentFrame);
                clickCurrentFrame++;
            }
        }

        //Przeciwieństwo UpdateClickFrame
        public void UpdateClickReverse()
        {
            if (clickCurrentFrame > 0)
            {
                clickCurrentFrame--;
            }
        }
        //Reset klatek dla klikniętego sprite'a
        public void ResetClickFrame()
        {
            clickCurrentFrame = 0;
        }

        //Zwraca pole prostokąta
        public Rectangle GetFrameRectangle()
        {
            return new Rectangle((int)spritePosition.X, (int)spritePosition.Y, this.TextureList[currentFrame].Width, this.TextureList[currentFrame].Height);
        }
    }
}
