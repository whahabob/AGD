using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour 
{
	[SerializeField] private GameObject _entity;

	private void Start()
	{
		GameObject temp = Instantiate (_entity);
		temp.transform.position = transform.position;
		temp.transform.parent = transform;
	}
}
