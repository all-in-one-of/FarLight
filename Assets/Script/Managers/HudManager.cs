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
        public GameObject visualAssistant;
        public Transform pointHud;
        public Text speedText;
        public Text exchangeSpeedText;
        public Text powerText;

        public bool linePointHud = true;

        [HideInInspector] public Vector3 pointHudPosition;

        // ... 

        [HideInInspector] public string speedIndicatorText;
        [HideInInspector] public string exchangeSpeedIndicatorText;
        [HideInInspector] public string powerIndicatorText;

        //[HideInInspector] public bool activeVisualAssistant;

        private static HudManager Instance;
        private GameScreenManager gameScreenM;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            gameScreenM = GameScreenManager.GetInstance();
        }

        private void Update()
        {
            // ОПТИМИЗАЦИЯ. TO DO
            //visualAssistant.SetActive(activeVisualAssistant);

            pointHud.position = Vector3.Lerp(pointHud.position, pointHudPosition, Time.deltaTime * 100);
            speedText.text = speedIndicatorText;
            exchangeSpeedText.text = exchangeSpeedIndicatorText;
            powerText.text = powerIndicatorText;
        }

        void OnGUI()
        {
            if (linePointHud)
            {
                Drawing.DrawLine(gameScreenM.screenCenter, new Vector2(pointHud.position.x, pointHud.position.y), Color.white, 2f);
            }
        }

        public static HudManager GetInstance()
        {
            return Instance;
        }
    }
}
