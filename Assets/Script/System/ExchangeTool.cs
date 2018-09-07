using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class ExchangeTool : MonoBehaviour
    {
        private GameObject m_object;
        private GameObject m_Prefab;
        private Rigidbody m_Rigidbody;
        public Transform ship;

        void Start()
        {
            m_Prefab = ResourceManager.GetPrefab("LaserBeam");
        }

        public void Attack()
        {
            m_object = Instantiate(m_Prefab);
            m_object.transform.position = transform.position;
            m_Rigidbody = m_object.GetComponent<Rigidbody>();
            Vector3 newFwd = ship.transform.rotation * Vector3.forward;     // Нужно правильно развернуть лазер!
            m_Rigidbody.AddForce(newFwd * 10f);
        }
    }
}
