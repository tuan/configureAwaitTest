using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConfigureAwaitIssue.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetNumber()
        {
            var cultureBefore = Thread.CurrentThread.CurrentCulture.ToString();
            
            await Task.Delay(1000 * new Random().Next(5))
                .ConfigureAwait(false);

            var cultureAfter = Thread.CurrentThread.CurrentCulture.ToString();

            if (string.Compare(cultureAfter, cultureBefore, true) != 0)
            {
                return NotFound();
            }

            var formattedString = Convert.ToString(10.12);
            return Ok(formattedString);
        }
    }
}
