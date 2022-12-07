using System;
using System.Threading;
using System.Threading.Tasks;
using ExtensionsMain;
using UnityEngine;
using UnityEngine.UI;

namespace UtilsSubmodule.Extensions
{
    public static class ExtensionsAsync
    {
        public static async Task FadeAsync(this CanvasGroup group, CancellationToken token, float endValue,
            float duration = 0.1f)
        {
            float transition = 0;
            while (transition < 1)
            {
                transition += Time.deltaTime / Mathf.Clamp(duration, float.Epsilon, float.MaxValue);
                group.alpha = Mathf.Lerp(group.alpha, endValue, transition);
                await Task.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async Task BlinkAsync(this Renderer renderer, CancellationToken token, float duration = 0.1f)
        {
            await renderer.RecolorAsync(Color.white, token, duration / 2);
            await renderer.RecolorAsync(Color.black, token, duration / 2);
        }

        public static async Task BlinkAsync(this Renderer renderer, Color color, CancellationToken token,
            float duration = 0.1f)
        {
            await renderer.RecolorAsync(color, token, duration / 2);
            await renderer.RecolorAsync(Color.black, token, duration / 2);
        }

        public static async Task RecolorAsync(this Renderer renderer, Color target, CancellationToken token,
            float duration = 0.1f)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);

            int propertyID =
                renderer.material.shader.GetPropertyNameId(
                    renderer.material.shader.FindPropertyIndex("_EmissionColor"));
            Color startColor = renderer.material.GetColor(propertyID);

            var materials = renderer.materials;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetColor(propertyID, Color.Lerp(startColor, target, transition));
                }


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
            var color = Color.white;
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
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            var startPos = transform.position;

            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                transform.position = Vector3.Lerp(startPos, target, transition);

                await Task.Yield();
            }
        }

        public static async Task LerpAsync(this Transform transform, Transform target, CancellationToken token,
            float duration = 0.1f, bool rotate = true)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);

            var startPos = transform.position;
            var startRot = transform.rotation;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
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
            CancellationToken token, float rotationSpeed = 5)
        {
            float timeElapsed = 0;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                transform.HorizontalSoftLookAt(target.position, rotationSpeed);
                await Task.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async Task CurveMoveAsync(this Transform transform, Transform target,
            TransitionCurves curves, CancellationToken token = default, bool rotate = true)
        {
            float transition = 0;

            var startPos = transform.position;
            var startRot = transform.rotation;

            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
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
            float transition = 0;

            var startPos = transform.position;
            var startRot = transform.rotation;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / curves.Duration;

                var nextPosition = Vector3.Lerp(startPos, targetPos, curves.MoveCurve.Evaluate(transition));
                nextPosition.y += curves.HeightCurve.Evaluate(transition);
                transform.position = nextPosition;

                var scaleValue = curves.ScaleCurve.Evaluate(transition);
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                transform.rotation = Quaternion.Lerp(startRot, targetRotation, transition);
                await Task.Yield();
            }
        }

        public static async Task MoveAsync(this Transform transform, Transform target, int speed, bool look = false,
            CancellationToken token = default)
        {
            float transition = 0;
            var startPos = transform.position;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                var timeLeft = target.DistanceTo(startPos) / speed;

                transition += Time.deltaTime / timeLeft;
                transform.position = Vector3.Lerp(startPos, target.position, transition);
                if (look) transform.LookAt(target);
                await Task.Yield();
            }
        }
    }
}