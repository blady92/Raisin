using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cyber2O;
using System.Threading;

namespace Cyber2OTests
{
    [TestClass]
    public class ClockTests
    {
        int checkCondition = 0;
        Semaphore sema = new Semaphore(1, 1);

        [TestMethod]
        public void ShouldCallHandlerAfterZeroSeconds()
        {
            //given
            Clock clock = Clock.Instance;
            clock.RemainingSeconds = 60;
            //when
            sema.WaitOne();
            clock.AddEvent(0, TickEventHandler);
            sema.WaitOne();
            //then
            Assert.AreEqual<int>(1, checkCondition, "Tick event not called!");
            sema.Release();
            sema.Close();
        }

        private void TickEventHandler(object sender, int time)
        {
            checkCondition = 1;
            sema.Release();
        }

        [TestMethod]
        public void ShouldShowSecondsUntilGameOver()
        {
            //given
            Clock clock = Clock.Instance;
            clock.RemainingSeconds = 60;
            //when
            Int64 secTillOver = clock.RemainingSeconds;
            //then
            //if it is lasting more than 3 seconds it is bad despite working
            Assert.AreEqual(60, secTillOver, 3);
        }

        [TestMethod]
        public void ShouldModifyClockTime()
        {
            //given
            Clock clock = Clock.Instance;
            clock.RemainingSeconds = 60;
            //when
            clock.AddSeconds(120);
            clock.AddSeconds(-60);
            Int64 secTillOver = clock.RemainingSeconds;
            //then
            //if it is lasting more than 3 seconds it is bad despite working
            Assert.AreEqual(120, secTillOver, 3);
        }

        [TestMethod]
        public void ShouldPauseAndResumeTheClock()
        {
            //given
            Clock clock = Clock.Instance;
            clock.RemainingSeconds = 60;
            //when
            clock.Pause();
            Thread.Sleep(5000);
            clock.Resume();
            Thread.Sleep(2000);
            Assert.AreEqual(58, clock.RemainingSeconds, 0);
        }
    }
}
