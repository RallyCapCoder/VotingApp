using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VotingApp.Models;
using VotingWeb.Models;
using NLog;
using VotingApp.Context;
using VotingApp.Managers;
using Ballot = VotingApp.Models.Ballot;

namespace VotingWeb.Controllers
{
    public class VotingController : Controller
    {
        private VotingManager Manager { get; set; }
        private new ApplicationUser User { get; set; }
        private VotingViewModel ViewModel { get; set; }
        private static readonly Logger Log = LogManager.GetLogger("VoteLog");

        public VotingController()
        {
            Manager = new VotingManager();
            ViewModel = new VotingViewModel();
            User = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
        }
       
        public ActionResult Index()
        { 
            Log.Info("Attempting To Find Existing Ballot");
            var ballot = Manager.FindExistingBallot(User.Id);
            if (ballot != null)
            {
                Log.Info("User has already voted returning to home screen");
                TempData["AlreadyVoted"] = "You have already voted!";
                return RedirectToAction("Index", "Home");
            }

            Log.Info("Getting Presidential and Vice Presidential Canindates");
            ViewModel.PresidentAndVicePres = Manager.RankedVotingManager.GetRankedVoteItems();
            Log.Info("Getting Supreme Court Canindate");
            ViewModel.SupremeCourt = Manager.SingleVoteManager.GetSingleVoteItems().FirstOrDefault(x => x.Issue == null);
            Log.Info("Getting State Representative Canindates");
            ViewModel.StateRep = Manager.MultiVoteManager.GetMultiVoteItems();
            Log.Info("Getting Ballot Issues");
            ViewModel.BallotIssue = Manager.SingleVoteManager.GetSingleVoteItems().FirstOrDefault(x => x.CandidateItem == null);

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult Vote(VotingViewModel viewModel)
        {
            Log.Info("User Has Voted!");

            var ballot = Manager.CreateBallot("National Election" + DateTime.Now, User.Id);

            Log.Info("Saving Results");
            var electionResults = new List<VoteResult>();

            electionResults = CheckForElectionWriteIns(viewModel, electionResults, ballot);
            TallyBallotVotes(viewModel, electionResults, ballot);

            Manager.SaveElectionResults(electionResults);
            Log.Info("Sending User to Home Screen");
            TempData["Success"] = "You Successfully Voted!";
            return RedirectToAction("Index", "Home");
        }

        private static void TallyBallotVotes(VotingViewModel viewModel, List<VoteResult> electionResults, Ballot ballot)
        {
            Log.Info("Tallying Votes");
            electionResults.AddRange(viewModel.PresidentAndVicePres.Select(presidentAndVice => new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = ballot.BallotId,
                RankingVoteId = presidentAndVice.RankingVoteItemId,
                Ranking = presidentAndVice.Ranking,
            }));

            electionResults.AddRange(viewModel.StateRep.Select(stateRep => new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = ballot.BallotId,
                MultipleVoteId = stateRep.MultipleVoteItemId,
                VotedFor = stateRep.VotedFor,
            }));
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
        }

        private List<VoteResult> CheckForElectionWriteIns(VotingViewModel viewModel, List<VoteResult> electionResults, Ballot ballot)
        {
            if (viewModel.PresidentWriteIn.PrimeCandidateItem.Name != null)
            {
                electionResults =
                    Manager.RankedVotingManager.AddRankingWriteInToElection(electionResults, viewModel.PresidentWriteIn,
                        ballot.BallotId, viewModel.PresidentAndVicePres.First());
            }

            if (viewModel.StateRepWriteIn.CandidateItem.Name != null)
            {
                electionResults =
                    Manager.MultiVoteManager.AddMultiVoteWriteInToElection(electionResults, viewModel.StateRepWriteIn,
                        ballot.BallotId, viewModel.StateRep.First());
            }
            return electionResults;
        }
    }
}