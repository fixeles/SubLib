using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool IsReached(this NavMeshAgent agent) =>
            agent.transform.DistanceTo(agent.destination) < agent.stoppingDistance;


        public static bool IsReached(this NavMeshAgent agent, Vector3 target) =>
            agent.transform.DistanceTo(target) < agent.stoppingDistance;


        public static Vector3 FindPointOnPath(this NavMeshAgent agent, Vector3 target, float distanceFromEnd)
        {
            if (distanceFromEnd <= 0) return target;
            var path = new NavMeshPath();
            agent.CalculatePath(target, path);

            var corners = path.corners;

            for (int i = corners.Length - 2; i >= 0; i--)
            {
                float segmentDistance = (corners[i] - corners[i + 1]).magnitude;
                if (segmentDistance >= distanceFromEnd)
                    return Vector3.Lerp(corners[i], corners[i + 1], distanceFromEnd / segmentDistance);

                distanceFromEnd -= segmentDistance;
            }

            return corners[0];
        }

        public static T GetNearestObject<T>(this List<T> list, Vector3 target) where T : Behaviour
        {
            if (list.Count == 0) return null;
            return list.Aggregate((x, y) =>
                (x.transform.position - target).sqrMagnitude < (y.transform.position - target).sqrMagnitude
                    ? x
                    : y);
        }

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            Type type = original.GetType();
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

            return dst;
        }

        public static void CopyTransform(this Transform copy, Transform to)
        {
            to.position = copy.position;
            to.rotation = copy.rotation;
            to.localScale = copy.localScale;
        }
    }
}