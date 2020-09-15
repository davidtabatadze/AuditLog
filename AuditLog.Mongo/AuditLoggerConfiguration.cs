using MongoDB.Driver.Wrapper;
using System.Collections.Generic;

namespace AuditLog.Mongo
{

    /// <summary>
    /// Represents audit logger configuration <see cref="AuditLogger"/>
    /// </summary>
    public class AuditLoggerConfiguration
    {

        /// <summary>
        /// Mongo configuration
        /// </summary>
        public MongoConfiguration MongoConfiguration { get; set; }

        /// <summary>
        /// Mongo log collection
        /// </summary>
        public string Collection { get; set; }

        /// <summary>
        /// Property names to be ignored in logs
        /// </summary>
        public List<string> IgnoreLogProperties { get; set; }

    }

}
