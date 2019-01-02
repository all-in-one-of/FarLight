using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class BoundingOval : MonoBehaviour
    {
        public Vector2 normalPosition = new Vector2(0f, 0f);
        public Vector2 ellipsePoint = new Vector2(0f, 0f);

        private HudManager m_hudM;
        private GameScreenManager m_GameSM;

        private static BoundingOval Instance;
        private void Awake()
        {
            Instance = this;
        }
        public static BoundingOval GetInstance()
        {
            return Instance;
        }

        // HudManager инициализируется в Awake. Поключаться ко всем менеджерам нужно в Start.
        private void Start()
        {
            m_hudM = HudManager.GetInstance();
            m_GameSM = GameScreenManager.GetInstance();
        }

        private void Update()
        {
            ellipsePoint = PositionEllipse(m_GameSM.ellipseRadius.x, m_GameSM.ellipseRadius.y, m_GameSM.mouseDeflection.x, m_GameSM.mouseDeflection.y);

            if (ellipsePoint.magnitude > m_GameSM.mouseDeflection.magnitude)
            {
                normalPosition = Input.mousePosition;
            }
            else
            {
                normalPosition = ellipsePoint + m_GameSM.screenCenter;
            }

            // Переделать нахер. TO DO
            if (float.IsNaN(normalPosition.x))
            {
                normalPosition.x = 0;
            }
            if (float.IsNaN(normalPosition.y))
            {
                normalPosition.y = 0;
            }

            m_hudM.pointHudPosition = normalPosition;
            m_GameSM.ellipseConstraints = normalPosition - m_GameSM.screenCenter;
        }
         
        private Vector2 PositionEllipse(float r1, float r2, float x, float y) // ТУТ ВПОЛНЕ МОЖЕТ БЫТЬ НЕПРАВИЛЬНЫЙ ВЫВОД. TO DO
        {
            float a = x / r1;
            float b = y / r2;
            float k = Mathf.Sqrt(a * a + b * b);
            //print(k);
            //if (k == 0) k = 1;
            return new Vector2(x / k, y / k);
        }
    }
}