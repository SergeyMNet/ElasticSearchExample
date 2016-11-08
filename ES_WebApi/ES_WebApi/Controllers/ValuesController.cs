using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using ESService.Interfaces;
using ESService.Models;
using Ninject;

namespace ES_WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        [Inject]
        public ISearchService SearchService { get; set; }


        /// <summary>
        /// api/search
        /// Get path to XML file by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("search")]
        public IHttpActionResult PostSearch(FilterModel filter)
        {
            if (!filter.IsValid)
            {
                return BadRequest("Filter is not valid");
            }

            var result = SearchService.GetSearch(filter);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            if (result.IsSuccess && result.Path.Any())
            {
                return Ok(result.Path);
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// api/submit
        /// post some XML file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("submit")]
        public IHttpActionResult PostSubmit(HttpRequestMessage request)
        {
            if (request == null)
            {
                return BadRequest("Data not valid");
            }

            //return doc.DocumentElement.OuterXml;

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(request.Content.ReadAsStreamAsync().Result);
            }
            catch (Exception ex)
            {
                return BadRequest("XML not valid: " + ex.Message);

            }

            
            // TODO: add path to
            string pathTo = "path to ...";

            // Send to ES
            var result = SearchService.PostData(doc, pathTo);

            #region Response

            if (result.IsSuccess)
            {
                return Ok("Its Ok, " + result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }

            #endregion
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
