using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    #region COMMON
    public class ReturnCodeInfo
    {
        public ReturnCode code { get; set; }
        public string description { get; set; }
        public string enDescription { get; set; }

    }

    public class PluginServerInfo
    {
        public string mainLableName { get; set; }
        public string mtType { get; set; }
        public string pluginName { get; set; }
    }

    public class PluginSymbolInfo
    {
        public string symbolName { get; set; }
        public string symbolGroup { get; set; }
        public string symbolCurrency { get; set; }
        public string symbolMarginCurrency { get; set; }
        public int symbolDigit { get; set; }
        public int symbolContractSize { get; set; }
        public int symbolMarginMode { get; set; }
    }

    public class ErrMsg {
        public string RouteName { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class ReturnModel<T> {
        public ReturnCode ReturnCode { get; set; }
        public string CnDescription { get; set; }
        public string EnDescription { get; set; }
        public string NextURL { get; set; }
        public T Values { get; set; }
    }
    #endregion

    #region MONITOR

    #region Dynamic Leverage
    public class MonitorDynamicLeveragePluginInfoRet
    {
        public PluginServerInfo server { get; set; }
        public string WhiteLableName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int availableDay { get; set; }
        public string restartKey { get; set; }
        public bool enable { get; set; }
    }

    [Serializable]
    public class MonitorDynamicLeverageLevelGradeInfo
    {
        public double Begin { get; set; }
        public double End { get; set; }
        public int Lever { get; set; }
    }

    [Serializable]
    public class MonitorDynamicLeverageAccountInfo
    {
        //public int accountSetId { get; set; }
        //public string accountSetName { get; set; }
        public string Type { get; set; }
        public string MTGroups { get; set; }
        public string MTLogins { get; set; }
        //public string createBy { get; set; }
        //public string updateBy { get; set; }
        public string UpdateTime { get; set; }
    }

    [Serializable]
    public class MonitorDynamicLeverageSymbolInfo
    {
        //public string symbolSetId { get; set; }
        //public string symbolSetName { get; set; }
        public string Type { get; set; }
        public string Symbol { get; set; }
        public string SecName { get; set; }
        //public string createBy { get; set; }
        //public string updateBy { get; set; }
        public string UpdateTime { get; set; }
    }

    [Serializable]
    public class MonitorDynamicLeverageLevelInfo
    {
        public int levelId { get; set; }
        ////public string patternType { get; set; }
        ////public string leverName { get; set; }
        //public string explain { get; set; }
        public List<MonitorDynamicLeverageLevelGradeInfo> LevelGradeList { get; set; }
        public string UpdateTime { get; set; }
    }

    public class MonitorDynamicLeverageRuleInfo
    {
        //public string ruleName { get; set; }
        public string Status { get; set; }
        public List<MonitorDynamicLeverageLevelInfo> Level { get; set; }
        public List<MonitorDynamicLeverageAccountInfo> Accounts { get; set; }
        public List<MonitorDynamicLeverageSymbolInfo> Symbols { get; set; }
    }
    #endregion


    #endregion

    #region CRM

    #endregion

    #region MT SYSTEM

    #region Dynamic Leverage
    public class DynamicLeveragePositionInfo
    {
        public int OrderID { get; set; }
        public int Login { get; set; }
        public string Symbol { get; set; }
        public int Cmd { get; set; }
        public int Volume { get; set; }
        public double MarginRate { get; set; }
        public string Currency { get; set; }
        public string MarginCurrency { get; set; }
    }

    [Serializable]
    public class DynamicLeverageSettingRangeInfo
    {
        public int InfoID { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public double Leverage { get; set; }
    }

    [Serializable]
    public class DynamicLeverageSettingInfo
    {
        public int SettingID { get; set; }
        public string Sec { get; set; }     //商品类别
        public string Symbol { get; set; }
        public List<DynamicLeverageSettingRangeInfo> Ranges { get; set; }
    }

    public class DynamicLeverageSetting
    {
        public string Name { get; set; }            //组名
        public int Login { get; set; }              //账号
        public List<DynamicLeverageSettingInfo> Settings { get; set; }
    }

    public class DynamicLeverageEquityInfo
    {
        public int Login { get; set; }
        public double EquityDaily { get; set; }
        public double EquityMonthly { get; set; }
    }

    #endregion

    #endregion

    #region Combination（组合）
    public class MT_SymbolList
    {
        public PluginServerInfo Server { get; set; }
        public List<PluginSymbolInfo> SymbolList { get; set; }
    }

    public class DynamicLeveragePosition
    {
        public DynamicLeveragePositionMode Mode { get; set; }
        public PluginServerInfo Server { get; set; }
        public List<DynamicLeveragePositionInfo> Positions { get; set; }
    }

    public class DynamicLeverageUser
    {
        public PluginServerInfo Server { get; set; }
        public uint CurTimeStamp { get; set; }
        public string CurTime { get; set; }
        public List<DynamicLeverageEquityInfo> Users { get; set; }
    }
    #endregion
}