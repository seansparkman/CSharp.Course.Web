using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSharp.Course.Web.Models
{
    public class BoardEntry
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
        public DateTime? Submitted { get; set; }
    }
}