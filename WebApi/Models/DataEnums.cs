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
        DATA_UploadError,
        RunningError=901,
    }

    public enum DynamicLeveragePositionMode
    {
        UPLOAD_ALL_POSITION = 0,
        UPLOAD_NEW_POSITION,
        UPLOAD_ACTIVE_POSITION,
        UPLOAD_CLOSE_POSITION,
        UPLOAD_DELETE_POSITION,
        UPLOAD_RESOTRE_POSITION
        //UPDATE_NEW_POSITION = 10,
        //UPDATE_CLOSE_POSITION,
        //UPDATE_DELETE_POSITION
    }

    public enum DynamicLeverageRuleMode
    {
        NetPosition=1,
        DailyEquity,
        WeeklyEquity
    }
}