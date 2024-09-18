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

        [HttpPost]
        public object getDynamicLeverageSettingsList(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new MTWebApiDAL().DynamicLeverage_GetSettingsList(Server);
            return new { Groups = Result.Values.Where(grp => grp.Login == 0).ToList(), Accounts = Result.Values.Where(acc => acc.Login != 0).ToList() };
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
            ReturnModel<List<DynamicLeverageEquityInfo>> UserList = new MTWebApiDAL().DynamicLeverage_GetUserList(Server);
            return new { Users = UserList };
        }

        [HttpPost]
        public object UploadSymbolSetting(MT_SymbolList SymbolList)
        {
            ReturnCodeInfo Result = new MTWebApiDAL().UploadSymbolList(SymbolList.Server, SymbolList.SymbolList);
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
            ReturnCodeInfo Result = new MTWebApiDAL().DynamicLeverage_UploadUserList(UserList.Server, UserList.CurTimeStamp, UserList.CurTime, UserList.Users);
            return Result.code;
        }
    }
}