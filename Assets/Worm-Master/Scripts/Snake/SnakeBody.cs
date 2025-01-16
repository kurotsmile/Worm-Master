using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : SnakePart
{
	private Queue<TurningPoint> turningPoints = new Queue<TurningPoint>();
	public Queue<TurningPoint> TurningPoints { get { return turningPoints; } }

	// --------------------------------------------------

	public void addTurningPoint(TurningPoint turningPoint) {
		turningPoints.Enqueue(turningPoint);
	}

	/// <summary>
	/// Method checks whether the SnakeBody object is close to its first turning point.</br>
	///	If so, it will update objects' direction to the one stored inside of TurningPoint class
	/// </summary>
	public void checkTurningPoints() {
		if(turningPoints.Count == 0) {
			return;
		}

		TurningPoint firstTurningPoint = turningPoints.Peek();
		float distance = Vector3.Distance(gameObject.transform.position, firstTurningPoint.Position);

		if(distance <= 0.01f) {
			TurningPoint turningPoint = turningPoints.Dequeue();
			Direction = turningPoint.Direction;
		}
	}
}