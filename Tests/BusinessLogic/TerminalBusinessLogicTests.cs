using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HX.Terminal.BusinessLogic;
using HX.Terminal.Models;

namespace HX.Terminal.Tests.BusinessLogic
{
    /// <summary>
    /// 端末登録ビジネスロジックのテストクラス
    /// </summary>
    [TestClass]
    public class TerminalRegistBusinessLogicTests
    {
        #region フィールド

        /// <summary>
        /// テスト対象のビジネスロジック
        /// </summary>
        private TerminalRegistBusinessLogic businessLogic;

        /// <summary>
        /// HTTPファイルのモック
        /// </summary>
        private Mock<HttpPostedFileBase> mockFile;

        #endregion

        #region テスト初期化・終了処理

        /// <summary>
        /// テスト初期化処理
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            businessLogic = new TerminalRegistBusinessLogic();
            mockFile = new Mock<HttpPostedFileBase>();
        }

        /// <summary>
        /// テスト終了処理
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            businessLogic = null;
            mockFile = null;
        }

        #endregion

        #region CSVファイルバリデーションテスト

        /// <summary>
        /// CSVファイルバリデーション_ファイルが未選択の場合_エラーメッセージを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateCsvFile_ファイルが未選択の場合_エラーメッセージを返す()
        {
            // Arrange
            HttpPostedFileBase nullFile = null;

            // Act
            var result = businessLogic.ValidateCsvFile(nullFile);

            // Assert
            Assert.IsFalse(result.IsValid, "バリデーションは失敗する必要があります");
            Assert.AreEqual("ファイルが選択されていません。", result.Errors[0], "適切なエラーメッセージが返される必要があります");
        }

        /// <summary>
        /// CSVファイルバリデーション_ファイルサイズが0の場合_エラーメッセージを返すことを検証
        /// </summary>
        [TestMethod]
        [TestCategory("Validation")]
        public void ValidateCsvFile_ファイルサイズが0の場合_エラーメッセージを返す()
        {
            // Arrange
            mockFile.Setup(f => f.ContentLength).Returns(0);

            // Act
            var result = businessLogic.ValidateCsvFile(mockFile.Object);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("ファイルが選択されていません。", result.Errors[0]);
        }

        [TestMethod]
        public void ValidateCsvFile_拡張子がcsv以外の場合_エラーメッセージを返す()
        {
            // Arrange
            mockFile.Setup(f => f.ContentLength).Returns(1000);
            mockFile.Setup(f => f.FileName).Returns("test.txt");

            // Act
            var result = businessLogic.ValidateCsvFile(mockFile.Object);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("ファイル形式が正しくありません。CSVファイルをアップロードしてください。", result.Errors[0]);
        }

        [TestMethod]
        public void ValidateCsvFile_正常なCSVファイルの場合_成功を返す()
        {
            // Arrange
            mockFile.Setup(f => f.ContentLength).Returns(1000);
            mockFile.Setup(f => f.FileName).Returns("test.csv");

            // Act
            var result = businessLogic.ValidateCsvFile(mockFile.Object);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        #endregion

        #region 端末CSVファイル処理テスト

        [TestMethod]
        public void ProcessTerminalCsvFile_正常なCSVデータの場合_成功を返す()
        {
            // Arrange
            var csvContent = "No,端末製造番号1,端末製造番号2,機器PINコード,納品予定日,商品コード,商品名\n" +
                           "1,123456789012345,123456789012346,12345,2023/12/01,ABCD1234,テスト商品\n";
            
            var stream = new MemoryStream(Encoding.GetEncoding("Shift_JIS").GetBytes(csvContent));
            mockFile.Setup(f => f.InputStream).Returns(stream);
            mockFile.Setup(f => f.ContentLength).Returns(1000);
            mockFile.Setup(f => f.FileName).Returns("terminal.csv");

            // Act
            var result = businessLogic.ProcessTerminalCsvFile(mockFile.Object, "TestProgram", "TestUser");

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Message.Contains("端末データを"));
            Assert.IsTrue(result.Message.Contains("件登録しました。"));
        }

        [TestMethod]
        public void ProcessTerminalCsvFile_端末製造番号の桁数が不正な場合_エラーメッセージを返す()
        {
            // Arrange
            var csvContent = "No,端末製造番号1,端末製造番号2,機器PINコード,納品予定日,商品コード,商品名\n" +
                           "1,12345678901234,123456789012346,12345,2023/12/01,ABCD1234,テスト商品\n"; // 14桁
            
            var stream = new MemoryStream(Encoding.GetEncoding("Shift_JIS").GetBytes(csvContent));
            mockFile.Setup(f => f.InputStream).Returns(stream);

            // Act
            var result = businessLogic.ProcessTerminalCsvFile(mockFile.Object, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].Contains("桁数が正しくないためエラーとなりました"));
        }

        [TestMethod]
        public void ProcessTerminalCsvFile_商品コードに全角文字が含まれる場合_エラーメッセージを返す()
        {
            // Arrange
            var csvContent = "No,端末製造番号1,端末製造番号2,機器PINコード,納品予定日,商品コード,商品名\n" +
                           "1,123456789012345,123456789012346,12345,2023/12/01,ＡＢＣＤ1234,テスト商品\n"; // 全角文字
            
            var stream = new MemoryStream(Encoding.GetEncoding("Shift_JIS").GetBytes(csvContent));
            mockFile.Setup(f => f.InputStream).Returns(stream);

            // Act
            var result = businessLogic.ProcessTerminalCsvFile(mockFile.Object, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].Contains("全角文字が含まれています"));
        }

        #endregion
    }

    /// <summary>
    /// SIM登録ビジネスロジックのテストクラス
    /// </summary>
    [TestClass]
    public class SimRegistBusinessLogicTests
    {
        private SimRegistBusinessLogic businessLogic;
        private Mock<HttpPostedFileBase> mockFile;

        [TestInitialize]
        public void Setup()
        {
            businessLogic = new SimRegistBusinessLogic();
            mockFile = new Mock<HttpPostedFileBase>();
        }

        [TestMethod]
        public void ProcessSimCsvFile_正常なCSVデータの場合_成功を返す()
        {
            // Arrange
            var csvContent = "No,端末電話番号,加入者コード,製造番号,ネットワーク暗証番号,PINロック解除,機器PINコード,納品予定日,受注日\n" +
                           "1,09012345678,ABCD1234,1234567890123456789,1234,UNLOCK12,12345,2023/12/01,2023/11/01\n";
            
            var stream = new MemoryStream(Encoding.GetEncoding("Shift_JIS").GetBytes(csvContent));
            mockFile.Setup(f => f.InputStream).Returns(stream);

            // Act
            var result = businessLogic.ProcessSimCsvFile(mockFile.Object, "TestProgram", "TestUser");

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Message.Contains("SIMデータを"));
            Assert.IsTrue(result.Message.Contains("件登録しました。"));
        }

        [TestMethod]
        public void ProcessSimCsvFile_電話番号の桁数が不正な場合_エラーメッセージを返す()
        {
            // Arrange
            var csvContent = "No,端末電話番号,加入者コード,製造番号,ネットワーク暗証番号,PINロック解除,機器PINコード,納品予定日,受注日\n" +
                           "1,090123456789,ABCD1234,1234567890123456789,1234,UNLOCK12,12345,2023/12/01,2023/11/01\n"; // 12桁
            
            var stream = new MemoryStream(Encoding.GetEncoding("Shift_JIS").GetBytes(csvContent));
            mockFile.Setup(f => f.InputStream).Returns(stream);

            // Act
            var result = businessLogic.ProcessSimCsvFile(mockFile.Object, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].Contains("桁数が正しくないためエラーとなりました"));
        }

        [TestMethod]
        public void ProcessSimCsvFile_ICカード番号の桁数が不正な場合_エラーメッセージを返す()
        {
            // Arrange
            var csvContent = "No,端末電話番号,加入者コード,製造番号,ネットワーク暗証番号,PINロック解除,機器PINコード,納品予定日,受注日\n" +
                           "1,09012345678,ABCD1234,123456789012345678,1234,UNLOCK12,12345,2023/12/01,2023/11/01\n"; // 18桁
            
            var stream = new MemoryStream(Encoding.GetEncoding("Shift_JIS").GetBytes(csvContent));
            mockFile.Setup(f => f.InputStream).Returns(stream);

            // Act
            var result = businessLogic.ProcessSimCsvFile(mockFile.Object, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].Contains("SIM情報（ICCID）の桁数が正しくありません"));
        }
    }

    /// <summary>
    /// 端末-SIM紐付けビジネスロジックのテストクラス
    /// </summary>
    [TestClass]
    public class TerminalSimHimodukeBusinessLogicTests
    {
        private TerminalSimHimodukeBusinessLogic businessLogic;

        [TestInitialize]
        public void Setup()
        {
            businessLogic = new TerminalSimHimodukeBusinessLogic();
        }

        [TestMethod]
        public void ProcessHimoduke_正常な入力値の場合_成功を返す()
        {
            // Arrange
            var terminalNo = "123456789012345"; // 15桁
            var icCardNo = "1234567890123456789"; // 19桁

            // Act
            var result = businessLogic.ProcessHimoduke(terminalNo, icCardNo, "TestProgram", "TestUser");

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Message.Contains("で紐付け登録しました"));
        }

        [TestMethod]
        public void ProcessHimoduke_端末製造番号が未入力の場合_エラーメッセージを返す()
        {
            // Arrange
            var terminalNo = "";
            var icCardNo = "1234567890123456789";

            // Act
            var result = businessLogic.ProcessHimoduke(terminalNo, icCardNo, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("端末製造番号（IMEI）は必須です。", result.Errors[0]);
        }

        [TestMethod]
        public void ProcessHimoduke_端末製造番号の桁数が不正な場合_エラーメッセージを返す()
        {
            // Arrange
            var terminalNo = "12345678901234"; // 14桁
            var icCardNo = "1234567890123456789";

            // Act
            var result = businessLogic.ProcessHimoduke(terminalNo, icCardNo, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].Contains("桁数が正しくないためエラーとなりました"));
        }

        [TestMethod]
        public void ProcessHimoduke_SIM情報が未入力の場合_エラーメッセージを返す()
        {
            // Arrange
            var terminalNo = "123456789012345";
            var icCardNo = "";

            // Act
            var result = businessLogic.ProcessHimoduke(terminalNo, icCardNo, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("SIM情報（ICCID）は必須です。", result.Errors[0]);
        }

        [TestMethod]
        public void ProcessHimoduke_SIM情報の桁数が不正な場合_エラーメッセージを返す()
        {
            // Arrange
            var terminalNo = "123456789012345";
            var icCardNo = "123456789012345678"; // 18桁

            // Act
            var result = businessLogic.ProcessHimoduke(terminalNo, icCardNo, "TestProgram", "TestUser");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].Contains("桁数が正しくないためエラーとなりました"));
        }
    }
}
