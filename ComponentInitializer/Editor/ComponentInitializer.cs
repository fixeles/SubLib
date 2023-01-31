using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace SubLib.ComponentInitializer.Editor
{
    [InitializeOnLoad]
    public class ComponentInitializer
    {
        static ComponentInitializer()
        {
            ObjectFactory.componentWasAdded += ComponentsAutoInit;
            ObjectFactory.componentWasAdded += AttributesCheck;

            EditorApplication.quitting -= OnEditorQuiting;
            EditorApplication.quitting += OnEditorQuiting;
        }

        private static void ComponentsAutoInit(Component obj)
        {
            if (obj is IAutoInit autoinit)
            {
                autoinit.AutoInit();
            }
        }

        private static async void AttributesCheck(Component obj)
        {
            var attributes = obj.GetType().GetTypeInfo().GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                if (attribute is not ComponentDependency dependency) continue;
                if (dependency.ComponentsCheck(obj)) continue;

                await UniTask.Yield();
                Object.DestroyImmediate(obj);
            }
        }

        private static void OnEditorQuiting()
        {
            ObjectFactory.componentWasAdded -= ComponentsAutoInit;
            ObjectFactory.componentWasAdded -= AttributesCheck;
            EditorApplication.quitting -= OnEditorQuiting;
        }
    }
}