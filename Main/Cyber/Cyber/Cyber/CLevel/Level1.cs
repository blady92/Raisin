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
    public class Level1 : LevelStage
    {
        public override void ParseStage()
        {
            Stage = StageParser.ParseBitmap("../../../CStageParsing/stage3.bmp");
        }

        protected override void LoadGates()
        {
            
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
            
        }

        public Level1(GameStateMainGame game) : base(game)
        {
            level = Level.level1;
        }
    }
}
