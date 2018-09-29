using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class Logger : AspNetCore.PluginManager.ILogger
    {
        public void AddToLog(string data)
        {
        }

        public void AddToLog(Exception exception)
        {
        }

        public void AddToLog(Exception exception, string data)
        {
        }
    }
}
