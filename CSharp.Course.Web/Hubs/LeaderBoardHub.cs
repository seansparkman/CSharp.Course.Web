using CSharp.Course.Web.Models;
using CSharp.Course.Web.Models.Dto;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CSharp.Course.Web.Hubs
{
    public class LeaderboardHub : Hub
    {
        public async Task<List<BoardEntryDto>> GetBoard()
        {
            using (var dbContext = new LeaderboardContext())
            {
                return await dbContext.TopTenEntries.Select(entry => new BoardEntryDto
                {
                    Id = entry.Id,
                    Failed = entry.Failed,
                    Passed = entry.Passed,
                    Skipped = entry.Skipped,
                    Username = entry.Username,
                    Submitted = entry.Submitted
                }).ToListAsync();
            }
        }
    }
}