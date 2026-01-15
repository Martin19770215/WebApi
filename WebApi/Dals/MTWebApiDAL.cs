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
        com.logicnx.ws.common.WS_COMMON ws_common = new com.logicnx.ws.common.WS_COMMON();
        com.logicnx.ws.mysql.WS_MYSQL ws_mysql = new com.logicnx.ws.mysql.WS_MYSQL();
        List<string> param = new List<string>();

        #region 通用

        #region 上传 SymbolList
        public ReturnCodeInfo UploadSymbolList(PluginServerInfo Server, List<PluginSymbolInfo> SymbolList)
        {
            //ReturnModel<List<string>> Result = new ReturnModel<List<string>> { ReturnCode=ReturnCode.OK};
            //List<string> lstResult = new List<string>() { $"Existing records cleared " };
            ReturnCodeInfo Result = new ReturnCodeInfo();

            PluginModuleInfo ModuleInfo = new CommonDAL().getPluginModuleInfo(Server);

            //string strSqlSelect = $"SELECT SettingURL FROM MT_PluginModule WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='{Server.pluginName}' AND ModuleName='{Server.moduleName}' AND SettingName='{Server.settingName}';";
            string strPostData = "{\"server\":{\"mainLableName\":\"" + Server.mainLableName + "\",\"mTType\":\"" + Server.mtType + "\",\"pluginName\":\"" + Server.moduleName + "\"},\"symbolList\":[";
            MonitorReturnInfo PostResult = new MonitorReturnInfo();

            List<string> lstSql = new List<string>() { $"DELETE FROM MT_Symbol WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';" };
            List<string> lstPostData = new List<string>();
            
            SymbolList.ForEach(sym =>
            {
                lstSql.Add($"INSERT INTO MT_Symbol(`SymbolName`,`SymbolGroup`,`SymbolCurrency`,`SymbolMarginCurrency`,`SymbolDigit`,`SymbolContractSize`,`SymbolMarginInit`,`SymbolMarginHedge`,`SymbolMarginRatio`,`SymbolMarginMode`,`MainLableName`,`MTType`) VALUES('{sym.symbolName}','{sym.symbolGroup}','{sym.symbolCurrency}','{sym.symbolMarginCurrency}',{sym.symbolDigit},{sym.symbolContractSize},{sym.symbolMarginInit},{sym.symbolMarginHedge},{sym.symbolMarginRatio},{sym.symbolMarginMode},'{Server.mainLableName}','{Server.mtType}');");
                switch (ModuleInfo.PluginType)
                {
                    case "Monitor":
                        lstPostData.Add("{\"symbol\":\"" + sym.symbolName + "\",\"secName\":\"" + sym.symbolGroup + "\"}");
                        break;
                    case "CRM":
                        break;
                    default:
                        break;
                }
            });

            strPostData += string.Join(",", lstPostData);
            strPostData += "]}";

            //int iIndex = 0;

            try
            {
                PostResult = Newtonsoft.Json.JsonConvert.DeserializeObject<MonitorReturnInfo>(ws_common.Post(ModuleInfo.SettingURL, strPostData));

                Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
                //lstSql.ForEach(sql => {

                //    ws_mysql.ExecuteNonQuery(param.ToArray(), PublicConst.CommandTypeDefault, sql, PublicConst.Database);
                //    //lstResult[iIndex] += (ws_mysql.ExecuteNonQuery(param.ToArray(), PublicConst.CommandTypeDefault, sql, PublicConst.Database) > 0) ? "successfully." : "failure.";
                //    //iIndex += 1;
                //});
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadSymbolList/" + Server.pluginName+"/"+Server.moduleName });
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

        #region 上传运行中的错误信息
        public ReturnCodeInfo UploadErrorMsg(PluginServerInfo Server, List<ErrorMsg> Messages)
        {
            ReturnCodeInfo Result = new ReturnCodeInfo();

            List<string> lstSql = new List<string>();
            try
            {
                Messages.ForEach(msg => {
                    lstSql.Add($"INSERT INTO MT_ErrorMsg(`MainLableName`,`MTType`,`PluginName`,`Message`) VALUES('{Server.mainLableName}','{Server.mtType}','{Server.pluginName}','{msg}');");
                });
                Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadErrorMsg/" + Server.pluginName+"/"+Server.moduleName });
                Result.code = ReturnCode.RunningError;
                Result.description = "失败";
                Result.enDescription = "Failure";
            }
            return Result;
        }
        #endregion

        #endregion

        #region DynamicLeverage

        #region 获取设置信息
        public ReturnModel<List<DynamicLeverageSetting>> DynamicLeverage_GetSettingsList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new ReturnModel<List<DynamicLeverageSetting>> { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Success" };

            //List<DynamicLeverageSetting> lstResult = new List<Models.DynamicLeverageSetting>();

            //string sSqlSelectValue = $"SELECT OrderType FROM PluginOrders WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}' AND PluginName='{Server.pluginName}';";

            try
            {
                PluginModuleInfo ModuleInfo = new CommonDAL().getPluginModuleInfo(Server);

                if (!ModuleInfo.IsExpired)
                {
                    switch (ModuleInfo.PluginType)
                    {
                        case "Monitor":
                            Result = new MonitorWebApiDAL().getRemoteJsonString(new MonitorPluginInfo() { mainLableName = Server.mainLableName, mtType = Server.mtType, pluginName = Server.moduleName }, ModuleInfo.SettingURL,Server.CurrentTimeStamp);
                            break;
                        case "CRM":

                            break;
                        default:
                            //自身系统
                            Result = new CustomerDAL().DYNAMICLEVERAGE_GetSettingList(Server);
                            break;
                    }
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

            //Result.Values = lstResult;
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
                        Login = UInt64.Parse(mDr["Login"].ToString()),
                        Symbol = mDr["Symbol"].ToString(),
                        Cmd = int.Parse(mDr["Cmd"].ToString()),
                        Currency = mDr["Currency"].ToString(),
                        MarginCurrency = mDr["MarginCurrency"].ToString(),
                        MarginRate = double.Parse(mDr["MarginRate"].ToString()),
                        OpenPrice=double.Parse(mDr["OpenPrice"].ToString()),
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
                        Login=UInt64.Parse(mDr["Login"].ToString()),
                        EquityDaily=double.Parse(mDr["EquityDaily"].ToString()),
                        EquityWeekly=double.Parse(mDr["EquityWeekly"].ToString())
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
                case DynamicLeveragePositionMode.UPLOAD_ALL_POSITION:
                    Positions.ForEach(info =>
                    {
                        lstSql.Add($"INSERT INTO Riskmanagement_DynamicLeveragePosition(`OrderId`,`Login`,`Symbol`,`Cmd`,`Currency`,`MarginCurrency`,`MarginRate`,`OpenPrice`,`Volume`,`MTType`,`MainLableName`) VALUES({info.OrderID},{info.Login},'{info.Symbol}',{info.Cmd},'{info.Currency}','{info.MarginCurrency}',{info.MarginRate},{info.OpenPrice},{info.Volume},'{Server.mtType}','{Server.mainLableName.Trim()}');");
                    });
                    break;
                case DynamicLeveragePositionMode.UPLOAD_NEW_POSITION:
                    lstSql.Add($"INSERT INTO Riskmanagement_DynamicLeveragePosition(`OrderId`,`Login`,`Symbol`,`Cmd`,`Currency`,`MarginCurrency`,`MarginRate`,`OpenPrice`,`Volume`,`MTType`,`MainLableName`) VALUES({Positions[0].OrderID},{Positions[0].Login},'{Positions[0].Symbol}',{Positions[0].Cmd},'{Positions[0].Currency}','{Positions[0].MarginCurrency}',{Positions[0].MarginRate},{Positions[0].OpenPrice},{Positions[0].Volume},'{Server.mtType}','{Server.mainLableName.Trim()}');");
                    break;
                case DynamicLeveragePositionMode.UPLOAD_ACTIVE_POSITION:
                    lstSql.Add($"UPDATE Riskmanagement_DynamicLeveragePosition SET MarginRate={Positions[0].MarginRate},Cmd='{Positions[0].Cmd}' WHERE OrderID={Positions[0].OrderID} AND MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");
                    break;
                case DynamicLeveragePositionMode.UPLOAD_RESOTRE_POSITION:
                    lstSql.Add($"UPDATE Riskmanagement_DynamicLeveragePosition SET Enable=1 WHERE OrderID={Positions[0].OrderID} AND MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");
                    break;
                case DynamicLeveragePositionMode.UPLOAD_CLOSE_POSITION:
                case DynamicLeveragePositionMode.UPLOAD_DELETE_POSITION:
                    //Close(3),Delete(4)
                    lstSql.Add($"UPDATE Riskmanagement_DynamicLeveragePosition SET NetVolumeBeforeClosed={Positions[0].NetVolumeBeforeClosed},NetVolumeAfterClosed={Positions[0].NetVolumeAfterClosed},MarginBeforeClosed={Positions[0].MarginBeforeClosed},MarginAfterClosed={Positions[0].MarginAfterClosed},CloseTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', Enable=0 WHERE OrderID={Positions[0].OrderID} AND MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");
                    break;
                default:
                    break;
            }

            try
            {
                //if (mode == DynamicLeveragePositionMode.UPLOAD_NEW_POSITION || mode==DynamicLeveragePositionMode.UPDATE_NEW_POSITION)
                if (mode == DynamicLeveragePositionMode.UPLOAD_NEW_POSITION)
                {
                    if (int.Parse(ws_mysql.ExecuteScalar(param.ToArray(), PublicConst.CommandTypeDefault, sSqlCheck, PublicConst.Database)) > 0) { Result.code = ReturnCode.DATA_Existed; return Result; }
                }

                Result.code = (ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database)) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadDynamicLeveragePositionList/" + Server.pluginName + "/" + Server.moduleName });
                Result.code = ReturnCode.RunningError;
            }
            return Result;
        }
        #endregion

        #region 上传帐户净值（每天结算一次）
        public ReturnCodeInfo DynamicLeverage_UploadUserList(PluginServerInfo Server, uint CurTimeStamp, string CurTime,int WeekDay, List<DynamicLeverageEquityInfo> Users)
        {
            ReturnCodeInfo Result = new ReturnCodeInfo();

            List<string> lstSql = new List<string>();
            List<UInt64> lstSqlUser = new List<UInt64>();

            string sSQLUserCheck = $"SELECT Login FROM Riskmanagement_DynamicLeverageUser WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';";

            DateTime dt;

            if ((CurTimeStamp == 0) || (!DateTime.TryParse(CurTime, out dt))) { Result.code = ReturnCode.DATA_TimeStampError; return Result; }

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSQLUserCheck, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstSqlUser.Add(UInt64.Parse(mDr["Login"].ToString()));
                }

                Users.ForEach(user =>
                {
                    if (lstSqlUser.Exists(SqlUser => SqlUser == user.Login))
                    {
                        if ((DayOfWeek)WeekDay == DayOfWeek.Sunday)
                            lstSql.Add($"UPDATE Riskmanagement_DynamicLeverageUser SET EquityDaily={user.EquityDaily},EquityWeekly={user.EquityDaily},CurTimeStamp={CurTimeStamp},CurTime='{CurTime}',WeekDay={WeekDay} WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}' AND Login={user.Login};");
                        else
                            lstSql.Add($"UPDATE Riskmanagement_DynamicLeverageUser SET EquityDaily={user.EquityDaily},CurTimeStamp={CurTimeStamp},CurTime='{CurTime}',WeekDay={WeekDay} WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}' AND Login={user.Login};");
                    }
                    else
                    {
                        lstSql.Add($"INSERT INTO Riskmanagement_DynamicLeverageUser VALUES(NULL,{user.Login},{user.EquityDaily},{user.EquityDaily},{CurTimeStamp},'{CurTime}',{WeekDay},'{Server.mainLableName.Trim()}','{Server.mtType}');");
                    }
                });

                Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;

            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadSymbolList/" + Server.pluginName + "/" + Server.moduleName });
                Result.code = ReturnCode.RunningError;
            }
            return Result;
        }

        #endregion

        #region 上传帐户信息（帐户资金情况，帐户持仓情况，每25秒更新）
        public ReturnCodeInfo DynamicLeverage_UploadAccountList(PluginServerInfo Server,List<DynamicLeverageSymbolSummaryNodeInfo> Symbols)
        {
            ReturnCodeInfo Result = new ReturnCodeInfo();
            List<string> lstSql = new List<string>();
            List<string> lstSqlCheck = new List<string>();

            string sSQLCheck = $"SELECT Login,Symbol FROM RiskManagement_DynamicLeverageSymbolSummary WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';";

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSQLCheck, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstSqlCheck.Add(mDr["Login"].ToString() + "|" + mDr["Symbol"].ToString());
                }


                lstSql.Add($"DELETE FROM RiskManagement_DynamicLeverageSymbolLevelDetail WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");

                Symbols.ForEach(sym => {
                    if (lstSqlCheck.Exists(info => info == sym.Login.ToString() + "|" + sym.Symbol))
                    {
                        lstSql.Add($"UPDATE RiskManagement_DynamicLeverageSymbolSummary SET `LongDeals`={sym.LongDeals},`LongVolume`={sym.LongVolume},`ShortDeals`={sym.ShortDeals},`ShortVolume`={sym.ShortVolume}, `HedgeVolume`={sym.HedgeMargin},`HedgeMargin`={sym.HedgeMargin}*{sym.AverageRealPrice},`RuleID`={sym.RuleID},`UpdateTime`='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + $"' WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND Login={sym.Login} AND Symbol='{sym.Symbol}';");
                    }
                    else
                    {
                        lstSql.Add($"INSERT RiskManagement_DynamicLeverageSymbolSummary(`Login`,`Symbol`,`LongDeals`,`LongVolume`,`ShortDeals`,`ShortVolume`,`HedgeVolume`,`RuleID`,`MainLableName`,`MTType`) VALUES({sym.Login},'{sym.Symbol}',{sym.LongDeals},{sym.LongVolume},{sym.ShortDeals},{sym.ShortVolume},{sym.HedgeMargin},{sym.RuleID},'{Server.mainLableName}','{Server.mtType}');");
                    }
                    sym.Details.ForEach(detail => {
                        if (detail.NetVolume != 0)
                        {
                            lstSql.Add($"INSERT INTO RiskManagement_DynamicLeverageSymbolLevelDetail(`Login`,`Symbol`,`LevelFrom`,`LevelTo`,`LevelLeverage`,`NetVolume`,`LevelMargin`,`MainLableName`,`MTType`) VALUES({sym.Login},'{sym.Symbol}',{detail.From},{detail.To},{detail.Leverage},{detail.NetVolume},{detail.NetVolume * sym.AverageRealPrice / detail.Leverage},'{Server.mainLableName.Trim()}','{Server.mtType}');");
                        }
                    });
                });

                Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UpdloadAccountList/" + Server.pluginName + "/" + Server.moduleName });
                Result.code = ReturnCode.RunningError;
            }
            return Result;
        }

        public ReturnCodeInfo DynamicLeverage_UploadAccountSummaryList(PluginServerInfo Server, List<DynamicLeverageAccountSummaryInfo> Accounts)
        {
            ReturnCodeInfo Result = new ReturnCodeInfo();
            List<string> lstSql = new List<string>();
            List<string> lstSqlCheck = new List<string>();


            DateTime dtLastLoginTime,dtLastTradingTime, dtLastOpenTime,dtLastCloseTime;
            double Profit = 0.0;

            string sSQLCheck = $"SELECT Login FROM RiskManagement_DynamicLeverageAccount WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';";
            string sSQLSelect = $"SELECT Acc.`Login`,Acc.`Name`,Acc.`Group`,Acc.`LastOpenTime`,Acc.`LastCloseTime`,Summary.`Balance`,Summary.`Credit`,Summary.`Equity`,Summary.`Margin`,Summary.`FreeMargin`,Summary.`MarginLevel`,Summary.`Profit`,Summary.`LastLoginTime`,Summary.`MarginRule`,Summary.`PositionCount` FROM MT_Accounts as Acc,RiskManagement_DynamicLeverageAccount as Summary WHERE Acc.Login=Summary.Login AND Acc.MainLableName='{Server.mainLableName.Trim()}' AND Acc.MTType='{Server.mtType}' AND Summary.MainLableName='{Server.mainLableName.Trim()}' AND Summary.MTType='{Server.mtType}';";
            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSQLCheck, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstSqlCheck.Add(mDr["Login"].ToString());
                }

                Accounts.ForEach(acc =>
                {
                    dtLastLoginTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0), TimeZoneInfo.Local).Add(new TimeSpan(acc.LastLoginTime * 10000000));
                    //dtLastTradeTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1, 0, 0, 0), TimeZoneInfo.Local).Add(new TimeSpan(acc.LastTradeTime * 10000000));
                    Profit = Math.Round(acc.Equity - acc.Balance - acc.Credit, 2);
                    if (lstSqlCheck.Exists(info => info == acc.Login.ToString()))
                    {
                        //if (acc.IsUpdate)
                        //{
                            lstSql.Add("UPDATE RiskManagement_DynamicLeverageAccount SET `Balance`=" + Math.Round(acc.Balance, 2).ToString() + ",`Credit`=" + Math.Round(acc.Credit, 2).ToString() + ",`Equity`=" + Math.Round(acc.Equity, 2).ToString() + ",`Margin`=" + Math.Round(acc.Margin, 2).ToString() + ",`FreeMargin`=" + Math.Round(acc.FreeMargin, 2).ToString() + ",`MarginLevel`=" + Math.Round(acc.MarginLevel, 2).ToString() + "," + (acc.LastLoginTime == 0 ? "" : "`LastLoginTime`='" + dtLastLoginTime.ToString("yyyy-MM-dd HH:mm:ss") + "',")+"`Profit`=" + Profit.ToString() + ",`UpdateTime`='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE MainLableName='" + Server.mainLableName.Trim() + "' AND MTType='" + Server.mtType + "' AND Login=" + acc.Login.ToString() + ";");
                        //}
                        //else if(acc.LastLoginTime>0)
                        //{ 
                        //    lstSql.Add("UPDATE RiskManagement_DynamicLeverageAccount SET `LastLoginTime`='" + dtLastLoginTime.ToString("yyyy-MM-dd HH:mm:ss") + "',`UpdateTime`='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE MainLableName='" + Server.mainLableName.Trim() + "' AND MTType='" + Server.mtType + "' AND Login=" + acc.Login.ToString() + ";");
                        //}
                    }
                    else
                    {
                        lstSql.Add($"INSERT INTO RiskManagement_DynamicLeverageAccount(`Login`,`Balance`,`Credit`,`Equity`,`Margin`,`FreeMargin`,`MarginLevel`,`LastLoginTime`,`Profit`,`MainLableName`,`MTType`,`UpdateTime`) VALUES({acc.Login},{Math.Round(acc.Balance, 2)},{Math.Round(acc.Credit, 2)},{Math.Round(acc.Equity,2)},{Math.Round(acc.Margin, 2)},{Math.Round(acc.FreeMargin, 2)},{Math.Round(acc.MarginLevel, 2)},'{dtLastLoginTime.ToString("yyyy-MM-dd HH:mm:ss")}',{Profit},'{Server.mainLableName.Trim()}','{Server.mtType}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');");
                    }
                });

                Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;

                if (Result.code == ReturnCode.OK)
                {
                    PluginModuleInfo ModuleInfo = new CommonDAL().getPluginModuleInfo(Server);

                    lstSql.Clear();
                    lstSql.Add($"CALL `{PublicConst.Database}`.`CreateIndex`('{ModuleInfo.ReportDataBase}','{ModuleInfo.Index_TableName}','{ModuleInfo.IndexName}','{ModuleInfo.IndexField}');");

                    //bool TmpResult = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database);

                    //param.Clear();
                    //param.Add($"[@DataBaseName],[{ModuleInfo.ReportDataBase}]");
                    //param.Add($"[@TableName],[{ModuleInfo.Index_TableName}]");
                    //param.Add($"[@IndexName],[{ModuleInfo.IndexName}]");
                    //param.Add($"[@IndexField],[{ModuleInfo.IndexField}]");

                    //string TmpResult= ws_mysql.ExecuteScalar(param.ToArray(), PublicConst.StoredProcedure, "CreateIndex", PublicConst.Database);            //创建索引

                    lstSql.Add($"DELETE FROM MT_Accounts WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';");
                    lstSql.Add($"INSERT INTO MT_Accounts(`Login`,`LastOpenTime`,`LastCloseTime`,`MainLableName`,`MTType`) SELECT Login,Max(Open_Time) as LastOpenTime,Max(Close_Time) as LastCloseTime,'{Server.mainLableName.Trim()}' AS MainLableName,'{Server.mtType}' AS MTType FROM `{ModuleInfo.ReportDataBase}`.`{ModuleInfo.Index_TableName}` WHERE `{ModuleInfo.ReportDataBase}`.`{ModuleInfo.Index_TableName}`.`cmd`<2 GROUP BY Login;");
                    lstSql.Add($"UPDATE MT_Accounts Acc,`{ModuleInfo.ReportDataBase}`.`mt4_users` MT_Acc SET Acc.`Name`=MT_Acc.`Name`,Acc.`Group`=MT_Acc.`Group` WHERE Acc.Login=MT_Acc.Login AND Acc.MainLableName='{Server.mainLableName.Trim()}' AND Acc.MTType='{Server.mtType}';");
                    lstSql.Add($"UPDATE RiskManagement_DynamicLeverageAccount Acc,RiskManagement_DynamicLeverageSymbolSummary Summary,RiskManagement_DynamicLeverageMarginRule Rule SET Acc.`MarginRuleID`=Rule.`RuleID`,Acc.`MarginRule`=Rule.`RuleName` WHERE Acc.Login=Summary.Login AND Acc.MainLableName=Summary.MainLableName AND Acc.MTType=Summary.MTType AND Summary.RuleID=Rule.RuleID AND Summary.MainLableName=Rule.MainLableName AND Summary.MTType=Rule.MTType;");
                    lstSql.Add($"UPDATE RiskManagement_DynamicLeverageAccount Acc,(SELECT `Login`, SUM(`LongDeals`)+SUM(`ShortDeals`) AS PositionCount FROM RiskManagement_DynamicLeverageSymbolSummary WHERE `MainLableName`='{Server.mainLableName.Trim()}' AND `MTType`='{Server.mtType}' GROUP BY `Login`) Summary SET Acc.`PositionCount`=Summary.`PositionCount` WHERE Acc.`Login`=Summary.`Login` AND Acc.`MainLableName`='{Server.mainLableName.Trim()}' AND Acc.`MTType`='{Server.mtType}';");

                    Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;

                    if (Result.code == ReturnCode.OK)
                    {
                        //string strSqlSelect = $"SELECT SettingURL FROM MT_PluginModule WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='{Server.pluginName}' AND ModuleName='{Server.moduleName}' AND SettingName='{Server.settingName}';";
                        string strPostData = "{\"server\":{\"mainLableName\":\"" + Server.mainLableName + "\",\"mTType\":\"" + Server.mtType + "\",\"pluginName\":\"" + Server.moduleName + "\"}";
                        switch (ModuleInfo.PluginType)
                        {
                            case "Monitor":

                                #region 上传帐户汇总信息
                                List<MonitorDynamicLevergeAccountSummary> lstMonitorAccount = new List<Models.MonitorDynamicLevergeAccountSummary>();

                                ds = ws_mysql.ExecuteDataSetBySQL(sSQLSelect, PublicConst.Database);
                                foreach (DataRow mDr in ds.Tables[0].Rows)
                                {
                                    if (!DateTime.TryParse(mDr["LastOpenTime"].ToString(), out dtLastOpenTime)) { dtLastOpenTime = DateTime.Parse("1970-1-1 0:0:0"); }
                                    if (!DateTime.TryParse(mDr["LastCloseTime"].ToString(), out dtLastCloseTime)) { dtLastCloseTime = DateTime.Parse("1970-1-1 0:0:0"); }

                                    dtLastTradingTime = (DateTime.Compare(dtLastOpenTime, dtLastCloseTime) > 0) ? dtLastOpenTime : dtLastCloseTime;

                                    lstMonitorAccount.Add(new MonitorDynamicLevergeAccountSummary
                                    {
                                        account = int.Parse(mDr["Login"].ToString()),
                                        name = mDr["Name"].ToString(),
                                        group = mDr["Group"].ToString(),
                                        balance = Math.Round(double.Parse(mDr["Balance"].ToString()), 2),
                                        credit = Math.Round(double.Parse(mDr["Credit"].ToString()), 2),
                                        equity = Math.Round(double.Parse(mDr["Equity"].ToString()), 2),
                                        margin = Math.Round(double.Parse(mDr["Margin"].ToString()), 2),
                                        freeMargin = Math.Round(double.Parse(mDr["FreeMargin"].ToString()), 2),
                                        marginLevel = double.Parse(mDr["MarginLevel"].ToString()).ToString("f2"),
                                        marginRule = mDr["MarginRule"].ToString(),
                                        lastLoginTime = DateTime.Parse(mDr["LastLoginTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                        lastTradingTime = dtLastTradingTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                        pl = Math.Round(double.Parse(mDr["Profit"].ToString()), 2),
                                        positionCount=int.Parse(mDr["PositionCount"].ToString())
                                    });
                                }
                                MonitorReturnInfo PostResult = new MonitorReturnInfo();

                                //List<string> lstPostData = new List<string>();

                                var postdata = Newtonsoft.Json.JsonConvert.SerializeObject(lstMonitorAccount);
                                strPostData += ",\"creditExposureList\":" + postdata.ToString() + "}";

                                PostResult = Newtonsoft.Json.JsonConvert.DeserializeObject<MonitorReturnInfo>(ws_common.Post(ModuleInfo.SettingURL, strPostData));

                                Result.code = PostResult.returnCode;
                                #endregion

                                #region 上传已平仓订单的 净头寸 保证金 信息

                                Server.settingName = "UploadTrade";
                                ModuleInfo = new CommonDAL().getPluginModuleInfo(Server);

                                strPostData = "{\"server\":{\"mainLableName\":\"" + Server.mainLableName + "\",\"mTType\":\"" + Server.mtType + "\",\"pluginName\":\"" + Server.moduleName + "\"}";

                                List<MonitorDynamicLeverageClosedOrder> lstMonitorClosedOrders = new List<MonitorDynamicLeverageClosedOrder>();

                                sSQLSelect = $"SELECT OrderID,NetVolumeBeforeClosed,NetVolumeAfterClosed,MarginBeforeClosed,MarginAfterClosed FROM RiskManagement_DynamicLeveragePosition WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND CloseTime>'{DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss")}';";
                                ds = ws_mysql.ExecuteDataSetBySQL(sSQLSelect, PublicConst.Database);
                                string NetVolumeBeforeClosed = "", NetVolumeAfterClosed = "";
                                foreach (DataRow mDr in ds.Tables[0].Rows)
                                {
                                    NetVolumeBeforeClosed = Math.Round(double.Parse(mDr["NetVolumeBeforeClosed"].ToString()) / 100.00, 2).ToString();
                                    NetVolumeAfterClosed = Math.Round(double.Parse(mDr["NetVolumeAfterClosed"].ToString()) / 100.00, 2).ToString();

                                    lstMonitorClosedOrders.Add(new MonitorDynamicLeverageClosedOrder
                                    {
                                        ticket = int.Parse(mDr["OrderID"].ToString()),
                                        netPositions =  double.Parse(NetVolumeBeforeClosed).ToString("n2") + " - " + double.Parse(NetVolumeAfterClosed).ToString("n2"),
                                        margin =  double.Parse(mDr["MarginBeforeClosed"].ToString()).ToString("n2") + " - " + double.Parse(mDr["MarginAfterClosed"].ToString()).ToString("n2")
                                    });
                                }

                                postdata = Newtonsoft.Json.JsonConvert.SerializeObject(lstMonitorClosedOrders);
                                strPostData += ",\"tradesVo\":" + postdata.ToString() + "}";

                                PostResult = Newtonsoft.Json.JsonConvert.DeserializeObject<MonitorReturnInfo>(ws_common.Post(ModuleInfo.SettingURL, strPostData));

                                Result.code = PostResult.returnCode;

                                #endregion
                                break;
                            case "CRM":
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadAccountSummaryList/" + Server.pluginName + "/" + Server.moduleName });
                Result.code = ReturnCode.RunningError;
            }
            finally {
                param.Clear();
            }
            return Result;
        }
        //public ReturnCodeInfo DynamicLeverage_UpdloadAccountList(PluginServerInfo Server, List<DynamicLeverageAccountSummaryInfo> Accounts)
        //{
        //    ReturnCodeInfo Result = new ReturnCodeInfo();

        //    List<string> lstSql = new List<string>();
        //    List<uint> lstSqlUser = new List<uint>();

        //    string sSQLUserCheck = $"SELECT Login FROM RiskManagement_DynamicLeverageAccount WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';";

        //    try
        //    {
        //        DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSQLUserCheck, PublicConst.Database);
        //        foreach (DataRow mDr in ds.Tables[0].Rows)
        //        {
        //            lstSqlUser.Add(uint.Parse(mDr["Login"].ToString()));
        //        }

        //        Accounts.ForEach(acc =>
        //        {
        //            if (lstSqlUser.Exists(SqlUser => SqlUser == acc.Login))
        //            {
        //                    lstSql.Add($"UPDATE RiskManagement_DynamicLeverageAccount SET `Login`={acc.Login},`Name`='{acc.Name}',`Group`='{acc.Group}',`Balance`={acc.Balance},`Credit`={acc.Credit},`Equity`={acc.Equity},`Margin`={acc.Margin},`FreeMargin`={acc.FreeMargin},`MarginLevel`={acc.MarginLevel},`MarginRuleID`={acc.MarginRuleID},`LastLoginTime`='{acc.LastLoginTime}',`LastTradeTime`='{acc.LastTradeTime}' WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}' AND Login={acc.Login};");
        //            }
        //            else
        //            {
        //                lstSql.Add($"INSERT INTO RiskManagement_DynamicLeverageAccount(`Login`,`Name`,`Group`,`Balance`,`Credit`,`Equity`,`Margin`,`FreeMargin`,`MarginLevel`,`MarginRuleID`,`LastLoginTime`,`LastTradeTime`,`MainLableName`,`MTType`) VALUES({acc.Login},'{acc.Name}','{acc.Group}',{acc.Balance},{acc.Credit},{acc.Equity},{acc.Margin},{acc.FreeMargin},{acc.MarginLevel},{acc.MarginRuleID},'{acc.LastLoginTime},'{acc.LastTradeTime}','{Server.mainLableName.Trim()}','{Server.mtType}');");
        //            }
        //        });

        //        lstSql.Add("UPDATE RiskManagement_DynamicLeverageAccount SET MarginRule=RiskManagement_DynamicLeverageMarginRule.RuleName FROM RiskManagement_DynamicLeverageMarginRule WHERE RiskManagement_DynamicLeverageAccount.MainLableName=RiskManagement_DynamicLeverageMarginRule.MainLableName AND RiskManagement_DynamicLeverageAccount.MTType=RiskManagement_DynamicLeverageMarginRule.MTType AND RiskManagement_DynamicLeverageAccount.MarginRuleID=RiskManagement_DynamicLeverageMarginRule.RuleID;");


        //        Result.code = ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database) ? ReturnCode.OK : ReturnCode.SQL_TransactionErr;
        //    }
        //    catch (Exception ex)
        //    {
        //        new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/UploadSymbolList/" + Server.pluginName + "/" + Server.moduleName });
        //        Result.code = ReturnCode.RunningError;
        //    }

        //    string strPostUserData = "{\"server\":{\"mainLableName\":\"" + Server.mainLableName + "\",\"mTType\":\"" + Server.mtType + "\",\"pluginName\":\"" + Server.moduleName + "\"}";
        //    string strPostTradeData = "{\"server\":{\"mainLableName\":\"" + Server.mainLableName + "\",\"mTType\":\"" + Server.mtType + "\",\"pluginName\":\"" + Server.moduleName + "\"}";

        //    List<string> lstPostUser = new List<string>() ;
        //    List<string> lstPostTrade = new List<string>();

        //    PluginModuleInfo ModuleInfo = new CommonDAL().getPluginModuleInfo(Server);

        //    string strPostUserItem = "";

        //    switch (ModuleInfo.PluginType.ToUpper())
        //    {
        //        case "MONITOR":
        //            Accounts.ForEach(acc => { });
        //            break;
        //        default:
        //            break;
        //    }

        //    Accounts.ForEach(acc => {

        //    });


        //    return Result;
        //}
        #endregion


        #endregion

        #region CopyTrader
        #region 获取主账号信息
        public ReturnModel<List<MasterAccount>> COPYTRADER_GetMasterList(PluginServerInfo Server, bool isIncludSlave)
        {
            ReturnModel<List<MasterAccount>> Result = new ReturnModel<List<MasterAccount>>();
            List<MasterAccount> lstResult = new List<MasterAccount>();

            try
            {
                PluginModuleInfo ModuleInfo = new CommonDAL().getPluginModuleInfo(Server);
                switch (ModuleInfo.PluginType)
                {
                    case "Monitor":
                        
                        break;
                    case "CRM":

                        break;
                    default:
                        //自身系统，通过AccountName获取设置信息
                        Result = new CustomerDAL().COPYTRADER_GetMasterList(ModuleInfo.AccountName, Server, isIncludSlave);
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
                Result.Values.Clear();
            }

            //Result.Values = lstResult;
            return Result;
        }

        #endregion
        #endregion

        #region QuoteControl

        #region 返回调控商品价格
        public ReturnModel<List<SymbolPriceRule>> QuoteControl_getSymbolPriceRules(PluginServerInfo Server)
        {
            ReturnModel<List<SymbolPriceRule>> Result = new ReturnModel<List<SymbolPriceRule>>();
            List<SymbolPriceRule> lstResult = new List<SymbolPriceRule>();

            string sSqlLicense = $"SELECT ValidDate FROM vwPluginOrders WHERE MainLableName='{Server.mainLableName}' AND PluginName='{Server.moduleName}';";
            string sSqlSelect = $"SELECT * FROM vwPosition WHERE MainLableName='{Server.mainLableName}' AND PluginName='{Server.moduleName}';";
            string sSqlUpdate = "";

            DateTime sValidDate;

            string sIDs = "";

            string sID_Rules = "";

            try
            {
                //                ws_mysql.Credentials = new System.Net.NetworkCredential(PublicConst.Sys_WSUserName, PublicConst.Sys_WSUserPWD);

                sValidDate = DateTime.Parse(ws_mysql.ExecuteScalar(param.ToArray(), "", sSqlLicense, PublicConst.Database));
                if (sValidDate > DateTime.Now)
                {
                    DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                    string NextStepSQL = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["ID_Rule"].ToString()))
                        {
                            lstResult.Add(new SymbolPriceRule
                            {
                                RuleID = uint.Parse(dr["ID_Rule"].ToString()),
                                Symbol = dr["Symbol"].ToString(),
                                SymbolOriginal = dr["SymbolOriginal"].ToString(),
                                TargetPrice = double.Parse(dr["TargetPrice"].ToString()),
                                Direct = int.Parse(dr["Direct"].ToString()),
                                Maintenance_PriceMax = double.Parse(dr["Maintenance_PriceMax"].ToString()),
                                Maintenance_PriceMin = double.Parse(dr["Maintenance_PriceMin"].ToString()),
                                IsFixChart = dr["IsFixChart"].ToString(),
                                FixTimeStart = string.IsNullOrEmpty(dr["FixTimeStart"].ToString()) ? 0 : UInt64.Parse(dr["FixTimeStart"].ToString()),
                                FixTimeEnd = string.IsNullOrEmpty(dr["FixTimeEnd"].ToString()) ? 0 : UInt64.Parse(dr["FixTimeEnd"].ToString()),
                                RunningStatus = int.Parse(dr["RunningStatus"].ToString()),
                                RemainingTime = int.Parse(dr["RemainingTime"].ToString()),
                                Volumes = string.IsNullOrEmpty(dr["Volumns"].ToString()) ? 0.0 : double.Parse(dr["Volumns"].ToString()),
                                CurrentPrice = string.IsNullOrEmpty(dr["CurrentPrice"].ToString()) ? 0.0 : double.Parse(dr["CurrentPrice"].ToString())
                            });

                            if (!string.IsNullOrEmpty(dr["ID_Price"].ToString())) { sIDs += dr["ID_Price"].ToString() + ","; }

                            if (!string.IsNullOrEmpty(dr["NextStepSQL"].ToString())) { NextStepSQL += dr["NextStepSQL"].ToString(); }

                            sID_Rules += dr["ID_Rule"].ToString() + ",";
                        }
                    }

                    if (!string.IsNullOrEmpty(sIDs))
                    {
                        sIDs = sIDs.Substring(0, sIDs.Length - 1);
                        sSqlUpdate += $"UPDATE PriceManagement_Prices SET `Enable`='N' WHERE ID IN (" + sIDs + ");";

                    }
                    sID_Rules = sID_Rules.Substring(0, sID_Rules.Length - 1);

                    sSqlUpdate += "UPDATE PriceManagement_Rules SET Volumns=(CASE WHEN RemainingTime<>0 THEN Volumns-Volumns/RemainingTime*5 ELSE 0 END) WHERE RunningStatus='2' AND ID IN (" + sID_Rules + ");";
                    sSqlUpdate += "UPDATE PriceManagement_Rules SET RemainingTime=(CASE WHEN RemainingTime>5 THEN RemainingTime-5 ELSE 0 END) WHERE (RunningStatus='5' OR RunningStatus='2') AND ID IN (" + sID_Rules + ");";
                    sSqlUpdate += "UPDATE PriceManagement_Rules SET RunningStatus='1' WHERE RunningStatus='2' AND RemainingTime=0 AND ID IN (" + sID_Rules + ");";
                    sSqlUpdate += "UPDATE PriceManagement_Rules SET Volumns=0 WHERE RunningStatus='1' AND ID IN (" + sID_Rules + ");";
                    sSqlUpdate += "UPDATE PriceManagement_Rules SET IsFixChart='N',FixTimeStart=0,FixTimeEnd=0 WHERE IsFixChart='Y' AND ID IN (" + sID_Rules + ");";
                    ws_mysql.ExecuteNonQuery(param.ToArray(), "", sSqlUpdate, PublicConst.Database);

                    if (!string.IsNullOrEmpty(NextStepSQL)) { ws_mysql.ExecuteNonQuery(param.ToArray(), "", NextStepSQL, PublicConst.Database); }
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getQuoteControlSymbolPriceRules" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                lstResult.Clear();
            }
            finally
            {
                param.Clear();
            }

            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region 更新商品最新报价到Web
        public ReturnCodeInfo QuoteControl_UploadPriceSymbolInfo(QuoteControlSymbolTickInfo TickInfo)
        {
            ReturnCodeInfo Result = new ReturnCodeInfo();
            Result.code = ReturnCode.OK;
            string sResult = "0";
            string sUpdate = "";

            int i;
            for (i = 0; i <TickInfo.PriceData.Count; i = i + 1)
            {
                if (!string.IsNullOrEmpty(TickInfo.PriceData[i].Symbol))
                {
                    sUpdate += $"INSERT INTO PriceManagement_Ticks(`MainLableName`,`Symbol`,`TimeStamp`,`Bid`,`Ask`,`Digit`) VALUES('{TickInfo.Server.mainLableName}','{TickInfo.PriceData[i].Symbol}',{TickInfo.PriceData[i].Time},{TickInfo.PriceData[i].Bid},{TickInfo.PriceData[i].Ask},{TickInfo.PriceData[i].Digit}) ON DUPLICATE KEY UPDATE TimeStamp={TickInfo.PriceData[i].Time}, Bid={TickInfo.PriceData[i].Bid},Ask={TickInfo.PriceData[i].Ask};";
                    sUpdate += $"UPDATE PriceManagement_Ticks SET Digit ={ TickInfo.PriceData[i].Digit} WHERE MainLableName='{TickInfo.Server.mainLableName}' AND Symbol='{TickInfo.PriceData[i].Symbol}' AND Digit<{TickInfo.PriceData[i].Digit};";
                }
            }

            for (i = 0; i < TickInfo.PriceVolumeData.Count; i = i + 1)
            {
                if (!string.IsNullOrEmpty(TickInfo.PriceVolumeData[i].Symbol))
                {
                    sUpdate += $"UPDATE PriceManagement_Rules SET Volumns={TickInfo.PriceVolumeData[i].Volumes} WHERE ID_Plugin IN (SELECT ID FROM PluginOrders WHERE  MainLableName='{TickInfo.Server.mainLableName}' AND MTType='{TickInfo.Server.mtType}' AND PluginName='{TickInfo.Server.moduleName}') AND RunningStatus<>'2' AND RunningStatus<>'1' AND Symbol='{TickInfo.PriceVolumeData[i].Symbol}';";
                }
            }
            try
            {
                sResult = ws_mysql.ExecuteNonQuery(param.ToArray(), "", sUpdate, PublicConst.Database).ToString();
                Result.description = "更新了 " + sResult + " 条信息";
                Result.enDescription = sResult + " records upload.";
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(TickInfo.Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getDynamicLeverageSettingsList" });
                Result.code = ReturnCode.RunningError;
            }

            return Result;

        }
        #endregion

        #endregion

        #region DelaySlip

        #endregion

        #region RiskManage

        #region AdvMCSO
        public ReturnModel<List<RiskManagementAdvMCSOInfo>> getAdvMCSORules(PluginServerInfo Server)
        {
            ReturnModel<List<RiskManagementAdvMCSOInfo>> Result = new ReturnModel<List<RiskManagementAdvMCSOInfo>>();
            List<RiskManagementAdvMCSOInfo> lstResult = new List<RiskManagementAdvMCSOInfo>();
            string strCount = $"SELECT COUNT(id) AS iCount FROM PluginOrders WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='AdvMCSO' AND ValidDate>='{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}';";
            string strSql = $"SELECT * FROM RiskManagement_AdvMCSOSettings WHERE MTType='{Server.mtType}' AND MainLableName='{Server.mainLableName}' AND Enable=1;";

            try
            {
                if (int.Parse(ws_mysql.ExecuteScalar(param.ToArray(), "", strCount, PublicConst.Database) )==0) { Result.Values = lstResult; return Result; }

                DataSet ds = ws_mysql.ExecuteDataSetBySQL(strSql, PublicConst.Database);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new RiskManagementAdvMCSOInfo
                    {
                        GroupName = dr["GroupName"].ToString().Trim(),
                        Login = UInt64.Parse(dr["Login"].ToString()),
                        MCSOType = int.Parse(dr["MCSOType"].ToString()),
                        SODelayTime = int.Parse(dr["SODelayTime"].ToString()),
                        MCSOManualType = int.Parse(dr["MCSOManualType"].ToString()),
                        MCValue = double.Parse(dr["MCValue"].ToString()),
                        SOValue = double.Parse(dr["SOValue"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getAdvMCSORules" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                lstResult.Clear();
            }
            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region AdvPOLMT
        public ReturnModel<List<RiskManagementAdvPOLMTInfo>> getAdvPOLMTRules(PluginServerInfo Server)
        {
            ReturnModel<List<RiskManagementAdvPOLMTInfo>> Result = new ReturnModel<List<RiskManagementAdvPOLMTInfo>>();
            List<RiskManagementAdvPOLMTInfo> lstResult = new List<RiskManagementAdvPOLMTInfo>();
            string strCount = $"SELECT COUNT(id) AS iCount FROM PluginOrders WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='AdvPOLMT' AND ValidDate>='{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}';";

            string strSql = $"SELECT * FROM RiskManagement_AdvPOLMTSettings WHERE MTType='{Server.mtType}' AND MainLableName='{Server.mainLableName}' AND Enable=1;";

            try
            {
                if (int.Parse(ws_mysql.ExecuteScalar(param.ToArray(), "", strCount, PublicConst.Database)) == 0) { Result.Values = lstResult; return Result; }

                DataSet ds = ws_mysql.ExecuteDataSetBySQL(strSql, PublicConst.Database);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new RiskManagementAdvPOLMTInfo() {
                        GroupName = dr["GroupName"].ToString(),
                        Login = UInt64.Parse(dr["Login"].ToString()),
                        POLMTType=(RiskManage_AdvPOLMT_Type)int.Parse(dr["POLMTTYpe"].ToString()),
                        Symbol=dr["SymbolName"].ToString(),
                        VolumeLimit=UInt64.Parse(dr["VolumeLimit"].ToString()),
                        SummaryType=(RiskManage_AdvPOLMT_SummaryType)int.Parse(dr["SummaryType"].ToString()),
                        HedgeType=(RiskManage_AdvPOLMT_HedgeType)int.Parse(dr["HedgeType"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getAdvPOLMTRules" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                lstResult.Clear();
            }
            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region AdvTransFreq
        public ReturnModel<List<RiskManagementAdvTransFreqInfo>> getAdvTransFreqRules(PluginServerInfo Server)
        {
            ReturnModel<List<RiskManagementAdvTransFreqInfo>> Result = new ReturnModel<List<RiskManagementAdvTransFreqInfo>>();
            List<RiskManagementAdvTransFreqInfo> lstResult = new List<RiskManagementAdvTransFreqInfo>();
            string strCount = $"SELECT COUNT(id) AS iCount FROM PluginOrders WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='AdvTransFreq' AND ValidDate>='{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}';";

            string strSql = $"SELECT * FROM RiskManagement_AdvTransFreqSettings WHERE MTType='{Server.mtType}' AND MainLableName='{Server.mainLableName}' AND Enable=1;";

            try
            {
                if (int.Parse(ws_mysql.ExecuteScalar(param.ToArray(), "", strCount, PublicConst.Database)) == 0) { Result.Values = lstResult; return Result; }

                DataSet ds = ws_mysql.ExecuteDataSetBySQL(strSql, PublicConst.Database);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new RiskManagementAdvTransFreqInfo()
                    {
                        GroupName = dr["GroupName"].ToString(),
                        Login = UInt64.Parse(dr["Login"].ToString()),
                        Symbol = dr["SymbolName"].ToString(),
                        IntervalTime = UInt64.Parse(dr["IntervalTime"].ToString()),
                        SummaryType = (RiskManage_AdvTransFreq_SummaryType)int.Parse(dr["SummaryType"].ToString()),
                        SettingType = (RiskManage_AdvTransFreq_SettingType)int.Parse(dr["SettingType"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/getAdvTransFreqRules" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                lstResult.Clear();
            }
            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #endregion


        #region QuoteControll

        #endregion

        #region CreditManagement

        #endregion
    }
}