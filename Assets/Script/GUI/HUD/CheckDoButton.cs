using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class CheckDoButton : MonoBehaviour {
        public Camera _camera;

        private AudioManager m_AudioM;
        private CockpitCamera cocpitC;

        private void Awake()
        {
            cocpitC = _camera.GetComponent<CockpitCamera>();
            m_AudioM = AudioManager.GetInstance();
        }

        void Update()
        {
            if (_camera.enabled && cocpitC.inputCaptured)
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.collider.transform;

                    if (objectHit.tag == "ShipButton" && Input.GetMouseButtonDown(0))
                    {
                        ShipButton sb = objectHit.GetComponent<ShipButton>();
                        if (sb.activeButton)
                        {
                            sb.ButtonDiactivation();
                        }
                        else
                        {
                            sb.ButtonActivate();
                        }
                        m_AudioM.Play("Button1");
                    }
                }
            }
        }
    }
}
