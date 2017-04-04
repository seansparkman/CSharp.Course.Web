using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSharp.Course.Web.Models.Dto
{
    public class MinAttribute : ValidationAttribute
    {
        public MinAttribute(int min)
        {
            Min = min;
        }

        public MinAttribute(int min, string errorMessage)
            : base(errorMessage)
        {
            Min = min;
        }

        protected MinAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            if (value is int)
            {
                var number = (int)value;

                return number >= Min;
            }

            return false;
        }

        public int Min { get; private set; }
    }
}