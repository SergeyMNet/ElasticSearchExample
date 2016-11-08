using System.Xml;
using ESService.Models;


namespace ESService.Interfaces
{
    public interface ISearchService
    {
        ResultSearch GetSearch(FilterModel filter);
        
        ResultBase PostData(XmlDocument doc, string path);
    }
}
