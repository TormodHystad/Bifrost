﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bifrost.Events
{
	/// <summary>
	/// Extensions for <see cref="EventSource"/>
	/// </summary>
    public static class EventSourceExtensions
	{
#pragma warning disable 1591 // Xml Comments
		public static class EventSourceHandleMethods<T>
        {
            public static readonly Dictionary<Type, MethodInfo> MethodsPerEventType = new Dictionary<Type, MethodInfo>();

            static EventSourceHandleMethods()
            {
                var eventSourceType = typeof(T);

                Func<MethodInfo, bool> hasEventParameter = (m) =>
                                                               {
                                                                   var parameters = m.GetParameters();
                                                                   if (parameters.Length != 1)
                                                                       return false;

                                                                   return typeof(IEvent).IsAssignableFrom(parameters.Single().ParameterType);
                                                               };

                var methods = eventSourceType.GetMethods(BindingFlags.NonPublic|BindingFlags.Instance).Where(m => m.Name.Equals("Handle") && hasEventParameter(m));
                foreach (var method in methods)
                    MethodsPerEventType[method.GetParameters()[0].ParameterType] = method;
            }
        }


        private static Dictionary<Type, MethodInfo> GetHandleMethodsFor(Type eventSourceType)
        {
            // ReSharper disable PossibleNullReferenceException
            var methods = typeof(EventSourceHandleMethods<>)
                              .MakeGenericType(eventSourceType)
                              .GetField("MethodsPerEventType", BindingFlags.Public | BindingFlags.Static)
                              .GetValue(null) as Dictionary<Type, MethodInfo>;
            // ReSharper restore PossibleNullReferenceException
            return methods;


        }

#pragma warning restore 1591 // Xml Comments


		/// <summary>
		/// Get handle method from an <see cref="EventSource"/> for a specific <see cref="IEvent"/>, if any
		/// </summary>
		/// <param name="eventSource"><see cref="EventSource"/> to get method from</param>
		/// <param name="event"><see cref="IEvent"/> to get method for</param>
		/// <returns><see cref="MethodInfo"/> containing information about the handle method, null if none exists</returns>
		public static MethodInfo GetHandleMethod(this EventSource eventSource, IEvent @event)
        {
            var eventType = @event.GetType();
            var handleMethods = GetHandleMethodsFor(eventSource.GetType());
            return handleMethods.ContainsKey(eventType) ? handleMethods[eventType] : null;
        }

        /// <summary>
        /// Indicates whether the Event Source maintains state and requires to handles events to restore that state
        /// </summary>
        /// <param name="eventSource"><see cref="EventSource"/> to test for state</param>
        /// <returns>true if the Event Source does not maintain state</returns>
        public static bool IsStateless(this EventSource eventSource)
        {
            return GetHandleMethodsFor(eventSource.GetType()).Count == 0;
        }
    }
}