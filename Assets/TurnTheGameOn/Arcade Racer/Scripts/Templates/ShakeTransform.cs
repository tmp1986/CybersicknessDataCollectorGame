namespace TurnTheGameOn.RacingGameTemplate
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShakeTransform : MonoBehaviour
    {
        public class ShakeEvent
        {
            float duration;
            float timeRemaining;
            ShakeData data;

            public ShakeData.Target target
            {
                get
                {
                    return data.target;
                }
            }

            Vector3 noiseOffset;
            public Vector3 noise;
            public ShakeEvent(ShakeData _data)
            {
                this.data = _data;
                duration = data.duration;
                timeRemaining = duration;
                float rand = 32.0f;
                noiseOffset.x = Random.Range(0.0f, rand);
                noiseOffset.y = Random.Range(0.0f, rand);
                noiseOffset.z = Random.Range(0.0f, rand);
            }

            public void Update()
            {
                float deltaTime = Time.deltaTime;
                timeRemaining -= deltaTime;
                float noiseOffsetDelta = deltaTime * data.frequency;
                noiseOffset.x += noiseOffsetDelta;
                noiseOffset.y += noiseOffsetDelta;
                noiseOffset.z += noiseOffsetDelta;

                noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
                noise.y = Mathf.PerlinNoise(noiseOffset.x, 1.0f);
                noise.z = Mathf.PerlinNoise(noiseOffset.x, 2.0f);

                noise -= Vector3.one * 0.5f;

                noise *= data.amplitude;

                float agePercent = 1.0f - (timeRemaining / duration);
                noise *= data.blendOverLifetime.Evaluate(agePercent);
            }

            public bool IsAlive()
            {
                return timeRemaining > 0.0f;
            }
        }

        List<ShakeEvent> shakeEvents = new List<ShakeEvent>();

        public void AddShakeEvent(ShakeData data)
        {
            shakeEvents.Add(new ShakeEvent(data));
        }

        public void AddShakeEvent(float _amplitude, float _frequency, float _duration, AnimationCurve _blendOverLifetime, ShakeData.Target _target)
        {
            ShakeData data = ScriptableObject.CreateInstance<ShakeData>();
            data.Init(_amplitude, _frequency, _duration, _blendOverLifetime, _target);

            AddShakeEvent(data);
        }

        private void Start()
        {
            if (ShakeTransformManager.Instance != null)
            {
                ShakeTransformManager.Instance.shakeTransforms.Add(this);
            }
        }

        private void LateUpdate()
        {
            Vector3 positionOffset = Vector3.zero;
            Vector3 rotationOffset = Vector3.zero;

            for (int i = shakeEvents.Count - 1; i != -1; i--)
            {
                ShakeEvent shakeEvent = shakeEvents[i];
                shakeEvent.Update();

                if (shakeEvent.target == ShakeData.Target.Position)
                {
                    positionOffset += shakeEvent.noise;
                }
                else
                {
                    rotationOffset += shakeEvent.noise;
                }

                if (!shakeEvent.IsAlive())
                {
                    shakeEvents.RemoveAt(i);
                }
            }

            transform.localPosition = positionOffset;
            transform.localEulerAngles = rotationOffset;
        }
    }
}