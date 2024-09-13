using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class PublicConst
    {
        #region 帐号角色
        //public readonly static string Account_Roles_Anonymous = "0";
        //public readonly static string Account_Roles_SystemAdmin = "1";
        //public readonly static string Account_Roles_AppManager = "2";
        //public readonly static string Account_Roles_Agent = "3";
        //public readonly static string Account_Roles_User = "4";
        //public readonly static string Account_Role_ComapnyManager = "5";
        //public readonly static string Account_Role_Personal = "6";
        #endregion

        #region 系统配置

        #region 系统语言
        //public readonly static string Sys_Language = "zh-cn,en-us,zh-tw";
        //public readonly static string Sys_DefaultLanguage = WebConfigurationManager.AppSettings["DefaultLanguage"];
        //public readonly static bool Sys_Diglossia = bool.Parse(WebConfigurationManager.AppSettings["IsAlwaysShowEN"]);
        #endregion

        ////获取webservice的用户名和密码
        //private static byte[] key = Encoding.UTF32.GetBytes("DGM_WS");
        //public readonly static string Sys_WSUserName = WebConfigurationManager.AppSettings["UserName"];
        //public readonly static string Sys_WSUserPWD = "";
        //        public readonly static string Sys_WSUserPWD = MD5Helper.DESDecrypt(WebConfigurationManager.AppSettings["UserPwd"].ToString(), key).Replace("\0", "");

        //Get和Post转发URL
        ////public readonly static string TransferURL = WebConfigurationManager.AppSettings["TransferURL"];
        ////public readonly static string MonirotTransferURL = WebConfigurationManager.AppSettings["MonitorTransferURL"];

        //创建账户的角色
        //public readonly static string Sys_AccountRole = WebConfigurationManager.AppSettings["DeaultAccountRole"];

        //登录MT4
        //public readonly static string Sys_MasterMT4Server = WebConfigurationManager.AppSettings["MasterMT4Server"].ToString();
        //public readonly static string Sys_SlaveMT4Server = WebConfigurationManager.AppSettings["SlaveMT4Server"].ToString();
        //public readonly static string Sys_DemoMT4Server = WebConfigurationManager.AppSettings["DemoMT4Server"].ToString();

        //发送邮件使用
        //public readonly static string Sys_PlatformName = WebConfigurationManager.AppSettings["PlatformName"];
        //public readonly static string Sys_EmailSendAccount = WebConfigurationManager.AppSettings["EmailSendAccount"];
        //public readonly static string Sys_EmailSendAccountPwd = WebConfigurationManager.AppSettings["EmailSendAccountPwd"];
        ////public readonly static string Sys_EmailSendAccountPwd = MD5Helper.DESDecrypt(WebConfigurationManager.AppSettings["EmailSendAccountPwd"], key).Replace("\0", "");
        //public readonly static string Sys_EmailSmtpServer = WebConfigurationManager.AppSettings["EmailSmtpServer"];
        //public readonly static int Sys_EmailSmtpServerPort = int.Parse(WebConfigurationManager.AppSettings["EmailSmtpServerPort"]);
        //public readonly static bool Sys_EmailIsSmtpAuth = bool.Parse(WebConfigurationManager.AppSettings["EmailIsSmtpAuth"]);
        //public readonly static bool Sys_EmailEnableSsl = bool.Parse(WebConfigurationManager.AppSettings["EmailEnableSsl"]);

        //public readonly static string Sys_EmailSmtpFrom = WebConfigurationManager.AppSettings["EmailSmtpFrom"];     //如果代发，用该账号作为发送人；否则为空

        //WEB数据库
        public readonly static string Database = "LogicTrader";
        public readonly static string DefaultDateTime = "1970-01-01 00:00:00";
        //public readonly static string Database = WebConfigurationManager.AppSettings["Database"].ToString().Replace("\0", "");
        //public readonly static string Database = MD5Helper.DESDecrypt(WebConfigurationManager.AppSettings["Database"].ToString(), key).Replace("\0", "");


        //帐号的初始密码
        //public readonly static string DefaultUserPwd = "";
        //public readonly static string DefaultUserPwd = MD5Helper.DESDecrypt(WebConfigurationManager.AppSettings["DefaultUserPwd"].ToString(), key).Replace("\0", "");
        #endregion

        #region SQL Command类型
        public readonly static string StoredProcedure = "StoredProcedure";
        public readonly static string CommandTypeDefault = "";
        #endregion

    }
}