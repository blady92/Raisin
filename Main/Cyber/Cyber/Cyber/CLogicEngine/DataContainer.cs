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
        }

        public void Apply(GameStateMainGame gameStateMainGame)
        {
            Debug.Print("DataContainer: nie działa wczytywanie do aktualnego levelu!");
            gameStateMainGame.level = this.level;
            gameStateMainGame.samanthaGhostController.Position = samGhostPosition;
            gameStateMainGame.samanthaGhostController.FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
            gameStateMainGame.samanthaActualPlayer.Position = samRealPosition;
        }

        [DataMember]
        Level level { get; set; }

        [DataMember]
        Vector3 samGhostPosition { get; set; }

        [DataMember]
        Vector3 samRealPosition { get; set; }
    }
}
