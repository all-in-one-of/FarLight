using System;
using UnityEngine;

namespace fl
{
    [RequireComponent(typeof (AeroplaneController))]
    public class AeroplaneUserControl2Axis : MonoBehaviour
    {
        private AeroplaneController m_Aeroplane;


        private void Awake()
        {
            m_Aeroplane = GetComponent<AeroplaneController>();
        }


        private void FixedUpdate()
        {
            float roll = CrossPlatformInputManager.GetAxis("Horizontal");
            float pitch = CrossPlatformInputManager.GetAxis("Vertical");

            float throttle = 1;
            m_Aeroplane.Move(roll, pitch, 0, throttle, false);
        }
    }
}
