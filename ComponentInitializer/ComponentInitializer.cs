#if UNITY_EDITOR
using UnityEditor;
#endif

[InitializeOnLoad]
public class ComponentInitializer
{
#if UNITY_EDITOR

    static ComponentInitializer()
    {
        ObjectFactory.componentWasAdded += ComponentsAutoInit;

        EditorApplication.quitting -= OnEditorQuiting;
        EditorApplication.quitting += OnEditorQuiting;
    }

    private static void ComponentsAutoInit(UnityEngine.Component obj)
    {
        if (obj is IAutoInit autoinit)
        {
            autoinit.AutoInit();
        }
    }

    private static void OnEditorQuiting()
    {
        ObjectFactory.componentWasAdded -= ComponentsAutoInit;
        EditorApplication.quitting -= OnEditorQuiting;
    }

#endif
}

public interface IAutoInit
{
    public void AutoInit();
}
