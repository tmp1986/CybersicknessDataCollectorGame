namespace TurnTheGameOn.RacingGameTemplate
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShakeOnColissionEnter : MonoBehaviour
    {
        public float lifetime = 5.0f;

        [HideInInspector] public Rigidbody _rigidbody;
        public ShakeTransform shakeTransform;

        public List<ShakeData> birthShakeEvents;
        public List<ShakeData> deathShakeEvents;

        public GameObject spawnOnColissionEnter;
        public GameObject spawnOnDestroy;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        // Update is called once per frame
        private void OnCollisionEnter(Collision collision)
        {
            float impactMagnitude = collision.relativeVelocity.magnitude;

            for (int i = 0; i < birthShakeEvents.Count; i++)
            {
                ShakeData shakeData = birthShakeEvents[i];
                shakeTransform.AddShakeEvent(shakeData.amplitude * impactMagnitude, birthShakeEvents[i].frequency, birthShakeEvents[i].duration, birthShakeEvents[i].blendOverLifetime, birthShakeEvents[i].target );
            }

            GameObject go = Instantiate(spawnOnColissionEnter, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

            AudioSource audioSource = go.GetComponentInChildren<AudioSource>();
            audioSource.volume = Mathf.Lerp(0.0f, 1.0f, impactMagnitude / 15.0f);
            audioSource.Play();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < deathShakeEvents.Count; i++)
            {
                ShakeData shakeData = deathShakeEvents[i];
                shakeTransform.AddShakeEvent(deathShakeEvents[i]);
            }

            Instantiate(spawnOnDestroy, transform.position, Quaternion.identity);
        }
    }
}