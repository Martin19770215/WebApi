﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using WebApi.Models;

namespace WebApi.Dals
{
    public class CommonDAL
    {
        com.logicnx.ws.mysql.WS_MYSQL ws_mysql = new com.logicnx.ws.mysql.WS_MYSQL();
        List<string> param = new List<string>();

        public void UploadErrMsg(PluginServerInfo Server,ErrMsg ErrMsg)
        {
            string sSqlUpload = $"INSERT INTO RunningErrorMsg(`PluginName`,`MainLableName`,`ErrorMsg`,`RouteName`) VALUES('{Server.moduleName}','{Server.mainLableName}','{ErrMsg.ErrorMsg.Replace("\'","\"")}','{ErrMsg.RouteName}');";

            try
            {
                ws_mysql.ExecuteNonQuery(param.ToArray(), PublicConst.CommandTypeDefault, sSqlUpload, PublicConst.Database);
            }
            catch
            {

            }
        }

        public void UploadErrMsg(MonitorPluginInfo Server, ErrMsg ErrMsg)
        {
            string sSqlUpload = $"INSERT INTO RunningErrorMsg(`PluginName`,`MainLableName`,`ErrorMsg`,`RouteName`) VALUES('{Server.pluginName}','{Server.mainLableName}','{ErrMsg.ErrorMsg.Replace("\'", "\"")}','{ErrMsg.RouteName}');";

            try
            {
                ws_mysql.ExecuteNonQuery(param.ToArray(), PublicConst.CommandTypeDefault, sSqlUpload, PublicConst.Database);
            }
            catch
            {

            }
        }


        public List<PluginModuleInfo> getPluginModuleList()
        {
            List<PluginModuleInfo> Result = new List<PluginModuleInfo>();

            string sSqlSelect = $"SELECT * from MT_PluginModules;";
            try
            {
                DataSet dt = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in dt.Tables[0].Rows)
                {
                    Result.Add(new PluginModuleInfo {
                        ID=int.Parse(mDr["ID"].ToString()),
                        AccountName=mDr["AccountName"].ToString(),
                        MainLableName=mDr["MainLableName"].ToString(),
                        MTType=mDr["MTType"].ToString(),
                        PluginName=mDr["PluginName"].ToString(),
                        PluginType=mDr["PluginType"].ToString(),
                        ModuleName=mDr["ModuleName"].ToString(),
                        SettingName=mDr["SettingName"].ToString(),
                        SettingURL=mDr["SettingURL"].ToString(),
                    });
                }
            }
            catch
            {
                Result.Clear();
            }

            return Result;
        }

        public PluginModuleInfo getPluginModuleInfo(PluginServerInfo Server)
        {
            PluginModuleInfo Result = new PluginModuleInfo();
            string PluginName = Server.moduleName == "CopyTrader" ? "PAMM" : Server.moduleName;
            List<string> lstAccount = new List<string>();

            string strSqlSelect = $"SELECT * FROM MT_PluginModule WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='{Server.pluginName}' AND ModuleName='{Server.moduleName}' AND SettingName='{Server.settingName}';";
            string strSqlAccount = $"SELECT AccountName FROM pluginorders WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='{PluginName}' AND IsDelete='N' AND ValidDate>='{DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss")}';";
            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(strSqlAccount, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstAccount.Add("'" + mDr["AccountName"].ToString() + "'");
                }

                Result.IsExpired = lstAccount.Count <= 0;

                ds = ws_mysql.ExecuteDataSetBySQL(strSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    Result.AccountName = string.Join(",", lstAccount);
                    Result.MainLableName = mDr["MainLableName"].ToString();
                    Result.MTType = mDr["MTType"].ToString();
                    Result.PluginName = mDr["PluginName"].ToString();
                    Result.PluginType = mDr["PluginType"].ToString();
                    Result.ModuleName = mDr["ModuleName"].ToString();
                    Result.SettingName = mDr["SettingName"].ToString();
                    Result.SettingURL = mDr["SettingURL"].ToString();
                }

            }
            catch
            {
            }

            return Result;
        }

        #region 类深拷贝
        public T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter buf = new BinaryFormatter();
                buf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = buf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
        #endregion

    }
}