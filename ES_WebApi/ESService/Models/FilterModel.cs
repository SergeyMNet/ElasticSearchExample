using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ESService.Enums;
using Newtonsoft.Json;

namespace ESService.Models
{
    public class FilterModel
    {
        public string Category { get; set; }
        public List<SearchModel> SearchModels { get; set; } = new List<SearchModel>();

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (
                    !SearchModels.Any() ||
                    SearchModels.Any(m => String.IsNullOrEmpty(m.Field)) ||
                    SearchModels.Any(m => String.IsNullOrWhiteSpace(m.Field)) ||
                    SearchModels.Any(m => !Enum.IsDefined(typeof(Oper), m.Oper)) ||
                    SearchModels.Any(m => !Enum.IsDefined(typeof(OperForNext), m.NextOper) ||
                    SearchModels.Any(mm => (mm.Oper > 0 && !Regex.IsMatch(mm.Text, @"^-*[0-9\.]+$")))))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}