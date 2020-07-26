using System;
using System.Linq;

namespace ServiceRequestManagement.API.Application.Extensions
{
    /// <summary>
    /// Class extensions to Generic Types.
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// Checks if the type passed is a generic type. If it is, it will return the parsed type name. If it is not, it will just return the type name as a string.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Type name of the passed type</returns>
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(type => type.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
                typeName = type.Name;

            return typeName;
        }

        /// <summary>
        /// Overloaded implementation of GetGenericTypeName. Will grab the type off of the object, and then used the extension method to return the type name as a string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Type name of the passed object</returns>
        public static string GetGenericTypeName(this object obj)
        {
            return obj.GetType().GetGenericTypeName();
        }
    }
}
