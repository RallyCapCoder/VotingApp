using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingApp.DataManagement;
using VotingApp.Models;
using VotingWeb.Models;

namespace VotingWeb.Controllers
{
    public class VotingController : Controller
    {
        // GET: Voting
        public ActionResult Index()
        {
            //When you go to page we should create a new ballot and save it to db
            var _manager = new VotingManager();
            var viewModel = new VotingViewModel();

            var ballot = _manager.CreateBallot("National Election" + DateTime.Now);
            viewModel.BallotId = ballot.BallotId;
            viewModel.PresidentAndVicePres = _manager.GetRankedVoteItems();
            viewModel.SupremeCourt = _manager.GetSingleVoteItems().FirstOrDefault(x => x.Issue == null);
            viewModel.StateRep = _manager.GetMultiVoteItems();
            viewModel.BallotIssue = _manager.GetSingleVoteItems().FirstOrDefault(x => x.Canidate == null);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Vote(VotingViewModel viewModel)
        {

            var _manager = new VotingManager();

            var electionResults = new List<VoteResult>();
            foreach (var presidentAndVice in viewModel.PresidentAndVicePres)
            {
                electionResults.Add(new VoteResult
                {
                    VoteResultsId = Guid.NewGuid(),
                    BallotId = viewModel.BallotId,
                    RankingVoteId = presidentAndVice.RankingVoteItemId,
                    Ranking = presidentAndVice.Ranking,
                });
            }
            foreach (var stateRep in viewModel.StateRep)
            {
                electionResults.Add(new VoteResult
                {
                    VoteResultsId = Guid.NewGuid(),
                    BallotId = viewModel.BallotId,
                    MultipleVoteId = stateRep.MultipleVoteItemId,
                    VotedFor = stateRep.VotedFor,
                });
            }
            electionResults.Add(new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = viewModel.BallotId,
                SingleVoteId = viewModel.SupremeCourt.SingleVoteTicketId,
                VoteYes = viewModel.SupremeCourt.YesVote,
                VoteNo = viewModel.SupremeCourt.NoVote,
            });

            electionResults.Add(new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = viewModel.BallotId,
                SingleVoteId = viewModel.BallotIssue.SingleVoteTicketId,
                VoteYes = viewModel.BallotIssue.YesVote,
                VoteNo = viewModel.BallotIssue.NoVote,
            });

            _manager.SaveElectionResults(electionResults);

            return RedirectToAction("Index", "Home");
        }
    }
}