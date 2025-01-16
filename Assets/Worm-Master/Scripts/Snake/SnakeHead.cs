using UnityEngine;

public class SnakeHead : SnakePart
{
	public Snake snake;
	public Animator ani;
	private void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == Tags.SnakeBodyTag) {
			if(ani.enabled==false)GameManager.Instance_Obj().onSnakeAteItself();
		}
		else if(collider.gameObject.tag == Tags.ItemTag) {
			game_item item_get = collider.gameObject.GetComponent<game_item>();
			this.snake.snake_scores.add_scores(item_get.scores);
			if (item_get.is_increase_size) this.snake.extend();
			if (item_get.is_speed_up) this.snake.speedUp();
			GameObject.Find("Game").GetComponent<GameManager>().item_manager.remove(collider.gameObject);
			GameObject.Find("Game").GetComponent<GameManager>().create_effect(collider.gameObject.transform.localPosition, item_get.type_effect);
			GameObject.Find("Game").GetComponent<GameManager>().play_sound(1);
			Destroy(collider.gameObject);
			this.eat_item();
		}
		else if (collider.gameObject.tag == Tags.Animal)
		{
			int type = collider.GetComponent<Cow>().type;
            if (type == 0)
            {
				GameObject.Find("Game").GetComponent<GameManager>().play_sound(1);
				GameObject.Find("Game").GetComponent<GameManager>().create_effect(collider.gameObject.transform.localPosition, 0);
				this.snake.extend();
				Destroy(collider.gameObject);
				this.eat_item();
            }
            else
            {
				GameManager.Instance_Obj().onSnakeLeftBoard();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if(collider.gameObject.tag == Tags.BoundsTag) {
			GameManager.Instance_Obj().onSnakeLeftBoard();
		}
	}

	public void on_statr()
    {
		this.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		ani.enabled = false;
	}

	public void eat_item()
    {
		this.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		ani.enabled = true;
	}

	public void stop_eat_item()
    {
		ani.enabled = false;
		this.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
	}
}