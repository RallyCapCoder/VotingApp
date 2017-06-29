using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingApp.Models;
using VotingWeb.Models;

namespace VotingWeb.Controllers
{
    public class VotingController : Controller
    {
        // GET: Voting
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Vote(VotingViewModel viewModel)
        {
            return View();
        }
    }
}