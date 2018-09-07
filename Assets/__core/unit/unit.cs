using UnityEngine;
using System.Collections;

public class unit : MonoBehaviour {

	[HideInInspector] public Rigidbody rigidBody;


	virtual public void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();

		unit_identify[] unitIdentify = GetComponentsInChildren<unit_identify>();

		foreach (unit_identify identify in unitIdentify)
		{
			identify._unit = this;
		}
	}



}
