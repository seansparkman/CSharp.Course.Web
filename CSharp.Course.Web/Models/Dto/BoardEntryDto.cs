using System;
using System.ComponentModel.DataAnnotations;

namespace CSharp.Course.Web.Models.Dto
{
    public class BoardEntryDto
    {
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Username { get; set; }
        [Required, Min(0)]
        public int Passed { get; set; }
        [Required, Min(0)]
        public int Failed { get; set; }
        [Required, Min(0)]
        public int Skipped { get; set; }
    }
}