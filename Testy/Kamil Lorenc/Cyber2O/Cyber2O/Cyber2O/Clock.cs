﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cyber2O
{
    [Serializable]
    public class Clock
    {
        private DateTime startTime;
        private DateTime gameOverTime;
        private TimeSpan pausedState = new TimeSpan(0);

        private static volatile Clock instance;
        private static object syncRoot = new Object();

        SortedDictionary<int, TickEventHandler> eventQueue;

        public delegate void TickEventHandler(object sender, int time);

        //TODO: clock manipulation routines
        //TODO: test serialization

        private Clock() 
        {
            //set game startup time and time the game will end
            startTime = DateTime.Now;
            TimeSpan gameLength = new TimeSpan(48, 0, 0);
            gameOverTime = startTime + gameLength;

            //initialize infinite loop in new thread
            eventQueue = new SortedDictionary<int, TickEventHandler>();
            Thread t = new Thread(new ParameterizedThreadStart(EventLoop));
            t.Start();
            while(!t.IsAlive);
        }

        private void EventLoop(object obj)
        {
            while (DateTime.Now < gameOverTime)
            {
                int secAfterStart = (int)((DateTime.Now - startTime).TotalSeconds);
                if (eventQueue.ContainsKey(secAfterStart) && pausedState.Ticks == 0)
                {
                    TickEventHandler handler = eventQueue[secAfterStart];
                    handler(this, secAfterStart);
                    eventQueue.Remove(secAfterStart);
                }
            }
        }

        /// <summary>
        /// Adds user's event handler to queue
        /// </summary>
        /// <param name="time">Time in seconds counting from game start</param>
        /// <param name="toDo">The event that will be executed on TIME</param>
        public void AddEvent(int time, TickEventHandler toDo)
        {
            eventQueue.Add(time,toDo);
        }

        public Int64 RemainingSeconds
        {
            get
            {
                return (Int64)(gameOverTime - DateTime.Now).TotalSeconds;
            }
            set
            {
                TimeSpan ts = new TimeSpan(0,0,(int)value);
                gameOverTime = DateTime.Now + ts;
            }
        }

        /// <summary>
        /// Adds or subtracts number of seconds given
        /// </summary>
        /// <param name="seconds">Signed number of seconds to add</param>
        public void AddSeconds(int seconds)
        {
            gameOverTime = gameOverTime + new TimeSpan(0, 0, seconds);
        }

        /// <summary>
        /// Pauses the clock
        /// </summary>
        public void Pause()
        {
            pausedState = gameOverTime - DateTime.Now;
        }

        /// <summary>
        /// Resumes the clock
        /// </summary>
        public void Resume()
        {
            if (pausedState.Ticks == 0)
                throw new CannotResumeException();
            gameOverTime = DateTime.Now + pausedState;
            pausedState = new TimeSpan(0);
        }

        /// <summary>
        /// Safely get an instance of clock
        /// </summary>
        public static Clock Instance
        {
            get 
            {
                if (instance == null) 
                {
                lock (syncRoot) 
                {
                    if (instance == null)
                        instance = new Clock();
                }
                }

                return instance;
            }
        }
    }
}
