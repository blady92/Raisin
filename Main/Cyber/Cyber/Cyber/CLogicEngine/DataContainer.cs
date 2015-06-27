using Cyber.CGameStateEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Cyber.CLogicEngine
{
    [DataContract]
    class DataContainer
    {
        private GameStateMainGame state;

        public DataContainer(GameStateMainGame gameStateMainGame)
        {
            this.state = gameStateMainGame;
            this.level = state.level.level;
            this.samGhostPosition = state.samanthaGhostController.Position;
            this.samRealPosition = state.samanthaActualPlayer.Position;
            npcPositions = new List<Vector3>();
            foreach (var npc in gameStateMainGame.level.npcList)
            {
                npcPositions.Add(npc.Position);
            }
            this.plot = gameStateMainGame.plot;
            this.Ai = AI.Instance;
            this.clock = Clock.Instance;
        }

        public void Apply(GameStateMainGame gameStateMainGame, LogicEngine logicEngine)
        {
            Debug.Print("DataContainer: nie działa wczytywanie do aktualnego levelu!");
            logicEngine.level = this.level;
            logicEngine.LogicChangeLevel(logicEngine.theContentManager, logicEngine.device);
            gameStateMainGame.samanthaGhostController.Position = samGhostPosition;
            gameStateMainGame.samanthaGhostController.FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
            gameStateMainGame.samanthaActualPlayer.Position = samRealPosition;
            for (int i = 0; i < npcPositions.Count; i++)
            {
                gameStateMainGame.level.npcList[i].Position = npcPositions[i];
                gameStateMainGame.level.npcList[i].FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
                gameStateMainGame.level.npcList[i].FixColliderExternal(new Vector3(2, 2, 2), new Vector3(-15f, -15f, 10f));
            }
            gameStateMainGame.plot = plot;
            AI.Instance.lastSamPosition = Ai.lastSamPosition;
            AI.Instance.ResumeChase();
            Clock.Instance = clock;
            clock.ConstructThread();
        }

        [DataMember]
        Level level { get; set; }

        [DataMember]
        Vector3 samGhostPosition { get; set; }

        [DataMember]
        Vector3 samRealPosition { get; set; }

        [DataMember]
        List<Vector3> npcPositions { get; set; }

        [DataMember]
        PlotTwistClass plot { get; set; }

        [DataMember]
        AI Ai { get; set; }
        [DataMember]
        Clock clock { get; set; }
    }
}
