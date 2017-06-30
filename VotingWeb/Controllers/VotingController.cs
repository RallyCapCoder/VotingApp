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

            _manager.CreateBallot("National Election");


            viewModel.DemocraticCanidates = _manager.FillCanindateParty("Democrat");
            viewModel.RepublicanCanidates = _manager.FillCanindateParty("Republican");
            viewModel.IndependentCanidates = _manager.FillCanindateParty("Independent");
            viewModel.SupremeCourt = _manager.FillCanindateParty("Supreme Court");

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Vote(VotingViewModel viewModel)
        {

            var _manager = new VotingManager();

            var ballotId = _manager.GetBallotByName("National Election");
            List<VoteResult> electionResults = new List<VoteResult>();
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.DemocraticCanidates.President.CanidateId,
                BallotId = ballotId,
                Ranking = viewModel.DemocraticCanidates.President.Ranking
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.DemocraticCanidates.VicePresident.CanidateId,
                BallotId = ballotId,
                Ranking = viewModel.DemocraticCanidates.President.Ranking
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.RepublicanCanidates.President.CanidateId,
                BallotId = ballotId,
                Ranking = viewModel.RepublicanCanidates.President.Ranking
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.RepublicanCanidates.VicePresident.CanidateId,
                BallotId = ballotId,
                Ranking = viewModel.RepublicanCanidates.President.Ranking
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.IndependentCanidates.President.CanidateId,
                BallotId = ballotId,
                Ranking = viewModel.IndependentCanidates.President.Ranking
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.IndependentCanidates.VicePresident.CanidateId,
                BallotId = ballotId,
                Ranking = viewModel.IndependentCanidates.President.Ranking
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.SupremeCourt.SupremeCourtJustice.CanidateId,
                BallotId = ballotId,
                VotedFor = viewModel.SupremeCourt.SupremeCourtJustice.VotedFor
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.RepublicanCanidates.StateRep.CanidateId,
                BallotId = ballotId,
                VotedFor = viewModel.RepublicanCanidates.StateRep.VotedFor
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.DemocraticCanidates.StateRep.CanidateId,
                BallotId = ballotId,
                VotedFor = viewModel.DemocraticCanidates.StateRep.VotedFor
            });
            electionResults.Add(new VoteResult
            {
                CanindateId = viewModel.IndependentCanidates.StateRep.CanidateId,
                BallotId = ballotId,
                VotedFor = viewModel.IndependentCanidates.StateRep.VotedFor
            });

            _manager.SaveElectionResults(electionResults);

            return View("Index");
        }
    }
}