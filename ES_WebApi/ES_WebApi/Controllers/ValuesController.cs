using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ESService.Interfaces;
using ESService.Models;
using Ninject;

namespace ES_WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        [Inject]
        public ISearchService SearchService { get; set; }


        // POST api/values
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

        // POST api/values
        public void Post([FromBody]string value)
        {
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
