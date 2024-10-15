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
        List<string> param = new List<string>();
        com.logicnx.ws.mysql.WS_MYSQL ws_mysql = new com.logicnx.ws.mysql.WS_MYSQL();

        #region Dynamic Leverage
        public ReturnModel<List<DynamicLeverageSetting>> DYNAMICLEVERAGE_GetSettingList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new ReturnModel<List<DynamicLeverageSetting>>() { ReturnCode=ReturnCode.OK,CnDescription="成功",EnDescription="Successfully"};
            List<DynamicLeverageSetting> lstResult = new List<DynamicLeverageSetting>();

            List<DynamicLeverageSettingRangeInfo> RangeList = new List<DynamicLeverageSettingRangeInfo>();
            List<DynamicLeverageSettingInfo> SettingInfoList = new List<DynamicLeverageSettingInfo>();

            string strSqlLicenseCheck = $"SELECT ValidDate FROM pluginorders WHERE MainLableName = '{Server.mainLableName.Trim()}' AND MTType = '{Server.mtType.Trim()}' AND PluginName = 'DynamicLeverage';";

            string strSqlRange = $"SELECT * FROM RiskManagement_DynamicLeverageSettingRange WHERE MTType='{Server.mtType}' AND MainLableName='{Server.mainLableName.Trim()}' ORDER BY InfoID,`From`;";
            string strSqlInfo = $"SELECT * FROM RiskManagement_DynamicLeverageSettingInfo WHERE MTType='{Server.mtType}' AND MainLableName='{Server.mainLableName.Trim()}';";
            string strSqlSetting = $"SELECT * FROM RiskManagement_DynamicLeverageSetting WHERE MTType='{Server.mtType}' AND MainLableName='{Server.mainLableName.Trim()}';";
            try
            {

                DateTime LicenseDate = DateTime.Parse(ws_mysql.ExecuteScalar(param.ToArray(), "", strSqlLicenseCheck, PublicConst.Database));

                if (DateTime.Compare(LicenseDate, DateTime.Now) < 0) { Result.ReturnCode = ReturnCode.DATA_LicenseTimeOut;Result.Values = lstResult; return Result; }

                DataSet ds = ws_mysql.ExecuteDataSetBySQL(strSqlRange, PublicConst.Database);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RangeList.Add(new DynamicLeverageSettingRangeInfo
                    {
                        InfoID = int.Parse(dr["InfoID"].ToString()),
                        From = int.Parse(dr["From"].ToString()),
                        To = int.Parse(dr["To"].ToString()),
                        Leverage = double.Parse(dr["Leverage"].ToString())
                    });
                }

                ds = ws_mysql.ExecuteDataSetBySQL(strSqlInfo, PublicConst.Database);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SettingInfoList.Add(new DynamicLeverageSettingInfo
                    {
                        SettingID = int.Parse(dr["SettingID"].ToString()),
                        Sec = dr["Sec"].ToString(),
                        Symbol = dr["Symbol"].ToString(),
                        Ranges = RangeList.Where(range => range.InfoID == int.Parse(dr["ID"].ToString())).ToList()
                    });
                }

                ds = ws_mysql.ExecuteDataSetBySQL(strSqlSetting, PublicConst.Database);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new DynamicLeverageSetting
                    {
                        Name = dr["Group"].ToString(),
                        Login = UInt64.Parse(dr["Login"].ToString()),
                        Settings = SettingInfoList.Where(info => info.SettingID == int.Parse(dr["ID"].ToString())).ToList()
                    });
                }
            }
            catch(Exception ex)
            {
                new CommonDAL().UploadErrMsg(Server, new ErrMsg { ErrorMsg = ex.Message, RouteName = "MTWebApi/DYNAMICLEVERAGE_GetSettingList" });
                Result.ReturnCode = ReturnCode.RunningError;
                Result.CnDescription = "失败";
                Result.EnDescription = "Failure";
                lstResult.Clear();
            }
            Result.Values = lstResult;
            return Result;
        }
        #endregion

        #region Copy Trader
        public ReturnModel< List<MasterAccount>> COPYTRADER_GetMasterList(string AccountName, PluginServerInfo Server, bool isIncludeSlave)
        {
            ReturnModel<List<MasterAccount>> Result = new ReturnModel<List<MasterAccount>>() { ReturnCode = ReturnCode.OK, CnDescription = "成功", EnDescription = "Successfully" };
            List<MasterAccount> lstResult = new List<MasterAccount>();
            List<SlaveAccount> lstSlave = new List<SlaveAccount>();
            List<SymbolRelations> lstSymbols = new List<SymbolRelations>();

            string sSqlSelectMaster = $"SELECT * FROM PAMM_MasterAcc WHERE AccountName in ({AccountName}) AND IsDelete='N';";
            string sSqlSelectSlave = $"SELECT * FROM PAMM_SlaveAcc WHERE AccountName in ({AccountName}) AND IsDelete='N';";
            string sSqlSelectSymbols = $"SELECT * FROM PAMM_SymbolRelations WHERE AccountName in ({AccountName}) AND IsDelete='N';";

            MasterAccount masteracc = new MasterAccount();

            try
            {
                DataSet ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelectMaster, PublicConst.Database);
                foreach (DataRow mDr in ds.Tables[0].Rows)
                {
                    lstResult.Add(new MasterAccount
                    {
                        Login = UInt64.Parse(mDr["AccNo"].ToString()),
                        //IsDelete = mDr["IsDelete"].ToString() == "Y",
                        Comment = mDr["Comment"].ToString(),
                        //CreateTime = DateTime.Parse(mDr["CreateTime"].ToString()),
                        //sCreateTime = mDr["CreateTime"].ToString(),
                        SlaveCount = 0,
                        Slaves = new List<SlaveAccount>()
                    });
                }
                if (isIncludeSlave)
                {
                    ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelectSymbols, PublicConst.Database);
                    foreach (DataRow mDr in ds.Tables[0].Rows)
                    {
                        lstSymbols.Add(new SymbolRelations {
                            MasterAcc=UInt64.Parse(mDr["MasterAcc"].ToString()),
                            SlaveAcc=UInt64.Parse(mDr["SlaveAcc"].ToString()),
                            MasterSymbol=mDr["MasterSymbol"].ToString(),
                            SlaveSymbol=mDr["SlaveSymbol"].ToString(),
                        });
                    }

                    ds = ws_mysql.ExecuteDataSetBySQL(sSqlSelectSlave, PublicConst.Database);
                    foreach (DataRow mDr in ds.Tables[0].Rows)
                    {
                        lstSlave.Add(new SlaveAccount
                        {
                            Login = UInt64.Parse(mDr["SlaveAcc"].ToString()),
                            Reverse=mDr["Reverse"].ToString()=="Y",
                            Delay = int.Parse(mDr["Delay"].ToString()),
                            //ProportionType = mDr["ProportionType"].ToString(),
                            ProportionType = mDr["Proportion_Type"].ToString(),
                            Proportion = double.Parse(mDr["Proportion"].ToString()),
                            Pedding = mDr["IsPedding"].ToString() == "Y",
                            SL = mDr["IsSL"].ToString() == "Y",
                            TP = mDr["IsTP"].ToString() == "Y",
                            IsFollowClosedOrder = mDr["IsFollowClosedOrder"].ToString() == "Y",
                            MasterLogin = UInt64.Parse(mDr["MasterAcc"].ToString()),
                            Symbols=lstSymbols.Where(acc=>acc.MasterAcc== UInt64.Parse(mDr["MasterAcc"].ToString()) && acc.SlaveAcc== UInt64.Parse(mDr["SlaveAcc"].ToString())).ToList<SymbolRelations>()
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