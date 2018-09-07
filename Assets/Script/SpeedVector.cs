using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class SpeedVector : MonoBehaviour
    {
        public bool active = false;
        public Transform shipTransform;
        ShipController sc;
        LineRenderer lr;

        private void Awake()
        {
            sc = GetComponent<ShipController>();
            lr = GetComponent<LineRenderer>();
        }

        void Update()
        {
            if (active)
            {
                //Debug.DrawLine(shipTransform.position, shipTransform.position + sc.localVelocity, Color.red);
                lr.SetPosition(0, shipTransform.position);
                lr.SetPosition(1, shipTransform.position + sc.directionVelocity);
            }
        }
    }
}
