using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnakeAbstract : MonoBehaviour
{
	private float timer = 0.0f;

	// ----------------------------------------

	public abstract void checkInput();

	public abstract void move();

	public abstract void updateDirections();

	// ----------------------------------------

	public void snakeLoop_TemplateMethod() {
		timer += Time.deltaTime;
		if(timer > Snake.MoveTime) {
			checkInput();
			move();
			updateDirections();

			timer = 0.0f;
		}
	}
}