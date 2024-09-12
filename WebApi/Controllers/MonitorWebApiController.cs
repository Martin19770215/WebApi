using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("MonitorWebApi")]
    public class MonitorWebApiController : Controller
    {
        #region POST Method

        #endregion

        #region GET Method
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPluginInfo(PluginServerInfo server)
        {
            return Json( new MonitorDynamicLeveragePluginInfoRet { server=server},JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}