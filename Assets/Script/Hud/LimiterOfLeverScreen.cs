using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class LimiterOfLeverScreen : MonoBehaviour
    {

        private GameScreen m_Gs;
        private Vector2 m_EllipsePoint;
        private Vector3 m_NormalPosition;
        private HudManager hudM;

        private void Awake()
        {
            m_Gs = GetComponent<GameScreen>();
        }

        // HudManager инициализируется в Awake. Поключаться ко всем менеджерам нужно в Start.
        private void Start()
        {
            hudM = HudManager.GetInstance();
        }

        private void Update()
        {
            m_EllipsePoint = PositionEllipse(m_Gs.ellipseConstraints.x, m_Gs.ellipseConstraints.y, m_Gs.mouseDeflection.x, m_Gs.mouseDeflection.y);

            if (m_EllipsePoint.magnitude > m_Gs.mouseDeflection.magnitude)
            {
                hudM.pointHudPosition = Input.mousePosition;
                //print(Input.mousePosition);
            }
            else
            {
                hudM.pointHudPosition = m_EllipsePoint + m_Gs.screenCenter;
                //print(m_EllipsePoint + m_Gs.screenCenter);
            }
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