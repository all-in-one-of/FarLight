using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fl
{
    public class ExchangeTool : MonoBehaviour
    {
        public float fireRate = 4f;
        public float fireBulletSpeed = 30f;
        public float bulletLifeTime = 4f;
        public float crossExchangeToolFactor = 0.3f;
        public float shiftToCamera = -1.3f;
        public GameObject mazzle;
        public GameObject spray;
        public GameObject heatRemoval;

        public enum GunSide { Right = 0, Left = 1 };
        public GunSide gunSide = GunSide.Right;

        public Transform listShells;

        private ParticleSystem mazzleParticle;
        private ParticleSystem sprayParticle;
        private ParticleSystem heatRemovalParticle;
        private GameObject m_object;
        private GameObject m_Prefab;
        private Rigidbody m_Rigidbody;
        private ResourceManager resM;
        private float timeToFire = 0;
        private float crossFactor = 0;

        private void Awake()
        {
            mazzleParticle = mazzle.GetComponent<ParticleSystem>();
            sprayParticle = spray.GetComponent<ParticleSystem>();
            heatRemovalParticle = heatRemoval.GetComponent<ParticleSystem>();
            crossFactor = (gunSide == GunSide.Right) ? -crossExchangeToolFactor : crossExchangeToolFactor;
        }

        private void Start()
        {
            resM = ResourceManager.GetInstance();
            m_Prefab = resM.GetPrefab("PlasmaProjectile");
        }

        public void Attack()
        {
            if (Time.time >= timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                m_object = Instantiate(m_Prefab, listShells);
                m_object.transform.position = transform.position;
                m_Rigidbody = m_object.GetComponent<Rigidbody>();
                Vector3 bulletVector = Quaternion.Euler(shiftToCamera, crossFactor, 0f) * Camera.main.transform.forward * fireBulletSpeed;
                m_Rigidbody.AddForce(bulletVector);
                mazzleParticle.Play();
                sprayParticle.Play();
                Destroy(m_object, bulletLifeTime);
            }
        }
        public void EndAttack()
        {
            heatRemovalParticle.Play();
        }
    }
}
