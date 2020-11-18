using Nest;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using CoreKit.Sync;
using CoreKit.Extension.String;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AuditLog.Elastic
{

    /// <summary>
    /// Represents audit logger manager
    /// </summary>
    public class AuditLogger : IDisposable
    {

        /// <summary>
        /// Ending class lifecycle
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="http">Http accessor</param>
        /// <param name="configuration">Configuration <see cref="AuditLoggerConfiguration"/></param>
        public AuditLogger(IHttpContextAccessor http, IOptions<AuditLoggerConfiguration> configuration) : this(http, configuration.Value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="http">Http accessor</param>
        /// <param name="configuration">Configuration <see cref="AuditLoggerConfiguration"/></param>
        public AuditLogger(IHttpContextAccessor http, AuditLoggerConfiguration configuration)
        {
            // ...
            Http = http;
            Configuration = configuration;
            Context = new ElasticClient(new ConnectionSettings(new Uri(configuration.Server)));
        }

        /// <summary>
        /// Http accessor object
        /// </summary>
        private IHttpContextAccessor Http { get; set; }

        /// <summary>
        /// Configuration object
        /// </summary>
        private AuditLoggerConfiguration Configuration { get; set; }

        /// <summary>
        /// Elastic context object
        /// </summary>
        public ElasticClient Context { get; set; }

        /// <summary>
        /// Gets serialized json string for object
        /// </summary>
        /// <param name="obj">Object to be serialized</param>
        /// <returns>Json string</returns>
        private string Serialize(object obj)
        {
            // If object is not present, we serialize nothing
            if (obj == null)
            {
                return null;
            }
            // If object is empty string, we serialize nothing
            if (obj is string && obj.ToString().TrimFull() == "")
            {
                return null;
            }
            // Ignoring configuration properties and serializing object
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ContractResolver = new LogPropertyIgnoreResolver(
                    Configuration.IgnoreLogProperties ?? new List<string> { }
                )
            });
        }

        /// <summary>
        /// Generates actual log record
        /// </summary>
        /// <param name="page">Application page</param>
        /// <param name="description">Event description</param>
        /// <param name="status">Event status</param>
        /// <param name="raw">Event Raw data</param>
        /// <param name="response">Event Response data</param>
        /// <param name="user">Application user</param>
        /// <returns>Log record</returns>
        private Log GenerateLog(string page, string description, string status, object raw = null, object response = null, long user = 0)
        {
            return new Log
            {
                DateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
                IP = Http.HttpContext.Connection.RemoteIpAddress.ToString(),
                Host = Http.HttpContext.Request.Host.ToString(),
                Page = page,
                Description = description,
                Status = status,
                Raw = Serialize(raw),
                Response = Serialize(response),
                User = user
            };
        }

        /// <summary>
        /// Log data
        /// </summary>
        /// <param name="page">Application page</param>
        /// <param name="description">Event description</param>
        /// <param name="status">Event status</param>
        /// <param name="raw">Event Raw data</param>
        /// <param name="response">Event Response data</param>
        /// <param name="user">Application user</param>
        public void Log(string page, string description, string status, object raw = null, object response = null, long user = 0)
        {
            // Do log
            SyncKit.Run(() => LogAsync(page, description, status, raw, response, user));
        }

        /// <summary>
        /// Log data
        /// </summary>
        /// <param name="page">Application page</param>
        /// <param name="description">Event description</param>
        /// <param name="status">Event status</param>
        /// <param name="raw">Event Raw data</param>
        /// <param name="response">Event Response data</param>
        /// <param name="user">Application user</param>
        /// <returns>Nothing</returns>
        public async Task LogAsync(string page, string description, string status, object raw = null, object response = null, long user = 0)
        {
            // Generating log record
            var log = GenerateLog(page, description, status, raw, response, user);
            // Saving log record to mongo
            await Context.IndexManyAsync(new List<Log> { log }, Configuration.Index);
        }

    }

}
