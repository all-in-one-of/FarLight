using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class CheckDoButton : MonoBehaviour {
        public Camera _camera;

        void Update()
        {
            if (_camera.enabled)
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
                    }
                }
            }
        }
    }
}
