using System;
using System.Collections.Generic;
using System.Linq;

namespace HX.Terminal.Models
{
    /// <summary>
    /// バリデーション結果クラス
    /// </summary>
    public class ValidationResult
    {
        #region フィールド

        /// <summary>
        /// エラーメッセージリスト
        /// </summary>
        private readonly List<string> errors;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ValidationResult()
        {
            errors = new List<string>();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// バリデーション成功フラグ
        /// </summary>
        public bool IsValid
        {
            get { return !errors.Any(); }
        }

        /// <summary>
        /// エラーメッセージリスト（読み取り専用）
        /// </summary>
        public IReadOnlyList<string> Errors
        {
            get { return errors.AsReadOnly(); }
        }

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// エラーメッセージを追加
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        public void AddError(string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                errors.Add(errorMessage);
            }
        }

        /// <summary>
        /// 複数のエラーメッセージを追加
        /// </summary>
        /// <param name="errorMessages">エラーメッセージリスト</param>
        public void AddErrors(IEnumerable<string> errorMessages)
        {
            if (errorMessages != null)
            {
                foreach (var message in errorMessages.Where(m => !string.IsNullOrWhiteSpace(m)))
                {
                    errors.Add(message);
                }
            }
        }

        /// <summary>
        /// エラーメッセージをクリア
        /// </summary>
        public void ClearErrors()
        {
            errors.Clear();
        }

        /// <summary>
        /// 全てのエラーメッセージを結合した文字列を取得
        /// </summary>
        /// <param name="separator">区切り文字</param>
        /// <returns>結合されたエラーメッセージ</returns>
        public string GetErrorMessage(string separator = "\n")
        {
            return string.Join(separator, errors);
        }

        #endregion
    }

    /// <summary>
    /// 処理結果クラス
    /// </summary>
    /// <typeparam name="T">結果データ型</typeparam>
    public class ProcessResult<T> : ValidationResult
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessResult()
        {
        }

        /// <summary>
        /// コンストラクタ（データ付き）
        /// </summary>
        /// <param name="data">結果データ</param>
        public ProcessResult(T data)
        {
            Data = data;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 処理結果データ
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 処理成功フラグ
        /// </summary>
        public bool IsSuccess
        {
            get { return IsValid; }
        }

        #endregion
    }
}
