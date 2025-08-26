using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HX.Terminal.Models;
using HX.Terminal.DataAccess;

namespace HX.Terminal.BusinessLogic
{
    /// <summary>
    /// 端末登録ビジネスロジック
    /// </summary>
    public class TerminalRegistBusinessLogic
    {
        private readonly TerminalRegistDataAccess dataAccess;

        public TerminalRegistBusinessLogic()
        {
            dataAccess = new TerminalRegistDataAccess();
        }

        /// <summary>
        /// CSVファイルバリデーション
        /// </summary>
        public ValidationResult ValidateCsvFile(HttpPostedFileBase file)
        {
            var result = new ValidationResult();

            // ファイル存在チェック
            if (file == null || file.ContentLength == 0)
            {
                result.AddError("ファイルが選択されていません。");
                return result;
            }

            // 拡張子チェック
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".csv")
            {
                result.AddError("ファイル形式が正しくありません。CSVファイルをアップロードしてください。");
                return result;
            }

            return result;
        }

        /// <summary>
        /// 端末CSVファイル処理
        /// </summary>
        public ValidationResult ProcessTerminalCsvFile(HttpPostedFileBase file, string programId, string userId)
        {
            var result = new ValidationResult();
            var terminalList = new List<TerminalRegistModel>();

            try
            {
                using (var reader = new StreamReader(file.InputStream, Encoding.GetEncoding("Shift_JIS")))
                {
                    string line;
                    int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;
                        
                        // ヘッダー行スキップ
                        if (lineNumber == 1) continue;

                        var columns = line.Split(',');
                        if (columns.Length < 7)
                        {
                            result.AddError($"{lineNumber}行目：列数が不正です。");
                            continue;
                        }

                        var model = new TerminalRegistModel
                        {
                            TerminalNo1 = columns[1]?.Trim(),
                            TerminalNo2 = columns[2]?.Trim(),
                            PinCode = columns[3]?.Trim(),
                            ItemCode = columns[5]?.Trim()
                        };

                        // バリデーション
                        var validationResult = ValidateTerminalModel(model);
                        if (!validationResult.IsValid)
                        {
                            foreach (var error in validationResult.Errors)
                            {
                                result.AddError($"{lineNumber}行目：{error}");
                            }
                            continue;
                        }

                        // 重複チェック
                        if (dataAccess.CheckTerminalDuplicate(model.TerminalNo1))
                        {
                            result.AddError($"端末製造番号：{model.TerminalNo1}は既に登録されているためファイルアップロードできません。");
                            continue;
                        }

                        // ヘッダー情報設定
                        dataAccess.SetHeaderInfo(model, programId, userId);
                        terminalList.Add(model);
                    }
                }

                // エラーがある場合は処理停止
                if (!result.IsValid)
                {
                    return result;
                }

                // DB登録
                foreach (var terminal in terminalList)
                {
                    dataAccess.InsertTerminalRegist(terminal);
                }

                result.Message = $"端末データを{terminalList.Count}件登録しました。";
                return result;
            }
            catch (Exception ex)
            {
                result.AddError($"ファイル処理中にエラーが発生しました：{ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// 端末モデルバリデーション
        /// </summary>
        private ValidationResult ValidateTerminalModel(TerminalRegistModel model)
        {
            var result = new ValidationResult();

            // 端末製造番号1チェック
            if (string.IsNullOrEmpty(model.TerminalNo1))
            {
                result.AddError("端末製造番号（IMEI）1は必須です。");
            }
            else if (model.TerminalNo1.Length != 15)
            {
                result.AddError($"端末製造番号：{model.TerminalNo1}の桁数が正しくないためエラーとなりました。内容を確認し、再度アップロードしてください。");
            }

            // PINコードチェック
            if (!string.IsNullOrEmpty(model.PinCode) && model.PinCode.Length != 5)
            {
                result.AddError("機器PINコードの桁数が正しくありません。");
            }

            // 商品コードチェック
            if (!string.IsNullOrEmpty(model.ItemCode))
            {
                if (model.ItemCode.Length != 8)
                {
                    result.AddError("商品コードの桁数が正しくありません。");
                }
                else if (ContainsFullWidthCharacter(model.ItemCode))
                {
                    result.AddError("商品コードに全角文字が含まれています。");
                }
            }

            return result;
        }

        /// <summary>
        /// 全角文字チェック
        /// </summary>
        private bool ContainsFullWidthCharacter(string input)
        {
            return input.Any(c => c > 127);
        }
    }

    /// <summary>
    /// SIM登録ビジネスロジック
    /// </summary>
    public class SimRegistBusinessLogic
    {
        private readonly SimRegistDataAccess dataAccess;

        public SimRegistBusinessLogic()
        {
            dataAccess = new SimRegistDataAccess();
        }

        /// <summary>
        /// SIM CSVファイル処理
        /// </summary>
        public ValidationResult ProcessSimCsvFile(HttpPostedFileBase file, string programId, string userId)
        {
            var result = new ValidationResult();
            var simList = new List<SimRegistModel>();

            try
            {
                using (var reader = new StreamReader(file.InputStream, Encoding.GetEncoding("Shift_JIS")))
                {
                    string line;
                    int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        lineNumber++;
                        
                        // ヘッダー行スキップ
                        if (lineNumber == 1) continue;

                        var columns = line.Split(',');
                        if (columns.Length < 9)
                        {
                            result.AddError($"{lineNumber}行目：列数が不正です。");
                            continue;
                        }

                        var model = new SimRegistModel
                        {
                            MdpTel = columns[1]?.Trim(),
                            KanyusyaCode = columns[2]?.Trim(),
                            IcCardNo = columns[3]?.Trim(),
                            NetworkPassword = columns[4]?.Trim(),
                            SimUnlockCode = columns[5]?.Trim()
                        };

                        // バリデーション
                        var validationResult = ValidateSimModel(model);
                        if (!validationResult.IsValid)
                        {
                            foreach (var error in validationResult.Errors)
                            {
                                result.AddError($"{lineNumber}行目：{error}");
                            }
                            continue;
                        }

                        // 重複チェック
                        if (dataAccess.CheckSimDuplicate(model.MdpTel, model.IcCardNo))
                        {
                            result.AddError($"端末電話番号：{model.MdpTel}は既に登録されているためファイルアップロードできません。");
                            continue;
                        }

                        // ヘッダー情報設定
                        dataAccess.SetHeaderInfo(model, programId, userId);
                        simList.Add(model);
                    }
                }

                // エラーがある場合は処理停止
                if (!result.IsValid)
                {
                    return result;
                }

                // DB登録
                foreach (var sim in simList)
                {
                    dataAccess.InsertSimRegist(sim);
                }

                result.Message = $"SIMデータを{simList.Count}件登録しました。";
                return result;
            }
            catch (Exception ex)
            {
                result.AddError($"ファイル処理中にエラーが発生しました：{ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// SIMモデルバリデーション
        /// </summary>
        private ValidationResult ValidateSimModel(SimRegistModel model)
        {
            var result = new ValidationResult();

            // 端末電話番号チェック
            if (string.IsNullOrEmpty(model.MdpTel))
            {
                result.AddError("端末電話番号は必須です。");
            }
            else if (model.MdpTel.Length != 11)
            {
                result.AddError($"電話番号{model.MdpTel}の桁数が正しくないためエラーとなりました。内容を確認し、再度アップロードしてください。");
            }

            // 加入者コードチェック
            if (!string.IsNullOrEmpty(model.KanyusyaCode) && model.KanyusyaCode.Length != 8)
            {
                result.AddError("加入者コードの桁数が正しくありません。");
            }

            // ICカード番号チェック
            if (string.IsNullOrEmpty(model.IcCardNo))
            {
                result.AddError("SIM情報（ICCID）は必須です。");
            }
            else if (model.IcCardNo.Length != 19)
            {
                result.AddError("SIM情報（ICCID）の桁数が正しくありません。");
            }

            // ネットワーク暗証番号チェック
            if (!string.IsNullOrEmpty(model.NetworkPassword) && model.NetworkPassword.Length != 4)
            {
                result.AddError("ネットワーク暗証番号の桁数が正しくありません。");
            }

            // SIMロック解除コードチェック
            if (!string.IsNullOrEmpty(model.SimUnlockCode) && model.SimUnlockCode.Length != 8)
            {
                result.AddError("SIMロック解除コードの桁数が正しくありません。");
            }

            return result;
        }
    }

    /// <summary>
    /// 端末-SIM紐付けビジネスロジック
    /// </summary>
    public class TerminalSimHimodukeBusinessLogic
    {
        private readonly TerminalSimHimodukeDataAccess dataAccess;

        public TerminalSimHimodukeBusinessLogic()
        {
            dataAccess = new TerminalSimHimodukeDataAccess();
        }

        /// <summary>
        /// 紐付け処理
        /// </summary>
        public ValidationResult ProcessHimoduke(string terminalNo, string icCardNo, string programId, string userId)
        {
            var result = new ValidationResult();

            // バリデーション
            if (string.IsNullOrEmpty(terminalNo))
            {
                result.AddError("端末製造番号（IMEI）は必須です。");
            }
            else if (terminalNo.Length != 15)
            {
                result.AddError($"端末製造番号：{terminalNo}は桁数が正しくないためエラーとなりました。入力内容を確認し再登録してください。");
            }

            if (string.IsNullOrEmpty(icCardNo))
            {
                result.AddError("SIM情報（ICCID）は必須です。");
            }
            else if (icCardNo.Length != 19)
            {
                result.AddError($"SIM情報（ICCID）：{icCardNo}は桁数が正しくないためエラーとなりました。入力内容を確認し再登録してください。");
            }

            if (!result.IsValid)
            {
                return result;
            }

            // 重複チェック
            if (dataAccess.CheckHimodukeDuplicate(terminalNo, icCardNo))
            {
                result.AddError($"端末製造番号：{terminalNo}またはSIM情報（ICCID）：{icCardNo}は既に登録されているため紐付けできません。");
                return result;
            }

            try
            {
                var model = new TerminalSimHimodukeModel
                {
                    TerminalNo = terminalNo,
                    IcCardNo = icCardNo,
                    RegistDate = DateTime.Now
                };

                // ヘッダー情報設定
                dataAccess.SetHeaderInfo(model, programId, userId);

                // DB登録
                dataAccess.InsertTerminalSimHimoduke(model);

                result.Message = $"端末製造番号：{terminalNo}、SIM情報：{icCardNo}　で紐付け登録しました。";
                return result;
            }
            catch (Exception ex)
            {
                result.AddError($"登録処理中にエラーが発生しました：{ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// 当日の紐付け一覧取得
        /// </summary>
        public List<TerminalSimHimodukeModel> GetTodayHimodukeList(int pageNumber = 1)
        {
            return dataAccess.GetTodayHimodukeList(pageNumber, 20);
        }

        /// <summary>
        /// 紐付け削除処理
        /// </summary>
        public ValidationResult DeleteHimoduke(string terminalNo, string icCardNo)
        {
            var result = new ValidationResult();

            try
            {
                dataAccess.DeleteTerminalSimHimoduke(terminalNo, icCardNo);
                result.Message = "紐付け情報を削除しました。";
                return result;
            }
            catch (Exception ex)
            {
                result.AddError($"削除処理中にエラーが発生しました：{ex.Message}");
                return result;
            }
        }
    }

    /// <summary>
    /// バリデーション結果クラス
    /// </summary>
    public class ValidationResult
    {
        public List<string> Errors { get; set; }
        public string Message { get; set; }
        public bool IsValid => Errors == null || Errors.Count == 0;

        public ValidationResult()
        {
            Errors = new List<string>();
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}
