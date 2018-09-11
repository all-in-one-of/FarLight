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

        ResourceManager resM;

        void Start()
        {
            resM = ResourceManager.GetInstance();
            m_Prefab = resM.GetPrefab("LaserBeam");
        }

        public void Attack()
        {
            m_object = Instantiate(m_Prefab);
            m_object.transform.position = transform.position;
            m_object.transform.rotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);
            m_Rigidbody = m_object.GetComponent<Rigidbody>();
            Vector3 newFwd = ship.transform.rotation * Vector3.forward;

            m_Rigidbody.velocity = new Vector3(
                newFwd.x * 200f + Random.Range(-scatter, scatter), 
                newFwd.y * 200f + Random.Range(-scatter, scatter), 
                newFwd.z * 200f + Random.Range(-scatter, scatter)
                );
        }
    }
}
