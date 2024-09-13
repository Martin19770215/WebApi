﻿using System;
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
        public ReturnModel< List<MonitorDynamicLeveragePluginInfoRet>> getPluginInfo(PluginServerInfo Server)
        {
            ReturnModel<List<MonitorDynamicLeveragePluginInfoRet>> Result = new ReturnModel<List<MonitorDynamicLeveragePluginInfoRet>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Success" };

            List<MonitorDynamicLeveragePluginInfoRet> lstResult = new List<MonitorDynamicLeveragePluginInfoRet>();

            string sSqlSelect = $"SELECT * FROM PluginOrders WHERE PluginName='{Server.pluginName}' AND MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}';";

            try
            {
                DateTime dtStart,dtEnd;

                DataSet ds= ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    if (!DateTime.TryParse(mDr["OrderDate"].ToString(), out dtStart)) { dtStart = DateTime.Parse(PublicConst.DefaultDateTime); }
                    if (!DateTime.TryParse(mDr["ValidDate"].ToString(), out dtEnd)) { dtEnd = DateTime.Parse(PublicConst.DefaultDateTime); }
                    lstResult.Add(new MonitorDynamicLeveragePluginInfoRet
                    {
                        server = Server,
                        WhiteLableName=mDr["WhiteLableName"].ToString().Trim(),
                        startDate = dtStart.ToString("yyyy-MM-dd HH:mm:ss"),
                        endDate = dtEnd.ToString("yyyy-MM-dd HH:mm:ss"),
                        availableDay = (dtEnd - DateTime.Now.Date).Days,
                        enable=mDr["IsDelete"].ToString().Trim().ToUpper()=="Y",
                        restartKey=mDr["RestartKey"].ToString().Trim()
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

        #region 获取  PluginSetting

        #region Dynamic Leverage
        public ReturnModel<List< DynamicLeverageSetting>> getRemoteJsonString(PluginServerInfo Server,string RemoteJsonURL)
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
                new CommonDAL().UploadErrMsg(Server, new ErrMsg {ErrorMsg=ex.Message,RouteName= "MonitorWebApi/getRemoteJsonString/"+Server.pluginName });
            }



            return Result;
        }
        #endregion

        #endregion

        #region Private ProcuduresOrFunctions
        private ReturnModel<List<DynamicLeverageSetting>> getDynamicLeverageSettingList(PluginServerInfo Server, string sJsonSetting)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new ReturnModel<List<DynamicLeverageSetting>>() { ReturnCode = ReturnCode.OK };

            List<DynamicLeverageSetting> lstResult = new List<DynamicLeverageSetting>();

            if (string.IsNullOrEmpty(sJsonSetting)) { Result.ReturnCode = ReturnCode.EmptySettingString; Result.CnDescription = "设置信息无法识别"; Result.Values = lstResult; return Result; }

            List<DynamicLeverageSettingInfo> lstSettingInfo = new List<DynamicLeverageSettingInfo>();
            List<DynamicLeverageSettingRangeInfo> lstRange = new List<DynamicLeverageSettingRangeInfo>();

            List<string> lstLastUpdateTime = new List<string>() { "19700101"};

            List<string> lstSymbol = new List<string>();
            List<string> lstAccount = new List<string>();
            List<int> lstLevelID = new List<int>();

            DateTime dtLastUpdateTime;

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
                        Rule.Level.ForEach(Level =>
                        {
                            lstLevelID.Add(Level.levelId);
                            Level.LevelGradeList.ForEach(RangeInfo =>
                            {
                                lstRange.Add(new DynamicLeverageSettingRangeInfo
                                {
                                    InfoID = Level.levelId,
                                    From = Convert.ToInt32(Math.Round(RangeInfo.Begin * 100, 0)),
                                    To = Convert.ToInt32(Math.Round(RangeInfo.End * 100, 0)),
                                    Leverage = RangeInfo.Lever
                                });
                            });
                            if (DateTime.TryParse(Level.UpdateTime, out dtLastUpdateTime)) { lstLastUpdateTime.Add(dtLastUpdateTime.ToString("yyyyMMddHHmmss")); }
                        });

                        Rule.Symbols.ForEach(sym =>
                        {
                            if (DateTime.TryParse(sym.UpdateTime, out dtLastUpdateTime)) { lstLastUpdateTime.Add(dtLastUpdateTime.ToString("yyyyMMddHHmmss")); }

                            lstSymbol = sym.Type == "1" ? sym.SecName.Split(',').ToList() : sym.Symbol.Split(',').ToList();
                            lstSymbol.ForEach(symInfo =>
                            {
                                lstLevelID.ForEach(LevelID =>
                                {
                                    lstSettingInfo.Add(new DynamicLeverageSettingInfo
                                    {
                                        Symbol = sym.Type == "2" ? symInfo : "*",
                                        Sec = sym.Type == "1" ? symInfo : "*",
                                        Ranges = new CommonDAL().DeepCopy<List<DynamicLeverageSettingRangeInfo>>(lstRange.Where(RangeInfo => RangeInfo.InfoID == LevelID).ToList())
                                    });
                                });
                            });
                        });

                        Rule.Accounts.ForEach(acc =>
                        {
                            if (DateTime.TryParse(acc.UpdateTime, out dtLastUpdateTime)) { lstLastUpdateTime.Add(dtLastUpdateTime.ToString("yyyyMMddHHmmss")); }

                            lstAccount = acc.Type == "1" ? acc.MTGroups.Split(',').ToList() : acc.MTLogins.Split(',').ToList();
                            lstAccount.ForEach(accInfo =>
                            {
                                lstResult.Add(new DynamicLeverageSetting
                                {
                                    Login = acc.Type == "2" ? int.Parse(accInfo) : 0,
                                    Name = acc.Type == "1" ? accInfo : "*",
                                    Settings = new CommonDAL().DeepCopy<List<DynamicLeverageSettingInfo>>(lstSettingInfo)
                                });
                            });
                        });
                    }

                    lstLevelID.Clear();
                    lstRange.Clear();
                    lstSettingInfo.Clear();
                });
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