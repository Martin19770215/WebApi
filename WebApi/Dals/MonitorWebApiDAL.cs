using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace WebApi.Dals
{
    public class MonitorWebApiDAL
    {
        com.logicnx.ws.common.WS_COMMON ws_common = new com.logicnx.ws.common.WS_COMMON();
        com.logicnx.ws.mysql.WS_MYSQL ws_mysql = new com.logicnx.ws.mysql.WS_MYSQL();
        List<string> param = new List<string>();

        #region 获取  PluginInfo
        public ReturnModel<List<MonitorDynamicLeveragePluginInfoRet>> getPluginInfo(PluginServerInfo Server)
        {
            ReturnModel<List<MonitorDynamicLeveragePluginInfoRet>> Result = new ReturnModel<List<MonitorDynamicLeveragePluginInfoRet>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Success" };

            List<MonitorDynamicLeveragePluginInfoRet> lstResult = new List<MonitorDynamicLeveragePluginInfoRet>();

            string sSqlSelect = $"SELECT * FROM PluginOrders WHERE PluginName='{Server.pluginName}' AND MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}';";

            try
            {
                DateTime dtStart, dtEnd;

                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    if (!DateTime.TryParse(mDr["OrderDate"].ToString(), out dtStart)) { dtStart = DateTime.Parse(PublicConst.DefaultDateTime); }
                    if (!DateTime.TryParse(mDr["ValidDate"].ToString(), out dtEnd)) { dtEnd = DateTime.Parse(PublicConst.DefaultDateTime); }
                    lstResult.Add(new MonitorDynamicLeveragePluginInfoRet
                    {
                        server = Server,
                        whiteLableName = mDr["WhiteLableName"].ToString().Trim(),
                        startDate = dtStart.ToString("yyyy-MM-dd HH:mm:ss"),
                        endDate = dtEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                        availableDay = (dtEnd - DateTime.Now.Date).Days,
                        enable = mDr["IsDelete"].ToString().Trim().ToUpper() == "N",
                        restartKey = mDr["RestartKey"].ToString().Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MonitorWebApi/getPluginInfo" });
                lstResult.Clear();

                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
            }

            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region 获取  SymbolList
        public ReturnModel<List<PluginSymbolInfo>> getSymbolList(PluginServerInfo Server)
        {
            ReturnModel<List<PluginSymbolInfo>> Result = new ReturnModel<List<PluginSymbolInfo>> { ReturnCode = ReturnCode.OK, CnDescription = "成功" };
            List<PluginSymbolInfo> lstResult = new List<PluginSymbolInfo>();

            string sSqlSelect = $"SELECT * FROM MT_Symbol WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}';";

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new PluginSymbolInfo
                    {
                        symbolName = mDr["SymbolName"].ToString(),
                        symbolGroup = mDr["SymbolGroup"].ToString(),
                        symbolCurrency = mDr["SymbolCurrency"].ToString(),
                        symbolMarginCurrency = mDr["SymbolMarginCurrency"].ToString(),
                        symbolDigit = int.Parse(mDr["SymbolDigit"].ToString()),
                        symbolContractSize = int.Parse(mDr["SymbolContractSize"].ToString()),
                        symbolMarginHedge = int.Parse(mDr["SymbolMarginHedge"].ToString()),
                        symbolMarginMode = int.Parse(mDr["SymbolMarginMode"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MonitorWebApi/getSymbolList/" + Server.pluginName });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                lstResult.Clear();
            }

            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region 获取  PluginSetting

        #region Dynamic Leverage
        public ReturnModel<List<DynamicLeverageSetting>> getRemoteJsonString(MonitorPluginInfo Server, string RemoteJsonURL)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new ReturnModel<List<DynamicLeverageSetting>>();

            try
            {
                string sJsonSetting = ws_common.Post(RemoteJsonURL, JsonConvert.SerializeObject(Server));

                JObject joRules = (JObject)JsonConvert.DeserializeObject(sJsonSetting);

                switch (Server.pluginName.Trim().ToUpper())
                {
                    case "DYNAMICLEVERAGE":
                        Result = getDynamicLeverageSettingList(Server, joRules["value"].ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MonitorWebApi/getRemoteJsonString/" + Server.pluginName });
            }



            return Result;
        }
        #endregion

        #endregion

        #region 获取  LevelDetail
        public ReturnModel<List<MonitorDynamicLeverageSymbolSummary>> getLevelDetail(MonitorPluginInfo Server, uint Login)
        {
            ReturnModel<List<MonitorDynamicLeverageSymbolSummary>> Result = new ReturnModel<List<MonitorDynamicLeverageSymbolSummary>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功" };
            List<MonitorDynamicLeverageSymbolSummary> lstResult = new List<MonitorDynamicLeverageSymbolSummary>();
            List<MonitoryDynamicLeverageSymbolLevelDetail> lstSubResult = new List<MonitoryDynamicLeverageSymbolLevelDetail>();


            string sSqlSelect = $"SELECT Symbol,HedgeVolume,HedgeMargin FROM RiskManagement_DynamicLeverageSymbolSummary WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND Login={Login}";
            string sSqlSubSelect = $"SELECT * FROM RiskManagement_DynamicLeverageSymbolLevelDetail WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND Login={Login} ORDER BY LevelFrom;";
            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSubSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    if (uint.Parse(mDr["NetVolume"].ToString()) > 0)
                    {
                        lstSubResult.Add(new MonitoryDynamicLeverageSymbolLevelDetail
                        {
                            login = Login,
                            symbol = mDr["Symbol"].ToString(),
                            levelFrom = Math.Round(int.Parse(mDr["LevelFrom"].ToString()) / 100.00, 2),
                            levelTo = Math.Round(int.Parse(mDr["LevelTo"].ToString()) / 100.00, 2),
                            leverage = uint.Parse(mDr["LevelLeverage"].ToString()),
                            netVolume = Math.Round(uint.Parse(mDr["NetVolume"].ToString()) / 100.00, 2),
                            levelMargin = Math.Round(double.Parse(mDr["LevelMargin"].ToString()), 2),
                            updateTime = string.Format(mDr["UpdateTime"].ToString(), "yyyy-MM-dd HH:mm:ss")
                        });
                    }
                }

                ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new MonitorDynamicLeverageSymbolSummary {
                        login=Login,
                        symbol=mDr["Symbol"].ToString(),
                        hedgeVolume=Math.Round( double.Parse( mDr["HedgeVolume"].ToString()),2),
                        hedgeMargin=Math.Round(double.Parse(mDr["HedgeMargin"].ToString()),2),
                        details=lstSubResult.Where(level=>level.symbol==mDr["Symbol"].ToString()).ToList<MonitoryDynamicLeverageSymbolLevelDetail>()
                    });
                }

                Result.Values = lstResult;
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MonitorWebApi/getLevelDetail/" + Server.pluginName+"/Login="+Login.ToString() });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                lstResult.Clear();
            }

            return Result;

        }
        #endregion

        #region 上传 AccountSummary
        public ReturnCodeInfo DynamicLeverage_UploadAccountSummary(MonitorPluginInfo Server, List<MonitorDynamicLevergeAccountSummary> creditExposureList)
        {
            ReturnCodeInfo Result = new Models.ReturnCodeInfo();


            return Result;
        }
        #endregion

        #region Private ProcuduresOrFunctions
        private ReturnModel<List<DynamicLeverageSetting>> getDynamicLeverageSettingList(MonitorPluginInfo Server, string sJsonSetting)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new ReturnModel<List<DynamicLeverageSetting>>() { ReturnCode = ReturnCode.OK };

            List<DynamicLeverageSetting> lstResult = new List<DynamicLeverageSetting>();

            List<string> lstSql = new List<string>() { $"DELETE FROM RiskManagement_DynamicLeverageMarginRule WHERE MainLableName='{Server.mainLableName.Trim()}' AND MTType='{Server.mtType}';"};

            if (string.IsNullOrEmpty(sJsonSetting)) { Result.ReturnCode = ReturnCode.EmptySettingString; Result.CnDescription = "设置信息无法识别"; Result.Values = lstResult; return Result; }

            List<DynamicLeverageSettingInfo> lstSettingInfo = new List<DynamicLeverageSettingInfo>();
            List<DynamicLeverageSettingRangeInfo> lstRange = new List<DynamicLeverageSettingRangeInfo>();

            List<string> lstLastUpdateTime = new List<string>() { "19700101" };

            //List<string> lstSymbol = new List<string>();
            List<string> lstSec = new List<string>();
            List<string> lstAccount = new List<string>();
            List<int> lstLeverage = new List<int>();
            //List<int> lstLevelID = new List<int>();

            DateTime dtLastUpdateTime;

            DateTime dtRuleStartTime, dtRuleEndTime;
            int intRuleHedgeLeverage = 100;

            try
            {
                JObject joRules = (JObject)JsonConvert.DeserializeObject(sJsonSetting);
                //string sSetting = joRules["rows"].ToString();
                //List < MonitorDynamicLeverageRuleInfo> lstRule = JsonConvert.DeserializeObject<List<MonitorDynamicLeverageRuleInfo>>(joRules["rows"].ToString());
                List<MonitorDynamicLeverageRuleInfo> lstRule = new JavaScriptSerializer().Deserialize<List<MonitorDynamicLeverageRuleInfo>>(joRules["rows"].ToString());

                lstRule.ForEach(Rule =>
                {
                    if (Rule.Status == "1")
                    {
                        lstSql.Add($"INSERT INTO RiskManagement_DynamicLeverageMarginRule(`RuleID`,`RuleName`,`MainLableName`,`MTType`) VALUES({Rule.id},'{Rule.ruleName}','{Server.mainLableName.Trim()}','{Server.mtType}');");

                        Rule.RulesRelationList.ForEach(RuleRelation =>
                        {
                            RuleRelation.Level.LevelGradeList.ForEach(Level =>
                            {
                                lstRange.Add(new DynamicLeverageSettingRangeInfo
                                {
                                    From = Convert.ToInt32(Math.Round(Level.Begin * 100, 0)),
                                    To = Convert.ToInt32(Math.Round(Level.End * 100, 0)),
                                    Leverage = Level.Lever
                                });
                                lstLeverage.Add(Level.Lever);
                            });
                            if (DateTime.TryParse(RuleRelation.Level.UpdateTime, out dtLastUpdateTime)) { lstLastUpdateTime.Add(dtLastUpdateTime.ToString("yyyyMMddHHmmss")); }

                            if (DateTime.TryParse(RuleRelation.Symbols.UpdateTime, out dtLastUpdateTime)) { lstLastUpdateTime.Add(dtLastUpdateTime.ToString("yyyyMMddHHmmss")); }

                            //lstSymbol = RuleRelation.Symbols.Type == "2" ? new List<string>(RuleRelation.Symbols.Symbol.Split(',')) : new List<string> { "*" };
                            lstSec = RuleRelation.Symbols.Type == "1" ? new List<string>(RuleRelation.Symbols.SecName.Split(',')) : new List<string> { "*" };


                            //lstSymbol.ForEach(sym => {
                                lstSec.ForEach(sec => {
                                    lstSettingInfo.Add(new DynamicLeverageSettingInfo
                                    {
                                        //Symbol = sym,
                                        Symbol= RuleRelation.Symbols.Type == "2" ? RuleRelation.Symbols.Symbol : "*",
                                        Sec = sec,
                                        Ranges = new CommonDAL().DeepCopy<List<DynamicLeverageSettingRangeInfo>>(lstRange),
                                        ExcludeSymbols = (RuleRelation.Symbols.Type == "2" || string.IsNullOrEmpty(RuleRelation.Symbols.Symbol)) ? new List<string>() : new List<string>(RuleRelation.Symbols.Symbol.Split(','))
                                    });
                                //});
                            });

                            lstRange.Clear();
                            //lstSymbol.Clear();
                            lstSec.Clear(); 
                        });

                        if (DateTime.TryParse(Rule.Account.UpdateTime, out dtLastUpdateTime)) { lstLastUpdateTime.Add(dtLastUpdateTime.ToString("yyyyMMddHHmmss")); }

                        if (!DateTime.TryParse(Rule.StartTime, out dtRuleStartTime)) { dtRuleStartTime = DateTime.Parse("1970-01-01 0:0:1"); }
                        if (!DateTime.TryParse(Rule.EndTime, out dtRuleEndTime)) { dtRuleEndTime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " 23:59:59"); }

                        if (!int.TryParse(Rule.hedgingLeverage, out intRuleHedgeLeverage)) { intRuleHedgeLeverage = lstLeverage.Min(); }

                        lstAccount = Rule.Account.Type == "1" ? Rule.Account.MTGroups.Split(',').ToList() : Rule.Account.MTLogins.Split(',').ToList();
                        lstAccount.ForEach(accInfo =>
                        {
                            lstResult.Add(new DynamicLeverageSetting
                            {
                                RuleID = Rule.id,
                                RuleName=Rule.ruleName,
                                Login = Rule.Account.Type == "2" ? UInt64.Parse(accInfo) : 0,
                                Name = Rule.Account.Type == "1" ? accInfo : "*",
                                RuleMode = (DynamicLeverageRuleMode)Enum.Parse(typeof(DynamicLeverageRuleMode), Rule.Type),
                                StartTime = dtRuleStartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                StartTimeStamp=(dtRuleStartTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                                //StartTimeStamp=new DateTimeOffset(dtRuleStartTime.ToUniversalTime()).ToUnixTimeSeconds(),
                                EndTime = dtRuleEndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                EndTimeStamp=(dtRuleEndTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                                //EndTimeStamp=new DateTimeOffset(dtRuleEndTime.ToUniversalTime()).ToUnixTimeSeconds(),
                                HedgeLeverage = intRuleHedgeLeverage,
                                ExcludeLogins = (Rule.Account.Type == "2" || string.IsNullOrEmpty(Rule.Account.MTLogins)) ? new List<ulong>() : new List<UInt64>(Rule.Account.MTLogins.Split(',').Select(UInt64.Parse).ToArray()),

                                Settings = new CommonDAL().DeepCopy<List<DynamicLeverageSettingInfo>>(lstSettingInfo)
                            });
                        });

                        lstLeverage.Clear();
                        lstSettingInfo.Clear();
                    }
                });

                ws_mysql.ExecuteTransactionBySql(lstSql.ToArray(), PublicConst.Database);
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MonitorWebApi/getRemoteJsonString/private/" + Server.pluginName });
                lstResult.Clear();
                Result.ReturnCode = ReturnCode.RunningError;
            }

            Result.Values = lstResult;
            return Result;
        }
        #endregion

    }
}