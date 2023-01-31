using UnityEngine;

namespace SubLib
{
    public class SingleInstanceController : MonoBehaviour
    {
        [SerializeField] private Behaviour _component;

        private void Start()
        {
            var components = FindObjectsOfType(_component.GetType()) as Behaviour[];
            if (components.Length == 1) return;

            for (int i = 0; i < components.Length - 1; i++)
            {
                components[i].enabled = false;
            }
        }
    }
}