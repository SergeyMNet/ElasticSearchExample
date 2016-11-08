using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESService.Models
{
    public class SearchModel
    {
        public string Field { get; set; }
        public string Text { get; set; }

        public int Oper { get; set; } = 0;
        public int NextOper { get; set; } = 0;
    }
}