using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cyber2O;

namespace Cyber2OTests
{
    /// <summary>
    /// Summary description for UnitTest
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        Terminal terminal;
        ResponsableMock responser;
        public UnitTest()
        {
            terminal = new Terminal();
            responser = new ResponsableMock();
            terminal.Responser = responser;
        }
        public class ResponsableMock : Responsable
        {
            public string LastResponse { get; set; }
            public void response(string message)
            {
                LastResponse = message;
            }
        }

        [TestMethod]
        public void ShouldResponseOnOpenCommand()
        {
            terminal.process("open");
            Assert.AreEqual("Command \"open\" needs an argument", responser.LastResponse);
            terminal.process("open door");
            Assert.AreEqual("Opened door", responser.LastResponse);
        }

        [TestMethod]
        public void ShouldResponseOnCloseCommand()
        {
            terminal.process("close");
            Assert.AreEqual("Command \"close\" needs an argument", responser.LastResponse);
            terminal.process("close door");
            Assert.AreEqual("Closed door", responser.LastResponse);
        }

        [TestMethod]
        public void ShouldResponseOnSudoCommand()
        {
            terminal.process("sudo");
            Assert.IsTrue(responser.LastResponse.Length > 20);
        }

        
    }
}
