using UnityEditor;
using UnityEngine;

namespace SubLib.Tools.Editor
{
    public class SwapObjects : ScriptableWizard
    {
        public GameObject DesiredObject;

        private void OnWizardUpdate()
        {
            helpString = "Select Game Objects";
            isValid = (DesiredObject != null);
        }

        private void OnWizardCreate()
        {
            var selected = Selection.gameObjects;
            foreach (var go in selected)
            {
                var newPrefab = PrefabUtility.InstantiatePrefab(DesiredObject, go.transform.parent) as GameObject;
                Undo.RegisterCreatedObjectUndo(newPrefab, "swap");
                CopyTransform(go.transform, newPrefab.transform);
                Undo.DestroyObjectImmediate(go);
            }
        }

        private static void CopyTransform(Transform copy, Transform to)
        {
            to.position = copy.position;
            to.rotation = copy.rotation;
            to.localScale = copy.localScale;
        }

        [MenuItem("GameObject/Swap objects", false, 4)]
        private static void CreateWindow()
        {
            DisplayWizard("Swap objects", typeof(SwapObjects), "Swap");
        }
    }
}