using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HX.Terminal.Models;

namespace HX.Terminal.Tests.Models
{
    /// <summary>
    /// 端末情報モデルのテストクラス
    /// </summary>
    [TestClass]
    public class TerminalRegistModelTests
    {
        [TestMethod]
        public void TerminalRegistModel_正常な値を設定_正常に設定される()
        {
            // Arrange
            var model = new TerminalRegistModel();
            var now = DateTime.Now;

            // Act
            model.CreatedDate = now;
            model.CreatedProgramId = "TestProgram";
            model.CreatedUserId = "TestUser";
            model.ModifiedDate = now;
            model.ModifiedProgramId = "TestProgram";
            model.ModifiedUserId = "TestUser";
            model.DeleteFlg = "0";
            model.TerminalNo1 = "123456789012345";
            model.TerminalNo2 = "123456789012346";
            model.PinCode = "12345";
            model.ItemCode = "ABCD1234";

            // Assert
            Assert.AreEqual(now, model.CreatedDate);
            Assert.AreEqual("TestProgram", model.CreatedProgramId);
            Assert.AreEqual("TestUser", model.CreatedUserId);
            Assert.AreEqual(now, model.ModifiedDate);
            Assert.AreEqual("TestProgram", model.ModifiedProgramId);
            Assert.AreEqual("TestUser", model.ModifiedUserId);
            Assert.AreEqual("0", model.DeleteFlg);
            Assert.AreEqual("123456789012345", model.TerminalNo1);
            Assert.AreEqual("123456789012346", model.TerminalNo2);
            Assert.AreEqual("12345", model.PinCode);
            Assert.AreEqual("ABCD1234", model.ItemCode);
        }

        [TestMethod]
        public void TerminalRegistModel_DeleteFlgのデフォルト値_0が設定される()
        {
            // Arrange & Act
            var model = new TerminalRegistModel();

            // Assert
            Assert.AreEqual("0", model.DeleteFlg);
        }
    }

    /// <summary>
    /// SIM情報モデルのテストクラス
    /// </summary>
    [TestClass]
    public class SimRegistModelTests
    {
        [TestMethod]
        public void SimRegistModel_正常な値を設定_正常に設定される()
        {
            // Arrange
            var model = new SimRegistModel();
            var now = DateTime.Now;

            // Act
            model.CreatedDate = now;
            model.CreatedProgramId = "TestProgram";
            model.CreatedUserId = "TestUser";
            model.ModifiedDate = now;
            model.ModifiedProgramId = "TestProgram";
            model.ModifiedUserId = "TestUser";
            model.DeleteFlg = "0";
            model.MdpTel = "09012345678";
            model.KanyusyaCode = "ABCD1234";
            model.IcCardNo = "1234567890123456789";
            model.NetworkPassword = "1234";
            model.SimUnlockCode = "UNLOCK12";

            // Assert
            Assert.AreEqual(now, model.CreatedDate);
            Assert.AreEqual("TestProgram", model.CreatedProgramId);
            Assert.AreEqual("TestUser", model.CreatedUserId);
            Assert.AreEqual(now, model.ModifiedDate);
            Assert.AreEqual("TestProgram", model.ModifiedProgramId);
            Assert.AreEqual("TestUser", model.ModifiedUserId);
            Assert.AreEqual("0", model.DeleteFlg);
            Assert.AreEqual("09012345678", model.MdpTel);
            Assert.AreEqual("ABCD1234", model.KanyusyaCode);
            Assert.AreEqual("1234567890123456789", model.IcCardNo);
            Assert.AreEqual("1234", model.NetworkPassword);
            Assert.AreEqual("UNLOCK12", model.SimUnlockCode);
        }

        [TestMethod]
        public void SimRegistModel_DeleteFlgのデフォルト値_0が設定される()
        {
            // Arrange & Act
            var model = new SimRegistModel();

            // Assert
            Assert.AreEqual("0", model.DeleteFlg);
        }
    }

    /// <summary>
    /// 端末-SIM紐付けモデルのテストクラス
    /// </summary>
    [TestClass]
    public class TerminalSimHimodukeModelTests
    {
        [TestMethod]
        public void TerminalSimHimodukeModel_正常な値を設定_正常に設定される()
        {
            // Arrange
            var model = new TerminalSimHimodukeModel();
            var now = DateTime.Now;

            // Act
            model.CreatedDate = now;
            model.CreatedProgramId = "TestProgram";
            model.CreatedUserId = "TestUser";
            model.ModifiedDate = now;
            model.ModifiedProgramId = "TestProgram";
            model.ModifiedUserId = "TestUser";
            model.DeleteFlg = "0";
            model.TerminalNo = "123456789012345";
            model.IcCardNo = "1234567890123456789";
            model.RegistDate = now;
            model.R3SendFlg = "0";
            model.R3SendDate = now;

            // Assert
            Assert.AreEqual(now, model.CreatedDate);
            Assert.AreEqual("TestProgram", model.CreatedProgramId);
            Assert.AreEqual("TestUser", model.CreatedUserId);
            Assert.AreEqual(now, model.ModifiedDate);
            Assert.AreEqual("TestProgram", model.ModifiedProgramId);
            Assert.AreEqual("TestUser", model.ModifiedUserId);
            Assert.AreEqual("0", model.DeleteFlg);
            Assert.AreEqual("123456789012345", model.TerminalNo);
            Assert.AreEqual("1234567890123456789", model.IcCardNo);
            Assert.AreEqual(now, model.RegistDate);
            Assert.AreEqual("0", model.R3SendFlg);
            Assert.AreEqual(now, model.R3SendDate);
        }

        [TestMethod]
        public void TerminalSimHimodukeModel_デフォルト値の確認()
        {
            // Arrange & Act
            var model = new TerminalSimHimodukeModel();

            // Assert
            Assert.AreEqual("0", model.DeleteFlg);
            Assert.AreEqual("0", model.R3SendFlg);
        }
    }
}
