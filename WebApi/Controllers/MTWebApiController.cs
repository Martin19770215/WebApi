using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;
using WebApi.Models;
using WebApi.Dals;

namespace WebApi.Controllers
{
    [System.Web.Mvc.RoutePrefix("/api/MTWebApi")]
    public class MTWebApiController : ApiController
    {
        [HttpPost]
        public object getPluginSettings(PluginServerInfo Server)
        {
            switch (Server.pluginName.ToUpper())
            {
                case "DYNAMICLEVERAGE":
                    ReturnModel<List<DynamicLeverageSetting>> lstDynamicLeverageResult = new MTWebApiDAL().DynamicLeverage_GetSettingsList(Server);
                    return new { Groups = lstDynamicLeverageResult.Values.Where(grp => grp.Login == 0).ToList(), Accounts = lstDynamicLeverageResult.Values.Where(acc => acc.Login != 0).ToList() };
                case "PAMM":
                    ReturnModel<List<MasterAccount>> lstCopyTraderResult = new MTWebApiDAL().COPYTRADER_GetMasterList(Server,true);
                    return new { MasterAccounts = lstCopyTraderResult };
                default:
                    return new { };
            }
        }

        #region DynamicLeverage

        [HttpPost]
        public object getDynamicLeverageSettingsList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new MTWebApiDAL().DynamicLeverage_GetSettingsList(Server);
            try
            {
                return new { Groups = Result.Values.Where(grp => grp.Login == 0).ToList(), Accounts = Result.Values.Where(acc => acc.Login != 0).ToList() };
            }
            catch
            {
                return new { Groups = new List<DynamicLeverageSetting>(), Accounts = new List<DynamicLeverageSetting>() };
            }
            //return new { Groups = Result.Values.Where(grp => grp.Login == 0).ToList(), Accounts = Result.Values.Where(acc => acc.Login != 0).ToList() };
        }

        [HttpPost]
        public object getDynamicLeveragePositionList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeveragePositionInfo>> Result = new MTWebApiDAL().DynamicLeverage_GetPositionList(Server);
            return new { Positions = Result.Values };
        }

        [HttpPost]
        public object getDynamicLeverageUserList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageEquityInfo>> Result = new MTWebApiDAL().DynamicLeverage_GetUserList(Server);
            return new { Users = Result.Values };
        }

        [HttpPost]
        public object UploadSymbolSetting(MT_SymbolList SymbolList)
        {
            ReturnCodeInfo Result = new MTWebApiDAL().UploadSymbolList(SymbolList.Server, SymbolList.Symbols);
            return Result.code;
        }

        [HttpPost]
        public object UploadDynamicLeveragePositionList(DynamicLeveragePosition PositionList)
        {
            ReturnCodeInfo Result = new MTWebApiDAL().DynamicLeverage_UploadPositionList(PositionList.Mode, PositionList.Server, PositionList.Positions);
            return Result.code;
        }

        [HttpPost]
        public object UploadDynamicLeverageUserList(DynamicLeverageUser UserList)
        {
            ReturnCodeInfo Result = new MTWebApiDAL().DynamicLeverage_UploadUserList(UserList.Server, UserList.TimeStamp, UserList.CurTime,UserList.Weekday, UserList.Users);
            return Result.code;
        }

        #endregion

        #region Copy Trader
        [HttpPost]
        public object getCopyTraderSettingsList(PluginServerInfo Server)
        {
            ReturnModel<List<MasterAccount>> lstCopyTraderResult = new MTWebApiDAL().COPYTRADER_GetMasterList(Server, true);
            return new { MasterAccounts = lstCopyTraderResult.Values };
        }
        #endregion


        [HttpPost]
        public object getAdvMCSORules(PluginServerInfo Server)
        {
            ReturnModel<List<RiskManagementAdvMCSOInfo>> Result = new MTWebApiDAL().getAdvMCSORules(Server);
            return new { Groups = Result.Values.Where(grp => grp.Login == 0), Accounts = Result.Values.Where(acc => acc.GroupName == "*") };
        }


        [HttpPost]
        public object UploadErrorMsg(MT_ErrorMsg MsgList)
        {
            ReturnCodeInfo Result = new MTWebApiDAL().UploadErrorMsg(MsgList.Server, MsgList.Messages);
            return Result.code;
        }
    }
}