using System.Collections.Generic;

namespace AuditLog.Elastic
{

    /// <summary>
    /// Represents audit logger configuration <see cref="AuditLogger"/>
    /// </summary>
    public class AuditLoggerConfiguration
    {

        /// <summary>
        /// Elastic server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Elastic log index
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Property names to be ignored in logs
        /// </summary>
        public List<string> IgnoreLogProperties { get; set; }

    }

}
