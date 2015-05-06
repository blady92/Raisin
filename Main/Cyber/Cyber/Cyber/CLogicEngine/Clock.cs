﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cyber.CLogicEngine
{
    [Serializable]
    public class Clock
    {
        private DateTime startTime;
        private DateTime gameOverTime;
        private TimeSpan pausedState = new TimeSpan(0);

        private const int length = 48;

        private TimeSpan gameLength = new TimeSpan(length,0,0);

        public const int AFTERSTART = 0;
        public const int BEFOREOVER = 1;
        public const int FROMNOW = 2;

        private static volatile Clock instance;
        private static object syncRoot = new Object();

        int secAfterStart, secTillEnd;

        SortedDictionary<int, TickEventHandler> startQueue;
        SortedDictionary<int, TickEventHandler> endQueue;
        SortedDictionary<DateTime, TickEventHandler> actualQueue;

        public delegate void TickEventHandler(object sender, int time);

        //TODO: test serialization

        private Clock() 
        {
            //set game startup time and time the game will end
            startTime = DateTime.Now;
            TimeSpan gameLength = new TimeSpan(length, 0, 0);
            gameOverTime = startTime + gameLength;

            //initialize infinite loop in new thread
            startQueue = new SortedDictionary<int, TickEventHandler>();
            endQueue = new SortedDictionary<int, TickEventHandler>();
            actualQueue = new SortedDictionary<DateTime, TickEventHandler>();

            Thread t = new Thread(new ParameterizedThreadStart(EventLoop));
            t.Start();
            while(!t.IsAlive);
        }

        private void EventLoop(object obj)
        {
            while (DateTime.Now < gameOverTime || pausedState.Ticks > 0)
            {
                #if DEBUG
                if (secAfterStart != (int)((DateTime.Now - startTime).TotalSeconds))
                {
                    Debug.WriteLine("From start: "+secAfterStart+"; till end: "+(int)(gameOverTime-DateTime.Now).TotalSeconds);
                }
                #endif
                secAfterStart = (int)((DateTime.Now - startTime).TotalSeconds);
                secTillEnd = (int)(gameOverTime - DateTime.Now).TotalSeconds;
                if (startQueue.ContainsKey(secAfterStart) && pausedState.Ticks == 0)
                {
                    TickEventHandler handler = startQueue[secAfterStart];
                    handler(this, secAfterStart);
                    startQueue.Remove(secAfterStart);
                }
                if (endQueue.ContainsKey(secTillEnd) && pausedState.Ticks == 0)
                {
                    TickEventHandler handler = endQueue[secTillEnd];
                    handler(this, secTillEnd);
                    endQueue.Remove(secTillEnd);
                }
                Thread.Sleep(1);
                //TODO: kill after main game closed
            }
        }

        /// <summary>
        /// Adds user's event handler to queue
        /// </summary>
        /// <param name="whence">Start counting from now/game start/game over</param>
        /// <param name="time">Time in seconds counting from game start</param>
        /// <param name="toDo">The event that will be executed on TIME</param>
        public void AddEvent(int whence, int time, TickEventHandler toDo)
        {
            switch(whence)
            { 
                case AFTERSTART:
                    startQueue.Add(time,toDo);
                    break;
                case BEFOREOVER:
                    //TODO: unit tests
                    endQueue.Add(time, toDo);
                    break;
                case FROMNOW:
                    //TODO: unit tests
                    TimeSpan tonow = DateTime.Now - startTime;
                    startQueue.Add((int)tonow.TotalSeconds + time, toDo);
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException("Invalid time base for event addition");
            }
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
                gameLength = ts;
                gameOverTime = DateTime.Now + ts;
                startTime = gameOverTime - gameLength;
            }
        }

        /// <summary>
        /// Adds or subtracts number of seconds given
        /// </summary>
        /// <param name="seconds">Signed number of seconds to add</param>
        public void AddSeconds(int seconds)
        {
            gameOverTime = gameOverTime + new TimeSpan(0, 0, seconds);
            gameLength += new TimeSpan(0, 0, seconds);
        }

        /// <summary>
        /// Pauses the clock
        /// </summary>
        public void Pause()
        {
            pausedState = gameOverTime - DateTime.Now;
            Debug.WriteLine(pausedState);
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
            startTime = gameOverTime - gameLength;
            //TODO: dokończyć !!! !!! !!! !!! !!!
        }

        public bool CanResume()
        {
            if (pausedState.Ticks == 0)
                return false;
            else
                return true;
        }

        public bool CanPause()
        {
            return !CanResume();
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
