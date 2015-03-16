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
        private static volatile Clock instance;
        private static object syncRoot = new Object();
        SortedDictionary<int, TickEventHandler> eventQueue;

        public delegate void TickEventHandler(object sender, int time);
        private event TickEventHandler TickEvent;

    private Clock() 
    {
        starttime = DateTime.Now;
        eventQueue = new SortedDictionary<int, TickEventHandler>();
        throw new NotImplementedException("TODO: add infinite wait in new thread");
    }

    public void AddEvent(int time, TickEventHandler toDo)
    {
        eventQueue.Add(time,toDo);
    }

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
