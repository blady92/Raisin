using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cyber2O
{
    [Serializable]
    public class Clock
    {
        private DateTime starttime;
        private DateTime gameOverTime;
        private static volatile Clock instance;
        private static object syncRoot = new Object();
        SortedDictionary<int, TickEventHandler> eventQueue;

        public delegate void TickEventHandler(object sender, int time);

        //TODO: clock manipulation routines
        //TODO: test serialization

        private Clock() 
        {
            //set game startup time and time the game will end
            starttime = DateTime.Now;
            TimeSpan gameLength = new TimeSpan(48, 0, 0);
            gameOverTime = starttime + gameLength;

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
                int secAfterStart = (int)((DateTime.Now - starttime).TotalSeconds);
                if (eventQueue.ContainsKey(secAfterStart))
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

        /// <summary>
        /// Get time until the game's over
        /// </summary>
        /// <returns>Remaining time in seconds</returns>
        public Int64 GetRemainingSeconds()
        {
            return (Int64)(gameOverTime - DateTime.Now).TotalSeconds;
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
