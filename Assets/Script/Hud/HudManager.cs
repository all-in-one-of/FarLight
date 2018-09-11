using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    /// <summary>
    ///  Данный класс отвечает за все отображаемые объекты в виде Hud-а.
    ///  Движения спрайтов, их рендер и т.д. - всё тут!
    /// </summary>
    public class HudManager : MonoBehaviour
    {
        public Transform pointHud;
        [HideInInspector] public Vector3 pointHudPosition;

        // Блокировка управления кораблём посредством мыши (режим "исследователя" в кабине)
        [HideInInspector] public bool controlLock = false;

        // ... 


        private void Update()
        {
            pointHud.position = Vector3.Lerp(pointHud.position, pointHudPosition, Time.deltaTime * 10);
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
