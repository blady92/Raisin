using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cyber.CItems.Notes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cyber.CItems
{
    class CommandNotes
    {
        private List<Command> normalCommands;
        private List<Command> defensiveCommands;
        private List<Command> attackCommands;

        private SpriteAnimationDynamic commandIconNormal;
        private SpriteAnimationDynamic commandIconDefense;
        private SpriteAnimationDynamic commandIconAttack;

        private KeyboardState newKeyState, oldKeyState;
        private Sprite dashboard;

        private bool normalLearndNew, defenseLearnNew, attackLearnNew;
        private float dashboardMax;
        private bool dashboardShown;

        enum TypeCall
        {
            none,
            normal,
            defense,
            attack
        }

        private TypeCall call;

        public void InitializeNotes()
        {
            normalCommands = new List<Command>();
            defensiveCommands = new List<Command>();
            attackCommands = new List<Command>();

            normalLearndNew = defenseLearnNew = attackLearnNew = false;

            commandIconNormal = new SpriteAnimationDynamic("Assets/2D/Commands/commandNormal", false);
            commandIconDefense = new SpriteAnimationDynamic("Assets/2D/Commands/commanDefense", false);
            commandIconAttack = new SpriteAnimationDynamic("Assets/2D/Commands/commandAttack", false);

            commandIconNormal.Loaded = commandIconDefense .Loaded = commandIconAttack.Loaded = false;
            dashboard = new Sprite(0,0);

            call = TypeCall.none;
        }

        public void LearnNewCommand(Command command)
        {
            if (command.CommandType == CommandType.normal) { normalCommands.Add(command); normalLearndNew = true; }
            if (command.CommandType == CommandType.defense) { defensiveCommands.Add(command); defenseLearnNew = true; }
            if (command.CommandType == CommandType.attack) { attackCommands.Add(command); attackLearnNew = true; }
        }

        public void DrawNote(SpriteBatch theSpriteBatch)
        {
            if (normalLearndNew) NormalIconAnimation();
            if (defenseLearnNew) DefenseIconAnimation();
            if (attackLearnNew) AttackIconAnimatino();

            dashboard.DrawByRectangle(theSpriteBatch);
            if (dashboardShown && (call == TypeCall.normal))
                foreach (Command command in normalCommands)
                    command.Draw();;
            
            if(dashboardShown && (call == TypeCall.defense))
                foreach (Command command in defensiveCommands)
                    command.Draw();

            if(dashboardShown && (call == TypeCall.attack))
                foreach (Command command in attackCommands)
                    command.Draw();
        }

        public void Update()
        {
            newKeyState = Keyboard.GetState();
            if (newKeyState.IsKeyDown(Keys.D1) && oldKeyState.IsKeyUp(Keys.D1))
            {
                ReopenDashboard();
                normalLearndNew = false;
                call = TypeCall.none;
                Debug.WriteLine("Normal Commands Pressed");
            }
            if (newKeyState.IsKeyDown(Keys.D2) && oldKeyState.IsKeyUp(Keys.D2))
            {
                ReopenDashboard();
                defenseLearnNew = false;
                call = TypeCall.none;
                Debug.WriteLine("Defense Commands Pressed");
            }
            if (newKeyState.IsKeyDown(Keys.D3) && oldKeyState.IsKeyUp(Keys.D3))
            {
                ReopenDashboard();
                attackLearnNew = false;
                call = TypeCall.none;
                Debug.WriteLine("Attack Commands Pressed");                
            }
        }

        #region Dashboard Animation
        public void ShowDashboard()
        {
            if (dashboard.X < dashboardMax)
            {
                dashboard.X += 0.5;
            }
            else
            {
                dashboardShown = true;
            }
        }

        public void HideDashboard()
        {
            if (dashboard.X > 0)
            {
                dashboard.X -= 0.5;
            }
            else
            {
                dashboardShown = false;
            }
        }

        public void ReopenDashboard()
        {
            if (dashboardShown)
                HideDashboard();
            else
                ShowDashboard();   
        }
        #endregion
        #region Animate Icons

        public void NormalIconAnimation()
        {
            commandIconNormal.AnimationThereAndBackAgain();
        }

        public void DefenseIconAnimation()
        {
            commandIconDefense.AnimationThereAndBackAgain();
        }

        public void AttackIconAnimatino()
        {
            commandIconAttack.AnimationThereAndBackAgain();
        }
        #endregion
    }
}
