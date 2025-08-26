using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using HX.Terminal.Models;

namespace HX.Terminal.DataAccess
{
    /// <summary>
    /// データアクセス基底クラス
    /// </summary>
    public class BaseDataAccess
    {
        protected string connectionString;

        public BaseDataAccess()
        {
            connectionString = ConfigurationManager.ConnectionStrings["HXConnectionString"].ConnectionString;
        }

        /// <summary>
        /// ヘッダー情報設定
        /// </summary>
        protected void SetHeaderInfo<T>(T model, string programId, string userId) where T : class
        {
            var now = DateTime.Now;
            var type = typeof(T);

            type.GetProperty("CreatedDate")?.SetValue(model, now);
            type.GetProperty("CreatedProgramId")?.SetValue(model, programId);
            type.GetProperty("CreatedUserId")?.SetValue(model, userId);
            type.GetProperty("ModifiedDate")?.SetValue(model, now);
            type.GetProperty("ModifiedProgramId")?.SetValue(model, programId);
            type.GetProperty("ModifiedUserId")?.SetValue(model, userId);
        }
    }

    /// <summary>
    /// 端末情報データアクセス
    /// </summary>
    public class TerminalRegistDataAccess : BaseDataAccess
    {
        /// <summary>
        /// 端末情報重複チェック
        /// </summary>
        public bool CheckTerminalDuplicate(string terminalNo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT COUNT(*) 
                    FROM HX.NEW_TERMINAL_REGIST 
                    WHERE TERMINAL_NO_1 = @TerminalNo 
                       OR EXISTS (
                           SELECT 1 FROM HX.MAMORINO_MST 
                           WHERE TERMINAL_NO = @TerminalNo
                       )";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TerminalNo", terminalNo);
                    var count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// 端末情報登録
        /// </summary>
        public void InsertTerminalRegist(TerminalRegistModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    INSERT INTO HX.NEW_TERMINAL_REGIST (
                        CREATED_DATE, CREATED_PROGRAM_ID, CREATED_USER_ID,
                        MODIFIED_DATE, MODIFIED_PROGRAM_ID, MODIFIED_USER_ID,
                        DELETE_FLG, TERMINAL_NO_1, TERMINAL_NO_2, PIN_CD, ITEM_CD
                    ) VALUES (
                        @CreatedDate, @CreatedProgramId, @CreatedUserId,
                        @ModifiedDate, @ModifiedProgramId, @ModifiedUserId,
                        @DeleteFlg, @TerminalNo1, @TerminalNo2, @PinCode, @ItemCode
                    )";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreatedDate", model.CreatedDate);
                    command.Parameters.AddWithValue("@CreatedProgramId", model.CreatedProgramId);
                    command.Parameters.AddWithValue("@CreatedUserId", model.CreatedUserId);
                    command.Parameters.AddWithValue("@ModifiedDate", model.ModifiedDate);
                    command.Parameters.AddWithValue("@ModifiedProgramId", model.ModifiedProgramId);
                    command.Parameters.AddWithValue("@ModifiedUserId", model.ModifiedUserId);
                    command.Parameters.AddWithValue("@DeleteFlg", model.DeleteFlg);
                    command.Parameters.AddWithValue("@TerminalNo1", model.TerminalNo1);
                    command.Parameters.AddWithValue("@TerminalNo2", (object)model.TerminalNo2 ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PinCode", (object)model.PinCode ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ItemCode", (object)model.ItemCode ?? DBNull.Value);
                    
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// SIM情報データアクセス
    /// </summary>
    public class SimRegistDataAccess : BaseDataAccess
    {
        /// <summary>
        /// SIM情報重複チェック
        /// </summary>
        public bool CheckSimDuplicate(string mdpTel, string icCardNo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT COUNT(*) 
                    FROM HX.NEW_SIM_REGIST 
                    WHERE MDP_TEL = @MdpTel OR IC_CARD_NO = @IcCardNo
                       OR EXISTS (
                           SELECT 1 FROM HX.MAMORINO_MST 
                           WHERE MDP_TEL = @MdpTel OR IC_CARD_NO = @IcCardNo
                       )";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MdpTel", mdpTel);
                    command.Parameters.AddWithValue("@IcCardNo", icCardNo);
                    var count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// SIM情報登録
        /// </summary>
        public void InsertSimRegist(SimRegistModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    INSERT INTO HX.NEW_SIM_REGIST (
                        CREATED_DATE, CREATED_PROGRAM_ID, CREATED_USER_ID,
                        MODIFIED_DATE, MODIFIED_PROGRAM_ID, MODIFIED_USER_ID,
                        DELETE_FLG, MDP_TEL, KANYUSYA_CD, IC_CARD_NO, 
                        NETWORK_PASSWORD, SIM_UNLOCK_CD
                    ) VALUES (
                        @CreatedDate, @CreatedProgramId, @CreatedUserId,
                        @ModifiedDate, @ModifiedProgramId, @ModifiedUserId,
                        @DeleteFlg, @MdpTel, @KanyusyaCode, @IcCardNo,
                        @NetworkPassword, @SimUnlockCode
                    )";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreatedDate", model.CreatedDate);
                    command.Parameters.AddWithValue("@CreatedProgramId", model.CreatedProgramId);
                    command.Parameters.AddWithValue("@CreatedUserId", model.CreatedUserId);
                    command.Parameters.AddWithValue("@ModifiedDate", model.ModifiedDate);
                    command.Parameters.AddWithValue("@ModifiedProgramId", model.ModifiedProgramId);
                    command.Parameters.AddWithValue("@ModifiedUserId", model.ModifiedUserId);
                    command.Parameters.AddWithValue("@DeleteFlg", model.DeleteFlg);
                    command.Parameters.AddWithValue("@MdpTel", model.MdpTel);
                    command.Parameters.AddWithValue("@KanyusyaCode", model.KanyusyaCode);
                    command.Parameters.AddWithValue("@IcCardNo", model.IcCardNo);
                    command.Parameters.AddWithValue("@NetworkPassword", model.NetworkPassword);
                    command.Parameters.AddWithValue("@SimUnlockCode", model.SimUnlockCode);
                    
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// 端末-SIM紐付けデータアクセス
    /// </summary>
    public class TerminalSimHimodukeDataAccess : BaseDataAccess
    {
        /// <summary>
        /// 紐付け情報重複チェック
        /// </summary>
        public bool CheckHimodukeDuplicate(string terminalNo, string icCardNo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT COUNT(*) 
                    FROM HX.TERMINAL_SIM_HIMODUKE 
                    WHERE TERMINAL_NO = @TerminalNo OR IC_CARD_NO = @IcCardNo";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TerminalNo", terminalNo);
                    command.Parameters.AddWithValue("@IcCardNo", icCardNo);
                    var count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// 紐付け情報登録
        /// </summary>
        public void InsertTerminalSimHimoduke(TerminalSimHimodukeModel model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    INSERT INTO HX.TERMINAL_SIM_HIMODUKE (
                        CREATED_DATE, CREATED_PROGRAM_ID, CREATED_USER_ID,
                        MODIFIED_DATE, MODIFIED_PROGRAM_ID, MODIFIED_USER_ID,
                        DELETE_FLG, TERMINAL_NO, IC_CARD_NO, REGIST_DATE, R3_SEND_FLG
                    ) VALUES (
                        @CreatedDate, @CreatedProgramId, @CreatedUserId,
                        @ModifiedDate, @ModifiedProgramId, @ModifiedUserId,
                        @DeleteFlg, @TerminalNo, @IcCardNo, @RegistDate, @R3SendFlg
                    )";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CreatedDate", model.CreatedDate);
                    command.Parameters.AddWithValue("@CreatedProgramId", model.CreatedProgramId);
                    command.Parameters.AddWithValue("@CreatedUserId", model.CreatedUserId);
                    command.Parameters.AddWithValue("@ModifiedDate", model.ModifiedDate);
                    command.Parameters.AddWithValue("@ModifiedProgramId", model.ModifiedProgramId);
                    command.Parameters.AddWithValue("@ModifiedUserId", model.ModifiedUserId);
                    command.Parameters.AddWithValue("@DeleteFlg", model.DeleteFlg);
                    command.Parameters.AddWithValue("@TerminalNo", model.TerminalNo);
                    command.Parameters.AddWithValue("@IcCardNo", model.IcCardNo);
                    command.Parameters.AddWithValue("@RegistDate", model.RegistDate);
                    command.Parameters.AddWithValue("@R3SendFlg", model.R3SendFlg);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 当日の紐付け情報一覧取得
        /// </summary>
        public List<TerminalSimHimodukeModel> GetTodayHimodukeList(int pageNumber = 1, int pageSize = 20)
        {
            var list = new List<TerminalSimHimodukeModel>();
            
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT * FROM (
                        SELECT ROW_NUMBER() OVER (ORDER BY CREATED_DATE DESC) AS RowNum,
                               TERMINAL_NO, IC_CARD_NO, REGIST_DATE
                        FROM HX.TERMINAL_SIM_HIMODUKE
                        WHERE CONVERT(DATE, REGIST_DATE) = CONVERT(DATE, GETDATE())
                          AND DELETE_FLG = '0'
                    ) AS T
                    WHERE T.RowNum BETWEEN @StartRow AND @EndRow";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartRow", (pageNumber - 1) * pageSize + 1);
                    command.Parameters.AddWithValue("@EndRow", pageNumber * pageSize);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TerminalSimHimodukeModel
                            {
                                TerminalNo = reader["TERMINAL_NO"].ToString(),
                                IcCardNo = reader["IC_CARD_NO"].ToString(),
                                RegistDate = Convert.ToDateTime(reader["REGIST_DATE"])
                            });
                        }
                    }
                }
            }
            
            return list;
        }

        /// <summary>
        /// 紐付け情報削除
        /// </summary>
        public void DeleteTerminalSimHimoduke(string terminalNo, string icCardNo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    DELETE FROM HX.TERMINAL_SIM_HIMODUKE 
                    WHERE TERMINAL_NO = @TerminalNo AND IC_CARD_NO = @IcCardNo";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TerminalNo", terminalNo);
                    command.Parameters.AddWithValue("@IcCardNo", icCardNo);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
