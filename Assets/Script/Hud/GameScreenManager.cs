using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class GameScreenManager : MonoBehaviour
    {
        [HideInInspector] public Vector2 resolution;
        [HideInInspector] public Vector2 screenCenter;
        [HideInInspector] public Vector2 mouseDeflection;
        [HideInInspector] public Vector2 ellipseRadius;
        [HideInInspector] public Vector3 ellipseConstraints;

        private static GameScreenManager Instance;
        private void Awake()
        {
            Instance = this;
            ReconfigureScreen();
        }
        public static GameScreenManager GetInstance()
        {
            return Instance;
        }

        private void Update()
        {
            mouseDeflection = Input.mousePosition - new Vector3(screenCenter.x, screenCenter.y);

            // Если изменили разрешения экрана.
            if (resolution.x != Screen.width || resolution.y != Screen.height)
            {
                resolution.x = Screen.width;
                resolution.y = Screen.height;
                ReconfigureScreen();
            }
        }

        private void ReconfigureScreen()
        {
            // Разрешения экрана.
            resolution = new Vector2(Screen.width, Screen.height);
            // Координаты центра экрана.
            screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            // А и В для ограничивающего овала.
            ellipseRadius = new Vector2(screenCenter.x * 0.75f, screenCenter.y * 0.75f);
        }
    }
}
