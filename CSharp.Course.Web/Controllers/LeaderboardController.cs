using CSharp.Course.Web.Models;
using CSharp.Course.Web.Models.Dto;
using System;
using System.Collections.Generic;
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
                    Failed = boardEntryDto.Failed,
                    Passed = boardEntryDto.Passed,
                    Skipped = boardEntryDto.Skipped,
                    Username = boardEntryDto.Username
                };

                // remove existing entry before adding new one
                // should probably have a password or something
                dbContext.Leaderboard.RemoveRange(dbContext.Leaderboard.Where(l => l.Username == boardEntryDto.Username));

                // add new entry
                var result = dbContext.Leaderboard.Add(boardEntry);
                await dbContext.SaveChangesAsync();

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
    }
}
