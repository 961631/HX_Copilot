using Microsoft.VisualStudio.TestTools.UnitTesting;
using HX.Terminal.Models;
using System.Collections.Generic;
using System.Linq;

namespace HX.Terminal.Tests.Models
{
    /// <summary>
    /// ValidationResultクラスのテストクラス
    /// </summary>
    [TestClass]
    public class ValidationResultTests
    {
        #region フィールド

        /// <summary>
        /// テスト対象のValidationResultインスタンス
        /// </summary>
        private ValidationResult validationResult;

        #endregion

        #region テスト初期化・終了処理

        /// <summary>
        /// テスト初期化処理
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            validationResult = new ValidationResult();
        }

        /// <summary>
        /// テスト終了処理
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            validationResult = null;
        }

        #endregion

        #region 初期状態テスト

        /// <summary>
        /// ValidationResult初期化_初期状態でエラーなしで有効であることを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Validation")]
        public void ValidationResult_初期状態_エラーなしで有効()
        {
            // Arrange & Act
            var result = new ValidationResult();

            // Assert
            Assert.IsTrue(result.IsValid, "初期状態ではバリデーションは有効である必要があります");
            Assert.AreEqual(0, result.Errors.Count, "初期状態ではエラーは0件である必要があります");
        }

        #endregion

        #region エラー追加テスト

        /// <summary>
        /// AddError_エラーメッセージ追加_エラーが追加され無効になることを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Validation")]
        public void AddError_エラーメッセージ追加_エラーが追加され無効になる()
        {
            // Arrange
            const string errorMessage = "テストエラーメッセージ";

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
