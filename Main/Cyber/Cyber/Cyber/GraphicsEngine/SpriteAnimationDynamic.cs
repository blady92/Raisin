using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NaturalSorting;
using Cyber.AudioEngine;

namespace Cyber
{
    public class SpriteAnimationDynamic
    {
        //Położenie obrazka
        private Vector2 spritePosition;
        
        //Obecna klatka standardowej animacji
        public int currentFrame { get; set; }
        //Łączna ilość klatek standardowej animacji
        public int totalFrames { get; set; }

        //Obecna klatka animacji po kliknięciu
        private int clickCurrentFrame;
        //Łączna ilość klatek animacji po kliknięciu
        public int clickTotalFrames { get; set; }

        //Ścieżka do folderu z paczką do animacji na hover
        private string pathToDirectory;
        //Ścieżka do folderu z paczką do animacji po kliknięciu
        private string pathToClickAnimation;
        
        //Status czy został wciśnięty czy nie
        private bool clicked;
        //Status oznaczający, czy dany element jest klikalny
        private bool setToClick;

        //Status oznaczający, że ładowanie zakończono
        public bool Loaded { get; set; }

        public Texture2D[] TextureList { get; set; }
        public Texture2D[] ClickTextureList { get; set; }
        public int ClickCurrentFrameAccessor
        {
            set { this.clickCurrentFrame = value; }
            get { return this.clickCurrentFrame; }
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

        public string SpriteName { get; set;  }

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

        //Metoda pozwalająca ładować klatki dla tego modelu
        public void LoadAnimation(ContentManager theContentManager, int frames, Texture2D[] list, string path)
        {
            //Ty kurwa, próbuję załadować plik, który nie istnieje TUTAJ, ale jest w folderze na dysku...
            //SPAAAAAććććć ;____;
            //Chcę do fajnego cycka się przytulić ;_;
            /// ....
            string[] sequences = Directory.GetFiles(@"..//..//..//..//CyberContent/" + path);
            Array.Sort(sequences, new AlphanumComparatorFast());
            for (int i = 0; i < sequences.Length; i++)
            {
                string s = sequences[i];
                s = s.Remove(s.Length - 4);
                s = s.Replace(@"\", "/");
                sequences[i] = s.Substring(s.IndexOf("Assets"));
            }
            for (int i = 0; i < list.Length; i++)
            {
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
            int sequences = Directory.GetFiles(@"..//..//..//..//CyberContent/"+path).Length;
            return sequences;
        }

        //Rysowanie animacji
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
                spriteBatch.Draw(ClickTextureList[clickCurrentFrame], sourceRectangle, Color.White);
            }
            else
            {
                spriteBatch.Draw(TextureList[currentFrame], sourceRectangle, Color.White);
            }
            spriteBatch.End();
        }

        public bool LoadingFinished()
        {
            if (currentFrame == totalFrames-1)
                return true;
            return false;
        }
        //Update w sposób ciągły i reset do zera
        public void Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames-1)
            {
                currentFrame = 0;
            }
        }

        //Update do ostatniej klatki i stop pojedynczo
        public void UpdateAnimation()
        {
            if (currentFrame < totalFrames-1)
            {
                currentFrame++;
            }
        }

        //Update do samego końca, bez zatrzymywania się
        public void UpdateTillEnd()
        {
            do
            {
                currentFrame++;
            } while (currentFrame > totalFrames - 1);
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

        public void AnimationThereAndBackAgain(bool stop)
        {
            if (!stop)
                if (!Loaded)
                {
                    UpdateAnimation();
                    if (currentFrame == totalFrames - 1)
                        Loaded = !Loaded;
                }
                else
                {
                    UpdateReverse();
                    if (currentFrame == 0)
                        Loaded = !Loaded;
                }
            else { 
                currentFrame = 0;
            }
        }
        //Zwraca pole prostokąta
        public Rectangle GetFrameRectangle()
        {
            return new Rectangle((int)spritePosition.X, (int)spritePosition.Y, this.TextureList[currentFrame].Width, this.TextureList[currentFrame].Height);
        }
    }
}
