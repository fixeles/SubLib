using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using ExtensionsMain;
using UnityEngine.UI;

namespace ExtensionsAsync
{
    public static class ExtensionsAsync
    {
        public static async Task BlinkAsync(this Renderer renderer, CancellationToken token, float duration = 0.1f)
        {
            await renderer.RecolorAsync(Color.white, token, duration / 2);
            await renderer.RecolorAsync(Color.black, token, duration / 2);
        }

        public static async Task RecolorAsync(this Renderer renderer, Color target, CancellationToken token,
            float duration = 0.1f)
        {
            var levelToken = AsyncCancellation.Token;

            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            int propertyID =
                renderer.material.shader.GetPropertyNameId(
                    renderer.material.shader.FindPropertyIndex("_EmissionColor"));
            Color startColor = renderer.material.GetColor(propertyID);

            while (transition < 1)
            {
                if (AsyncCancellation.IsCancelled(token, levelToken)) return;
                transition += Time.deltaTime / duration;
                renderer.material.SetColor(propertyID, Color.Lerp(startColor, target, transition));

                await Task.Yield();
            }
        }

        public static async Task FillAsync(this Image image, CancellationToken token, float speed = 1)
        {
            while (true)
            {
                image.fillAmount += Time.deltaTime * speed;
                if (image.fillAmount is 0 or 1) return;
                await Task.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async Task FadeAsync(this Image image, CancellationToken token, float duration = 0.1f)
        {
            Color color = Color.white;
            while (color.a > 0)
            {
                color.a -= Time.deltaTime / duration;
                image.color = color;
                await Task.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async Task MoveAsync(this Transform transform, Vector3 target, CancellationToken token,
            float duration = 0.1f)
        {
            var levelToken = AsyncCancellation.Token;
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            Vector3 startPos = transform.position;

            while (transition < 1)
            {
                if (AsyncCancellation.IsCancelled(token, levelToken)) return;
                transition += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(startPos, target, transition);

                await Task.Yield();
            }
        }

        public static async Task LerpAsync(this Transform transform, Transform target, CancellationToken token,
            float duration = 0.1f, bool rotate = true)
        {
            var levelToken = AsyncCancellation.Token;
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            while (transition < 1)
            {
                if (AsyncCancellation.IsCancelled(token, levelToken)) return;
                transition += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(startPos, target.position, transition);
                if (rotate) transform.rotation = Quaternion.Lerp(startRot, target.rotation, transition);
                await Task.Yield();
            }

            transform.position = target.position;
            if (rotate) transform.rotation = target.rotation;
        }

        public static async Task RescaleAsync(this Transform transform, Vector3 from, Vector3 to,
            CancellationToken token, float duration = 0.1f)
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

        public static async Task HorizontalSoftLookAtAsync(this Transform transform, Transform target, float duration,
            float rotationSpeed = 5, params CancellationToken[] tokens)
        {
            float timeElapsed = 0;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                transform.HorizontalSoftLookAt(target.position, rotationSpeed);
                await Task.Yield();
                if (AsyncCancellation.IsCancelled(tokens)) return;
            }
        }

        public static async Task CurveMoveAsync(this Transform transform, Transform target,
            TransitionCurves curves, CancellationToken token = default, bool rotate = true)
        {
            var levelToken = AsyncCancellation.Token;
            float transition = 0;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;

            while (transition < 1)
            {
                if (AsyncCancellation.IsCancelled(token, levelToken)) return;
                transition += Time.deltaTime / curves.Duration;

                var nextPosition = Vector3.Lerp(startPos, target.position, curves.MoveCurve.Evaluate(transition));
                nextPosition.y += curves.HeightCurve.Evaluate(transition);
                transform.position = nextPosition;

                float scaleValue = curves.ScaleCurve.Evaluate(transition);
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                if (rotate) transform.rotation = Quaternion.Lerp(startRot, target.rotation, transition);
                await Task.Yield();
            }
        }

        public static async Task CurveMoveAsync(this Transform transform, Vector3 targetPos,
            Quaternion targetRotation,
            TransitionCurves curves, CancellationToken token = default, bool rotate = true)
        {
            var levelToken = AsyncCancellation.Token;
            float transition = 0;

            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;
            while (transition < 1)
            {
                if (AsyncCancellation.IsCancelled(token, levelToken)) return;
                transition += Time.deltaTime / curves.Duration;

                var nextPosition = Vector3.Lerp(startPos, targetPos, curves.MoveCurve.Evaluate(transition));
                nextPosition.y += curves.HeightCurve.Evaluate(transition);
                transform.position = nextPosition;

                float scaleValue = curves.ScaleCurve.Evaluate(transition);
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                transform.rotation = Quaternion.Lerp(startRot, targetRotation, transition);
                await Task.Yield();
            }
        }

        public static async Task MoveAsync(this Transform transform, Transform target, int speed,
            CancellationToken token = default)
        {
            var levelToken = AsyncCancellation.Token;
            float transition = 0;
            Vector3 startPos = transform.position;

            while (transition < 1)
            {
                if (AsyncCancellation.IsCancelled(token, levelToken)) return;
                float timeLeft = transform.DistanceTo(target) / speed;
                transition += Time.deltaTime / timeLeft;
                transform.position = Vector3.Lerp(startPos, target.position, transition);

                await Task.Yield();
            }
        }
    }
}