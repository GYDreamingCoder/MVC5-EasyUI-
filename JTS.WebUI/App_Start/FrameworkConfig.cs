using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JTS.Core;

namespace JTS.WebUI
{
    public class FrameworkConfig
    {
        public static void Register()
        { 
            APP.Init();
        }
    }
}