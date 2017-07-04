using VotingApp.Context;

namespace VotingApp.Builders
{
    public class BallotIssueBuilder
    {
        public Models.BallotIssueItem GetModel(BallotIssue issue)
        {
            return new Models.BallotIssueItem()
            {
                VoteIssueId = issue.BallotIssueId,
                Name = issue.Name,
                OfficalName = issue.OfficalName,
                Description = issue.Description,
                Subtext = issue.Subtext,
                VotedFor = issue.VotedFor
            };
        }

        public BallotIssue GetEntity(Models.BallotIssueItem issue)
        {
            return new BallotIssue()
            {
                BallotIssueId = issue.VoteIssueId,
                Name = issue.Name,
                OfficalName = issue.Name,
                Description = issue.Description,
                Subtext = issue.Subtext,
                VotedFor = issue.VotedFor
            };
        }
    }
}
