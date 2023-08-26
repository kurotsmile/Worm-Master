using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint
{
	public Vector3 Position { get; set; }
	public Direction Direction { get; set; }

	public TurningPoint(Vector3 position, Direction direction) {
		this.Position = position;
		this.Direction = direction;
	}
}