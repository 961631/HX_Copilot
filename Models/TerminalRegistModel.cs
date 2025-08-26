using System;
using System.ComponentModel.DataAnnotations;

namespace HX.Terminal.Models
{
    /// <summary>
    /// 端末情報登録モデル
    /// </summary>
    public class TerminalRegistModel
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedProgramId { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedProgramId { get; set; }
        public string ModifiedUserId { get; set; }
        public string DeleteFlg { get; set; } = "0";

        [Required(ErrorMessage = "端末製造番号（IMEI）1は必須です。")]
        [StringLength(20, MinimumLength = 15, ErrorMessage = "端末製造番号（IMEI）1の桁数が正しくありません。")]
        public string TerminalNo1 { get; set; }

        [StringLength(20, MinimumLength = 15, ErrorMessage = "端末製造番号（IMEI）2の桁数が正しくありません。")]
        public string TerminalNo2 { get; set; }

        [StringLength(10, MinimumLength = 5, ErrorMessage = "機器PINコードの桁数が正しくありません。")]
        public string PinCode { get; set; }

        [StringLength(10, MinimumLength = 8, ErrorMessage = "商品コードの桁数が正しくありません。")]
        public string ItemCode { get; set; }
    }

    /// <summary>
    /// SIM情報登録モデル
    /// </summary>
    public class SimRegistModel
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedProgramId { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedProgramId { get; set; }
        public string ModifiedUserId { get; set; }
        public string DeleteFlg { get; set; } = "0";

        [Required(ErrorMessage = "端末電話番号は必須です。")]
        [StringLength(13, MinimumLength = 11, ErrorMessage = "端末電話番号の桁数が正しくありません。")]
        public string MdpTel { get; set; }

        [Required(ErrorMessage = "加入者コードは必須です。")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "加入者コードの桁数が正しくありません。")]
        public string KanyusyaCode { get; set; }

        [Required(ErrorMessage = "SIM情報（ICCID）は必須です。")]
        [StringLength(20, MinimumLength = 19, ErrorMessage = "SIM情報（ICCID）の桁数が正しくありません。")]
        public string IcCardNo { get; set; }

        [Required(ErrorMessage = "ネットワーク暗証番号は必須です。")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "ネットワーク暗証番号の桁数が正しくありません。")]
        public string NetworkPassword { get; set; }

        [Required(ErrorMessage = "SIMロック解除コードは必須です。")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "SIMロック解除コードの桁数が正しくありません。")]
        public string SimUnlockCode { get; set; }
    }

    /// <summary>
    /// 端末-SIM紐付けモデル
    /// </summary>
    public class TerminalSimHimodukeModel
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedProgramId { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedProgramId { get; set; }
        public string ModifiedUserId { get; set; }
        public string DeleteFlg { get; set; } = "0";

        [Required(ErrorMessage = "端末製造番号（IMEI）は必須です。")]
        [StringLength(20, MinimumLength = 15, ErrorMessage = "端末製造番号（IMEI）の桁数が正しくありません。")]
        public string TerminalNo { get; set; }

        [Required(ErrorMessage = "SIM情報（ICCID）は必須です。")]
        [StringLength(20, MinimumLength = 19, ErrorMessage = "SIM情報（ICCID）の桁数が正しくありません。")]
        public string IcCardNo { get; set; }

        public DateTime RegistDate { get; set; }
        public string R3SendFlg { get; set; } = "0";
        public DateTime? R3SendDate { get; set; }
    }
}
