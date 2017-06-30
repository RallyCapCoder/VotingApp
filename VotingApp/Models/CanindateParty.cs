using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class CanindateParty
    {
        public Canidate President { get; set; }
        public Canidate VicePresident { get; set; }
        public Canidate StateRep { get; set; }
        public Canidate SupremeCourtJustice { get; set; }
        public string PartyAffliation { get; set; }
    }
}
