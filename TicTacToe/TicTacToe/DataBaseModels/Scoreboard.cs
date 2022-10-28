using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TicTacToe.DataBaseModels
{
    public partial class Scoreboard
    {
        public int UserId { get; set; }
        public int Wins { get; set; }
        public int Drafts { get; set; }
        public int Loses { get; set; }

        public virtual Users User { get; set; }
    }
}
