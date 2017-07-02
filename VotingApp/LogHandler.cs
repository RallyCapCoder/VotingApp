using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
namespace VotingApp
{
    public static class LogHandler
    {
        
        public static void CreateLogger()
        {
            Logger log = LogManager.GetLogger("VoteLog");
        }
    }
}
