using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CSharp.Course.Web.Models
{
    public class LeaderboardContext
        : DbContext
    {
        public LeaderboardContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<BoardEntry> Leaderboard { get; set; }

        public IOrderedQueryable<BoardEntry> SortedLeaderboard
        {
            get { return Leaderboard.OrderBy(be => be.Passed).ThenBy(be => be.Submitted).ThenBy(be => be.Username); }
        }

        public IQueryable<BoardEntry> TopTenEntries
        {
            get { return SortedLeaderboard.Take(10); }
        }
    }
}