﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class RankingVoteItem
    {
        public Guid RankingVoteItemId { get; set; }
        public Canidate PrimeCanidate { get; set; }
        public Canidate SubCanidate { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int Ranking { get; set; }
        public bool IsWriteIn { get; set; }
    }
}
