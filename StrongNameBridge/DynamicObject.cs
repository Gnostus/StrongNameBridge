﻿using System;
using System.Reflection;
using System.Reflection.Emit;

namespace StrongNameBridge
{
    internal class DynamicObject
    {
        private readonly object _instance;
        private readonly Type _type;

        public DynamicObject(Object instance, Type type)
        {
            _instance = instance;
            _type = type;
        }

        public object Call(string methodName, object[] args = null)
        {
            return _type.InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, _instance, args);
        }

        public T Get<T>(string propertyName)
        {
            return (T)_type.GetField(propertyName).GetValue(_instance);
        }

        public void AddEvent(string eventName, Action handler)
        { 
            var typeEvent = _type.GetEvent(eventName); 
            var del = handler.Method.CreateDelegate(typeEvent.EventHandlerType);
            _type.GetEvent(eventName).AddEventHandler(_instance, del); 
        }

        public void Set(string propertyName, object value)
        {
           _type.GetField(propertyName).SetValue(_instance, value);
        }
    }
}
