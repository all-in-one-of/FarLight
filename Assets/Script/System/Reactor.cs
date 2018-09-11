using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reactor : MonoBehaviour {

    public bool isReactorRunning = false;
    public string DamageSituation { get; set; }

    private bool RestartLaunching = false;

    public Text state;

    private void Awake()
    {
        DamageSituation = "normal";
        //TryLaunchingReactor();
    }

    private void FixedUpdate()
    {
        state.text = DamageSituation;
        if (DamageSituation == "critical" || DamageSituation == "slight")
        {
            FailuresReactor();
        }
        if (DamageSituation == "ruined")
        {
            isReactorRunning = false;
        }
    }

    // Попытка запустить реактор.
    public void TryLaunchingReactor()
    {
        RestartLaunching = false;
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
        yield return new WaitForSecondsRealtime(deley);
        RestartLaunching = true;
        TryLaunchingReactor();
    }

    // Запуск реактора.
    public IEnumerator LaunchingReactor(float deley)
    {
        LogReactor("Реактор запускается.");
        yield return new WaitForSecondsRealtime(deley);
        isReactorRunning = true;
        LogReactor("Реактор успешно запущен.");
    }
    
    // Выключение реактора.
    public void ShutdownReactor()
    {
        isReactorRunning = false;
        LogReactor("Реактор выключен.");
    }

    // Сбои реактора.
    private void FailuresReactor()
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
        LogReactor("Взрыв реактора. Переход на резервный генератор.");
    }

    // Лог реактора.
    private void LogReactor(string message)
    {
        Debug.Log("Реактор: " + message);
    }
}
