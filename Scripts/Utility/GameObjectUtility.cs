using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtility
{
	public static GameObject instantiateAsChild(GameObject prefab, Vector3 vector, Quaternion rotation, Transform parent) {
		Vector3 relativePosition = vector - parent.position;

		//GameObject snakePart = GameObject.Instantiate(prefab, parent);
		GameObject snakePart = GameObject.Instantiate(prefab, Vector3.zero, rotation, parent);
		snakePart.transform.localPosition = relativePosition;

		return snakePart;
	}

	public static List<GameObject> getChildren(GameObject gameobject) {
		List<GameObject> result = new List<GameObject>();

		foreach(Transform transform in gameobject.GetComponentInChildren<Transform>()) {
			result.Add(transform.gameObject);
		}

		return result;
	}
}