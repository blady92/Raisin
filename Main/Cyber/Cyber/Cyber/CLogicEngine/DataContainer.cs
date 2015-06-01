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
            this.level = state.level;
            this.samGhostPosition = state.samanthaGhostController.Position;
            this.samRealPosition = state.samanthaActualPlayer.Position;
            npcPositions = new List<Vector3>();
            foreach (var npc in gameStateMainGame.npcList)
            {
                npcPositions.Add(npc.Position);
            }
        }

        public void Apply(GameStateMainGame gameStateMainGame)
        {
            Debug.Print("DataContainer: nie działa wczytywanie do aktualnego levelu!");
            gameStateMainGame.level = this.level;
            gameStateMainGame.samanthaGhostController.Position = samGhostPosition;
            gameStateMainGame.samanthaGhostController.FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
            gameStateMainGame.samanthaActualPlayer.Position = samRealPosition;
            for (int i = 0; i < npcPositions.Count; i++)
            {
                gameStateMainGame.npcList[i].Position = npcPositions[i];
                gameStateMainGame.npcList[i].FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
                gameStateMainGame.npcList[i].FixColliderExternal(new Vector3(2, 2, 2), new Vector3(-15f, -15f, 10f));
            }
        }

        [DataMember]
        Level level { get; set; }

        [DataMember]
        Vector3 samGhostPosition { get; set; }

        [DataMember]
        Vector3 samRealPosition { get; set; }

        [DataMember]
        List<Vector3> npcPositions { get; set; } 
    }
}
