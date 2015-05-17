using System;
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
        private DateTime pauseTime = DateTime.MinValue;

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

        private Thread t;

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

            t = new Thread(new ParameterizedThreadStart(EventLoop));
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
                lock (syncRoot)
                {
                    if (actualQueue.Count > 0 && actualQueue.First().Key < DateTime.Now && pausedState.Ticks == 0)
                    {
                        TickEventHandler handler = actualQueue.First().Value;
                        handler(this, secTillEnd);
                        actualQueue.Remove(actualQueue.First().Key);
                    }
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
                    lock (syncRoot)
                    {
                        DateTime callingTime = DateTime.Now + new TimeSpan(0, 0, time);
                        if (actualQueue.ContainsValue(toDo))
                        {
                            List<DateTime> keysToRemove = new List<DateTime>();
                            foreach (var item in actualQueue)
                            {
                                if (item.Value == toDo)
                                {
                                    keysToRemove.Add(item.Key);
                                }
                            }
                            foreach (var item in keysToRemove)
                            {
                                actualQueue.Remove(item);
                            }
                        }
                        actualQueue.Add(callingTime, toDo);
                    }
                    break;
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
            pauseTime = DateTime.Now;
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
            SortedDictionary<DateTime, TickEventHandler> newQueue = new SortedDictionary<DateTime, TickEventHandler>();
            foreach (var item in actualQueue)
            {
                TimeSpan remainingTime = item.Key - pauseTime;
                newQueue.Add(DateTime.Now + remainingTime, item.Value);
            }
            actualQueue = newQueue;
            pauseTime = DateTime.MinValue;
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
        /// Enables destroying old clock when current game exits
        /// </summary>
        public void Destroy()
        {
            //TODO: force calling this method when game closed by X button
            t.Abort();
            instance = null;
            Debug.WriteLine("Destroying clock...");
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
