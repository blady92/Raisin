using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cyber.CItems.Notes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cyber.Audio;
using Cyber.AudioEngine;

namespace Cyber.CItems
{
    public class CommandNotes
    {
        private ContentManager theContentManager;
        private List<Command> normalCommands;
        private List<Command> defensiveCommands;
        private List<Command> attackCommands;

        private SpriteAnimationDynamic commandIconNormal;
        private SpriteAnimationDynamic commandIconDefense;
        private SpriteAnimationDynamic commandIconAttack;

        private KeyboardState newKeyState, oldKeyState;
        private Sprite dashboard;
        
        public List<DisplayMessage> learnedCommandsNormal = new List<DisplayMessage>();
        public List<DisplayMessage> descriptionLearnedCommandsNormal = new List<DisplayMessage>();

        public List<DisplayMessage> learnedCommandsDefense = new List<DisplayMessage>();
        public List<DisplayMessage> descriptionLearnedCommandsDefense = new List<DisplayMessage>();

        public List<DisplayMessage> learnedCommandsAttack = new List<DisplayMessage>();
        public List<DisplayMessage> descriptionLearnedCommandsAttack = new List<DisplayMessage>(); 

        private bool normalLearndNew, defenseLearnNew, attackLearnNew;
        private bool dashboardShown; //że narysowany już
        private bool dashboardHidden;
        private bool dashboardReload; //że animacja już zapełniona
        private SpriteFont header;
        private int moveX;
        public bool isNoteUsed { get; set; }

        private AudioModel audioModel = new AudioModel("CyberBank");
        private AudioController audioController;
        private bool soundOpenPlayed = false;
        private bool soundClosePlayed = false;

        public enum TypeCall
        {
            none,
            normal,
            defense,
            attack
        }

        public TypeCall call { get; set; }

        public void InitializeNotes(ContentManager theContentManager)
        {
            this.theContentManager = theContentManager;

            normalCommands = new List<Command>();
            defensiveCommands = new List<Command>();
            attackCommands = new List<Command>();

            normalLearndNew = defenseLearnNew = attackLearnNew = false;

            dashboard = new Sprite(Game1.maxWidth, 100);
            //Light version
            dashboard.LoadContent(theContentManager, "Assets/2D/Commands/dashboardLight");
            commandIconNormal = new SpriteAnimationDynamic("Assets/2D/Commands/lightBasic", false);
            commandIconDefense = new SpriteAnimationDynamic("Assets/2D/Commands/lightDefense", false);
            commandIconAttack = new SpriteAnimationDynamic("Assets/2D/Commands/lightAttack", false);

            //Dark version
            //dashboard.LoadContent(theContentManager, "Assets/2D/Commands/dashboardDark");
            //commandIconNormal = new SpriteAnimationDynamic("Assets/2D/Commands/darkBasic", false);
            //commandIconDefense = new SpriteAnimationDynamic("Assets/2D/Commands/darkDefense", false);
            //commandIconAttack = new SpriteAnimationDynamic("Assets/2D/Commands/darkAttack", false);

            commandIconNormal.LoadAnimationHover(theContentManager);
            commandIconDefense.LoadAnimationHover(theContentManager);
            commandIconAttack.LoadAnimationHover(theContentManager);

            commandIconNormal.SpritePosition = new Vector2(Game1.maxWidth - commandIconNormal.TextureList[0].Width, 100);
            commandIconDefense.SpritePosition = new Vector2(Game1.maxWidth - commandIconDefense.TextureList[0].Width, 148);
            commandIconAttack.SpritePosition = new Vector2(Game1.maxWidth - commandIconAttack.TextureList[0].Width, 196);

            commandIconNormal.Loaded = commandIconDefense.Loaded = commandIconAttack.Loaded = false;
            
            Debug.WriteLine("Normal loaded: " + commandIconNormal.Loaded);
            Debug.WriteLine("Defense loaded:" + commandIconDefense.Loaded);
            Debug.WriteLine("Attack loaded:" + commandIconAttack.Loaded);

            call = TypeCall.none;
            dashboardShown = false;
            dashboardHidden = true;
            dashboardReload = false;
            moveX = 10;

            audioController = new AudioController(audioModel);
            audioController.setAudio();
        }

        public void LearnNewCommand(Command command)
        {
            audioController.newCommandController("Play");
         
            if (command.CommandType == CommandType.normal)
            {
                command.LoadFontTypes(theContentManager);
                command.CommandDescription = command.ParseText(command.CommandDescription, dashboard.SpriteAccessor.Width - 50);
                normalCommands.Add(command);
                normalLearndNew = true;
            }
            if (command.CommandType == CommandType.defense)
            {
                command.LoadFontTypes(theContentManager);
                command.CommandDescription = command.ParseText(command.CommandDescription, dashboard.SpriteAccessor.Width - 50);
                defensiveCommands.Add(command); 
                defenseLearnNew = true;
            }
            if (command.CommandType == CommandType.attack)
            {
                command.LoadFontTypes(theContentManager);
                command.CommandDescription = command.ParseText(command.CommandDescription, dashboard.SpriteAccessor.Width - 50);
                attackCommands.Add(command); 
                attackLearnNew = true;
            }

            audioController.newCommandController("Pause");
        }

        public void DrawNote(SpriteBatch theSpriteBatch)
        {
            dashboard.DrawByRectangle(theSpriteBatch);
            commandIconAttack.DrawAnimation(theSpriteBatch);
            commandIconNormal.DrawAnimation(theSpriteBatch);
            commandIconDefense.DrawAnimation(theSpriteBatch);
            #region Draw Commands and descriptions
            if (dashboardShown && !dashboardHidden)
            {
                if (call == TypeCall.normal)
                {
                    int commands = 0;
                    int entersInDescription = 0;
                    commandIconNormal.currentFrame = 0;
                    for (int i = 0; i < normalCommands.Count; i++)
                    {
                        normalCommands[i].Draw(theSpriteBatch, dashboard.SpriteAccessor.Width, commands, 
                            entersInDescription);
                        commands++;
                        entersInDescription += normalCommands[i].CommandDescription.Split('\n').Length;
                    }
                }
                if (call == TypeCall.defense)
                {
                    int commands = 0;
                    int entersInDescription = 0;
                    commandIconDefense.currentFrame = 0;
                    for (int i = 0; i < defensiveCommands.Count; i++)
                    {
                        defensiveCommands[i].Draw(theSpriteBatch, dashboard.SpriteAccessor.Width, commands,
                            entersInDescription);
                        commands++;
                        entersInDescription += normalCommands[i].CommandDescription.Split('\n').Length;
                    }
                }
                if (call == TypeCall.attack)
                {
                    int commands = 0;
                    int entersInDescription = 0;
                    commandIconAttack.currentFrame = 0;
                    for (int i = 0; i < attackCommands.Count; i++)
                    {
                        attackCommands[i].Draw(theSpriteBatch, dashboard.SpriteAccessor.Width, commands,
                            entersInDescription);
                        commands++;
                        entersInDescription += attackCommands[i].CommandDescription.Split('\n').Length;
                    }
                }
            }
            #endregion
        }

        public void Update(bool blocked)
        {            
            newKeyState = Keyboard.GetState();
            if (isNoteUsed)
            {
                #region Call Dashboard State

                if (newKeyState.IsKeyDown(Keys.D4) && oldKeyState.IsKeyUp(Keys.D4))
                {
                    attackLearnNew = true;
                }
                if (newKeyState.IsKeyDown(Keys.D1) && oldKeyState.IsKeyUp(Keys.D1))
                {
                    
                    if (call == TypeCall.normal)
                    {
                        dashboardReload = false;
                        call = TypeCall.none;
                    }
                    else
                    {
                        dashboardReload = true;
                        call = TypeCall.normal;
                    }
                    commandIconNormal.AnimationThereAndBackAgain(true);
                    normalLearndNew = false;
                }


                if (newKeyState.IsKeyDown(Keys.D2) && oldKeyState.IsKeyUp(Keys.D2))
                {
                    
                    if (call == TypeCall.defense)
                    {
                        dashboardReload = false;
                        call = TypeCall.none;
                    }
                    else
                    {
                        dashboardReload = true;
                        call = TypeCall.defense;
                    }
                    commandIconDefense.AnimationThereAndBackAgain(true);
                    defenseLearnNew = false;
                }


                if (newKeyState.IsKeyDown(Keys.D3) && oldKeyState.IsKeyUp(Keys.D3))
                {
                    
                    if (call == TypeCall.attack)
                    {
                        dashboardReload = false;
                        call = TypeCall.none;
                       
                    }
                    else
                    {
                        dashboardReload = true;
                        call = TypeCall.attack;                      
                    }
                    commandIconAttack.AnimationThereAndBackAgain(true);
                    attackLearnNew = false;
                }

                #endregion
            }
            oldKeyState = newKeyState;

            if (normalLearndNew) commandIconNormal.AnimationThereAndBackAgain  (false);
            if (defenseLearnNew) commandIconDefense.AnimationThereAndBackAgain (false);
            if (attackLearnNew)  commandIconAttack.AnimationThereAndBackAgain  (false);

            if(call != TypeCall.none)
            {
                if (dashboardShown)
                    dashboardHidden = false;
                ShowDashboard();
            }
            else 
            {
                dashboardHidden = true;
                HideDashboard();
            }
        }
        #region  Build PrintedText

        #endregion
        #region Dashboard Animation
        public void ShowDashboard()
        {
            if (dashboard.X >= Game1.maxWidth - dashboard.SpriteAccessor.Width + moveX)
            {
                dashboard.X -= moveX;
                Vector2 move = new Vector2(-moveX, 0);
                MoveIcon(commandIconAttack, move);
                MoveIcon(commandIconDefense, move);
                MoveIcon(commandIconNormal, move);
                if (!soundOpenPlayed)
                {
                    audioController.notepadSoundEffect("Play");
                    soundOpenPlayed = true;
                }
            }
            else
            {
                dashboardShown = true;
                isNoteUsed = true;
                soundOpenPlayed = false;
            }
        }

        public void HideDashboard()
        {
            if (dashboard.X < Game1.maxWidth)
            {
                dashboard.X += moveX;
                Vector2 move = new Vector2(moveX, 0);
                MoveIcon(commandIconAttack, move);
                MoveIcon(commandIconDefense, move);
                MoveIcon(commandIconNormal, move);
                if(!soundClosePlayed)
                {
                    audioController.notepadSoundEffect("PlayInvert");
                    soundClosePlayed = true;
                }
               
            }
            else
            {
                dashboardShown = false;
                isNoteUsed = false;
                soundClosePlayed = false;
            }
        }
        #endregion
        #region Animate Icons

        public void MoveIcon(SpriteAnimationDynamic sprite, Vector2 vector)
        {
            sprite.SpritePosition += vector;
        }
        #endregion
    }
}
