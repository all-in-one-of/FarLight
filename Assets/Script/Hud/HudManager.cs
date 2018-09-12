using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace fl
{
    /// <summary>
    ///  Данный класс отвечает за все отображаемые объекты в виде Hud-а.
    ///  Движения спрайтов, их рендер и т.д. - всё тут!
    /// </summary>
    public class HudManager : MonoBehaviour
    {
        public Transform pointHud;
        public Text speedText;
        public Text exchangeSpeedText;
        public Text powerText;

        [HideInInspector] public Vector3 pointHudPosition;

        // Блокировка управления кораблём посредством мыши (режим "исследователя" в кабине)
        [HideInInspector] public bool controlLock = false;

        // ... 

        [HideInInspector] public string speedIndicatorText;
        [HideInInspector] public string exchangeSpeedIndicatorText;
        [HideInInspector] public string powerIndicatorText;


        private void Update()
        {
            pointHud.position = Vector3.Lerp(pointHud.position, pointHudPosition, Time.deltaTime * 10);
            speedText.text = speedIndicatorText;
            exchangeSpeedText.text = exchangeSpeedIndicatorText;
            powerText.text = powerIndicatorText;
        }

        private static HudManager Instance;
        private void Awake()
        {
            Instance = this;
        }
        public static HudManager GetInstance()
        {
            return Instance;
        }
    }
}
