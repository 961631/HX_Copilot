using System;
using System.ComponentModel.DataAnnotations;

namespace HX.Terminal.Models
{
    /// <summary>
    /// 端末情報登録モデル
    /// </summary>
    public class TerminalRegistModel
    {
        #region プロパティ

        /// <summary>
        /// 登録日付・時刻
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 登録プログラムID
        /// </summary>
        public string CreatedProgramId { get; set; }

        /// <summary>
        /// 登録ユーザー（ログインユーザー）
        /// </summary>
        public string CreatedUserId { get; set; }

        /// <summary>
        /// 最終更新日付・時刻
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// 最終更新プログラムID（クラス名）
        /// </summary>
        public string ModifiedProgramId { get; set; }

        /// <summary>
        /// 最終更新ユーザー（ログインユーザー）
        /// </summary>
        public string ModifiedUserId { get; set; }

        /// <summary>
        /// 削除フラグ（0:未削除 1:削除済）
        /// </summary>
        public string DeleteFlg { get; set; } = "0";

        /// <summary>
        /// 端末製造番号（IMEI）1
        /// </summary>
        [Required(ErrorMessage = "端末製造番号（IMEI）1は必須です。")]
        [StringLength(20, MinimumLength = 15, ErrorMessage = "端末製造番号（IMEI）1の桁数が正しくありません。")]
        public string TerminalNo1 { get; set; }

        /// <summary>
        /// 端末製造番号（IMEI）2
        /// </summary>
        [StringLength(20, MinimumLength = 15, ErrorMessage = "端末製造番号（IMEI）2の桁数が正しくありません。")]
        public string TerminalNo2 { get; set; }

        /// <summary>
        /// 機器PINコード
        /// </summary>
        [StringLength(10, MinimumLength = 5, ErrorMessage = "機器PINコードの桁数が正しくありません。")]
        public string PinCode { get; set; }

        /// <summary>
        /// 商品コード
        /// </summary>
        [StringLength(10, MinimumLength = 8, ErrorMessage = "商品コードの桁数が正しくありません。")]
        public string ItemCode { get; set; }

        #endregion
    }
    }

    /// <summary>
    /// SIM情報登録モデル
    /// </summary>
    public class SimRegistModel
    {
        #region プロパティ

        /// <summary>
        /// 登録日付・時刻
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 登録プログラムID
        /// </summary>
        public string CreatedProgramId { get; set; }

        /// <summary>
        /// 登録ユーザー（ログインユーザー）
        /// </summary>
        public string CreatedUserId { get; set; }

        /// <summary>
        /// 最終更新日付・時刻
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// 最終更新プログラムID（クラス名）
        /// </summary>
        public string ModifiedProgramId { get; set; }

        /// <summary>
        /// 最終更新ユーザー（ログインユーザー）
        /// </summary>
        public string ModifiedUserId { get; set; }

        /// <summary>
        /// 削除フラグ（0:未削除 1:削除済）
        /// </summary>
        public string DeleteFlg { get; set; } = "0";

        /// <summary>
        /// 端末電話番号
        /// </summary>
        [Required(ErrorMessage = "端末電話番号は必須です。")]
        [StringLength(13, MinimumLength = 11, ErrorMessage = "端末電話番号の桁数が正しくありません。")]
        public string MdpTel { get; set; }

        /// <summary>
        /// 加入者コード
        /// </summary>
        [Required(ErrorMessage = "加入者コードは必須です。")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "加入者コードの桁数が正しくありません。")]
        public string KanyusyaCode { get; set; }

        /// <summary>
        /// SIM情報（ICCID）
        /// </summary>
        [Required(ErrorMessage = "SIM情報（ICCID）は必須です。")]
        [StringLength(20, MinimumLength = 19, ErrorMessage = "SIM情報（ICCID）の桁数が正しくありません。")]
        public string IcCardNo { get; set; }

        /// <summary>
        /// ネットワーク暗証番号
        /// </summary>
        [Required(ErrorMessage = "ネットワーク暗証番号は必須です。")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "ネットワーク暗証番号の桁数が正しくありません。")]
        public string NetworkPassword { get; set; }

        /// <summary>
        /// SIMロック解除コード
        /// </summary>
        [Required(ErrorMessage = "SIMロック解除コードは必須です。")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "SIMロック解除コードの桁数が正しくありません。")]
        public string SimUnlockCode { get; set; }

        #endregion
    }
    }

    /// <summary>
    /// 端末-SIM紐付けモデル
    /// </summary>
    public class TerminalSimHimodukeModel
    {
        #region プロパティ

        /// <summary>
        /// 登録日付・時刻
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 登録プログラムID
        /// </summary>
        public string CreatedProgramId { get; set; }

        /// <summary>
        /// 登録ユーザー（ログインユーザー）
        /// </summary>
        public string CreatedUserId { get; set; }

        /// <summary>
        /// 最終更新日付・時刻
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// 最終更新プログラムID（クラス名）
        /// </summary>
        public string ModifiedProgramId { get; set; }

        /// <summary>
        /// 最終更新ユーザー（ログインユーザー）
        /// </summary>
        public string ModifiedUserId { get; set; }

        /// <summary>
        /// 削除フラグ（0:未削除 1:削除済）
        /// </summary>
        public string DeleteFlg { get; set; } = "0";

        /// <summary>
        /// 端末製造番号（IMEI）
        /// </summary>
        [Required(ErrorMessage = "端末製造番号（IMEI）は必須です。")]
        [StringLength(20, MinimumLength = 15, ErrorMessage = "端末製造番号（IMEI）の桁数が正しくありません。")]
        public string TerminalNo { get; set; }

        /// <summary>
        /// SIM情報（ICCID）
        /// </summary>
        [Required(ErrorMessage = "SIM情報（ICCID）は必須です。")]
        [StringLength(20, MinimumLength = 19, ErrorMessage = "SIM情報（ICCID）の桁数が正しくありません。")]
        public string IcCardNo { get; set; }

        /// <summary>
        /// 登録日
        /// </summary>
        public DateTime RegistDate { get; set; }

        /// <summary>
        /// R/3連携ファイル作成フラグ（0:未作成 1:作成済み）
        /// </summary>
        public string R3SendFlg { get; set; } = "0";

        /// <summary>
        /// R/3連携日
        /// </summary>
        public DateTime? R3SendDate { get; set; }

        #endregion
    }
}
