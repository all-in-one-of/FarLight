using UnityEngine;

namespace fl
{
    public class CockpitCamera : MonoBehaviour
    {
        [Range(0f, 1f)] public float mouseTurnSpeed = 0.5f;
        [Range(0f, 1f)] public float smoothness = 0.36f;

        public float maxFieldOfView = 60f;
        public float standartFieldOfView = 60f;
        public float minFieldOfView = 60f;

        [HideInInspector] public bool m_inputCaptured;

        private Camera m_Camera;
        private float m_ScrollPosition;
        private Quaternion m_Rotation;
        private float m_ScrollVelosity = 0;
        private ShipController m_Sc;
        private float m_Yaw, m_Pitch;
        private HudManager hudM;

        private void Awake()
        {
            m_Camera = GetComponent<Camera>();
            m_ScrollPosition = standartFieldOfView;
            m_Yaw = transform.localRotation.eulerAngles.y;
            m_Pitch = transform.localRotation.eulerAngles.x;
            m_Sc = GetComponent<CameraShip>().ship.GetComponent<ShipController>(); // ?! TO DO 
        }

        private void Start()
        {
            hudM = HudManager.GetInstance();
            //Cursor.visible = false;
        }

        // РЕЖИМ ОСМОТРА.
        void CaptureInput()
        {
            hudM.activeVisualAssistant = false;
            Cursor.lockState = CursorLockMode.Locked;
            m_inputCaptured = true;
            m_Sc.Blockage = true;
        }

        // РЕЖИМ УПРАВЛЕНИЯ.
        public void ReleaseInput()
        {
            hudM.activeVisualAssistant = true;

            m_Yaw = transform.localRotation.eulerAngles.y;
            m_Pitch = transform.localRotation.eulerAngles.x;

            //m_Camera.transform.localRotation = Quaternion.Euler(Vector3.zero);

            Cursor.lockState = CursorLockMode.None;
            m_inputCaptured = false;
            m_Sc.Blockage = false;
        }

        void OnApplicationFocus(bool focus)
        {
            if (m_inputCaptured && !focus)
                ReleaseInput();
        }

        void Update()
        {
            // TO DO. Настройка клавиши.
            if (Input.GetMouseButtonDown(1))
            {
                if (!m_inputCaptured)
                {
                    CaptureInput();
                }
                else
                {
                    ReleaseInput();
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                m_ScrollPosition += (Input.GetAxis("Mouse ScrollWheel") > 0f) ? 5 : -5;
            }

            if (m_ScrollPosition < minFieldOfView) m_ScrollPosition = minFieldOfView;
            if (m_ScrollPosition > maxFieldOfView) m_ScrollPosition = maxFieldOfView;

            m_Camera.fieldOfView = Mathf.SmoothDamp(m_Camera.fieldOfView, m_ScrollPosition, ref m_ScrollVelosity, 0.5f);

            if (!m_inputCaptured)
                return;

            m_Yaw = (m_Yaw + mouseTurnSpeed * 10 * Input.GetAxis("Mouse X")) % 360f;
            m_Pitch = (m_Pitch - mouseTurnSpeed * 10 * Input.GetAxis("Mouse Y")) % 360f;
            m_Rotation = Quaternion.AngleAxis(m_Yaw, Vector3.up) * Quaternion.AngleAxis(m_Pitch, Vector3.right);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, m_Rotation, smoothness);
        }
    }
}
