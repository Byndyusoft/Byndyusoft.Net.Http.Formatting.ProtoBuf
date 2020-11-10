using System;
using System.Collections.Generic;
using System.Linq;
using Byndyusoft.Net.Http.Formatting.ProtoBuf.Models;
using Microsoft.AspNetCore.Mvc;

namespace Byndyusoft.Net.Http.Formatting.ProtoBuf.Controllers
{
    [ApiController]
    [Route("peoples")]
    public class PeopleController : ControllerBase
    {
        private static readonly List<People> Peoples = new List<People>();

        [HttpGet]
        [Route("{id}")]
        public People Get(int id)
        {
            return Peoples.SingleOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public void Post([FromBody] People people)
        {
            if (people == null) throw new ArgumentNullException(nameof(people));

            Peoples.Add(people);
        }
    }
}