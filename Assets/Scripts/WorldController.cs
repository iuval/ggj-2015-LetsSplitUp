using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public PlayerMovement player1;
	public PlayerMovement player2;
	
	public static float floorAccel = 0;
	public static int playersCountTouchingRightWall = 0;
	private bool moveFloor = false;

	public static float playerSpeed = 0.1f;
	public static int playersMaxDistance = 5;
	public static int borderDistance = 10;
		
	public GameObject floor;
	public GameObject[] traps;
	
	public ArrayList floor1Objects;
	public ArrayList floor2Objects;
	
	public ArrayList floorObjectsToDestroy;

	void Start () {
		floor1Objects = new ArrayList();
		floor2Objects = new ArrayList();
		floorObjectsToDestroy = new ArrayList();
		for (int i = 0; i < 5; i ++) {
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
		PlayerMovement mov1 = player1.GetComponent<PlayerMovement> ();
		PlayerMovement mov2 = player2.GetComponent<PlayerMovement> ();
		
		moveFloor = (mov1.accel > 0 && mov1.touchingRightWall) || (mov2.accel > 0 && mov2.touchingRightWall);

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
	
		if (moveFloor && !((mov1.touchingLeftWall && mov2.touchingRightWall) || (mov1.touchingRightWall && mov2.touchingLeftWall))) {
			floorAccel = playerSpeed;
			MoveFloors(1);
			MoveFloors(2);
		} else {
			floorAccel = 0;
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
			AddFloorToLevel(level);
		}
		floorObjectsToDestroy.Clear ();
	}

	private ArrayList FloorObjectsForLevel(int level) {
		return level == 1 ? floor1Objects : floor2Objects;
	}
}
