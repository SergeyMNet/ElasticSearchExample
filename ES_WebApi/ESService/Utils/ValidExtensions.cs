using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESService.Utils
{
    public static class ValidExtensions
    {
        public static void AddIfNotNull(List<string> target, string s)
        {
            if (target == null)
                return;

            if (!String.IsNullOrEmpty(s))
                target.Add(s);
        }

        public static void AddIfNotNull(List<string> target, List<string> s)
        {
            if (target == null)
                return;

            if (s.Any())
                target.AddRange(s);
        }

        public static void AddIfNotNull(List<int> target, int i)
        {
            if (target == null)
                return;

            if (i != 0)
                target.Add(i);
        }

        public static void AddIfNotNull(List<int> target, List<int> i)
        {
            if (target == null)
                return;

            if (i.Any())
                target.AddRange(i);
        }


        public static void AddIfNotNull(List<double> target, double i)
        {
            if (target == null)
                return;

            if (i != 0)
                target.Add(i);
        }

        public static void AddIfNotNull(List<double> target, List<double> i)
        {
            if (target == null)
                return;

            if (i.Any())
                target.AddRange(i);
        }
    }
}
