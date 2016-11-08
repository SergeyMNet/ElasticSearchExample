using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESService.Models
{
    public class ResultSearch : ResultBase
    {
        public List<string> Path { get; set; }

        public ResultSearch()
        {

        }

        public ResultSearch(List<string> result)
        {
            Path = result;
            IsSuccess = true;
        }

        public ResultSearch(List<string> result, string message)
        {
            Path = result;
            Message = message;
            IsSuccess = true;
        }
    }
}