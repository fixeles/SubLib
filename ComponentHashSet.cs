using System;
using System.Linq;
using UnityEngine;

namespace UtilsSubmodule
{
    [Serializable]
    public class ComponentHashSet<T> : SerializableHashSet<T> where T : Component
    {
        public T FirstOrDefault() => HashSet.FirstOrDefault(x => !x.gameObject.activeSelf);
    }
}