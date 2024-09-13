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
        public object getDynamicLeverageSetting(PluginServerInfo Server)
        {
            ReturnModel<List<DynamicLeverageSetting>> Result = new MTWebApiDAL().getDynamicLeverageSettingsList(Server);
            return new { Groups = Result.Values.Where(grp => grp.Login == 0).ToList(), Accounts = Result.Values.Where(acc => acc.Login != 0).ToList() };
        }
    }
}