using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AuditLog.Elastic
{

    /// <summary>
    /// Represents property ignore functionality for log object
    /// </summary>
    internal class LogPropertyIgnoreResolver : DefaultContractResolver
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="properties">Ignore property names</param>
        public LogPropertyIgnoreResolver(IEnumerable<string> properties)
        {
            foreach (var property in properties)
            {
                Ignores.Add(property);
            }
        }

        /// <summary>
        /// Ignore property names array
        /// </summary>
        private List<string> Ignores = new List<string> { };

        /// <summary>
        /// Ignoring requesting properties of log object
        /// </summary>
        /// <param name="member">Member info</param>
        /// <param name="serialization">Member serialization</param>
        /// <returns>Returns property with flag: serialize of ignore</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization serialization)
        {
            // Creating property
            var property = base.CreateProperty(member, serialization);
            // In case if property name is the one to ignore
            if (Ignores.Contains(property.PropertyName))
            {
                // We mark the property to be ignored
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }
            // Returning falged property
            return property;
        }

    }

}
