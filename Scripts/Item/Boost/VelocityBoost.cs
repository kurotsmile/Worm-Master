using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityBoost : Boost
{
	// Use this for initialization
	private void Start() {
		LiveTime = 5.0f;
		Duration = 5.0f;
		Racio = 1.5f;
	}

	public override void begin() {
		//Snake.MoveTime /= Racio;
	}

	public override void finish() {
		//Snake.MoveTime *= Racio;
	}
}