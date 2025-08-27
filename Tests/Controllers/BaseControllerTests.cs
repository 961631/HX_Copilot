using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HX.Terminal.Controllers;

namespace HX.Terminal.Tests.Controllers
{
    /// <summary>
    /// BaseControllerのテストクラス
    /// </summary>
    [TestClass]
    public class BaseControllerTests
    {
        #region フィールド

        /// <summary>
        /// テスト対象のコントローラー（テスト用継承クラス）
        /// </summary>
        private TestableBaseController controller;

        /// <summary>
        /// HTTPコンテキストのモック
        /// </summary>
        private Mock<HttpContextBase> mockHttpContext;

        /// <summary>
        /// セッションのモック
        /// </summary>
        private Mock<HttpSessionStateBase> mockSession;

        #endregion

        #region テスト初期化・終了処理

        /// <summary>
        /// テスト初期化処理
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            controller = new TestableBaseController();
            mockHttpContext = new Mock<HttpContextBase>();
            mockSession = new Mock<HttpSessionStateBase>();
            
            mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
        }

        /// <summary>
        /// テスト終了処理
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            controller?.Dispose();
            controller = null;
            mockHttpContext = null;
            mockSession = null;
        }

        #endregion

        #region 権限チェックテスト

        /// <summary>
        /// CheckAuthorization_事務センター責任者権限の場合_trueを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Authorization")]
        public void CheckAuthorization_事務センター責任者権限の場合_trueを返す()
        {
            // Arrange
            mockSession.Setup(s => s["UserRole"]).Returns("21");
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestCheckAuthorization();

            // Assert
            Assert.IsTrue(result, "事務センター責任者権限(21)の場合はtrueを返す必要があります");
        }

        /// <summary>
        /// CheckAuthorization_工業担当者権限の場合_trueを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Authorization")]
        public void CheckAuthorization_工業担当者権限の場合_trueを返す()
        {
            // Arrange
            mockSession.Setup(s => s["UserRole"]).Returns("60");
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestCheckAuthorization();

            // Assert
            Assert.IsTrue(result, "工業担当者権限(60)の場合はtrueを返す必要があります");
        }

        /// <summary>
        /// CheckAuthorization_管理者権限の場合_trueを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Authorization")]
        public void CheckAuthorization_管理者権限の場合_trueを返す()
        {
            // Arrange
            mockSession.Setup(s => s["UserRole"]).Returns("99");
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestCheckAuthorization();

            // Assert
            Assert.IsTrue(result, "管理者権限(99)の場合はtrueを返す必要があります");
        }

        /// <summary>
        /// CheckAuthorization_無効な権限の場合_falseを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Authorization")]
        public void CheckAuthorization_無効な権限の場合_falseを返す()
        {
            // Arrange
            mockSession.Setup(s => s["UserRole"]).Returns("10");
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestCheckAuthorization();

            // Assert
            Assert.IsFalse(result, "無効な権限(10)の場合はfalseを返す必要があります");
        }

        /// <summary>
        /// CheckAuthorization_権限情報がnullの場合_falseを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Authorization")]
        public void CheckAuthorization_権限情報がnullの場合_falseを返す()
        {
            // Arrange
            mockSession.Setup(s => s["UserRole"]).Returns((string)null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestCheckAuthorization();

            // Assert
            Assert.IsFalse(result, "権限情報がnullの場合はfalseを返す必要があります");
        }

        #endregion

        #region ユーザー情報取得テスト

        /// <summary>
        /// GetCurrentUserId_セッションにユーザーIDがある場合_正しいIDを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("UserInfo")]
        public void GetCurrentUserId_セッションにユーザーIDがある場合_正しいIDを返す()
        {
            // Arrange
            const string expectedUserId = "USER001";
            mockSession.Setup(s => s["UserId"]).Returns(expectedUserId);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestGetCurrentUserId();

            // Assert
            Assert.AreEqual(expectedUserId, result, "セッションに設定されたユーザーIDが返される必要があります");
        }

        /// <summary>
        /// GetCurrentUserId_セッションにユーザーIDがない場合_デフォルト値を返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("UserInfo")]
        public void GetCurrentUserId_セッションにユーザーIDがない場合_デフォルト値を返す()
        {
            // Arrange
            mockSession.Setup(s => s["UserId"]).Returns((string)null);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestGetCurrentUserId();

            // Assert
            Assert.AreEqual("SYSTEM", result, "ユーザーIDがない場合はSYSTEMが返される必要があります");
        }

        /// <summary>
        /// GetCurrentUserName_セッションにユーザー名がある場合_正しい名前を返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("UserInfo")]
        public void GetCurrentUserName_セッションにユーザー名がある場合_正しい名前を返す()
        {
            // Arrange
            const string expectedUserName = "テストユーザー";
            mockSession.Setup(s => s["UserName"]).Returns(expectedUserName);
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            var result = controller.TestGetCurrentUserName();

            // Assert
            Assert.AreEqual(expectedUserName, result, "セッションに設定されたユーザー名が返される必要があります");
        }

        #endregion
    }

    /// <summary>
    /// BaseControllerのテスト用継承クラス
    /// </summary>
    public class TestableBaseController : BaseController
    {
        /// <summary>
        /// CheckAuthorizationメソッドのテスト用公開ラッパー
        /// </summary>
        /// <returns>認証結果</returns>
        public bool TestCheckAuthorization()
        {
            return CheckAuthorization();
        }

        /// <summary>
        /// GetCurrentUserIdメソッドのテスト用公開ラッパー
        /// </summary>
        /// <returns>現在のユーザーID</returns>
        public string TestGetCurrentUserId()
        {
            return GetCurrentUserId();
        }

        /// <summary>
        /// GetCurrentUserNameメソッドのテスト用公開ラッパー
        /// </summary>
        /// <returns>現在のユーザー名</returns>
        public string TestGetCurrentUserName()
        {
            return GetCurrentUserName();
        }
    }
}
