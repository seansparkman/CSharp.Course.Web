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

                if (dbContext.Leaderboard.OrderBy(eb => eb.Passed).ThenBy(eb => eb.Username).Any(be => be.Id == result.Id))
                {
                    var boardEntries = from entry in dbContext.Leaderboard.
                                            OrderBy(eb => eb.Passed).ThenBy(eb => eb.Username).
                                            Take(10)
                                       select new BoardEntryDto
                                       {
                                           Id = entry.Id,
                                           Failed = entry.Failed,
                                           Passed = entry.Passed,
                                           Skipped = entry.Skipped,
                                           Username = entry.Username
                                       };

                    var clients = GlobalHost.ConnectionManager.GetHubContext<LeaderboardHub>().Clients;
                    clients.All.leaderboardUpdate(boardEntries.ToListAsync());
                }

                return new BoardEntryDto
                {
                    Failed = result.Failed,
                    Id = result.Id,
                    Passed = result.Passed,
                    Skipped = result.Skipped,
                    Username = result.Username
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
                var boardEntries = from entry in dbContext.Leaderboard.
                                        OrderBy(eb => eb.Passed).ThenBy(eb => eb.Username).
                                        Skip((page - 1) * pageSize).Take(pageSize)
                                   select new BoardEntryDto
                                   {
                                       Id = entry.Id,
                                       Failed = entry.Failed,
                                       Passed = entry.Passed,
                                       Skipped = entry.Skipped,
                                       Username = entry.Username
                                   };

                return await boardEntries.ToListAsync();
            }
        }
    }
}
