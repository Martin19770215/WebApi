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

        #region 通用

        #region 上传 SymbolList
        public ReturnCodeInfo UploadSymbolList(PluginServerInfo Server, List<PluginSymbolInfo> SymbolList)
        {
            //ReturnModel<List<string>> Result = new ReturnModel<List<string>> { ReturnCode=ReturnCode.OK};
            //List<string> lstResult = new List<string>() { $"Existing records cleared " };
            ReturnCodeInfo Result = new ReturnCodeInfo();

            List<string> lstSql = new List<string>() { $"DELETE FROM MT_Symbol WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';" };
            SymbolList.ForEach(sym =>
            {
                lstSql.Add($"INSERT INTO MT_Symbol(`SymbolName`,`SymbolGroup`,`SymbolCurrency`,`SymbolMarginCurrency`,`SymbolDigit`,`SymbolContractSize`,`SymbolMarginMode`,`MainLableName`,`MTType`) VALUES('{sym.symbolName}','{sym.symbolGroup}','{sym.symbolCurrency}','{sym.symbolMarginCurrency}',{sym.symbolDigit},{sym.symbolContractSize},{sym.symbolMarginMode},'{Server.mainLableName}','{Server.mtType}');");
                //lstResult.Add($"Symbol:{sym.symbolName} updated ");
            });

            //int iIndex = 0;

            try
            {
                Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
                //lstSql.ForEach(sql => {

                //    ws_mysql.ExecuteNonQuery(param.ToArray(), PublicConst.CommandTypeDefault, sql, PublicConst.Database);
                //    //lstResult[iIndex] += (ws_mysql.ExecuteNonQuery(param.ToArray(), PublicConst.CommandTypeDefault, sql, PublicConst.Database) > 0) ? "successfully." : "failure.";
                //    //iIndex += 1;
                //});
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadSymbolList/" + Server.pluginName });
                Result.code = ReturnCode.RunningError;
                Result.description = "失败";
                Result.enDescription = "Failure";
                //Result.ReturnCode = ReturnCode.RunningError;
                //lstResult.RemoveRange(iIndex, lstResult.Count);
                //Result.CnDescription = "失败";
                //Result.EnDescription = "Failure";
            }

            //Result.Values = lstResult;
            return Result;
        }
        #endregion

        #endregion

        #region DynamicLeverage

        #region 获取设置信息
        public ReturnModel<List<DynamicLeverageSetting>> DynamicLeverage_GetSettingsList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new ReturnModel<List<DynamicLeverageSetting>> { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Success" };

            List<DynamicLeverageSetting> lstResult = new List<Models.DynamicLeverageSetting>();

            string sSqlSelectValue = $"SELECT OrderType FROM PluginOrders WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}' AND PluginName='{Server.pluginName}';";
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

        #region 获取持仓列表
        public ReturnModel<List<DynamicLeveragePositionInfo>> DynamicLeverage_GetPositionList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeveragePositionInfo>> Result = new ReturnModel<List<DynamicLeveragePositionInfo>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Successfully" };
            List<DynamicLeveragePositionInfo> lstResult = new List<DynamicLeveragePositionInfo>();

            string sSqlSelect = $"SELECT * FROM Riskmanagement_DynamicLeveragePosition WHERE Enable=1 AND MTType='{Server.mtType}' AND MainLableName='{Server.mainLableName.Trim()}' ORDER BY OrderID;";

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new DynamicLeveragePositionInfo
                    {
                        OrderID = int.Parse(mDr["OrderID"].ToString()),
                        Login = int.Parse(mDr["Login"].ToString()),
                        Symbol = mDr["Symbol"].ToString(),
                        Cmd = int.Parse(mDr["Cmd"].ToString()),
                        Currency = mDr["Currency"].ToString(),
                        MarginCurrency = mDr["MarginCurrency"].ToString(),
                        MarginRate = double.Parse(mDr["MarginRate"].ToString()),
                        Volume = int.Parse(mDr["Volume"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getDynamicLeveragePositionList" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                lstResult.Clear();
            }
            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region 获取帐户列表（包括净值）
        public ReturnModel<List<DynamicLeverageEquityInfo>> DynamicLeverage_GetUserList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageEquityInfo>> Result = new ReturnModel<List<DynamicLeverageEquityInfo>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Successfully" };
            List<DynamicLeverageEquityInfo> lstResult = new List<DynamicLeverageEquityInfo>();
            string sSqlSelect = $"SELECT * FROM RiskManagement_DynamicLeverageUser WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';";
            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new DynamicLeverageEquityInfo {
                        Login=int.Parse(mDr["Login"].ToString()),
                        EquityDaily=double.Parse(mDr["EquityDaily"].ToString()),
                        EquityMonthly=double.Parse(mDr["EquityMonthly"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getDynamicLeverageUserList" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                lstResult.Clear();
            }
            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region 上传（更新）持仓信息
        public ReturnCodeInfo DynamicLeverage_UploadPositionList(DynamicLeveragePositionMode mode, PluginServerInfo Server, List<DynamicLeveragePositionInfo> Positions)
        {
            ReturnCodeInfo Result = new ReturnCodeInfo() { code=ReturnCode.OK};

            List<string> lstSql = new List<string>();
            string sSqlCheck = $"SELECT COUNT(OrderID) as iCount FROM Riskmanagement_DynamicLeveragePosition WHERE OrderID={Positions[0].OrderID} AND MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';";

            switch (mode)
            {
                case DynamicLeveragePositionMode.UPDATE_ALL_POSITION:
                    Positions.ForEach(info =>
                    {
                        lstSql.Add($"INSERT INTO Riskmanagement_DynamicLeveragePosition(`OrderId`,`Login`,`Symbol`,`Cmd`,`Currency`,`MarginCurrency`,`MarginRate`,`Volume`,`MTType`,`MainLableName`) VALUES({info.OrderID},{info.Login},'{info.Symbol}',{info.Cmd},'{info.Currency}','{info.MarginCurrency}',{info.MarginRate},{info.Volume},'{Server.mtType}','{Server.mainLableName.Trim()}');");
                    });
                    break;
                case DynamicLeveragePositionMode.UPDATE_NEW_POSITION:
                    lstSql.Add($"INSERT INTO Riskmanagement_DynamicLeveragePosition(`OrderId`,`Login`,`Symbol`,`Cmd`,`Currency`,`MarginCurrency`,`MarginRate`,`Volume`,`MTType`,`MainLableName`) VALUES({Positions[0].OrderID},{Positions[0].Login},'{Positions[0].Symbol}',{Positions[0].Cmd},'{Positions[0].Currency}','{Positions[0].MarginCurrency}',{Positions[0].MarginRate},{Positions[0].Volume},'{Server.mtType}','{Server.mainLableName.Trim()}');");
                    break;
                case DynamicLeveragePositionMode.UPDATE_ACTIVE_POSITION:
                    lstSql.Add($"UPDATE Riskmanagement_DynamicLeveragePosition SET MarginRate={Positions[0].MarginRate},Cmd='{Positions[0].Cmd}' WHERE OrderID={Positions[0].OrderID} AND MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");
                    break;
                case DynamicLeveragePositionMode.UPDATE_RESOTRE_POSITION:
                    lstSql.Add($"UPDATE Riskmanagement_DynamicLeveragePosition SET Enable=1 WHERE OrderID={Positions[0].OrderID} AND MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");
                    break;
                default:
                    //Close(3),Delete(4)
                    lstSql.Add($"UPDATE Riskmanagement_DynamicLeveragePosition SET Enable=0 WHERE OrderID={Positions[0].OrderID} AND MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");
                    break;
            }

            try
            {
                if (mode == DynamicLeveragePositionMode.UPDATE_NEW_POSITION)
                {
                    if (int.Parse(ws_mysql.ExecuteScalar(param.ToArray(), PublicConst.CommandTypeDefault, sSqlCheck, PublicConst.Database)) > 0) { Result.code = ReturnCode.DATA_Existed; return Result; }
                }

                Result.code = (ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database)) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadDynamicLeveragePositionList/" + Server.pluginName });
                Result.code = ReturnCode.RunningError;
            }
            return Result;
        }
        #endregion

        #region 上传帐户净值
        public ReturnCodeInfo DynamicLeverage_UploadUserList(PluginServerInfo Server, uint CurTimeStamp, string CurTime, List<DynamicLeverageEquityInfo> Users)
        {
            ReturnCodeInfo Result = new ReturnCodeInfo();

            List<string> lstSql = new List<string>();
            string sSQLCheck = "";
            DateTime dt;

            if ((CurTimeStamp == 0) || (!DateTime.TryParse(CurTime, out dt))) { Result.code = ReturnCode.DATA_TimeStampError; return Result; }

            try
            {
                Users.ForEach(user => {
                    sSQLCheck = $"SELECT COUNT(ID) AS iCount FROM Riskmanagement_DynamicLeverageUser WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}' AND Login={user.Login};";
                    if (int.Parse(ws_mysql.ExecuteScalar(param.ToArray(), PublicConst.CommandTypeDefault, sSQLCheck, PublicConst.Database)) > 0)
                    {
                        lstSql.Add($"UPDATE Riskmanagement_DynamicLeverageUser SET EquityDaily={user.EquityDaily},EquityMonthly={user.EquityMonthly},CurTimeStamp={CurTimeStamp},CurTime='{CurTime}' WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}' AND Login={user.Login}");
                    }
                    else
                    {
                        lstSql.Add($"INSERT INTO Riskmanagement_DynamicLeverageUser VALUES(NULL,{user.Login},{user.EquityDaily},{user.EquityMonthly},{CurTimeStamp},'{CurTime}','{Server.mainLableName.Trim()}','{Server.mtType}')");
                    }

                    Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
                });

            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadSymbolList/" + Server.pluginName });
                Result.code = ReturnCode.RunningError;
            }
            return Result;
        }

        #endregion

        #endregion

        #region CopyTrader

        #endregion

        #region DelaySlip

        #endregion

        #region QuoteControll

        #endregion

        #region CreditManagement

        #endregion
    }
}