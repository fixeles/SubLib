using System;
using UnityEngine;

namespace SubLib.ComponentInitializer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentDependency : Attribute
    {
        private readonly Type[] _types;

        public ComponentDependency(params Type[] types)
        {
            _types = types;
        }

        public bool ComponentsCheck(Component component)
        {
            foreach (var type in _types)
            {
                if (component.TryGetComponent(type, out _)) continue;

                Debug.LogWarning($"{component} requires {type} on GameObject");
                return false;
            }

            return true;
        }
    }
}