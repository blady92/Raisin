using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyber.CGameStateEngine;
using Cyber.CItems.CStaticItem;
using Cyber.CLogicEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyber.CLevel
{
    public class Level2 : LevelStage
    {
        private StaticItem gate;
        public Level2(GameStateMainGame game)
            : base(game)
        {
            level = Level.level2;
        }
        public override void ParseStage()
        {
            Stage = StageParser.ParseBitmap("../../../CStageParsing/stage4.bmp");
        }

        protected override void LoadSceneEscape(GraphicsDevice device)
        {
            escapeemitter = new ParticleEmitter();
            escapeemitter.LoadContent(device, TheContentManager, "Assets/2D/blueGlow", 40, 70, 70, 100, new Vector3(-5, 270, 60), 1, 1);

            escapeCollider = new StaticItem("Assets/3D/escapeBoxFBX");
            escapeCollider.LoadItem(TheContentManager);
            escapeCollider.Position = new Vector3(0, 0, 0);
            escapeCollider.FixColliderInternal(new Vector3(1, 1, 1), new Vector3(-40, 280, 40));
            escapeCollider.Type = StaticItemType.teleporter;
            escapeCollider.bilboards = new BillboardSystem(device, TheContentManager,
                TheContentManager.Load<Texture2D>("Assets/2D/Bilboard/TerminalBack"),
                new Vector2(120), new Vector3(0, 0, 0)
            );

            escapeCollider.OnOffBilboard = false;
            escapeCollider.BilboardHeight = new Vector3(-60, 280, 180);

            generatorParticles = new ParticleEmitter();
            generatorParticles.LoadContent(device, TheContentManager, "Assets/2D/yellowGlow", 40, 70, 70, 100, new Vector3(1390, 600, 0), 1, 1);

            podjazd = new StaticItem("Assets/3D/podjazdFBX");
            podjazd.LoadItem(TheContentManager);
            podjazd.Position = new Vector3(50, 300, 0);
            podjazd.FixColliderInternal(new Vector3(2, 2, 2), new Vector3(50, 0, 0));
        }

        protected override void SetUpGates(GraphicsDevice device)
        {
            Vector3 moveGate = new Vector3(400, 150, 0);
            gate.Position = moveGate;
            gate.FixColliderInternal(new Vector3(1, 1, 1), new Vector3(0, 0, 0));
            gate.ID = generatedID.IDs[0];
            gate.DrawID = false;
            gate.OnOffBilboard = false;
            gate.MachineIDHeight = new Vector3(0, 90, 170);
            generatedID.IDs.RemoveAt(0);
            gate.ApplyIDBilboard(device, TheContentManager, moveGate);
            ConnectedColliders.Add(gate);
            StageElements.Add(gate);
        }

        protected override void LoadGates()
        {
//            rotateSam = 90;
            GameStateMainGame.plot.ThroughGate();
            gate = new StaticItem("Assets/3D/Interior/Interior_Gate_NoTexture");
            gate.LoadItem(TheContentManager);
            gate.Type = StaticItemType.gate;
            gate.Rotation = 0;
        }
    }
}
