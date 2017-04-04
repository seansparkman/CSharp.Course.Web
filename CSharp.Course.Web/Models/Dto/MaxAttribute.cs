using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSharp.Course.Web.Models.Dto
{
    public class MaxAttribute : ValidationAttribute
    {
        public MaxAttribute(int max)
        {
            Max = max;
        }

        public MaxAttribute(int max, string errorMessage)
            : base(errorMessage)
        {
            Max = max;
        }

        protected MaxAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            if (value is int)
            {
                var number = (int)value;

                return number < Max;
            }

            return false;
        }

        public int Max { get; private set; }
    }
}