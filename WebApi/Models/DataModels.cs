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
        public string moduleName { get; set; }
        public string settingName { get; set; }
    }

    public class PluginSymbolInfo
    {
        public string symbolName { get; set; }
        public string symbolGroup { get; set; }
        public string symbolCurrency { get; set; }
        public string symbolMarginCurrency { get; set; }
        public int symbolDigit { get; set; }
        public double symbolContractSize { get; set; }
        public int symbolMarginMode { get; set; }
        public double symbolMarginInit { get; set; }
        public double symbolMarginHedge { get; set; }
        public double symbolMarginRatio { get; set; }
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

    public class PluginModuleInfo {
        public int ID { get; set; }
        public string AccountName { get; set; }
        public string MainLableName { get; set; }
        public string MTType { get; set; }
        public string PluginName { get; set; }
        public string PluginType { get; set; }
        public string ModuleName { get; set; }
        public string SettingName { get; set; }
        public string SettingURL { get; set; }
    }
    #endregion

    #region MONITOR
    public class MonitorPluginInfo {
        public string mainLableName { get; set; }
        public string mtType { get; set; }
        public string pluginName { get; set; }

    }

    #region Dynamic Leverage
    public class MonitorDynamicLeveragePluginInfoRet
    {
        public PluginServerInfo server { get; set; }
        public string whiteLableName { get; set; }
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
        public string ExcludeLogins { get; set; }
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
        public string ExcludeSymbols { get; set; }
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
        public List<MonitorDynamicLeverageLevelInfo> Levels { get; set; }
        public List<MonitorDynamicLeverageAccountInfo> Accounts { get; set; }
        public List<MonitorDynamicLeverageSymbolInfo> Symbols { get; set; }
    }
    #endregion


    #endregion

    #region CRM

    #endregion

    #region MT SYSTEM

    #region Common
    public class ErrorMsg {
        public string Message { get; set; }
    }
    #endregion

    #region Copy Trader
    public class SlaveAccount {
        public UInt64 Login { get; set; }                          
        public string Symbol { get; set; }                      //跟随商品（主帐户）
        public string Suffix { get; set; }                      //子账户商品后缀
        public string Prefix { get; set; }                      //子账户商品前缀
        public int Delay { get; set; }                          //延时时长（秒）
        public string ProportionType { get; set; }              //跟随类型：Open，Solid，Euqity
        public double Proportion { get; set; }                  //跟随比例
        public bool Pedding { get; set; }                       //是否跟随挂单
        public bool SL { get; set; }                            //是否跟随止损
        public bool TP { get; set; }                            //是否跟随止盈
        public bool IsFollowClosedOrder { get; set; }           //是否跟随平仓订单（MT4）
        public UInt64 MasterLogin { get; set; }                    //跟随的主账号
    }

    public class MasterAccount {
        public UInt64 Login { get; set; }
        public int SlaveCount { get; set; }
        public bool IsDelete { get; set; }
        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
        public string sCreateTime { get; set; }
        public List<SlaveAccount> Slaves { get; set; }
    }
    #endregion

    #region Dynamic Leverage
    public class DynamicLeveragePositionInfo
    {
        public int OrderID { get; set; }
        public UInt64 Login { get; set; }
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
        public List<string> ExcludeSymbols { get; set; }       //需要排除的商品
        public List<DynamicLeverageSettingRangeInfo> Ranges { get; set; }
    }

    public class DynamicLeverageSetting
    {
        public string Name { get; set; }            //组名
        public UInt64 Login { get; set; }              //账号
        public List<UInt64> ExcludeLogins { get; set; }       //需要排除的账号
        public List<DynamicLeverageSettingInfo> Settings { get; set; }
    }

    public class DynamicLeverageEquityInfo
    {
        public UInt64 Login { get; set; }
        public double EquityDaily { get; set; }
        public double EquityWeekly { get; set; }
    }

    #endregion

    #region AdvMCSO
    public class RiskManagementAdvMCSOInfo
    {
        //public int ID { get; set; }
        public string GroupName { get; set; }
        public UInt64 Login { get; set; }
        public int MCSOType { get; set; }
        public int SODelayTime { get; set; }
        public int MCSOManualType { get; set; }
        public double MCValue { get; set; }
        public double SOValue { get; set; }
        //public bool Enable { get; set; }
        //public string MTType { get; set; }
        //public string MainLableName { get; set; }
    }

    #endregion

    #endregion

    #region Combination（组合）
    public class MT_ErrorMsg {
        public PluginServerInfo Server { get; set; }
        public List<ErrorMsg> Messages { get; set; }
    }

    public class MT_SymbolList
    {
        public PluginServerInfo Server { get; set; }
        public List<PluginSymbolInfo> Symbols { get; set; }
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
        public uint TimeStamp { get; set; }
        public string CurTime { get; set; }
        public int Weekday { get; set; }
        public List<DynamicLeverageEquityInfo> Users { get; set; }
    }
    #endregion
}