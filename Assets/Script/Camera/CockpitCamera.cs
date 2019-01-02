using UnityEngine;

namespace fl
{
    public class CockpitCamera : MonoBehaviour
    {
        /*[Range(0f, 1f)] */public float mouseSensitivityFactor = 0.005f;
        [Range(0f, 1f)] public float smoothness = 0.36f;

        public float maxFieldOfView = 60f;
        public float standartFieldOfView = 60f;
        public float minFieldOfView = 60f;
        public Transform ship;

        [HideInInspector] public bool inputCaptured;

        private float mouseSensitivity;
        private Camera m_Camera;
        private float m_ScrollPosition;
        private Quaternion m_Rotation;
        private float m_ScrollVelosity = 0;
        private ShipController m_Sc;
        private float m_Yaw, m_Pitch;
        //private HudManager m_hudM;

        private void Awake()
        {
            m_Camera = GetComponent<Camera>();
            m_ScrollPosition = standartFieldOfView;
            mouseSensitivity = mouseSensitivityFactor * standartFieldOfView;
            m_Yaw = transform.localRotation.eulerAngles.y;
            m_Pitch = transform.localRotation.eulerAngles.x;
            m_Sc = ship.GetComponent<ShipController>(); // ?! TO DO 
        }

        //private void Start()
        //{
            //m_hudM = HudManager.GetInstance();
        //}

        // РЕЖИМ ОСМОТРА.
        void CaptureInput()
        {
            //m_hudM.activeVisualAssistant = false;
            Cursor.lockState = CursorLockMode.Locked;
            inputCaptured = true;
            m_Sc.Blockage = true;
        }

        // РЕЖИМ УПРАВЛЕНИЯ.
        public void ReleaseInput()
        {
            m_Yaw = transform.localRotation.eulerAngles.y;
            m_Pitch = transform.localRotation.eulerAngles.x;

            //m_hudM.activeVisualAssistant = true;
            Cursor.lockState = CursorLockMode.None;
            inputCaptured = false;
            m_Sc.Blockage = false;
        }

        void OnApplicationFocus(bool focus)
        {
            if (inputCaptured && !focus)
                ReleaseInput();
        }

        void Update()
        {
            // TO DO. Настройка клавиши.
            if (Input.GetMouseButtonDown(1))
            {
                if (!inputCaptured)
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
                mouseSensitivity = mouseSensitivityFactor * m_ScrollPosition;
            }

            if (m_ScrollPosition < minFieldOfView) m_ScrollPosition = minFieldOfView;
            if (m_ScrollPosition > maxFieldOfView) m_ScrollPosition = maxFieldOfView;

            m_Camera.fieldOfView = Mathf.SmoothDamp(m_Camera.fieldOfView, m_ScrollPosition, ref m_ScrollVelosity, 0.5f);

            if (!inputCaptured)
                return;

            m_Yaw = (m_Yaw + mouseSensitivity * 10 * Input.GetAxis("Mouse X")) % 360f;
            m_Pitch = (m_Pitch - mouseSensitivity * 10 * Input.GetAxis("Mouse Y")) % 360f;
            m_Rotation = Quaternion.AngleAxis(m_Yaw, Vector3.up) * Quaternion.AngleAxis(m_Pitch, Vector3.right);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, m_Rotation, smoothness);
        }
    }
}
