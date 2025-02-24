using System;
using UnityEngine;

namespace fl
{
    public class ShipAudio : MonoBehaviour
    {

        [Serializable]
        public class AdvancedSetttings // A class for storing the advanced options.
        {
            public float engineMinDistance = 50f;                   // The min distance of the engine audio source.
            public float engineMaxDistance = 1000f;                 // The max distance of the engine audio source.
            public float engineDopplerLevel = 1f;                   // The doppler level of the engine audio source.
            [Range(0f, 1f)] public float engineMasterVolume = 0.5f; // An overall control of the engine sound volume.
        }

        [SerializeField] private AudioClip m_EngineSound;                     // Looped engine sound, whose pitch and volume are affected by the plane's throttle setting.
        [SerializeField] private float m_EngineMinThrottlePitch = 0.4f;       // Pitch of the engine sound when at minimum throttle.
        [SerializeField] private float m_EngineMaxThrottlePitch = 2f;         // Pitch of the engine sound when at maximum throttle.
        [SerializeField] private float m_EngineFwdSpeedMultiplier = 0.002f;   // Additional multiplier for an increase in pitch of the engine from the plane's speed.
        [SerializeField] private AdvancedSetttings m_AdvancedSetttings = new AdvancedSetttings();// container to make advanced settings appear as rollout in inspector

        private AudioSource m_EngineSoundSource;  // Reference to the AudioSource for the engine.
        private ShipController m_Plane;      // Reference to the aeroplane controller.
        private Rigidbody m_Rigidbody;


        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Plane = GetComponent<ShipController>();
            m_Rigidbody = GetComponent<Rigidbody>();

            // Add the audiosources and get the references.
            m_EngineSoundSource = gameObject.AddComponent<AudioSource>();
            m_EngineSoundSource.playOnAwake = false;

            // Assign clips to the audiosources.
            m_EngineSoundSource.clip = m_EngineSound;

            // Set the parameters of the audiosources.
            m_EngineSoundSource.minDistance = m_AdvancedSetttings.engineMinDistance;
            m_EngineSoundSource.maxDistance = m_AdvancedSetttings.engineMaxDistance;
            m_EngineSoundSource.loop = true;
            m_EngineSoundSource.dopplerLevel = m_AdvancedSetttings.engineDopplerLevel;

            // call update here to set the sounds pitch and volumes before they actually play
            Update();

            // Start the sounds playing.
            m_EngineSoundSource.Play();
        }


        private void Update()
        {
            var enginePowerProportion = Mathf.InverseLerp(0, m_Plane.MaxEnginePower, m_Plane.EnginePower);
            m_EngineSoundSource.pitch = Mathf.Lerp(m_EngineMinThrottlePitch, m_EngineMaxThrottlePitch, enginePowerProportion);
            m_EngineSoundSource.pitch += m_Plane.ForwardSpeed * m_EngineFwdSpeedMultiplier;
            m_EngineSoundSource.volume = Mathf.InverseLerp(0, m_Plane.MaxEnginePower * m_AdvancedSetttings.engineMasterVolume, m_Plane.EnginePower);
            float planeSpeed = m_Rigidbody.velocity.magnitude;
        }
    }
}
