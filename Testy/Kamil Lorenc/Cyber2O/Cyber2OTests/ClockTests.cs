using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cyber2O;

namespace Cyber2OTests
{
    [TestClass]
    public class ClockTests
    {
        [TestMethod]
        public void ShouldCallHandlerAfterZeroMinutes()
        {
            //given
            Clock clock = Clock.Instance;
            //when
            clock.AddEvent(0, TickEventHandler);
            //then
            Assert.Fail("Not expected to get here");
        }

        private void TickEventHandler(object sender, int time)
        {
            throw new NotImplementedException("tik, tak, tik, tak");
        }
    }
}
