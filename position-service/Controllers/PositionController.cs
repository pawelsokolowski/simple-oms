using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using position_service.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace position_service.Controllers
{
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private readonly ILogger<PositionController> _logger;
        private readonly PositionProcessor _positionProcessor;

        public PositionController(ILogger<PositionController> logger, PositionProcessor positionProcessor)
        {
            _logger = logger;
            _positionProcessor = positionProcessor;
        }

        // GET: api/<controller>
        [HttpGet]
        public List<Position> Get()
        {
            return _positionProcessor.Positions;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public List<Position> Get(string id)
        {
            return _positionProcessor.Positions.Where(p => p.Key == id).ToList();
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
