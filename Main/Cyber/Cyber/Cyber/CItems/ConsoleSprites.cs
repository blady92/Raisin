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
using Cyber.Audio;
using Cyber.CGameStateEngine;

namespace Cyber.CItems
{
    class ConsoleSprites
    {
        public bool IsUsed { get; set; }
        public SpriteAnimationDynamic Console { get; set; }
        public Sprite ConsoleAdditional { get; set; }
        public Sprite ConsoleButton { get; set; }
        public string Text { get; set; }
        public string LatestStoreCommand { get; set; }
        public string PrintedText { get; set; }
        public SpriteFont font { get; set; }
        public SpriteFont ToProcessKey { get; set; }
        public bool tabToExit { get; set; }

        private List<Keys> allKeys;
        private List<Keys> possibleKeys;
        private KeyboardState newPressKey;
        private KeyboardState oldPressKey;
        private int lenght;
        private float textBox;

        public List<DisplayMessage> messages = new List<DisplayMessage>(); 
        private string oldText = "";
        int messageCharCounter = 0;
        float spaceFromEdge = 20;

        private Dictionary<string, GameConsoleCommand> commands = new Dictionary<string,GameConsoleCommand>();
        private GameStateMainGame game;
        private AudioController audioController;
        private AudioModel audioModel = new AudioModel("CyberBank");
        private bool textWritingPlayed = false;
        private Dictionary<string, CommandType> possibleCommands;

        //Kwestie fabularne
        public PlotTwistClass plotAction { get; set; }

        public ConsoleSprites(GameStateMainGame game, AudioController audioController)
        {
            this.game = game;
            this.audioController = audioController;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Console = new SpriteAnimationDynamic("Assets/2D/consoleAnimation", false);
            Console.LoadAnimationHover(theContentManager);
            Console.SpritePosition = new Vector2(0, 768 - Console.TextureList[0].Height);

            //font = theContentManager.Load<SpriteFont>("Assets/Fonts/courbd");
            font = theContentManager.Load<SpriteFont>("Assets/Fonts/ConsoleFont");

            audioController = new AudioController(audioModel);
            audioController.setAudio();

            textBox = 400;
            Text = "";
            lenght = Text.Length;

            SetupKeys();
            SetupGameConsole();
        }

        private void SetupGameConsole()
        {
            switchColor = false;
            possibleCommands = new Dictionary<string, CommandType>();
            PutCommand(new GameConsoleCommand(new SayHelloCommand()));
            //possibleCommands.Add("Hello");
            PutCommand(new GameConsoleCommand(new AudioCommand(game, audioController)));
            //possibleCommands.Add("Hello");
            PutCommand(new GameConsoleCommand(new OpenGateCommand(game)));
            possibleCommands.Add("OpenGate", CommandType.normal);
            PutCommand(new GameConsoleCommand(new AccessGeneratorCommand(game)));
            possibleCommands.Add("AccessGenerator", CommandType.normal);
            PutCommand(new GameConsoleCommand(new AllySleepCommand(game)));
            possibleCommands.Add("AllySleep", CommandType.defense);
            PutCommand(new GameConsoleCommand(new FreeCommand(game)));
            possibleCommands.Add("Free", CommandType.normal);
            PutCommand(new GameConsoleCommand(new GetTimeCommand(game)));
            possibleCommands.Add("GetTime", CommandType.normal);
            //PutCommand(new GameConsoleCommand(new DestroyEnemyCommand(game)));
            //possibleCommands.Add("DestroyEnemy", CommandType.attack);
            //PutCommand(new GameConsoleCommand(new ScanEnemies(game)));
            possibleCommands.Add("ScanEnemies", CommandType.defense);
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
                    messageCharCounter = 45;
                }
                else
                {
                    messageCharCounter = 0;
                }
                Color color = new Color(121, 122, 125);
                
                spriteBatch.Begin();
                
                messages.Add(new DisplayMessage(PrintedText, TimeSpan.FromSeconds(5.0), new Vector2(spaceFromEdge, Game1.maxHeight - 240 + messageCharCounter), color));
                messages.Add(new DisplayMessage(PrintedText, TimeSpan.FromSeconds(5.0), new Vector2(spaceFromEdge, Game1.maxHeight - 240 + messageCharCounter), color));
                //UX hint
                if (plotAction.action)
                {
                    spriteBatch.DrawString(font, "Write or TAB to close", new Vector2(275, Game1.maxHeight-100), color);
                }
                else
                {
                    spriteBatch.DrawString(font, "ENTER to next", new Vector2(335, Game1.maxHeight-100), color);
                }
                //User command
                spriteBatch.DrawString(font, ">_ " + Text, new Vector2(spaceFromEdge, Game1.maxHeight - 45), color);
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
                //Jeżeli jest akcja do wykonania
                if (plotAction.action)
                {
                    #region sprawdzenie, czy wpisane są ()

                    for (int i = 0; i < allKeys.Count; i++)
                    {
                        if (newPressKey.IsKeyDown(allKeys[i]) && oldPressKey.IsKeyUp(allKeys[i]) &&
                            possibleKeys.Contains(allKeys[i]))
                        {
                            if (Text.Length + 1 < 27)
                            {
                                if (allKeys.Contains(Keys.LeftShift) || allKeys.Contains(Keys.RightShift))
                                {
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

                        #endregion

                    #region usunięcie znaku

                        if (newPressKey.IsKeyDown(Keys.Back) && oldPressKey.IsKeyUp(Keys.Back))
                        {
                            if (Text.Length > lenght)
                                Text = Text.Remove(Text.Length - 1);
                        }

                        #endregion
                    }
                    if (newPressKey.IsKeyDown(Keys.Enter) && oldPressKey.IsKeyUp(Keys.Enter))
                    {
                        if (Text.Length > 0)
                        {
                            //kuba edit
                            string result = ProcessCommand(Text);
                            PrintedText = "";
                            messages.Clear();
                            PrintedText += AddTheoLine() + result;
                            PrintedText += "\n\n";
                            PrintedText += plotAction.getActualDialog();
                            PrintedText = parseText(PrintedText);
                            LatestStoreCommand = Text;
                            Text = "";
                        }
                    }
                }
                else
                {
                    if (newPressKey.IsKeyDown(Keys.Enter) && oldPressKey.IsKeyUp(Keys.Enter)) { 
                        //oldText = PrintedText;
                        PrintedText = "";
                        messages.Clear();
                        PrintedText = (plotAction.dialogNumber < 2)
                            ? parseText("\n" + plotAction.getActualDialog())
                            : parseText(plotAction.getActualDialog());
                        if (plotAction.BreakPoints.Contains(plotAction.dialogNumber))
                        {
                            plotAction.action = true;
                        }
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
        public void SetDefault()
        {
            messages.Clear();
            oldText = "";
            PrintedText = "";
            if(!plotAction.BreakPoints.Contains(plotAction.dialogNumber))
                plotAction.dialogNumber--;
            PrintedText = parseText(plotAction.getActualDialog());
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

        public bool switchColor { get; set; }
        public void DrawMessages(SpriteBatch spriteBatch)
        {
            if (messages.Count > 0 && messages[0].DrawnMessage.Length < PrintedText.Length)
            {
                //if (!textWritingPlayed)
                //{
                //    audioController.textWritingController("Play");
                //    textWritingPlayed = true;
                //}
                //else
                //{
                //    //audioController.textWritingController("Resume");
                //}
                //if(!textPlayed)
                //{
                //    audioController.menuHoverController("Play");
                //    textPlayed = true;
                ////}
                DisplayMessage dm = messages[0];
                DisplayMessage displayCode = messages[1];

                spriteBatch.DrawString(font, dm.DrawnMessage, dm.Position, dm.DrawColor);
                spriteBatch.DrawString(font, displayCode.DrawnMessage, displayCode.Position, displayCode.DrawColor);

                dm.DrawnMessage += dm.Message[dm.CurrentIndex].ToString();
                foreach (KeyValuePair<string, CommandType> possibleCommand in possibleCommands)
                {
                    KeyValuePair<string, CommandType> commandKeyValue = possibleCommand;
                    if (dm.DrawnMessage.Split(' ').Last() == commandKeyValue.Key || dm.DrawnMessage.Split(' ').Last() == "\n" + commandKeyValue.Key)
                    {
                        #region Define color to command
                        //if (commandKeyValue.Value == CommandType.normal) displayCode.DrawColor = new Color(0, 0, 0);
                        if (commandKeyValue.Value == CommandType.normal) displayCode.DrawColor = new Color(24, 212, 216);
                        else if (commandKeyValue.Value == CommandType.defense) displayCode.DrawColor = new Color(69, 13, 230);
                        else if (commandKeyValue.Value == CommandType.attack) displayCode.DrawColor = new Color(255, 0, 98);
                        #endregion

                        string command;
                        command = (dm.DrawnMessage.Split(' ').Last() == commandKeyValue.Key) ? commandKeyValue.Key : "\n" + commandKeyValue.Key;
                        displayCode.DrawnMessage =
                            displayCode.DrawnMessage.Remove(displayCode.DrawnMessage.Length - command.Length);
                        displayCode.DrawnMessage += " " +command;
                    }
                }
                if (dm.DrawnMessage[dm.CurrentIndex].ToString() == "\n" )
                {
                    displayCode.DrawnMessage += " \n";
                }
                else
                {
                    displayCode.DrawnMessage += " ";
                }

                if (dm.CurrentIndex != dm.Message.Length - 1)
                {
                    dm.CurrentIndex++;
                    messages[0] = dm;
                    messages[1] = displayCode;
                }
            }
            else
            {
                //audioController.textWritingController("Pause");
                spriteBatch.DrawString(font, messages[0].DrawnMessage, messages[0].Position, messages[0].DrawColor);
                spriteBatch.DrawString(font, messages[1].DrawnMessage, messages[1].Position, messages[1].DrawColor);
            }
        }

        private string ProcessCommand(string command)
        {
            string[] paramss = command.Split(' ');
            command = paramss[0];
            if (!commands.ContainsKey(command))
            {
                return "I know you're clever Sam, but it doesn't work";
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
