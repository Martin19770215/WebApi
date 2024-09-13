using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Dals
{
    public class MTWebApiDAL
    {
        com.logicnx.ws.mysql.WS_MYSQL ws_mysql = new com.logicnx.ws.mysql.WS_MYSQL();
        List<string> param = new List<string>();

        #region DynamicLeverage

        public ReturnModel<List<DynamicLeverageSetting>> getDynamicLeverageSettingsList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new ReturnModel<List<DynamicLeverageSetting>> { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Success" };

            List<DynamicLeverageSetting> lstResult = new List<Models.DynamicLeverageSetting>();

            string sSqlSelectValue = $"SELECT OrderType FROM PluginOrders WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='{Server.pluginName}';";
            string sSqlSelectResult = "";

            try
            {
                ReturnModel<string> RouteInfo = new CommonDAL().getPluginNextURL(Server);
                switch (RouteInfo.Values)
                {
                    case "Monitor":
                        Result = new MonitorWebApiDAL().getRemoteJsonString(Server, RouteInfo.NextURL);
                        break;
                    case "CRM":
                        
                        break;
                    default:
                        break;
                }

                //DataSet dt = ws_mysql.ExecuteDataSetBySQL(sSelect, PublicConst.Database);
                //foreach (DataRow mDr in dt.Tables[0].Rows)
                //{
                //    lstResult.Add(new DynamicLeverageSetting
                //    {
                        
                //    });
                //}
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getDynamicLeverageSettingsList" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
            }

            Result.Values = lstResult;
            return Result;
        }

        #endregion
    }
}