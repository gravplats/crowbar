using System;

namespace Crowbar
{
    internal static class TypeExtensions
    {
        public static bool IsSerializable(this Exception exception)
        {
            return exception.GetType().IsSerializable();
        }

        public static bool IsSerializable(this Type type)
        {
            return type.GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0;
        }
    }
}