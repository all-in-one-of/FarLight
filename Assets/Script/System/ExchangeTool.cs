using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class ExchangeTool : MonoBehaviour
    {
        public Transform ship;
        public float scatter = 0.6f;

        private GameObject m_object;
        private GameObject m_Prefab;
        private Rigidbody m_Rigidbody;

        void Start()
        {
            //m_Prefab = ResourceManager.GetPrefab("LaserBeam");
        }

        public void Attack()
        {
            //m_object = Instantiate(m_Prefab);
            m_object = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_object.AddComponent<Rigidbody>();
            m_object.transform.position = transform.position;
            m_object.transform.rotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);
            m_Rigidbody = m_object.GetComponent<Rigidbody>();
            Vector3 newFwd = ship.transform.rotation * Vector3.forward;     // Нужно правильно развернуть лазер!

            m_Rigidbody.mass = 0.001f;
            m_Rigidbody.angularDrag = 0f;
            m_Rigidbody.drag = 0f;
            m_Rigidbody.useGravity = false;

            m_Rigidbody.velocity = new Vector3(
                newFwd.x * 200f + Random.Range(-scatter, scatter), 
                newFwd.y * 200f + Random.Range(-scatter, scatter), 
                newFwd.z * 200f + Random.Range(-scatter, scatter)
                );
        }
    }
}
