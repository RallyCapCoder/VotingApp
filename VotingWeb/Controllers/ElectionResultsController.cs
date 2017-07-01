using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingApp.DataManagement;
using VotingWeb.Models;

namespace VotingWeb.Controllers
{
    public class ElectionResultsController : Controller
    {
        // GET: ElectionResults
        public ActionResult Index()
        {
            var _manager = new VotingManager();
            var viewModel = new ElectionResultsViewModel();

            var ballotId = _manager.GetBallotByName("National Election");

            var results = _manager.GetVoteResults(ballotId);

            var job = _manager.GetJobByJobName("Commander and Cream");

            var presidentialCanindates = _manager.GetCanidatesByJobId(job.JobId);


            foreach (var result in results)
            {
                foreach (var presidentialCanindate in presidentialCanindates)
                {
                    if (result.CanidateId == presidentialCanindate.CanidateId)
                    {
                        viewModel.PreseidentResult.Add(new RankedResults
                        {
                            Name = presidentialCanindate.Name,
                            Party = presidentialCanindate.Party,
                            Ranking = result.Ranking ?? 0
                        });
                    }
                }
            }

            job = _manager.GetJobByJobName("Vice Ice");

            var vicePresidentCanidates = _manager.GetCanidatesByJobId(job.JobId);


            foreach (var result in vicePresidentCanidates)
            {
                foreach (var presidentialCanindate in presidentialCanindates)
                {
                    if (result.Party == presidentialCanindate.Party)
                    {
                        presidentialCanindate.Party += $"\n {result.Name}";
                    }
                }
            }

            job = _manager.GetJobByJobName("Chief Dairy Queen");

            var supremeCourt = _manager.GetCanidatesByJobId(job.JobId);

            foreach (var result in results)
            {
                foreach (var canindate in supremeCourt)
                {
                    if (result.CanidateId == canindate.CanidateId)
                    {
                        viewModel.SupremeCourtResult = (new SingleVoteResults()
                        {
                            Name = canindate.Name,
                            YesVotes = 1, 
                            NoVotes = 0
                        });
                    }
                }
            }

            job = _manager.GetJobByJobName("State Rep District M&M");

            var stateReps = _manager.GetCanidatesByJobId(job.JobId);

            foreach (var result in results)
            {
                foreach (var canindate in stateReps)
                {
                    if (result.CanidateId == canindate.CanidateId)
                    {
                        var votes = 0;
                        if (result.VotedFor)
                        {
                            votes++;
                        }
                        viewModel.StateRepResult.Add(new MultipleVotesResult()
                        {
                            Name = canindate.Name,
                            Party = canindate.Party,
                            Votes = votes
                        });
                    }
                }
            }

            return View(viewModel);
        }

    }
}
