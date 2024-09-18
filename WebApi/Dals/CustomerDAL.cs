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
        public ReturnModel< List<MasterAccount>> COPYTRADER_GetMasterList(string AccountName, PluginServerInfo Server, bool isIncludeSlave)
        {
            ReturnModel<List<MasterAccount>> Result = new ReturnModel<List<MasterAccount>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Successfully" };
            List<MasterAccount> lstResult = new List<MasterAccount>();
            List<SlaveAccount> lstSlave = new List<SlaveAccount>();

            string sSqlSelectMaster = $"SELECT * FROM PAMM_MasterAcc WHERE AccountName='{AccountName}';";
            string sSqlSelectSlave = $"SELECT * FROM PAMM_SlaveAcc WHERE AccountName='{AccountName}';";

            MasterAccount masteracc = new MasterAccount();

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelectMaster, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new MasterAccount
                    {
                        Login = int.Parse(mDr["AccNo"].ToString()),
                        IsDelete = mDr["IsDelete"].ToString() == "Y",
                        Comment = mDr["Comment"].ToString(),
                        CreateTime = DateTime.Parse(mDr["CreateTime"].ToString()),
                        sCreateTime = mDr["CreateTime"].ToString(),
                        SlaveCount = 0,
                        Slaves = new List<SlaveAccount>()
                    });
                }
                if (isIncludeSlave)
                {
                    ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelectSlave, PublicConst.Database);
                    foreach (DataRow mDr in ds.Tables[0].Rows)
                    {
                        lstSlave.Add(new SlaveAccount
                        {
                            Login = int.Parse(mDr["SlaveAcc"].ToString()),
                            //Symbol = mDr["Symbol"].ToString(),
                            Symbol = "*",
                            //Suffix = mDr["Suffix"].ToString(),
                            //Prefix = mDr["Prefix"].ToString(),
                            Delay = int.Parse(mDr["Delay"].ToString()),
                            //ProportionType = mDr["ProportionType"].ToString(),
                            ProportionType = mDr["Proportion_Type"].ToString(),
                            Proportion = double.Parse(mDr["Proportion"].ToString()),
                            //Pedding = mDr["Pedding"].ToString() == "Y",
                            //SL = mDr["SL"].ToString() == "Y",
                            //TP = mDr["TP"].ToString() == "Y",
                            SL=mDr["IsSL"].ToString()=="Y",
                            TP=mDr["IsTP"].ToString()=="Y",
                            IsFollowClosedOrder = mDr["IsFollowClosedOrder"].ToString() == "Y",
                            MasterLogin = int.Parse(mDr["MasterAcc"].ToString())

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
                lstResult.Clear();
            }
            if (isIncludeSlave)
            {
                lstResult.ForEach(master =>
                {
                    //master.Slaves = new CommonDAL().DeepCopy<List<SlaveAccount>>(lstSlave.Where(slave => slave.MasterLogin == master.Login).ToList());
                    master.Slaves = lstSlave.Where(slave => slave.MasterLogin == master.Login).ToList();
                    master.SlaveCount = master.Slaves.Count;
                });
            }

            Result.Values = lstResult;
            return Result;
        }
        #endregion
    }
}