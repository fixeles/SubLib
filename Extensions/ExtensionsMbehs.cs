using System;
using System.Collections.Generic;
using SubLib.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace SubLib.Extensions
{
    public static class ExtensionsMbehs
    {
        public static AnimationClip GetClip(this Animator animator, string name)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals(name)) return clip;
            }

            return null;
        }

        public static void LockJoint(this ConfigurableJoint joint, bool value = true)
        {
            if (value)
            {
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
            }
            else
            {
                joint.xMotion = ConfigurableJointMotion.Free;
                joint.yMotion = ConfigurableJointMotion.Free;
                joint.zMotion = ConfigurableJointMotion.Free;
            }
        }

        public static bool IsReached(this NavMeshAgent agent)
        {
            if (agent.transform.DistanceTo(agent.destination) < agent.stoppingDistance) return true;
            return false;
        }

        public static bool IsReached(this NavMeshAgent agent, Vector3 target)
        {
            if (agent.transform.DistanceTo(target) > agent.stoppingDistance) return false;
            return true;
        }

        public static T GetNearestObject<T>(this List<T> list, Vector3 target)
            where T : Behaviour
        {
            if (list.Count == 0) return null;
            T nearestObject = list[0];
            float minDistance = float.MaxValue;

            foreach (var item in list)
            {
                float distance = item.transform.DistanceTo(target);
                if (distance > minDistance) continue;

                minDistance = distance;
                nearestObject = item;
            }

            return nearestObject;
        }

        /*  public static T CopyComponent<T>(this T original, GameObject destination) where T : Component
          {
              System.Type type = original.GetType();
              Component copy;
              if (!destination.TryGetComponent(type, out copy)) destination.AddComponent(type);

              System.Reflection.FieldInfo[] fields = type.GetFields();
              foreach (System.Reflection.FieldInfo field in fields)
                  field.SetValue(copy, field.GetValue(original));
              return copy as T;
          }*/

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }

            return dst as T;
        }

        public static void CopyTransform(this Transform copy, Transform to)
        {
            to.position = copy.position;
            to.rotation = copy.rotation;
            to.localScale = copy.localScale;
        }
    }
}