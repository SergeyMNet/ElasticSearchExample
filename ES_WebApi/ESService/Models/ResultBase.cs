using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESService.Models
{
    public class ResultBase
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }


        public ResultBase()
        {

        }

        public ResultBase(bool result, string message)
        {
            IsSuccess = result;
            Message = message;
        }
    }
}