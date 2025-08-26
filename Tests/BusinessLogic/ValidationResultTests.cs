using Microsoft.VisualStudio.TestTools.UnitTesting;
using HX.Terminal.BusinessLogic;
using System.Collections.Generic;

namespace HX.Terminal.Tests.BusinessLogic
{
    /// <summary>
    /// ValidationResultクラスのテストクラス
    /// </summary>
    [TestClass]
    public class ValidationResultTests
    {
        [TestMethod]
        public void ValidationResult_初期状態_エラーなしで有効()
        {
            // Arrange & Act
            var result = new ValidationResult();

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
            Assert.IsNull(result.Message);
        }

        [TestMethod]
        public void AddError_エラーメッセージ追加_エラーが追加され無効になる()
        {
            // Arrange
            var result = new ValidationResult();
            var errorMessage = "テストエラーメッセージ";

            // Act
            result.AddError(errorMessage);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(errorMessage, result.Errors[0]);
        }

        [TestMethod]
        public void AddError_複数のエラーメッセージ追加_すべてのエラーが追加される()
        {
            // Arrange
            var result = new ValidationResult();
            var error1 = "エラー1";
            var error2 = "エラー2";
            var error3 = "エラー3";

            // Act
            result.AddError(error1);
            result.AddError(error2);
            result.AddError(error3);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(3, result.Errors.Count);
            Assert.AreEqual(error1, result.Errors[0]);
            Assert.AreEqual(error2, result.Errors[1]);
            Assert.AreEqual(error3, result.Errors[2]);
        }

        [TestMethod]
        public void Message_メッセージ設定_正常に設定される()
        {
            // Arrange
            var result = new ValidationResult();
            var message = "処理が完了しました";

            // Act
            result.Message = message;

            // Assert
            Assert.AreEqual(message, result.Message);
        }
    }
}
