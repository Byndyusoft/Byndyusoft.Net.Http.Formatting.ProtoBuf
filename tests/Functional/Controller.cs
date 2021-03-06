﻿using System.Net.Http.Formatting.Models;
using Microsoft.AspNetCore.Mvc;

namespace System.Net.Http.Formatting.Functional
{
    [Controller]
    [Route("protobuf-formatter")]
    public class ProtoBufFormatterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SimpleType model)
        {
            return Ok(model);
        }

        [HttpPut]
        public IActionResult Put([FromBody] SimpleType model)
        {
            return Ok(model);
        }
    }
}