using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	private List<GameObject> items = new List<GameObject>();

	public List<GameObject> itemPrefabs = new List<GameObject>();
	public GameObject fruitParticleSystemPrefab;
	public GameObject boostCollectParticleSystemPrefab;
	public GameObject boostDestroyParticleSystemPrefab;

	private float boardDown, boardTop, boardLeft, boardRight;
	private float boardOffset;
	private int maxItemsOnBoard;

	private GameObject snake;

	private static ItemManager instance;

	public static ItemManager Instance() {
		if(instance == null) {
			instance = GameObject.FindGameObjectWithTag(Tags.GameManagerTag).GetComponent<ItemManager>();
			if(instance == null) Debug.Log("error: There is no ItemManager script in the scene!");
		}
		return instance;
	}

	public void Start_game() {
		boardDown = -GameObject.FindGameObjectWithTag(Tags.BoundsTag).GetComponent<BoxCollider2D>().size.y / 2.0f;
		boardTop = -boardDown;
		boardLeft = -GameObject.FindGameObjectWithTag(Tags.BoundsTag).GetComponent<BoxCollider2D>().size.x / 2.0f;
		boardRight = -boardLeft;
		boardOffset = 2.0f;
		maxItemsOnBoard = 5;

		snake = GameObject.FindGameObjectWithTag(Tags.SnakeTag);

		GameManager.Instance_Obj().snakeAteItself += clearItems;
		GameManager.Instance_Obj().snakeLeftBoard += clearItems;
	}

	private void Update() {
		if(items.Count == 0 && snake != null) {
			for(var i = 0; i < maxItemsOnBoard; i++) {
				int randomIndex = Random.Range(0, itemPrefabs.Count);
				createItem(itemPrefabs[randomIndex]);
			}
			GameManager.Instance_Obj().onBoardFilled();
		}
	}

	private GameObject createItem(GameObject itemPrefab) {
		float minX = boardLeft + boardOffset,
			  maxX = boardRight - boardOffset,
			  minY = boardDown + boardOffset,
			  maxY = boardTop - boardOffset;

		Vector3 randomPosition = Vector3.zero;
		bool includedInSnakeParts = false;
		bool includedInItemParts = false;

		do {
			randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0.0f);

			includedInSnakeParts = VectorUtility.includedInElements(randomPosition, GameObjectUtility.getChildren(snake));
			includedInItemParts = VectorUtility.includedInElements(randomPosition, items);
		} while(includedInSnakeParts || includedInItemParts);

		GameObject itemInstance = Instantiate(itemPrefab, randomPosition, Quaternion.identity);
		items.Add(itemInstance);

		return itemInstance;
	}


	public GameObject createFruitParticleSystem(Fruit fruit) {
		return Instantiate(fruitParticleSystemPrefab, fruit.transform.position, Quaternion.identity);
	}

	public GameObject createBoostDestroyParticleSystem(Boost boost) {
		return Instantiate(boostDestroyParticleSystemPrefab, boost.transform.position, Quaternion.identity);
	}

	public GameObject createBoostCollectParticleSystem(Boost boost) {
		return Instantiate(boostCollectParticleSystemPrefab, boost.transform.position, Quaternion.identity);
	}

	public void collect(Item item) {
		item.collect();
		remove(item.gameObject);
	}

	public void remove(GameObject item) {
		items.Remove(item);
	}

	public void clearItems() {
		foreach(GameObject item in GameObject.FindGameObjectsWithTag(Tags.ItemTag)) Destroy(item);
		items.Clear();
	}
}