using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{
    [Route("api/Form/")]
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult PostEnvoyer([FromBody] string str)
        {
            return Ok();
        }
    }
}
