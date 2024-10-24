using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Threading.Tasks;
using WebApi.Models;
using System.Web.Http;
using Newtonsoft.Json;
using WebApi.Dals;

namespace WebApi.Controllers
{
    public class MonitorController : ApiController
    {
        #region POST Method

        #endregion

        #region GET Method
        [HttpGet]
        public object getPluginInfo(PluginServerInfo server)
        {
            ReturnModel<List<MonitorDynamicLeveragePluginInfoRet>> Result = new MonitorWebApiDAL().getPluginInfo(server);
            return new { code = Result.ReturnCode, description = Result.CnDescription,total=Result.Values.Count, rows = Result.Values };
        }

        [HttpGet]
        public object getSymbolList(PluginServerInfo server)
        {
            ReturnModel<List<PluginSymbolInfo>> Result = new MonitorWebApiDAL().getSymbolList(server);
            return new { code = Result.ReturnCode, description = Result.CnDescription, total = Result.Values.Count, rows = Result.Values };
        }
        [HttpGet]
        public object getLevelDetail(MonitorDynamicLeverageLevelDetailRequestInfo info)
        {
            ReturnModel<List<MonitorDynamicLeverageSymbolSummary>> Result = new MonitorWebApiDAL().getLevelDetail(info.server, info.login);
            return new { code = Result.ReturnCode, description = Result.CnDescription, total = Result.Values.Count, rows = Result.Values };
        }
        #endregion
    }
}