using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{UP,DOWN,LEFT,RIGHT}

public class Snake : SnakeAbstract
{
	public GameObject bodyPrefab;
	public game_scores snake_scores;
	private List<GameObject> snakeParts = new List<GameObject>();
	public Direction startingDirection = Direction.UP;
	private Direction desiredDirection = Direction.UP;

	private static readonly float moveVecLength = 0.9f;
	private static readonly float distanceBetweenSnakeParts = moveVecLength;

	public static float MoveTime { get; set; }
	private static float AccelerateRatio = 0.90f;
	public static float MoveVectorLength { get { return moveVecLength; } }


    public void Start_game(game_scores scores) {
		this.snake_scores = scores;

		GameObject head = GameObjectUtility.getChildren(gameObject)[0];
		head.GetComponent<SnakePart>().Direction = startingDirection;
		head.GetComponent<SnakeHead>().on_statr();
		snakeParts.Add(head);

		GameManager.Instance_Obj().snakeAteItself += kill;
		GameManager.Instance_Obj().snakeLeftBoard += kill;
		GameManager.Instance_Obj().boardFilled += speedUp;
		MoveTime = 0.3f;
	}

	private void Update() {
		snakeLoop_TemplateMethod();
	}

	public void go_left()
	{
		desiredDirection = Direction.LEFT;
		this.head().transform.rotation = Quaternion.Euler(0, 0, 90);
	}

	public void go_right()
	{
		desiredDirection = Direction.RIGHT;
		this.head().transform.rotation = Quaternion.Euler(0, 0, -90);
	}

	public void go_down()
	{
		desiredDirection = Direction.DOWN;
		this.head().transform.rotation = Quaternion.Euler(0, 0, 180);
	}

	public void go_up()
	{
		desiredDirection = Direction.UP;
		this.head().transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	// IMPLEMENTATION OF TEMPLATE PATTERN METHODS
	public sealed override void checkInput() {
		Direction headDirection = head().GetComponent<SnakeHead>().Direction;
		if(desiredDirection != headDirection && desiredDirection != VectorUtility.opposite(headDirection)) turn(desiredDirection);
	}

	public sealed override void move() {
		snakeParts.ForEach((GameObject snakePart) => {
			snakePart.GetComponent<SnakePart>().move();
		});
	}

	public sealed override void updateDirections() {
		snakeParts.ForEach((GameObject snakePart) => {
			if(snakePart == head()) return;
			snakePart.GetComponent<SnakeBody>().checkTurningPoints();
		});
	}

	private void turn(Direction direction) {
		head().GetComponent<SnakeHead>().Direction = direction;
		snakeParts.ForEach((GameObject snakePart) => {
			if(snakePart == head()) {return;}
			TurningPoint turningPoint = new TurningPoint(head().transform.position, head().GetComponent<SnakeHead>().Direction);
			snakePart.GetComponent<SnakeBody>().addTurningPoint(turningPoint);
		});
	}

	public void extend() {
		GameObject bodyPart = bodyPrefab;

		Direction direction = tail().GetComponent<SnakePart>().Direction;
		Direction oppositeDirection = VectorUtility.opposite(direction);

		Vector3 position = tail().transform.position;
		Vector3 spawnPosition = position + VectorUtility.createMoveVector(oppositeDirection, distanceBetweenSnakeParts);

		GameObject createdBodyPart = GameObjectUtility.instantiateAsChild(bodyPart, spawnPosition, Quaternion.identity, gameObject.transform);

		SnakeBody snakeBodyTail = tail().GetComponent<SnakeBody>();
		SnakeBody snakeBodyNew = createdBodyPart.GetComponent<SnakeBody>();

		snakeBodyNew.Direction = direction;

		if(tail() != head()) {
			foreach(var turningPoint in snakeBodyTail.TurningPoints) {
				snakeBodyNew.addTurningPoint(new TurningPoint(turningPoint.Position, turningPoint.Direction));
			}
		}
		snakeParts.Add(createdBodyPart);
	}

	public void speedUp() {
		MoveTime *= AccelerateRatio;
	}

	public void kill() {
		if(this!=null)this.gameObject.SetActive(false);
	}


	public GameObject head() {
		int size = snakeParts.Count;
		return size > 0 ? snakeParts[0] : null;
	}

	private GameObject tail() {
		int size = snakeParts.Count;
		return size > 0 ? snakeParts[size - 1] : null;
	}

}