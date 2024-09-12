using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApi.Models;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [System.Web.Mvc.RoutePrefix("/api/Monitor")]
    public class MonitorController : ApiController
    {
        #region POST Method

        #endregion

        #region GET Method
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
        public MonitorDynamicLeveragePluginInfoRet getPluginInfo(PluginServerInfo server)
        {
            return new MonitorDynamicLeveragePluginInfoRet { server = server,startDate="2024-09-01",endDate="2024-09-30",availableDay=18 };
        }
        #endregion
    }
}