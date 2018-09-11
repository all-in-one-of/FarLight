using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class GameScreen : MonoBehaviour
    {
        public Vector2 resolution;
        public Vector2 screenCenter;
        public Vector2 mouseDeflection;
        public Vector2 ellipseConstraints;

        private void Awake()
        {
            ReconfigureScreen();
        }

        private void Update()
        {
            mouseDeflection = Input.mousePosition - new Vector3(screenCenter.x, screenCenter.y);

            if (resolution.x != Screen.width || resolution.y != Screen.height)
            {
                resolution.x = Screen.width;
                resolution.y = Screen.height;
                ReconfigureScreen();
            }
        }

        private void ReconfigureScreen()
        {
            resolution = new Vector2(Screen.width, Screen.height);
            screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            // Ограничители тангажа и рыскания.
            ellipseConstraints = new Vector2(screenCenter.x * 0.75f, screenCenter.y * 0.75f);
        }
    }
}
