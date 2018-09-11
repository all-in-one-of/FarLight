using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class BoundingOval : MonoBehaviour
    {

        private Vector2 m_EllipsePoint;
        private Vector2 m_NormalPosition;
        private HudManager hudM;
        private GameScreenManager m_GameSM;

        // HudManager инициализируется в Awake. Поключаться ко всем менеджерам нужно в Start.
        private void Start()
        {
            hudM = HudManager.GetInstance();
            m_GameSM = GameScreenManager.GetInstance();
        }

        private void Update()
        {
            m_EllipsePoint = PositionEllipse(m_GameSM.ellipseRadius.x, m_GameSM.ellipseRadius.y, m_GameSM.mouseDeflection.x, m_GameSM.mouseDeflection.y);

            if (m_EllipsePoint.magnitude > m_GameSM.mouseDeflection.magnitude)
            {
                m_NormalPosition = Input.mousePosition;
            }
            else
            {
                m_NormalPosition = m_EllipsePoint + m_GameSM.screenCenter;
            }

            hudM.pointHudPosition = m_NormalPosition;
            m_GameSM.ellipseConstraints = m_NormalPosition - m_GameSM.screenCenter;
        }
         
        private Vector2 PositionEllipse(float r1, float r2, float x, float y)
        {
            float a = x / r1;
            float b = y / r2;
            float k = Mathf.Sqrt(a * a + b * b);
            return new Vector2(x / k, y / k);
        }
    }
}