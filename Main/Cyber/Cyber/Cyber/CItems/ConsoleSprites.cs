using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Cyber.CConsoleEngine;
using Cyber.AudioEngine;

namespace Cyber.CItems
{
    class ConsoleSprites
    {
        /* Uwaga jedna, tutaj się używa tylko czciontów Monospaced Font (bo wiesz, kombo czionka + font = czciont)
         * Inaczej na długość będzie serio kiepsko, a tak to...
         * Pixelgasm and perfectionist paradise :v
         */ 
        public bool IsUsed { get; set; }
        public SpriteAnimationDynamic Console { get; set; }
        public Sprite ConsoleAdditional { get; set; }
        public Sprite ConsoleButton { get; set; }
        public string Text { get; set; }
        public string LatestStoreCommand { get; set; }
        public string PrintedText { get; set; }
        public SpriteFont font { get; set; }
        private List<Keys> allKeys;
        private List<Keys> possibleKeys;
        private KeyboardState newPressKey;
        private KeyboardState oldPressKey;
        private int lenght;
        private float textBox;

        List<DisplayMessage> messages = new List<DisplayMessage>();
        private string oldText = "";
        int messageCharCounter = 0;
        float spaceFromEdge = 20;

        private Dictionary<string, GameConsoleCommand> commands = new Dictionary<string,GameConsoleCommand>();
        private Game game;
        private AudioController audioController;

        public ConsoleSprites(Game game, AudioController audioController)
        {
            this.game = game;
            this.audioController = audioController;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Console = new SpriteAnimationDynamic("Assets/2D/consoleAnimation", false); //Ustawienie byle jak
            Console.LoadAnimationHover(theContentManager);
            Console.SpritePosition = new Vector2(0, 768 - Console.TextureList[0].Height);
            font = theContentManager.Load<SpriteFont>("Assets/Fonts/ConsoleFont");

            textBox = 270;
            Text = "";
            PrintedText =
    AddTheoLine() + "Hello Samantho!";
            PrintedText = parseText(PrintedText);
            lenght = Text.Length;
            SetupKeys();
            SetupGameConsole();

            //consoleAdditional = new Sprite(0,0);
            //consoleAdditional.LoadContent(theContentManager, "Assets/2D/consoleAdditional");
            //pokazanie
            //consoleAdditional.Position = new Vector2(console.SpriteAccessor.Width, console.Position.Y);
        }

        private void SetupGameConsole()
        {
            PutCommand(new GameConsoleCommand(new SayHelloCommand()));
            PutCommand(new GameConsoleCommand(new AudioCommand(game, audioController)));
        }
        private void PutCommand(GameConsoleCommand gameConsoleCommand)
        {
            commands.Add(gameConsoleCommand.Name, gameConsoleCommand);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           Console.DrawAnimation(spriteBatch);
            if (Console.LoadingFinished())
            {
                if (oldText.Length > 130)
                {
                    messageCharCounter = 65;
                }
                else if(oldText.Length != 0)
                {
                    messageCharCounter = 25;
                }
                else
                {
                    messageCharCounter = 0;
                }
                //Debug.WriteLine("oT L: " + oldText.Length);
                messages.Add(new DisplayMessage(PrintedText, TimeSpan.FromSeconds(5.0), new Vector2(spaceFromEdge, Game1.maxHeight - 180 + messageCharCounter), new Color(121, 122, 125)));
                //od tego momentu można zacząć pisać tekst
                spriteBatch.Begin();
                spriteBatch.DrawString(font, oldText, new Vector2(spaceFromEdge, Game1.maxHeight-180), new Color(121,122,125));
                spriteBatch.DrawString(font, ">_ "+Text, new Vector2(spaceFromEdge, Game1.maxHeight-29), new Color(121,122,125));
                DrawMessages(spriteBatch);
                spriteBatch.End();
            }
        }

        public void Update()
        {
            if (IsUsed){
                Console.UpdateAnimation();
                allKeys = new List<Keys>(Keyboard.GetState().GetPressedKeys().ToArray());
                newPressKey = Keyboard.GetState();
                for (int i = 0; i < allKeys.Count; i++)
                {
                    if (newPressKey.IsKeyDown(allKeys[i]) && oldPressKey.IsKeyUp(allKeys[i]) && possibleKeys.Contains(allKeys[i]))
                    {
                        if (Text.Length+1 < 27) { 
                            if (allKeys.Contains(Keys.LeftShift) || allKeys.Contains(Keys.RightShift)){
                                if (allKeys.Contains(Keys.D9))
                                    Text += "(";
                                else if (allKeys.Contains(Keys.D0))
                                    Text += ")";
                                else
                                    Text += ParseKey((allKeys[i]));
                            }
                            else
                                Text += ParseKey(allKeys[i]).ToLower();
                        }
                    }
                }
                if (newPressKey.IsKeyDown(Keys.Back) && oldPressKey.IsKeyUp(Keys.Back))
                {
                    if (Text.Length > lenght)
                        Text = Text.Remove(Text.Length - 1);
                }
                else if (newPressKey.IsKeyDown(Keys.Enter) && oldPressKey.IsKeyUp(Keys.Enter))
                {
                    if (Text.Length > 0) {
                        //kuba edit
                        string result = ProcessCommand(Text);

                        oldText = PrintedText;
                        PrintedText = "";
                        messages.Clear();
                        PrintedText += AddSamanthaLine() + Text;
                        PrintedText += AddTheoLine() + result;
                        PrintedText = parseText(PrintedText);
                        LatestStoreCommand = Text;
                        
                        Text = "";
                    }
                }
                oldPressKey = newPressKey;
                
            }
            else 
                Console.UpdateReverse();
        }

        public string AddSamanthaLine()
        {
            return "\nSamantha:  ";
        }
        public string AddTheoLine()
        {
            return "\nTheo:  ";
        }

        public void ResetConsole()
        {
            Text = "";
        }

        public void SetupKeys()
        {
            possibleKeys = new List<Keys>();
            possibleKeys.Add(Keys.Q);
            possibleKeys.Add(Keys.W);
            possibleKeys.Add(Keys.E);
            possibleKeys.Add(Keys.R);
            possibleKeys.Add(Keys.T);
            possibleKeys.Add(Keys.Y);
            possibleKeys.Add(Keys.U);
            possibleKeys.Add(Keys.I);
            possibleKeys.Add(Keys.O);
            possibleKeys.Add(Keys.P);
            possibleKeys.Add(Keys.A);
            possibleKeys.Add(Keys.S);
            possibleKeys.Add(Keys.D);
            possibleKeys.Add(Keys.F);
            possibleKeys.Add(Keys.G);
            possibleKeys.Add(Keys.H);
            possibleKeys.Add(Keys.J);
            possibleKeys.Add(Keys.K);
            possibleKeys.Add(Keys.L);
            possibleKeys.Add(Keys.Z);
            possibleKeys.Add(Keys.X);
            possibleKeys.Add(Keys.C);
            possibleKeys.Add(Keys.V);
            possibleKeys.Add(Keys.B);
            possibleKeys.Add(Keys.N);
            possibleKeys.Add(Keys.M);
            possibleKeys.Add(Keys.I);
            
            possibleKeys.Add(Keys.OemSemicolon);
            possibleKeys.Add(Keys.OemPeriod);
            possibleKeys.Add(Keys.LeftShift);
            possibleKeys.Add(Keys.Space);
            
            possibleKeys.Add(Keys.D0);
            possibleKeys.Add(Keys.D1);
            possibleKeys.Add(Keys.D2);
            possibleKeys.Add(Keys.D3);
            possibleKeys.Add(Keys.D4);
            possibleKeys.Add(Keys.D5);
            possibleKeys.Add(Keys.D6);
            possibleKeys.Add(Keys.D7);
            possibleKeys.Add(Keys.D8);
            possibleKeys.Add(Keys.D9);

            possibleKeys.Add(Keys.NumPad0);
            possibleKeys.Add(Keys.NumPad1);
            possibleKeys.Add(Keys.NumPad2);
            possibleKeys.Add(Keys.NumPad3);
            possibleKeys.Add(Keys.NumPad4);
            possibleKeys.Add(Keys.NumPad5);
            possibleKeys.Add(Keys.NumPad6);
            possibleKeys.Add(Keys.NumPad7);
            possibleKeys.Add(Keys.NumPad8);
            possibleKeys.Add(Keys.NumPad9);
        }

        public string ParseKey(Keys k)
        {
            switch (k)
            {
                case Keys.NumPad0: case Keys.D0: return "0"; 
                case Keys.NumPad1: case Keys.D1: return "1"; 
                case Keys.NumPad2: case Keys.D2: return "2"; 
                case Keys.NumPad3: case Keys.D3: return "3"; 
                case Keys.NumPad4: case Keys.D4: return "4"; 
                case Keys.NumPad5: case Keys.D5: return "5"; 
                case Keys.NumPad6: case Keys.D6: return "6"; 
                case Keys.NumPad7: case Keys.D7: return "7"; 
                case Keys.NumPad8: case Keys.D8: return "8"; 
                case Keys.NumPad9: case Keys.D9: return "9"; 

                case Keys.OemPeriod: return ".";
                case Keys.LeftShift: return "";
                case Keys.OemSemicolon: return ";";
                case Keys.Space: return " ";
            }
            return k.ToString();
        }

        public void HideConsole()
        {
            Console.UpdateReverse();
        }

        public void ShowConsole()
        {
            Console.UpdateTillEnd();
        }

        private String parseText(String text)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');

            foreach (String word in wordArray)
            {
                if (font.MeasureString(line + word).Length() > textBox)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }


        //A po chuj to ja nie wie :v
        public void Action()
        {
            
        }


        // Kuba edit
        public void UpdateMessages(GameTime gameTime)
        {
            if(messages.Count > 0)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    DisplayMessage dm = messages[i];
                    dm.DisplayTime -= gameTime.ElapsedGameTime;
                    if(dm.DisplayTime <= TimeSpan.Zero)
                    {
                        messages.RemoveAt(i);
                    }
                    else
                    {
                        messages[i] = dm;
                    }
                }
            }
        }

        public void DrawMessages(SpriteBatch spriteBatch)
        {
            if(messages.Count > 0)
            {
                for(int i = 0; i < messages.Count; i++)
                {
                    DisplayMessage dm = messages[i];
                    dm.DrawnMessage += dm.Message[dm.CurrentIndex].ToString();
                    spriteBatch.DrawString(font, dm.DrawnMessage, dm.Position, dm.DrawColor);
                    if(dm.CurrentIndex != dm.Message.Length - 1)
                    {
                        dm.CurrentIndex++;
                        messages[i] = dm;
                    }
                }
            }
        }

        private string ProcessCommand(string command)
        {
            string[] paramss = command.Split(' ');
            command = paramss[0];
            if (!commands.ContainsKey(command))
            {
                return "Polecenia nie znaleziono";
            }
            if (paramss.Length == 1)
            {
                return commands[command].Execute(null);
            }
            string[] args = new string[paramss.Length - 1];
            Array.Copy(paramss, 1, args, 0, paramss.Length - 1);
            return commands[command].Execute(args);
        }
    }
}
