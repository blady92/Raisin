using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cyber2O;

namespace Cyber2OTests
{
    [TestClass]
    public class StageTests
    {
        [TestMethod]
        public void TestParseBitmapSquare()
        {
            StageParser parser = new StageParser();
            Stage stage = parser.ParseBitmap("../../Assets/preview.bmp");
            Assert.IsNotNull(stage);
            Assert.IsTrue(stage.Objects.Count == 1);
            Assert.IsTrue(stage.NPCs.Count == 2);
            Assert.IsTrue(stage.Rooms.Count == 3);
            Assert.IsTrue(stage.Corridors.Count == 2);
        }

        [TestMethod]
        public void TestParseBitmapNotSquare()
        {
            StageParser parser = new StageParser();
            Stage stage = parser.ParseBitmap("../../Assets/preview2.bmp");
            Assert.IsNotNull(stage);
            Assert.IsTrue(stage.Objects.Count == 2);
            Assert.IsTrue(stage.NPCs.Count == 1);
            Assert.IsTrue(stage.Rooms.Count == 3);
            Assert.IsTrue(stage.Corridors.Count == 1);
        }
    }
}
