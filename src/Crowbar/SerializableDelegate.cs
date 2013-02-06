using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Crowbar
{
    /// <summary>
    /// Adapted from http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx.
    /// </summary>
    [Serializable]
    public class SerializableDelegate<TDelegate> : ISerializable
        where TDelegate : class
    {
        /// <summary>
        /// Creates a new instance of <see cref="SerializableDelegate{TDelegate}"/>.
        /// </summary>
        /// <param name="delegate">The delegate that should be de/serialized.</param>
        public SerializableDelegate(TDelegate @delegate)
        {
            Delegate = @delegate;
        }

        /// <summary>
        /// The delegates the should be de/serialized.
        /// </summary>
        public TDelegate Delegate { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="SerializableDelegate{TDelegate}"/>.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        public SerializableDelegate(SerializationInfo info, StreamingContext context)
        {
            var delegateType = (Type)info.GetValue("delegateType", typeof(Type));
            if (info.GetBoolean("isSerializable"))
            {
                // If it's a "simple" delegate we just read it straight off.
                Delegate = (TDelegate)info.GetValue("delegate", delegateType);
            }
            else
            {
                // ... otherwise, we need to read its anonymous class.
                var methodInfo = (MethodInfo)info.GetValue("method", typeof(MethodInfo));
                var anonymousClassWrapper = (AnonymousClassWrapper)info.GetValue("class", typeof(AnonymousClassWrapper));
                Delegate = (TDelegate)(object)System.Delegate.CreateDelegate(delegateType, anonymousClassWrapper.TargetInstance, methodInfo);
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("delegateType", Delegate.GetType());
            var untypedDelegate = (Delegate)(object)Delegate;
            if ((untypedDelegate.Target == null || untypedDelegate.Method.DeclaringType.GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0) && Delegate != null)
            {
                // If it's an "simple" delegate we can serialize it directly...
                info.AddValue("isSerializable", true);
                info.AddValue("delegate", Delegate);
            }
            else
            {
                // ... otherwise, serialize anonymous class.
                info.AddValue("isSerializable", false);
                info.AddValue("method", untypedDelegate.Method);
                info.AddValue("class", new AnonymousClassWrapper(untypedDelegate.Method.DeclaringType, untypedDelegate.Target));
            }
        }

        [Serializable]
        private class AnonymousClassWrapper : ISerializable
        {
            private readonly Type targetType;

            public AnonymousClassWrapper(Type targetType, object targetInstance)
            {
                this.targetType = targetType;
                TargetInstance = targetInstance;
            }

            public object TargetInstance { get; private set; }

            protected AnonymousClassWrapper(SerializationInfo info, StreamingContext context)
            {
                var classType = (Type)info.GetValue("classType", typeof(Type));
                TargetInstance = Activator.CreateInstance(classType);

                foreach (var field in classType.GetFields())
                {
                    if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                    {
                        // If the field is a delegate.
                        field.SetValue(TargetInstance, ((SerializableDelegate<TDelegate>)info.GetValue(field.Name, typeof(SerializableDelegate<TDelegate>))).Delegate);
                    }
                    else if (!field.FieldType.IsSerializable)
                    {
                        // If the field is an anonymous class.
                        field.SetValue(TargetInstance, ((AnonymousClassWrapper)info.GetValue(field.Name, typeof(AnonymousClassWrapper))).TargetInstance);
                    }
                    else
                    {
                        field.SetValue(TargetInstance, info.GetValue(field.Name, field.FieldType));
                    }
                }
            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("classType", targetType);
                foreach (var field in targetType.GetFields())
                {
                    if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                    {
                        // If the field is a delegate.
                        info.AddValue(field.Name, new SerializableDelegate<TDelegate>((TDelegate)field.GetValue(TargetInstance)));
                    }
                    else if (!field.FieldType.IsSerializable)
                    {
                        // If the field is an anonymous class.
                        info.AddValue(field.Name, new AnonymousClassWrapper(field.FieldType, field.GetValue(TargetInstance)));
                    }
                    else
                    {
                        info.AddValue(field.Name, field.GetValue(TargetInstance));
                    }
                }
            }
        }
    }
}