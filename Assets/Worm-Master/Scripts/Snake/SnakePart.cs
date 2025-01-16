using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnakePart : MonoBehaviour
{
	public Direction Direction { get; set; }

	public void move() {
		transform.position += VectorUtility.createMoveVector(Direction, Snake.MoveVectorLength);
	}
}