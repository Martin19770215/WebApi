using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public enum ReturnCode
    {
        OK=200,
        EmptySettingString=301,
        SQL_TransactionErr=401,
        DATA_Existed=501,
        DATA_TimeStampError,
        DATA_LicenseTimeOut,
        RunningError=901,
    }

    public enum DynamicLeveragePositionMode
    {
        UPDATE_ALL_POSITION = 0,
        UPDATE_NEW_POSITION,
        UPDATE_ACTIVE_POSITION,
        UPDATE_CLOSE_POSITION,
        UPDATE_DELETE_POSITION,
        UPDATE_RESOTRE_POSITION
    }

    public enum DynamicLeverageRuleMode
    {
        NetPosition=1,
        DailyEquity,
        WeeklyEquity
    }
}