using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VotingApp.DataManagement;
using VotingApp.Models;
using VotingWeb.Models;
using NLog;

namespace VotingWeb.Controllers
{
    public class VotingController : Controller
    {
        private static Logger log = LogManager.GetLogger("VoteLog");
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //When you go to page we should create a new ballot and save it to db
            var _manager = new VotingManager();
            var viewModel = new VotingViewModel();
            log.Info("Attempting To Find Existing Ballot");
            var ballot = _manager.FindExistingBallot(user.Id);
            if (ballot != null)
            {
                
                return RedirectToAction("Index", "Home");
            }
            log.Info("User has not already voted!");
            log.Info("Getting Presidential and Vice Presidential Canindates");
            viewModel.PresidentAndVicePres = _manager.GetRankedVoteItems();
            log.Info("Getting Supreme Court Canindate");
            viewModel.SupremeCourt = _manager.GetSingleVoteItems().FirstOrDefault(x => x.Issue == null);
            log.Info("Getting State Representative Canindates");
            viewModel.StateRep = _manager.GetMultiVoteItems();
            log.Info("Getting Ballot Issues");
            viewModel.BallotIssue = _manager.GetSingleVoteItems().FirstOrDefault(x => x.Canidate == null);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Vote(VotingViewModel viewModel)
        {

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            log.Info("User Has Voted!");
            var _manager = new VotingManager();

            var ballot = _manager.CreateBallot("National Election" + DateTime.Now, user.Id);

            log.Info("Saving Results");
            var electionResults = new List<VoteResult>();

            if (viewModel.PresidentWriteIn != null)
            {
                electionResults =
                    _manager.AddRankingWriteInToElection(electionResults, viewModel.PresidentWriteIn, ballot.BallotId, viewModel.PresidentAndVicePres.First());
            }

            foreach (var presidentAndVice in viewModel.PresidentAndVicePres)
            {
                electionResults.Add(new VoteResult
                {
                    VoteResultsId = Guid.NewGuid(),
                    BallotId = ballot.BallotId,
                    RankingVoteId = presidentAndVice.RankingVoteItemId,
                    Ranking = presidentAndVice.Ranking,
                });
            }
            foreach (var stateRep in viewModel.StateRep)
            {
                electionResults.Add(new VoteResult
                {
                    VoteResultsId = Guid.NewGuid(),
                    BallotId = ballot.BallotId,
                    MultipleVoteId = stateRep.MultipleVoteItemId,
                    VotedFor = stateRep.VotedFor,
                });
            }
            electionResults.Add(new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = ballot.BallotId,
                SingleVoteId = viewModel.SupremeCourt.SingleVoteTicketId,
                VoteYes = viewModel.SupremeCourt.YesVote,
                VoteNo = viewModel.SupremeCourt.NoVote,
            });

            electionResults.Add(new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = ballot.BallotId,
                SingleVoteId = viewModel.BallotIssue.SingleVoteTicketId,
                VoteYes = viewModel.BallotIssue.YesVote,
                VoteNo = viewModel.BallotIssue.NoVote,
            });

            _manager.SaveElectionResults(electionResults);
            log.Info("Sending User to Home Screen");
            return RedirectToAction("Index", "Home");
        }
    }
}