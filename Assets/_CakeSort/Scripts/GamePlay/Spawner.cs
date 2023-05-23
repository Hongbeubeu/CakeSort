using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField] private Vector3[] _spawnerPositions;
	[SerializeField] private List<Plate> _plates = new(3);

	private void Start()
	{
		Spawn();
	}

	[Button(ButtonSizes.Gigantic)]
	public void Spawn()
	{
		DestroyPlates();
		for (int i = 0; i < _spawnerPositions.Length; i++)
		{
			var plate = GameManager.Instance.ObjectPooler.InstantiatePlate(transform, _spawnerPositions[i]);
			_plates.Add(plate);
			plate.RandomCake();
		}
	}

	private void DestroyPlates()
	{
		for (int i = 0; i < _plates.Count; i++)
		{
			_plates[i].Destroy();
		}

		_plates.Clear();
	}
}