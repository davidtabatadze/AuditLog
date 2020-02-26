﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuditLog.Test
{

    [Produces("application/json")]
    public class Test : Controller
    {

        public Test(Elastic.AuditLogger elogger)
        {
            ELogger = elogger;
        }

        private Elastic.AuditLogger ELogger { get; set; }

        [HttpPost]
        [Route("elastic/async")]
        public async Task<IActionResult> ElasticAsync(string page, string description, string status, object raw = null, object response = null, long user = 0)
        {
            try
            {
                //var ooo = JsonConvert.SerializeObject(new { a = "olaaa" });
                raw = new { a = "olaaa", b = "1323", c = 100 };
                await ELogger.LogAsync(page, description, status, raw, response, user);
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

        [HttpPost]
        [Route("elastic/sync")]
        public IActionResult ElasticSync(string page, string description, string status, object raw = null, object response = null, long user = 0)
        {
            try
            {
                response = new { a = "olaaa", booo = "1323", c = 100 };
                ELogger.Log(page, description, status, raw, response, user);
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

    }

}