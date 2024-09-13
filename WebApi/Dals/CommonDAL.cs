using System;
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
            string sSqlUpload = $"INSERT INTO RunningErrorMsg(`PluginName`,`MainLableName`,`ErrorMsg`,`RouteName`) VALUES('{Server.pluginName}','{Server.mainLableName}','{ErrMsg.ErrorMsg.Replace("\'","\"")}','{ErrMsg.RouteName}');";

            try
            {
                ws_mysql.ExecuteNonQuery(param.ToArray(), PublicConst.CommandTypeDefault, sSqlUpload, PublicConst.Database);
            }
            catch
            {

            }
        }

        public ReturnModel<string> getPluginNextURL(PluginServerInfo Server)
        {
            ReturnModel<string> Result = new ReturnModel<string>();

            string sSqlSelect = $"SELECT * from PluginOrders WHERE MainLableName='{Server.mainLableName}' AND MTType='{Server.mtType}' AND PluginName='{Server.pluginName}';";
            try
            {
                DataSet dt = ws_mysql.ExecuteDataSetBySQL(sSqlSelect, PublicConst.Database);
                foreach (DataRow mDr in dt.Tables[0].Rows)
                {
                    Result.Values = mDr["OrderType"].ToString();
                    Result.NextURL = mDr["NextSettingURL"].ToString();
                }
            }
            catch
            {
                Result.ReturnCode = ReturnCode.RunningError;
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