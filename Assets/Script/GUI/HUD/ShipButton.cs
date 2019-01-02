using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class ShipButton : MonoBehaviour
    {

        public Material ActiveMaterialButton;
        public Material NotActiveMaterialButton;

        public bool activeButton = false;

        private Renderer _renderer;

        private void Awake()
        {
            tag = "ShipButton";
            _renderer = GetComponent<Renderer>();
        }

        public void ButtonActivate()
        {
            activeButton = true;
            _renderer.material = ActiveMaterialButton;
        }

        public void ButtonDiactivation()
        {
            activeButton = false;
            _renderer.material = NotActiveMaterialButton;
        }
    }
}
