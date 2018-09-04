using UnityEngine;

namespace nm
{
    public class FreeCamera : MonoBehaviour
    {
        [Range(0f, 100f)]
        public float moveSpeed = 5f;
        [Range(0f, 100f)]
        public float sprintSpeed = 15f;
        [Range(0f, 1f)]
        public float mouseTurnSpeed = 0.5f;
        [Range(0f, 1f)]
        public float smoothness = 0.36f;

        [HideInInspector]
        public bool m_inputCaptured;
        float m_yaw, m_pitch, speed, forward, right, up;

        Quaternion rotation;

        // Режим полёта.
        void CaptureInput()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            m_inputCaptured = true;

            m_yaw = transform.eulerAngles.y;
            m_pitch = transform.eulerAngles.x;
        }

        // Режим изучения.
        void ReleaseInput()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            m_inputCaptured = false;
        }

        void OnApplicationFocus(bool focus)
        {
            if (m_inputCaptured && !focus)
                ReleaseInput();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!m_inputCaptured)
                {
                    CaptureInput();
                }
                else if (m_inputCaptured)
                {
                    ReleaseInput();
                }
            }

            if (!m_inputCaptured)
                return;

            // Вращение камеры
            m_yaw = (m_yaw + mouseTurnSpeed * 10 * Input.GetAxis("Mouse X")) % 360f;
            m_pitch = (m_pitch - mouseTurnSpeed * 10 * Input.GetAxis("Mouse Y")) % 360f;
            rotation = Quaternion.AngleAxis(m_yaw, Vector3.up) * Quaternion.AngleAxis(m_pitch, Vector3.right);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothness);

            // Перемещение камеры
            speed = Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed);
            right = speed * ((Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f));
            forward = speed * ((Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f));

            up = speed * ((Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.C) ? 1f : 0f));
            transform.position += transform.forward * forward + transform.right * right + Vector3.up * up;
        }
    }
}
