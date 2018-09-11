using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class CameraShip : MonoBehaviour
    {
        public Transform ship;
        public float smoothrate = 0.02f;

        public enum TypeCamera { Cockpit = 0, General = 1 };
        public TypeCamera cameraMode = TypeCamera.General;

        public Transform _camera;

        private Vector3 velocity;
        private Vector3 defaultDistance;
        [HideInInspector] public ShipController sc;
        private float velocityTo;
        private float shakingPower;

        void Awake()
        {
            sc = ship.GetComponent<ShipController>();
            defaultDistance = new Vector3(0f, 0f, 0f);
            velocity = new Vector3(0.05f, 0.05f, 0.05f);
        }

        void FixedUpdate()
        {
            if (cameraMode == TypeCamera.General)
            {
                SmoothFollow();
            }
            if (sc.EnginePower == sc.MaxWarpEnginePower)
            {
                _camera.transform.Rotate((Random.Range(-0.1f, 0.1f)), (Random.Range(-0.1f, 0.1f)), (Random.Range(-0.1f, 0.1f)));
            }
        }

        void Update()
        {
            if (cameraMode == TypeCamera.Cockpit)
            {
                Follow();
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
                _camera.transform.Rotate((Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)));
                yield return null;
            }
        }
        public IEnumerator FinishShaking()
        {
            //// 6 секунд.
            shakingPower = 0.2f;
            velocityTo = 0.1F;

            while (shakingPower > 0)
            {
                shakingPower = Mathf.SmoothDamp(shakingPower, -1f, ref velocityTo, 4);
                _camera.transform.Rotate((Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)), (Random.Range(-shakingPower, shakingPower)));
                yield return null;
            }
        }

        // Камера "не успевает" за объектом при больших скоростях потому что fixedupdate. TO DO.
        public void SmoothFollow()
        {
            Vector3 curPos = Vector3.SmoothDamp(_camera.transform.position, ship.position + (_camera.transform.rotation * defaultDistance), ref velocity, smoothrate);
            _camera.transform.position = curPos;

            _camera.transform.rotation = ship.rotation;
        }

        // Обычное точное следование, которое выполняется в Update.
        public void Follow()
        {
            _camera.transform.position = ship.position;
            _camera.transform.rotation = ship.rotation;
        }
    }
}
