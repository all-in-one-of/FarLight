using System;
using UnityEngine;

namespace fl
{
	public class JetParticle : MonoBehaviour
	{
		public Transform ship;

		private ShipController m_Ship;
		private Color m_OriginalStartColor;
        private Material m_Material;
        private Color minColour = new Color(0f, 0f, 0f, 0f);

        private void Awake()
		{
			m_Ship = ship.GetComponent<ShipController>();
            m_Material = GetComponent<Renderer>().material;
            m_OriginalStartColor = GetComponent<Renderer>().material.GetColor("_TintColor");
		}

		private void Update()
		{
            m_Material.SetColor("_TintColor", Color.Lerp(minColour, m_OriginalStartColor, m_Ship.Throttle));
		}
	}
}
