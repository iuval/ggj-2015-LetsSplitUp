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
	public static int playersMaxDistance = 5;
	public static int distanceToDestroyFloor = 25;
	public int floorCount = 4;
		
	public GameObject floor;
	public GameObject[] behindWalls;
	
	public ArrayList floor1Objects;
	public ArrayList floor2Objects;
	
	public ArrayList floorObjectsToDestroy;
	
	public float channelingTimeLimit = 1.0f;
	public float channelingTime = 0.0f;

	void Start () {
		player1.canHitHard = true;
		player2.canJumpHigh = true;

		floor1Objects = new ArrayList();
		floor2Objects = new ArrayList();
		floorObjectsToDestroy = new ArrayList();
		for (int i = 0; i < floorCount; i ++) {
			AddFloorToLevel (1);
			AddFloorToLevel (2);
		}
	}

	private void AddFloorToLevel(int level) {
		float x = GetNewXForFloor(level);

		GameObject newFloor = (GameObject)GameObject.Instantiate (floor);
		newFloor.transform.parent = gameObject.transform;
		if (level == 1) {
			
			newFloor.transform.position = new Vector2 (x, -7.8f);
			floor1Objects.Add (newFloor);
		} else {
			
			newFloor.transform.position = new Vector2 (x, 4.2f);
			floor2Objects.Add (newFloor);
		}
	}

	private float GetNewXForFloor(int level) {
		GameObject lastFloor;
		if (level == 1 && floor1Objects.Count > 0) {
			lastFloor = ((GameObject)floor1Objects [floor1Objects.Count - 1]);
		} else if (level == 2 && floor2Objects.Count > 0) {
			lastFloor = ((GameObject)floor2Objects [floor2Objects.Count - 1]);
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
		} else if (Input.GetKeyDown("right") && !moveFloor) {
			player1.accel = playerSpeed;
		} else if (Input.GetKeyUp("left") || Input.GetKeyUp("right")) {
			player1.accel = 0;
		}
		
		if (Input.GetKeyDown("a")) {
			player2.accel = -playerSpeed;
		} else if (Input.GetKeyDown("d") && !moveFloor) {
			player2.accel = playerSpeed;
		} else if (Input.GetKeyUp("a") || Input.GetKeyUp("d")) {
			player2.accel = 0;
		}
		
		if (moveFloor && !((player1.touchingLeftWall && player2.touchingRightWall) || (player1.touchingRightWall && player2.touchingLeftWall))) {
			floorAccel = playerSpeed;
			MoveFloors(1);
			MoveFloors(2);
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
	
	private void MoveFloors(int level){
		ArrayList floorObjects = FloorObjectsForLevel(level);
		foreach(GameObject floorObject in floorObjects) {
			if (floorObject) {
				Vector2 pos = floorObject.transform.position;
				pos.x -= floorAccel;
				floorObject.transform.position = pos;

				Floor floor = floorObject.GetComponent<Floor>();
				if (floor.IsOutside()) {
					floorObjectsToDestroy.Add(floorObject);
				}
			}
		}
		foreach (GameObject floorObject in floorObjectsToDestroy) {
			floorObjects.Remove(floorObject);
			Destroy(floorObject);
			AddFloorToLevel(level);
		}
		floorObjectsToDestroy.Clear ();

		foreach (GameObject wallObject in behindWalls) {
			Vector3 pos = wallObject.transform.position;
			pos.x -= behindWallSpeed;
			
			if (pos.x < -45.82f) {
				pos.x = 45.82f;
			}

			wallObject.transform.position = pos;
		}
	}

	private ArrayList FloorObjectsForLevel(int level) {
		return level == 1 ? floor1Objects : floor2Objects;
	}
}
