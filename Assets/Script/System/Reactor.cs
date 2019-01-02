using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace fl
{
    public class Reactor : MonoBehaviour
    {
        public bool isReactorRunning = false;
        public string DamageSituation { get; set; }
        //public Text state;

        public Transform ship;
        private ShipController m_Ship;
        private AudioManager m_AudioM;

        private void Awake()
        {
            m_Ship = ship.GetComponent<ShipController>();
            m_AudioM = AudioManager.GetInstance();
            DamageSituation = "normal";
        }

        private void FixedUpdate()
        {
            //state.text = DamageSituation;
            if (DamageSituation == "critical" || DamageSituation == "slight")
            {
                FailuresReactor();
            }
        }

        // Попытка запустить реактор.
        public void TryLaunchingReactor()
        {
            // Если реактор запущен или разрушен.
            if (isReactorRunning)
            {
                LogReactor("Реактор уже запущен.");
                return;
            }
            if (DamageSituation == "ruined")
            {
                LogReactor("Реактор разрушен. Запуск невозможен.");
                return;
            }
            if (DamageSituation == "normal")
            {
                StartCoroutine(LaunchingReactor(6));
                return;
            }
            if (DamageSituation == "slight")
            {
                //float delay = UnityEngine.Random.Range(2, 3);
                int chance = UnityEngine.Random.Range(0, 100);
                // Шанс запуска - 60%
                if (chance <= 60)
                {
                    StartCoroutine(LaunchingReactor(6));
                    return;
                }
            }
            if (DamageSituation == "critical")
            {
                //float delay = UnityEngine.Random.Range(4, 5);
                int chance = UnityEngine.Random.Range(0, 100);
                // Шанс взрыва - 3%
                if (chance >= 98)
                {
                    ReactorExplosion();
                    return;
                }
                // Шанс запуска - 10%
                if (chance <= 30)
                {
                    StartCoroutine(LaunchingReactor(6));
                    return;
                }
            }
            StartCoroutine(AutoRestartDelay(3));
        }

        // Система задержки авторестарта.
        public IEnumerator AutoRestartDelay(float deley)
        {
            LogReactor("Сбой запуска реактора. Перезапуск через " + deley + " секунды.");
            m_AudioM.Play("SoftBeep3");
            yield return new WaitForSecondsRealtime(deley);
            TryLaunchingReactor();
        }

        // Запуск реактора.
        public IEnumerator LaunchingReactor(float deley)
        {
            LogReactor("Реактор запускается.");
            m_AudioM.Play("StartReactor");
            yield return new WaitForSecondsRealtime(deley);
            isReactorRunning = true;
            m_Ship.BlockageThrottle = false;
            LogReactor("Реактор успешно запущен.");
        }

        // Выключение реактора.
        public void ShutdownReactor()
        {
            isReactorRunning = false;
            m_Ship.BlockageThrottle = true;
            LogReactor("Реактор выключен.");
            m_AudioM.Play("Beep2_1");
            //m_AudioM.Play("ShutDown");
        }

        // Сбои реактора.
        private void FailuresReactor() // Наверно, работать не будет. TO DO. Доработать.
        {
            float chance = UnityEngine.Random.Range(0, 1000000);
            if (chance <= 0.00001)
            {
                ShutdownReactor();
                LogReactor("Аварийное выключение реактора. Перезапуск через 3 секунды.");
            }
        }

        // Попытка починить реактор.
        public void FixReactor()
        {
            if (DamageSituation == "critical")
            {
                LogReactor("Ремонт реактора.");
                isReactorRunning = false;
                // <-- Ожидание запуска 20-25 секунд. TO DO.
                DamageSituation = "slight";
                TryLaunchingReactor();
            }
        }

        // Взрыв реактора.
        public void ReactorExplosion()
        {
            DamageSituation = "ruined";
            isReactorRunning = false;
            LogReactor("Взрыв реактора. Переход на резервный генератор.");
        }

        // Лог реактора.
        private void LogReactor(string message)
        {
            Debug.Log("Реактор: " + message);
        }
    }
}
