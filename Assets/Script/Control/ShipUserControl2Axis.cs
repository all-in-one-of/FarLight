using UnityEngine;

namespace fl
{
    public class ShipUserControl2Axis : MonoBehaviour
    {
        private float m_Throttle;
        private float m_Yaw;
        private float m_Roll;
        private float m_Pitch;

        private ShipController m_ship;

        //public Transform exchangeToolTransform;
        //private ExchangeTool et;

        private void Awake()
        {
            m_ship = GetComponent<ShipController>();
            //et = exchangeToolTransform.GetComponent<ExchangeTool>();
        }

        private void FixedUpdate()
        {
            m_Roll = CrossPlatformInputManager.GetAxis("Horizontal");
            m_Pitch = CrossPlatformInputManager.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                DoubleTap("Warp");
            }

            //if (Input.GetMouseButton(0) && !m_ship.Blockage)
            //{
            //    et.Attack();
            //}

            m_Throttle = (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f) - (Input.GetKey(KeyCode.Z) ? 1f : 0f);

            //m_ship.Move(m_Roll, m_Pitch, 0, m_Throttle);
        }

        public void Warp()
        {
            StartCoroutine(m_ship.Warp());
        }

        public float doubleTapDelay = 0.4f;
        private float tapCount = 0;

        public void DoubleTap(string methodLuck)
        {
            tapCount++;
            if (tapCount == 2)
            {
                CancelInvoke("FailDoubleTap");
                Invoke(methodLuck, 0f);
                tapCount = 0;
                return;
            }

            Invoke("FailDoubleTap", doubleTapDelay);
        }

        public void FailDoubleTap()
        {
            tapCount = 0;
        }
    }
}
