using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using ExtensionsMain;

namespace ExtensionsAsync
{
    public static class ExtensionsAsync
    {
        public static async Task BlinkAsync(this Renderer renderer, CancellationToken token, float duration = 0.1f)
        {
            await renderer.RecolorAsync(Color.white, token, duration / 2);
            await renderer.RecolorAsync(Color.black, token, duration / 2);
        }

        public static async Task RecolorAsync(this Renderer renderer, Color target, CancellationToken token, float duration = 0.1f)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            int sessionID = AsyncCancellation.SessionID;
            int propertyID = renderer.material.shader.GetPropertyNameId(renderer.material.shader.FindPropertyIndex("_EmissionColor"));
            Color startColor = renderer.material.GetColor(propertyID);

            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                renderer.material.SetColor(propertyID, Color.Lerp(startColor, target, transition));

                await Task.Yield();
                if (AsyncCancellation.IsCancelled(sessionID)) return;
            }
        }

        public static async Task MoveAsync(this Transform transform, Vector3 target, CancellationToken token, float duration = 0.1f)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            int sessionID = AsyncCancellation.SessionID;
            Vector3 startPos = transform.position;

            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(startPos, target, transition);

                await Task.Yield();
                if (AsyncCancellation.IsCancelled(sessionID)) return;
            }
        }

        public static async Task LerpAsync(this Transform transform, Transform target, CancellationToken token, float duration = 0.1f, bool rotate = true)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            int sessionID = AsyncCancellation.SessionID;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(startPos, target.position, transition);
                if (rotate) transform.rotation = Quaternion.Lerp(startRot, target.rotation, transition);
                await Task.Yield();

                if (AsyncCancellation.IsCancelled(sessionID)) return;
            }
            transform.position = target.position;
            if (rotate) transform.rotation = target.rotation;
        }

        public static async Task ArcLerpAsync(this Transform transform, Transform target, CancellationToken token, float height = 5, float duration = 0.1f, bool rotate = true)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            int sessionID = AsyncCancellation.SessionID;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;

                Vector3 thirdPoint = new Vector3(target.position.x, target.position.y + height, target.position.z);

                Vector3 firstLerp = transform.position = Vector3.Lerp(startPos, thirdPoint, transition);
                Vector3 secondLerp = transform.position = Vector3.Lerp(thirdPoint, target.position, transition);

                transform.position = Vector3.Lerp(firstLerp, secondLerp, transition);
                if (rotate) transform.rotation = Quaternion.Lerp(startRot, target.rotation, transition);
                await Task.Yield();

                if (AsyncCancellation.IsCancelled(sessionID)) return;
            }
            transform.position = target.position;
            if (rotate) transform.rotation = target.rotation;
        }

        public static async Task RescaleAsync(this Transform transform, Vector3 from, Vector3 to, CancellationToken token, float duration = 0.1f)
        {
            float transition = 0;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                transform.localScale = Vector3.Lerp(from, to, transition);
                await Task.Yield();
            }
        }

        public static async Task CurveMoveAsync(this Transform transform, Transform target,
        TransitionCurves curves, float duration = 0.1f, CancellationToken token = default, bool rotate = true)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            int sessionID = AsyncCancellation.SessionID;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            Vector3 startScale = transform.localScale;
            while (transition < 1)
            {
                if (token.IsCancellationRequested || AsyncCancellation.IsCancelled(sessionID)) return;
                transition += Time.deltaTime / duration;

                var nextPosition = Vector3.Lerp(startPos, target.position, curves.MoveCurve.Evaluate(transition));
                nextPosition.y = nextPosition.y + curves.HeightCurve.Evaluate(transition);
                transform.position = nextPosition;

                float scaleValue = curves.ScaleCurve.Evaluate(transition);
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                if (rotate) transform.rotation = Quaternion.Lerp(startRot, target.rotation, transition);
                await Task.Yield();
            }
            transform.position = target.position;
            if (rotate) transform.rotation = target.rotation;
        }

        public static async Task CurveMoveAsync(this Transform transform, Vector3 targetPos, Quaternion targetRotation,
                TransitionCurves curves, float duration = 0.1f, CancellationToken token = default, bool rotate = true)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            int sessionID = AsyncCancellation.SessionID;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            Vector3 startScale = transform.localScale;
            while (transition < 1)
            {
                if (token.IsCancellationRequested || AsyncCancellation.IsCancelled(sessionID)) return;
                transition += Time.deltaTime / duration;

                var nextPosition = Vector3.Lerp(startPos, targetPos, curves.MoveCurve.Evaluate(transition));
                nextPosition.y = nextPosition.y + curves.HeightCurve.Evaluate(transition);
                transform.position = nextPosition;

                float scaleValue = curves.ScaleCurve.Evaluate(transition);
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                transform.rotation = Quaternion.Lerp(startRot, targetRotation, transition);
                await Task.Yield();
            }
            transform.position = targetPos;
            if (rotate) transform.rotation = targetRotation;
        }

        public static async Task MoveAsync(this Transform transform, Transform target, int speed, CancellationToken token = default)
        {
            float transition = 0;
            int sessionID = AsyncCancellation.SessionID;
            Vector3 startPos = transform.position;

            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                float timeLeft = transform.DistanceTo(target) / speed;
                transition += Time.deltaTime / timeLeft;
                transform.position = Vector3.Lerp(startPos, target.position, transition);

                await Task.Yield();
                if (AsyncCancellation.IsCancelled(sessionID)) return;
            }
        }

    }
}


