using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace fl
{
    public class ShipCamera : MonoBehaviour
    {
        public Transform cameraAnimation;
        public Transform ship;
        public float smoothFactorMain = 0.1f;
        public float smoothFactorAngle = 0.1f;

        private float smoothTime = 0f;
        private float smoothTimeAngle = 0f;
        [HideInInspector] public ShipController shipController;
        private GameScreenManager m_gameScreenM;
        private HudManager m_hudM;

        public Vector3 offset;
        public float zoom = 0.25f;
        public float zoomMax = 10f;
        public float zoomMin = 3f;

        public float xSpeed = 20.0f;
        public float ySpeed = 20.0f;
        public float yMinLimit = -90f;
        public float yMaxLimit = 90f;
        public float smoothTimeM = 2f;
        float rotationYAxis = 0.0f;
        float rotationXAxis = 0.0f;
        float velocityX = 0.0f;
        float velocityY = 0.0f;

        private void Awake()
        {
            shipController = ship.GetComponent<ShipController>();
        }

        private void Start()
        {
            m_gameScreenM = GameScreenManager.GetInstance();
            m_hudM = HudManager.GetInstance();

            offset = new Vector3(offset.x, offset.y, -Mathf.Abs(zoomMax) / 2);
            transform.position = ship.position;
        }

        //public IEnumerator StartShaking()
        //{
        //shakingPower = 0f;
        //velocityTo = 0.01f;

        //while (shakingPower < 0.1f)
        //{
        //    shakingPower = Mathf.SmoothDamp(shakingPower, 0.2f, ref velocityTo, Time.deltaTime * 1000f);
        //    CameraShake.Shake(Time.deltaTime * 1000f, shakingPower);
        //    yield return null;
        //}
        //yield return null;
        //}

        //public IEnumerator FinishShaking()
        //{
        //shakingPower = 0.1f;
        //velocityTo = 0.01f;

        //while (shakingPower > 0)
        //{
        //    shakingPower = Mathf.SmoothDamp(shakingPower, -1f, ref velocityTo, Time.deltaTime * 1000f);
        //    CameraShake.Shake(Time.deltaTime * 1000f, shakingPower);
        //    yield return null;
        //}
        //yield return null;
        //}

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
                angle += 360f;
            if (angle > 360f)
                angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        private void Update()
        {
            // TO DO. Настройка клавиши.
            if (Input.GetKeyDown(KeyCode.V))
            {
                shipController.BlockageManeuver = true;
                shipController.BlockageAttack = true;
                m_hudM.linePointHud = false;
                m_hudM.pointHud.gameObject.SetActive(false);
            }
            // TO DO. Настройка клавиши.
            if (Input.GetKeyUp(KeyCode.V))
            {
                cameraAnimation.localRotation = Quaternion.Euler(Vector3.zero);
                cameraAnimation.localPosition = Vector3.zero;
                shipController.BlockageManeuver = false;
                shipController.BlockageAttack = false;
                m_hudM.linePointHud = true;
                m_hudM.pointHud.gameObject.SetActive(true);
            }
            // TO DO. Настройка клавиши.
            if (Input.GetKey(KeyCode.V))
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0) offset.z += zoom;
                else if (Input.GetAxis("Mouse ScrollWheel") < 0) offset.z -= zoom;
                offset.z = Mathf.Clamp(offset.z, -Mathf.Abs(zoomMax), -Mathf.Abs(zoomMin));
            }
        }

        private void FixedUpdate()
        {
            // TO DO. Настройка клавиши.
            if (Input.GetKey(KeyCode.V))
            {
                if (Input.GetMouseButton(0))
                {
                    velocityX += xSpeed * Input.GetAxis("Mouse X") * offset.z * 0.001f;
                    velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
                }
                rotationYAxis -= velocityX;
                rotationXAxis -= velocityY;
                rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
                Vector3 position = rotation * offset + shipController.centerOfMass;

                cameraAnimation.localRotation = rotation;
                cameraAnimation.localPosition = position;
                velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTimeM);
                velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTimeM);
            }
            else
            {
                Quaternion desiredRot = Quaternion.Euler(-m_gameScreenM.ellipseConstraints.y * 0.03f, m_gameScreenM.ellipseConstraints.x * 0.03f, shipController.RollInput * 10f);
                smoothTimeAngle = Time.deltaTime * smoothFactorAngle;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRot, smoothTimeAngle);

                float zPosCam = Mathf.Clamp(shipController.ForwardSpeed, 0, 50);
                smoothTime = Time.deltaTime * smoothFactorMain;
                Vector3 desiredPos = new Vector3(shipController.RightInput + shipController.YawInput, shipController.UpInput + shipController.PitchInput, -zPosCam * 0.05f);
                transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPos, smoothTime);
            }
        }
    }
}
