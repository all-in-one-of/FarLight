using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ShuntingUpRight : MonoBehaviour
    {
        public Transform ship;

        private ShipController m_Ship;
        private ParticleSystem m_System;
        private float m_OriginalStartSize;
        private float m_OriginalLifetime;
        private float controlFactor;

        private void Awake()
        {
            m_Ship = ship.GetComponent<ShipController>();
            m_System = GetComponent<ParticleSystem>();

            m_OriginalLifetime = m_System.main.startLifetime.constant;
            m_OriginalStartSize = m_System.main.startSize.constant;
        }

        private void Update()
        {
            controlFactor = 0;
            if (m_Ship.PitchInput < 0)
            {
                controlFactor += -m_Ship.PitchInput;
            }
            if (m_Ship.YawInput > 0)
            {
                controlFactor += m_Ship.YawInput;
            }
            if (m_Ship.RollInput == -1 || m_Ship.UpInput == -1)
            {
                controlFactor += 1;
            }

            controlFactor = Mathf.Clamp(controlFactor, 0, 1);

            ParticleSystem.MainModule mainModule = m_System.main;
            mainModule.startLifetime = Mathf.Lerp(0.0f, m_OriginalLifetime, controlFactor);
            mainModule.startSize = Mathf.Lerp(m_OriginalStartSize * 0.3f, m_OriginalStartSize, controlFactor);
        }
    }
}
