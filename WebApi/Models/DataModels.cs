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
        public long CurrentTimeStamp { get; set; }            //当前请求是MT平台的时间戳（转换成GMT+0）
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
        public string ReportDataBase { get; set; }
        public string Index_TableName { get; set; }                 //需要创建索引的表名称
        public string IndexName { get; set; }                       //需要创建的索引名称
        public string IndexField { get; set; }                      //需要索引的字段，以 , 分割，需要符合索引的格式
        public bool IsExpired { get; set; }
    }

    public class PluginLicenseInfo {
        public string MainLableName { get; set; }
        public string MTType { get; set; }
        public string ModuleName { get; set; }
        public bool IsExpired { get; set; }
        public string ExpiredTime { get; set; }                 //格式：yyyy-MM-dd HH:mm:ss
        public string LicenseInfo { get; set; }                 //记录日期信息：VaildTime:******，CurrentTime：*******，均以Web服务器时间为准（+8 时区）
        public string MD5Value { get; set; }                    //MD5校验值：MainLableName+","+ExpiredTime+",Kangaroo"
    }
    #endregion

    #region MONITOR
    public class MonitorPluginInfo {
        public string mainLableName { get; set; }
        public string mtType { get; set; }
        public string pluginName { get; set; }

    }

    public class MonitorReturnInfo {
        public ReturnCode returnCode { get; set; }
        public string cnDescription { get; set; }
        public string enDescription { get; set; }
        public string value { get; set; }
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
        public string MTLogins { get; set; }    //Type=组时，此处为排除的账号信息
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
        public string Symbol { get; set; }              //Type=组时，此处为排除的商品信息
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

    [Serializable]
    public class MonitorDynamicLeverageRulesRelation
    {
        public MonitorDynamicLeverageLevelInfo Level { get; set; }
        public MonitorDynamicLeverageSymbolInfo Symbols { get; set; }
    }
    public class MonitorDynamicLeverageRuleInfo
    {
        public int id { get; set; }
        public string ruleName { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string hedgingLeverage { get; set; }
        public MonitorDynamicLeverageAccountInfo Account { get; set; }
        public List<MonitorDynamicLeverageRulesRelation> RulesRelationList { get; set; }
        //public List<MonitorDynamicLeverageLevelInfo> Levels { get; set; }
        //public List<MonitorDynamicLeverageSymbolInfo> Symbols { get; set; }
    }

    public class MonitorDynamicLeverageLevelDetailRequestInfo {
        public MonitorPluginInfo server { get; set; }
        public uint login { get; set; }
    }
    public class MonitoryDynamicLeverageSymbolLevelDetail {
        public uint login { get; set; }
        public string symbol { get; set; }
        public double levelFrom { get; set; }
        public double levelTo { get; set; }
        public uint leverage { get; set; }
        public double netVolume { get; set; }
        public double levelMargin { get; set; }
        public string updateTime { get; set; }
    }
    public class MonitorDynamicLeverageSymbolSummary {
        public uint login { get; set; }
        public string symbol { get; set; }
        public double hedgeVolume { get; set; }
        public double hedgeMargin { get; set; }
        public List<MonitoryDynamicLeverageSymbolLevelDetail> details { get; set; }
    }

    public class MonitorDynamicLevergeAccountSummary {
        public int account { get; set; }
        public string name { get; set; }
        public string group { get; set; }
        public double balance { get; set; }
        public double credit { get; set; }
        public double equity { get; set; }
        public double margin { get; set; }
        public double freeMargin { get; set; }
        public string marginLevel { get; set; }             //保证金百分比
        public string marginRule { get; set; }              //使用的规则名称
        public string lastLoginTime { get; set; }
        public string lastTradingTime { get; set; }
        public double pl { get; set; }                      //盈利
        public int positionCount { get; set; }
    }

    public class MonitorDynamicLeverageClosedOrder {
        public int ticket { get; set; }
        public string netPositions { get; set; }
        public string margin { get; set; }
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
    public class MasterAccount
    {
        public UInt64 Login { get; set; }
        public int SlaveCount { get; set; }
        //public bool IsDelete { get; set; }
        public string Comment { get; set; }
        //public DateTime CreateTime { get; set; }
        //public string sCreateTime { get; set; }
        public List<SlaveAccount> Slaves { get; set; }
    }
    public class SlaveAccount {
        public UInt64 Login { get; set; }                          
        public int Delay { get; set; }                          //延时时长（秒）
        public string ProportionType { get; set; }              //跟随类型：Open，Solid，Euqity
        public double Proportion { get; set; }                  //跟随比例
        public bool Reverse { get; set; }                       //是否反向跟单
        public bool Pedding { get; set; }                       //是否跟随挂单
        public bool SL { get; set; }                            //是否跟随止损
        public bool TP { get; set; }                            //是否跟随止盈
        public bool IsFollowClosedOrder { get; set; }           //是否跟随平仓订单（MT4）
        public UInt64 MasterLogin { get; set; }                    //跟随的主账号
        public List<SymbolRelations> Symbols { get; set; }
    }
    public class SymbolRelations {
        public UInt64 MasterAcc { get; set; }
        public UInt64 SlaveAcc { get; set; }
        public string MasterSymbol { get; set; }
        public string SlaveSymbol { get; set; }
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
        public double OpenPrice { get; set; }
        public string Currency { get; set; }
        public string MarginCurrency { get; set; }
        public int NetVolumeBeforeClosed { get; set; }
        public int NetVolumeAfterClosed { get; set; }
        public double MarginBeforeClosed { get; set; }
        public double MarginAfterClosed { get; set; }
        public DateTime CloseTime { get; set; }                     //北京时间
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
        public int RuleID { get; set; }
        public string RuleName { get; set; }
        public string Name { get; set; }            //组名
        public UInt64 Login { get; set; }              //账号
        public List<UInt64> ExcludeLogins { get; set; }       //需要排除的账号
        public DynamicLeverageRuleMode RuleMode { get; set; }
        public string StartTime { get; set; }
        public long StartTimeStamp { get; set; }
        public string EndTime { get; set; }
        public long EndTimeStamp { get; set; }
        public int HedgeLeverage { get; set; }       //锁仓使用的杠杆
        public List<DynamicLeverageSettingInfo> Settings { get; set; }
    }

    public class DynamicLeverageEquityInfo
    {
        public UInt64 Login { get; set; }
        public double EquityDaily { get; set; }
        public double EquityWeekly { get; set; }
    }
    public class DynamicLeverageLevelDetail
    {
        public uint From { get; set; }
        public uint To { get; set; }
        public uint Leverage { get; set; }
        public uint NetVolume { get; set; }
    }
    public class DynamicLeverageSymbolSummaryNodeInfo
    {
        public uint Login { get; set; }
        public string Symbol { get; set; }
        public int LongDeals { get; set; }
        public int LongVolume { get; set; }
        public int ShortDeals { get; set; }
        public int ShortVolume { get; set; }
        public double HedgeVolume { get; set; }
        public double HedgeMargin { get; set; }
        public uint RuleID { get; set; }
        public double AverageRealPrice { get; set; }
        public List<DynamicLeverageLevelDetail> Details { get; set; }
    }

    //public class DynamicLeverageTradeInfo {
    //    public uint Ticket { get; set; }                        //订单号
    //    public string Symbol { get; set; }
    //    public int LongCount { get; set; }                      //持仓笔数（多仓）
    //    public int ShortCount { get; set; }                     //持仓笔数（空仓）
    //    public int LongVolume { get; set; }                   //持仓手数（多仓，MT4-放大100倍）
    //    public int ShortVolume { get; set; }                  //持仓手数（空仓，MT4-放大100倍）
    //    public double LongPrice { get; set; }                   //持仓平均价格（多仓）
    //    public double ShortPrice { get; set; }                  //持仓平均价格（空仓）
    //    public int HedgeVolume { get; set; }                  //锁仓手数（MT4-放大100倍，MT5-放大10^8倍）
    //    public double Commissions { get; set; }                 //持仓手续费（仅MT4，暂不支持MT5）
    //    public double Swaps { get; set; }                       //隔夜利息（仅MT4，暂不支持MT5）
    //    public double Profit { get; set; }                      //持仓浮动盈亏
    //    public string NetPositionChanges { get; set; }          //此订单的开仓（或平仓）对净头寸的影响（从...到...）
    //    public string MarginChanges { get; set; }               //此订单的开仓（或平仓）对保证金的影响（从...到...）
    //}

    public class DynamicLeverageAccountSummaryInfo
    {
        public int Login { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public double Balance { get; set; }
        public double Credit { get; set; }
        public double Equity { get; set; }
        public double Margin { get; set; }                      //已用保证金
        public double FreeMargin { get; set; }                  //可用保证金
        public double MarginLevel { get; set; }                 //保证金比例
        public int PositionCount { get; set; }                  //持仓笔数
        public int RuleID { get; set; }                   //使用的杠杆规则 ID
        public long LastLoginTime { get; set; }               //最后登录时间（时间戳）
        public long LastTradeTime { get; set; }               //最后交易时间（时间戳）
        public bool IsUpdate { get; set; }                      //记录账号是否经过杠杆重新计算，如果是False，仅更新LastLoginTime
        //public List<DynamicLeverageTradeInfo> Trades { get; set; }          //持仓订单信息（最近的 5 分钟）
        //public List<uint> lstTradesClosedID { get; set; }               //已经平仓的订单ID （最近的 5 分钟）
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

    public class DynamicLeverageSymbolSummary {
        public PluginServerInfo Server { get; set; }
        public List<DynamicLeverageSymbolSummaryNodeInfo> Symbols { get; set; }
    }

    public class DynamicLeverageAccountSummary {
        public PluginServerInfo Server { get; set; }
        public List<DynamicLeverageAccountSummaryInfo> Accounts { get; set; }
    }
    #endregion
}