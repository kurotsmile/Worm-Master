using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boost : Item
{
	private bool isActive;
	private float liveTimer;
	private float effectTimer;

	public float LiveTime { get; set; }
	public float Duration { get; set; }
	public float Racio { get; set; }

	private void Update() {
		if(!isActive) {
			liveTimer += Time.deltaTime;
			if(liveTimer > LiveTime) {
				destroy();
			}
		}
		else {
			effectTimer += Time.deltaTime;
			if(effectTimer > Duration) {
				finish();
				isActive = false;
				Destroy(gameObject);
			}
		}
	}

	public override void collect() {
		isActive = true;
		begin();

		// Make it invisible & not collectable
		Destroy(gameObject.GetComponent<SpriteRenderer>());
		Destroy(gameObject.GetComponent<Collider2D>());

		// Instantiate a particle system + set up its destruction after it's done
		GameObject boostCollectParticleSystemInstance = ItemManager.Instance().createBoostCollectParticleSystem(this);
		float duration = boostCollectParticleSystemInstance.GetComponent<ParticleSystem>().main.duration;
		Destroy(boostCollectParticleSystemInstance, duration+1f);
	}

	public void destroy() {
		Destroy(gameObject);
		ItemManager.Instance().remove(this.gameObject);

		// Play the sound
		GameObject.Find("Game").GetComponent<GameManager>().play_sound(2);

		// Instantiate a particle system + set up its destruction after it's done
		GameObject boostDestroyParticleSystemInstance = ItemManager.Instance().createBoostDestroyParticleSystem(this);
		float duration = boostDestroyParticleSystemInstance.GetComponent<ParticleSystem>().main.duration;
		Destroy(boostDestroyParticleSystemInstance, duration+1f);
	}

	public abstract void begin();

	public abstract void finish();
}