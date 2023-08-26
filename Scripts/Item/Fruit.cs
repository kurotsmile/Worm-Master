using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fruit : Item
{
	public float Points { get; set; }

	public override void collect() {
		Destroy(gameObject);

		Snake snake = GameObject.FindGameObjectWithTag(Tags.SnakeTag).GetComponent<Snake>();
		snake.extend();

		GameObject fruitParticleSystemInstance = ItemManager.Instance().createFruitParticleSystem(this);
		float duration = fruitParticleSystemInstance.GetComponent<ParticleSystem>().main.duration;
		Destroy(fruitParticleSystemInstance, duration+1f);
		Destroy(this.GetComponent<Collider2D>());
	}
}