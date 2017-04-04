using System;

namespace CSharp.Course.Web.Models.Dto
{
    public class BoardEntryDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
    }
}