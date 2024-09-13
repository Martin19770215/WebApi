using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public enum ReturnCode
    {
        OK=200,
        EmptySettingString=300,
        RunningError=400
    }
}