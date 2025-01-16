using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtility
{
	public static Vector3 createMoveVector(Direction direction, float delta) {
		float deltaX = 0;
		float deltaY = 0;

		if(direction == Direction.UP) {
			deltaX = 0;
			deltaY = delta;
		}
		else if(direction == Direction.DOWN) {
			deltaX = 0;
			deltaY = -delta;
		}
		else if(direction == Direction.LEFT) {
			deltaX = -delta;
			deltaY = 0;
		}
		else {
			deltaX = delta;
			deltaY = 0;
		}

		return new Vector3(deltaX, deltaY, 0.0f);
	}

	public static Direction opposite(Direction direction) {
		Direction opposite = Direction.UP;

		if(direction == Direction.UP) {
			opposite = Direction.DOWN;
		}
		else if(direction == Direction.DOWN) {
			opposite = Direction.UP;
		}
		else if(direction == Direction.LEFT) {
			opposite = Direction.RIGHT;
		}
		else {
			opposite = Direction.LEFT;
		}

		return opposite;
	}

	/// <summary>
	/// Each element of elements parameter is expected to have a CircleCollider2D component attatched to it
	/// </summary>
	public static bool includedInElements(Vector3 position, List<GameObject> elements) {
		bool result = false;

		foreach(var element in elements) {
			Vector3 elementPosition = element.transform.position;

			float permittedDistance = element.GetComponent<CircleCollider2D>().radius * 2 + 2;
			float distance = Vector3.Distance(elementPosition, position);

			if(distance < permittedDistance) {
				result = true;
				break;
			}
		}

		return result;
	}
}