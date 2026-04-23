using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using Newtonsoft.Json;
using System.Data;

namespace WebApi.Dals
{
    public class CRMWebApiDAL
    {
        com.logicnx.ws.common.WS_COMMON ws_comm = new com.logicnx.ws.common.WS_COMMON();
        com.logicnx.ws.mysql.WS_MYSQL ws_mysql = new com.logicnx.ws.mysql.WS_MYSQL();
        List<string> param = new List<string>();
        public ReturnModel<string> UploadPositionList(string MainLableName, string MTType)
        {
            ReturnModel<string> Result = new ReturnModel<string>() { Values = "No" };

            string sSqlSelect = $"SELECT * FROM RiskManagement_AdvSwapFeePositions WHERE MainLableName='{MainLableName}' AND MTType='{MTType}' AND AlreadyUpload='N';";

            List<string> lstSqlUpdate = new List<string>();

            Riskmanage_AdvSwapFeeCrmPosition PosList = new Riskmanage_AdvSwapFeeCrmPosition();
            PosList.owner = MainLableName;
            PosList.mType = MTType;
            PosList.data = new List<AdvSwapFeePositionCRMInfo>();

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PosList.data.Add(new AdvSwapFeePositionCRMInfo
                    {
                        positionId = ulong.Parse(dr["Position"].ToString()),
                        login = ulong.Parse(dr["Login"].ToString()),
                        symbol = dr["Symbol"].ToString(),
                        cmd = (dr["Entry"].ToString().ToUpper() == "BUY") ? 0 : 1,
                        volume = ulong.Parse(dr["Volume"].ToString()) / 10e7 * 1.0,
                        openPrice = double.Parse(dr["PriceOpen"].ToString()),
                        currentPrice = double.Parse(dr["PriceCurrent"].ToString()),
                        rate = double.Parse(dr["ProfitRate"].ToString()),
                        currency = dr["ProfitCurrency"].ToString().ToUpper(),
                        swap = double.Parse(dr["Storage"].ToString()),
                        swapDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(dr["TimeStamp"].ToString()) - 28801).UtcDateTime.ToString("yyyyMMdd"), //-3600*8+1
                        swapType = dr["StorageMode"].ToString(),
                        swapTypeValue = (dr["Entry"].ToString().ToUpper() == "BUY") ? double.Parse(dr["StorageLong"].ToString()) : double.Parse(dr["StorageShort"].ToString())
                    });

                    lstSqlUpdate.Add($"UPDATE RiskManagement_AdvSwapFeePositions SET `AlreadyUpload`='Y' WHERE `MainLableName`='{MainLableName}' AND `MTType`='{MTType}' AND `Position`=" + dr["Position"].ToString());
                }
                if (PosList.data.Count > 0)
                {
                    string sPostionList = JsonConvert.SerializeObject(PosList);
                    string PostResult = ws_comm.Post(PublicConst.TransferURL_SwapCRM, sPostionList);
                    ReturnModel<string> Res = JsonConvert.DeserializeObject<ReturnModel<string>>(PostResult);
                    if (Res.ReturnCode == ReturnCode.OK)
                    {
                        AdvSwapFeeCRMReturnInfo CRMRes = JsonConvert.DeserializeObject<AdvSwapFeeCRMReturnInfo>(Res.Values);
                        if (CRMRes.code == 0)
                        {
                            Result.Values = (ws_mysql.ExecuteTransactionBySql(lstSqlUpdate.ToArray(), PublicConst.Database)) ? "Yes" : "No";
                        }
                    }
                }
                else {
                    Result.Values = "Yes";
                }
            }
            catch (Exception ex)
            {
                Result.Values = ex.Message;
            }

            return Result;
        }
    }
}
