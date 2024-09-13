using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApi.Models;
using System.Web.Http;
using Newtonsoft.Json;
using WebApi.Dals;

namespace WebApi.Controllers
{
    [System.Web.Mvc.RoutePrefix("/api/Monitor")]
    public class MonitorController : ApiController
    {
        #region POST Method

        #endregion

        #region GET Method
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
        public object getPluginInfo(PluginServerInfo server)
        {
            ReturnModel<List<MonitorDynamicLeveragePluginInfoRet>> Result = new MonitorWebApiDAL().getPluginInfo(server);
            return new { code = Result.ReturnCode, description = Result.CnDescription,total=Result.Values.Count, rows = Result.Values };
        }


        #endregion
    }
}