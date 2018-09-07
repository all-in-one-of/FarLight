using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class CameraShip : MonoBehaviour
    {
        public Transform ship;
        public float smoothrate = 0.02f;

        private Vector3 velocity;
        private Vector3 defaultDistance;
        [HideInInspector] public ShipController sc;
        float velocityTo;
        float shakingPower;

        void Awake()
        {
            sc = ship.GetComponent<ShipController>();
            defaultDistance = new Vector3(0f, 0f, 0f);
            velocity = new Vector3(0.05f, 0.05f, 0.05f);
        }

        void FixedUpdate()
        {
            SmoothFollow();
            if (sc.EnginePower == sc.MaxWarpEnginePower)
            {
                transform.Rotate((Random.Range(-0.1f, 0.1f)), (Random.Range(-0.1f, 0.1f)), (Random.Range(-0.1f, 0.1f)));
            }
        }

        public IEnumerator StartShaking()
        {
            // 3 секунды.
            shakingPower = 0f;
            velocityTo = 0.1F;

            while (shakingPower < 0.2f)
            {
                shakingPower = Mathf.SmoothDamp(shakingPower, 0.3f, ref velocityTo, 2);
                transform.Rotate((Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)));
                yield return null;
            }
        }
        public IEnumerator FinishShaking()
        {
            // 6 секунд.
            shakingPower = 0.2f;
            velocityTo = 0.1F;

            while (shakingPower > 0)
            {
                shakingPower = Mathf.SmoothDamp(shakingPower, -1f, ref velocityTo, 4);
                transform.Rotate((Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)));
                yield return null;
            }
        }

        public void SmoothFollow()
        {
            Vector3 curPos = Vector3.SmoothDamp(transform.position, ship.position + (transform.rotation * defaultDistance), ref velocity, smoothrate);
            transform.position = curPos;

            transform.rotation = ship.rotation;
        }
    }
}
