﻿using UnityEngine;

namespace fl
{
    public class ShipUserControl4Axis : MonoBehaviour
    {
        private float m_Throttle = 0;   // Вперёд и назад.
        private float m_Yaw = 0;        // Рыскание.
        private float m_Pitch = 0;      // Тонгаж.
        private float m_Roll = 0;       // Вращение.
        private float m_Right = 0;      // Смещение влево и вправо.
        private float m_Up = 0;         // Вверх и вниз.

        private bool m_AttackRight = false;
        private bool m_AttackLeft = false;

        private ShipController m_Ship;
        private GameScreenManager m_GameSM;

        private void Awake()
        {
            m_Ship = GetComponent<ShipController>();
        }

        private void Start()
        {
            m_GameSM = GameScreenManager.GetInstance();
        }

        // Получать все клавиши строго в Update.
        // Менять физику строго в FixedUpdate.
        private void Update()
        {
            if (!m_Ship.Blockage)
            {
                if (!m_Ship.BlockageAttack)
                {
                    // TO DO. Настройка клавиши.
                    m_AttackRight = Input.GetMouseButton(1);
                    m_AttackLeft = Input.GetMouseButton(0);
                    if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetKeyDown(KeyCode.R)) // KEY MOUSE AND R
                    {
                        m_Ship.EndAttack();
                    }
                }
                else
                {
                    m_AttackRight = false;
                    m_AttackLeft = false;
                }

                if (!m_Ship.BlockageManeuver)
                {
                    // TO DO. Настройка клавиши.
                    // Вращение.
                    m_Roll = (Input.GetKey(KeyCode.Q) ? 1f : 0f) - (Input.GetKey(KeyCode.E) ? 1f : 0f);
                    // TO DO. Настройка клавиши.
                    // Влево и вправо. 
                    m_Right = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f);
                    // TO DO. Настройка клавиши.
                    // Вверх и вниз.
                    m_Up = (Input.GetKey(KeyCode.Space) ? 1f : 0f) - (Input.GetKey(KeyCode.LeftAlt) ? 1f : 0f);

                    // Рыскание.
                    if (Mathf.Abs(m_GameSM.mouseDeflection.x) < m_GameSM.screenCenter.x * 0.02f)
                    {
                        m_Yaw = 0;
                    }
                    else
                    {
                        m_Yaw = (Mathf.Abs(m_GameSM.mouseDeflection.x - m_GameSM.ellipseConstraints.x) > 0) ? m_GameSM.ellipseConstraints.x : m_GameSM.mouseDeflection.x;
                        m_Yaw = m_Yaw / m_GameSM.screenCenter.x;
                    }

                    // Тангаж.
                    if (Mathf.Abs(m_GameSM.mouseDeflection.y) < m_GameSM.screenCenter.y * 0.02f)
                    {
                        m_Pitch = 0;
                    }
                    else
                    {
                        m_Pitch = (Mathf.Abs(m_GameSM.mouseDeflection.y - m_GameSM.ellipseConstraints.y) > 0) ? m_GameSM.ellipseConstraints.y : m_GameSM.mouseDeflection.y;
                        m_Pitch = m_Pitch / m_GameSM.screenCenter.y;
                    }
                }

                if (!m_Ship.BlockageThrottle)
                {
                    if (!m_Ship.BlockageWarp)
                    {
                        // TO DO. Настройка клавиши.
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            DoubleTap("Warp");
                        }
                    }
                    // TO DO. Настройка клавиши.
                    // Вперёд и назад.
                    m_Throttle = (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f) - (Input.GetKey(KeyCode.Z) ? 1f : 0f);
                }
            }

            m_Ship.Move(m_Roll, m_Pitch, m_Yaw, m_Throttle, m_Right, m_Up);
            m_Ship.Attack(m_AttackRight, m_AttackLeft);
        }

        private void Warp()
        {
            StartCoroutine(m_Ship.Warp());
        }

        public float doubleTapDelay = 0.4f;

        private float tapCount = 0;
        private void DoubleTap(string methodLuck)
        {
            tapCount++;
            if (tapCount == 2)
            {
                CancelInvoke("FailDoubleTap");
                tapCount = 0;
                Invoke(methodLuck, 0f);
                return;
            }

            Invoke("FailDoubleTap", doubleTapDelay);
        }
        private void FailDoubleTap()
        {
            tapCount = 0;
        }
    }
}

