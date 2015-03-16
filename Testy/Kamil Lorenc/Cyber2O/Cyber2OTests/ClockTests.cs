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
            //when
            Int64 secTillOver = clock.GetRemainingSeconds();
            //then
            //if it is lasting more than 3 seconds it is bad despite working
            Assert.AreEqual(48 * 60 * 60, secTillOver, 3);
        }
    }
}
