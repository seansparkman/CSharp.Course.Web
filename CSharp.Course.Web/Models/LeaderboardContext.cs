using System;
using System.Collections.Generic;
using System.Data.Entity;

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
    }
}