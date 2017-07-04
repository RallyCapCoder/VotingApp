using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace VotingWeb.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private static readonly Logger Log = LogManager.GetLogger("VoteLog");
        public ActionResult Index()
        {
            Log.Info("User entered the site");
            return View();
        }

        public ActionResult VoterInformation()
        {
            Log.Info("User went to VoterInformation page");
            return View();
        }

        public ActionResult Contact()
        {
            Log.Info("User went to contact page");
            return View();
        }
    }
}