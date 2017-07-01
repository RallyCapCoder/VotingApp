using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.DataManagement.Builders
{
    public class VoteIssueBuilder
    {
        public Models.VoteIssue GetModel(VoteIssue issue)
        {
            return new Models.VoteIssue()
            {
                VoteIssueId = issue.VoteIssueId,
                Name = issue.Name,
                OfficalName = issue.OfficalName,
                Description = issue.Description,
                Subtext = issue.Subtext,
                VotedFor = issue.VotedFor
            };
        }

        public VoteIssue GetEntity(Models.VoteIssue issue)
        {
            return new VoteIssue()
            {
                VoteIssueId = issue.VoteIssueId,
                Name = issue.Name,
                OfficalName = issue.Name,
                Description = issue.Description,
                Subtext = issue.Subtext,
                VotedFor = issue.VotedFor
            };
        }
    }
}
