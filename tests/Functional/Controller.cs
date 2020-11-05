namespace System.Net.Http.Functional
{
    using Microsoft.AspNetCore.Mvc;
    using Models;

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