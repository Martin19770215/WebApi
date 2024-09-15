using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Dals
{
    //非Monitor，CRM
    public class CustomerDAL
    {
        com.logicnx.ws.mysql.WS_MYSQL ws_mysql = new com.logicnx.ws.mysql.WS_MYSQL();
        #region Copy Trader
        public ReturnModel< List<MasterAccount>> COPYTRADER_GetMasterList(string AccountName, PluginServerInfo Server, bool IsIncludeSlave)
        {
            ReturnModel<List<MasterAccount>> Result = new ReturnModel<List<MasterAccount>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Successfully" };
            List<SlaveAccount> lstSlave = new List<SlaveAccount>();

            string sSqlSelectMaster = $"SELECT * FROM PAMM_MasterAcc WHERE AccountName='{AccountName}';";
            string sSqlSelectSlave = $"SELECT * FROM PAMM_SlaveAcc WHERE AccountName='{AccountName}';";

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelectMaster, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    Result.Values.Add(new MasterAccount
                    {
                        Login = int.Parse(mDr["Login"].ToString()),
                        IsDelete = mDr["IsDelete"].ToString() == "Y",
                        Comment = mDr["Comment"].ToString(),
                        CreateTime = DateTime.Parse(mDr["CreateTime"].ToString()),
                        sCreateTime = mDr["CreateTime"].ToString()
                    });
                }
                if (IsIncludeSlave)
                {
                    ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelectSlave, PublicConst.Database);
                    foreach (DataRow mDr in ds.Tables[0].Rows)
                    {
                        lstSlave.Add(new SlaveAccount
                        {
                            Login = int.Parse(mDr["Login"].ToString()),
                            Symbol = mDr["Symbol"].ToString(),
                            Suffix = mDr["Suffix"].ToString(),
                            Prefix = mDr["Prefix"].ToString(),
                            Delay = int.Parse(mDr["Delay"].ToString()),
                            ProportionType = mDr["ProportionType"].ToString(),
                            Proportion = double.Parse(mDr["Proportion"].ToString()),
                            Pedding = mDr["Pedding"].ToString() == "Y",
                            SL = mDr["SL"].ToString() == "Y",
                            TP = mDr["TP"].ToString() == "Y",
                            IsFollowClosedOrder = mDr["IsFollowClosedOrder"].ToString() == "Y",
                            MasterLogin = int.Parse(mDr["MasterLogin"].ToString())

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/COPYTRADER_GetMasterList" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                Result.Values.Clear();
            }
            if (IsIncludeSlave)
            {
                Result.Values.ForEach(master =>
                {
                    master.Slaves = new CommonDAL().DeepCopy<List<SlaveAccount>>(lstSlave.Where(slave => slave.MasterLogin == master.Login).ToList());
                    master.SlaveCount = master.Slaves.Count;
                });
            }

            return Result;
        }
        #endregion
    }
}