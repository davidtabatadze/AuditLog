using System;
using MongoDB.Driver.Wrapper;
using MongoDB.Bson.Serialization.Attributes;

namespace AuditLog.Mongo
{

    /// <summary>
    /// Represents actual log record
    /// </summary>
    internal class Log : MongoEntity<long>
    {

        /// <summary>
        /// Event date and time
        /// </summary>
        [BsonSerializer(typeof(MongoSerializerDateTimeLocal))]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Source ip address
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Source host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Application page
        /// </summary>
        public string Page { get; set; }

        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Event status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Application user
        /// </summary>
        public long User { get; set; }

        /// <summary>
        /// Raw data
        /// </summary>
        public string Raw { get; set; }

        /// <summary>
        /// Response data
        /// </summary>
        public string Response { get; set; }

    }

}
