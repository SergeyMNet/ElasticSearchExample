using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ESService.Utils;

namespace ESService.Models.ModelsES
{
    public class SomeDataES
    {
        [String(Analyzer = "keyword_analyzer", Name = "primeid")]
        public string PrimeId { get; set; }

        [String(Analyzer = "keyword_analyzer", Name = "title")]
        public List<string> Title { get; set; } = new List<string>();

        [Date(Name = "date")]
        public List<DateTime> Date { get; set; } = new List<DateTime>();

        [String(Name = "PathTo")]
        public string PathTo { get; set; }

       

        public SomeDataES(string path = "", XmlDocument xmldoc = null)
        {
            XmlDocument doc = new XmlDocument();
            if (xmldoc == null)
            {
                doc.Load(path);
            }
            else
            {
                doc = xmldoc;
            }

            
            PathTo = path;

            PrimeId = GetDocPrimeID(doc);

            if (doc.DocumentElement != null)
                GetNodes(doc.DocumentElement.ChildNodes);
        }

       

        private void GetNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                // Title
                ValidExtensions.AddIfNotNull(Title, GetTitle(node));


                // Date
                if (node.Name == "date")
                {
                    DateTime res = new DateTime();
                    DateTime.TryParse(node.InnerText, out res);
                    if (res != new DateTime())
                        Date.Add(res);
                }

                

                if (node.HasChildNodes)
                {
                    GetNodes(node.ChildNodes);
                }

            }
        }

        private string GetDocPrimeID(XmlDocument doc)
        {
            if (doc.DocumentElement?.Attributes["primeID"] != null)
            {
                return doc.DocumentElement.Attributes["primeID"].Value;
            }
            return "";
        }

        private string GetTitle(XmlNode node)
        {
            if (node.Name == "title")
            {
                return node.InnerText;
            }
            return null;
        }
    }
}
