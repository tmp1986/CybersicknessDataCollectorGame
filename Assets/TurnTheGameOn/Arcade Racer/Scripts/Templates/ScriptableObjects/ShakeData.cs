namespace TurnTheGameOn.RacingGameTemplate
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ShakeData", menuName = "TurnTheGameOn/ShakeData")]
    public class ShakeData : ScriptableObject
    {
        public enum Target
        {
            Position,
            Rotation
        }

        public Target target = Target.Position;

        public float amplitude = 1.0f;
        public float frequency = 1.0f;
        public float duration = 1.0f;

        public AnimationCurve blendOverLifetime = new AnimationCurve(
            new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
            new Keyframe(0.2f, 1.0f),
            new Keyframe(1.0f, 0.0f));

        public void Init(float _amplitude, float _frequency, float _duration, AnimationCurve _blendOverLifetime, Target _target)
        {
            this.target = _target;
            this.amplitude = _amplitude;
            this.frequency = _frequency;

            this.duration = _duration;
            this.blendOverLifetime = _blendOverLifetime;
        }
    }
}