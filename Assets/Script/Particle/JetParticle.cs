using System;
using UnityEngine;

namespace fl
{
	[RequireComponent(typeof (ParticleSystem))]
	public class JetParticle : MonoBehaviour
	{
		public Color minColour;
		public Transform ship;

		private ShipController m_Ship;
		private ParticleSystem m_System;
		private float m_OriginalStartSize; // The original starting size of the particle system
		private float m_OriginalLifetime; // The original lifetime of the particle system
		private Color m_OriginalStartColor; // The original starting colout of the particle system

		private void Awake()
		{
			m_Ship = ship.GetComponent<ShipController>();
			m_System = GetComponent<ParticleSystem>();

			// set the original properties from the particle system
			m_OriginalLifetime = m_System.main.startLifetime.constant;
			m_OriginalStartSize = m_System.main.startSize.constant;
			m_OriginalStartColor = m_System.main.startColor.color;
		}

		private void Update()
		{
			ParticleSystem.MainModule mainModule = m_System.main;
			// update the particle system based on the jets throttle
			mainModule.startLifetime = Mathf.Lerp(0.0f, m_OriginalLifetime, m_Ship.Throttle);
			mainModule.startSize = Mathf.Lerp(m_OriginalStartSize * 0.3f, m_OriginalStartSize, m_Ship.Throttle);
			mainModule.startColor = Color.Lerp(minColour, m_OriginalStartColor, m_Ship.Throttle);
		}
	}
}
