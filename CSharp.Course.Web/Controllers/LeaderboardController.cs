using CSharp.Course.Web.Hubs;
using CSharp.Course.Web.Models;
using CSharp.Course.Web.Models.Dto;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CSharp.Course.Web.Controllers
{
    public class LeaderboardController : ApiController
    {
        [HttpPost, Route("api/leaderboard")]
        public async Task<BoardEntryDto> Post(BoardEntryDto boardEntryDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    ReasonPhrase = "Invalid data",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            using (var dbContext = new LeaderboardContext())
            {
                var boardEntry = new BoardEntry
                {
                    Id = Guid.NewGuid(),
                    Failed = boardEntryDto.Failed,
                    Passed = boardEntryDto.Passed,
                    Skipped = boardEntryDto.Skipped,
                    Username = boardEntryDto.Username,
                    Submitted = DateTime.UtcNow
                };

                // remove existing entry before adding new one
                // should probably have a password or something
                dbContext.Leaderboard.RemoveRange(dbContext.Leaderboard.Where(l => l.Username == boardEntryDto.Username));

                // add new entry
                var result = dbContext.Leaderboard.Add(boardEntry);
                await dbContext.SaveChangesAsync();

                if (dbContext.TopTenEntries.Any(be => be.Id == result.Id))
                {
                    var clients = GlobalHost.ConnectionManager.GetHubContext<LeaderboardHub>().Clients;
                    clients.All.leaderboardUpdate(await dbContext.TopTenEntries.Select(entry => new BoardEntryDto
                    {
                        Id = entry.Id,
                        Failed = entry.Failed,
                        Passed = entry.Passed,
                        Skipped = entry.Skipped,
                        Username = entry.Username,
                        Submitted = entry.Submitted
                    }).ToListAsync());
                }

                return new BoardEntryDto
                {
                    Failed = result.Failed,
                    Id = result.Id,
                    Passed = result.Passed,
                    Skipped = result.Skipped,
                    Username = result.Username,
                    Submitted = result.Submitted
                };
            }
        }


        [HttpGet, Route("api/leaderboard")]
        public async Task<IEnumerable<BoardEntryDto>> Get(int page = 1, int pageSize = 100)
        {
            if (page < 1 || pageSize > 1000 || pageSize < 1)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    ReasonPhrase = "Invalid pagination",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

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

        [HttpDelete, Route("api/leaderboard")]
        public async Task Delete()
        {
            using (var dbContext = new LeaderboardContext())
            {
                dbContext.Leaderboard.RemoveRange(dbContext.Leaderboard);
                await dbContext.SaveChangesAsync();
                
                var clients = GlobalHost.ConnectionManager.GetHubContext<LeaderboardHub>().Clients;
                clients.All.leaderboardUpdate(await dbContext.TopTenEntries.Select(entry => new BoardEntryDto
                {
                    Id = entry.Id,
                    Failed = entry.Failed,
                    Passed = entry.Passed,
                    Skipped = entry.Skipped,
                    Username = entry.Username,
                    Submitted = entry.Submitted
                }).ToListAsync());
            }
        }
    }
}
