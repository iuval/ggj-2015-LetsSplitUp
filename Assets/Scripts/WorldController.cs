using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public Player player1;
	public Player player2;
	
	public static float floorAccel = 0;
	public static int playersCountTouchingRightWall = 0;
	private bool moveFloor = false;
	
	public static float behindWallSpeed = 0.08f;
	public static float playerSpeed = 0.1f;
	public static float playerJumpSpeed = 10f;
	public static int playersMaxDistance = 5;
	public static int distanceToDestroyObstacle = 25;
	public int obstacleCount = 4;
		
	public GameObject obstacle;
	public GameObject[] behindWalls;
	
	public ArrayList obstacle1Objects;
	public ArrayList obstacle2Objects;
	
	public ArrayList obstacleObjectsToDestroy;
	
	public float channelingTimeLimit = 1.0f;
	public float channelingTime = 0.0f;

	void Start () {
		player1.canHitHard = true;
		player2.canJumpHigh = true;

		obstacle1Objects = new ArrayList();
		obstacle2Objects = new ArrayList();
		obstacleObjectsToDestroy = new ArrayList();
		for (int i = 0; i < obstacleCount; i ++) {
			AddObstacleToLevel (1);
			AddObstacleToLevel (2);
		}
	}

	private void AddObstacleToLevel(int level) {
		float x = GetNewXForFloor(level);

		GameObject newFloor = (GameObject)GameObject.Instantiate (obstacle);
		newFloor.transform.parent = gameObject.transform;
		if (level == 1) {
			
			newFloor.transform.position = new Vector2 (x, -8.674157f);
			obstacle1Objects.Add (newFloor);
		} else {
			
			newFloor.transform.position = new Vector2 (x, 4.2f);
			obstacle2Objects.Add (newFloor);
		}
	}

	private float GetNewXForFloor(int level) {
		GameObject lastFloor;
		if (level == 1 && obstacle1Objects.Count > 0) {
			lastFloor = ((GameObject)obstacle1Objects [obstacle1Objects.Count - 1]);
		} else if (level == 2 && obstacle2Objects.Count > 0) {
			lastFloor = ((GameObject)obstacle2Objects [obstacle2Objects.Count - 1]);
		} else {
			return -10;
		}
		return lastFloor.transform.position.x + (lastFloor.collider2D.bounds.size.x / 2);
	}
	
	// Update is called once per frame
	void Update () {
		CheckForMovement ();

		CheckForAbilityUse ();
	}

	private void CheckForMovement() {
		moveFloor = (player1.accel > 0 && player1.touchingRightWall) || (player2.accel > 0 && player2.touchingRightWall);
		
		if (Input.GetKeyDown("left")) {
			player1.accel = -playerSpeed;
		}
		if (Input.GetKeyDown("right") && !moveFloor) {
			player1.accel = playerSpeed;
		}
		if (Input.GetKeyUp("left") || Input.GetKeyUp("right")) {
			player1.accel = 0;
		}
		if (Input.GetKeyDown("up")) {
			player1.Jump();
		}
		
		if (Input.GetKeyDown("a")) {
			player2.accel = -playerSpeed;
		} 
		if (Input.GetKeyDown("d") && !moveFloor) {
			player2.accel = playerSpeed;
		} 
		if (Input.GetKeyUp("a") || Input.GetKeyUp("d")) {
			player2.accel = 0;
		}
		if (Input.GetKeyDown("w")) {
			player2.Jump();
		}
		
		if (moveFloor && !((player1.touchingLeftWall && player2.touchingRightWall) || (player1.touchingRightWall && player2.touchingLeftWall))) {
			floorAccel = playerSpeed;
			MoveMovables(1);
			MoveMovables(2);
		} else {
			floorAccel = 0;
		}
	}

	private void CheckForAbilityUse() {
		if (Input.GetKeyDown ("p")) {
			player1.wantsToChange = true;
			channelingTime = 0f;
		}
		else if (Input.GetKeyUp("p")) player1.wantsToChange = false;

		if (Input.GetKeyDown ("v")) {
			player2.wantsToChange = true;
			channelingTime = 0f;
		}
		else if (Input.GetKeyUp("v")) player2.wantsToChange = false;

		if (player1.wantsToChange && player2.wantsToChange) {
			channelingTime += Time.deltaTime;

			if (channelingTime >= channelingTimeLimit) {
				player1.canHitHard = !player1.canHitHard;
				player1.canJumpHigh = !player1.canJumpHigh;
				player2.canHitHard = !player2.canHitHard;
				player2.canJumpHigh = !player2.canJumpHigh;
				player1.wantsToChange = false;
				player2.wantsToChange = false;
			}
		}
	}

	private bool CanMoveRight() {
		return Mathf.Abs (player1.transform.position.x - player2.transform.position.x) < playersMaxDistance;
	}
	
	private void MoveMovables(int level){		
		foreach (GameObject wallObject in behindWalls) {
			Vector3 pos = wallObject.transform.position;
			pos.x -= behindWallSpeed;
			
			if (pos.x < -45.82f) {
				pos.x = 45.82f;
			}
			
			wallObject.transform.position = pos;
		}

		ArrayList obstacleObjects = ObstacleObjectsForLevel(level);
		foreach(GameObject obstacleObject in obstacleObjects) {
			if (obstacleObject) {
				Vector2 pos = obstacleObject.transform.position;
				pos.x -= floorAccel;
				obstacleObject.transform.position = pos;

				Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();
				if (obstacle.IsOutside()) {
					obstacleObjectsToDestroy.Add(obstacleObject);
				}
			}
		}
		foreach (GameObject obstacleObject in obstacleObjectsToDestroy) {
			obstacleObjects.Remove(obstacleObject);
			Destroy(obstacleObject);
			AddObstacleToLevel(level);
		}
		obstacleObjectsToDestroy.Clear ();
	}

	private ArrayList ObstacleObjectsForLevel(int level) {
		return level == 1 ? obstacle1Objects : obstacle2Objects;
	}
}
