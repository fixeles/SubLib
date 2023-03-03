using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SubLib.Extensions
{
    public static class ExtensionsAsync
    {
        public static async UniTask FadeAsync(this CanvasGroup group, CancellationToken token, float endValue,
            float duration = 0.1f)
        {
            duration = Mathf.Clamp(duration, float.Epsilon, float.MaxValue);
            float transition = 0;
            while (transition < 1)
            {
                transition += Time.deltaTime / duration;
                group.alpha = Mathf.Lerp(group.alpha, endValue, transition);
                await UniTask.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async UniTask FadeAsync(this Image image, CancellationToken token, float endValue,
            float duration = 0.1f)
        {
            duration = Mathf.Clamp(duration, float.Epsilon, float.MaxValue);
            var color = image.color;
            float transition = 0;
            while (transition < 1)
            {
                transition += Time.deltaTime / duration;
                color.a = Mathf.Lerp(color.a, endValue, transition);
                image.color = color;
                await UniTask.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async UniTask BlinkAsync(this Renderer renderer, CancellationToken token, float duration = 0.1f)
        {
            await renderer.RecolorAsync(Color.white, token, duration / 2);
            await renderer.RecolorAsync(Color.black, token, duration / 2);
        }

        public static async UniTask BlinkAsync(this Renderer renderer, Color color, CancellationToken token,
            float duration = 0.1f)
        {
            await renderer.RecolorAsync(color, token, duration / 2);
            await renderer.RecolorAsync(Color.black, token, duration / 2);
        }

        public static async UniTask RecolorAsync(this Renderer material, Color target, CancellationToken token,
            float duration = 0.1f)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);

            int propertyID =
                material.material.shader.GetPropertyNameId(
                    material.material.shader.FindPropertyIndex("_EmissionColor"));
            Color startColor = material.material.GetColor(propertyID);

            var materials = material.materials;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetColor(propertyID, Color.Lerp(startColor, target, transition));
                }


                await UniTask.Yield();
            }
        }

        public static async UniTask RecolorAsync(this Material material, Color target, int propertyID,
            CancellationToken token,
            float duration = 0.1f)
        {
            float transition = 0;
            duration = Mathf.Clamp(duration, 0.1f, float.MaxValue);
            Color startColor = material.GetColor(propertyID);

            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                material.SetColor(propertyID, Color.Lerp(startColor, target, transition));

                await UniTask.Yield();
            }
        }

        public static async UniTask FillAsync(this Image image, CancellationToken token, float speed = 1)
        {
            while (true)
            {
                image.fillAmount += Time.deltaTime * speed;
                if (image.fillAmount is 0 or 1) return;
                await UniTask.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async UniTask FadeAsync(this Image image, CancellationToken token, float duration = 0.1f)
        {
            var color = Color.white;
            while (color.a > 0)
            {
                color.a -= Time.deltaTime / duration;
                image.color = color;
                await UniTask.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async UniTask MoveAsync(this Transform transform, Vector3 target, CancellationToken token,
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

                await UniTask.Yield();
            }
        }

        public static async UniTask LerpAsync(this Transform transform, Transform target, CancellationToken token,
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
                await UniTask.Yield();
            }

            transform.position = target.position;
            if (rotate) transform.rotation = target.rotation;
        }

        public static async UniTask RescaleAsync(this Transform transform, Vector3 from, Vector3 to,
            CancellationToken token, float duration = 0.1f)
        {
            float transition = 0;
            while (transition < 1)
            {
                if (token.IsCancellationRequested) return;
                transition += Time.deltaTime / duration;
                transform.localScale = Vector3.Lerp(from, to, transition);
                await UniTask.Yield();
            }
        }

        public static async UniTask HorizontalSoftLookAtAsync(this Transform transform, Transform target,
            float duration,
            CancellationToken token, float rotationSpeed = 5)
        {
            float timeElapsed = 0;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                transform.HorizontalSoftLookAt(target.position, rotationSpeed);
                await UniTask.Yield();
                if (token.IsCancellationRequested) return;
            }
        }

        public static async UniTask CurveMoveAsync(this Transform transform, Transform target,
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
                await UniTask.Yield();
            }
        }

        public static async UniTask CurveMoveAsync(this Transform transform, Vector3 targetPos,
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
                await UniTask.Yield();
            }
        }

        public static async UniTask MoveAsync(this Transform transform, Transform target, int speed, bool look = false,
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
                await UniTask.Yield();
            }
        }
    }
}