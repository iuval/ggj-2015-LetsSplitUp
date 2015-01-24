using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public PlayerMovement player1;
	public PlayerMovement player2;

	public static float playerSpeed = 0.5f;
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
		if (Input.GetKeyDown("left")) {
			if (player1.gameObject.transform.position.x > -borderDistance) {
				player1.MoveX(-playerSpeed);
			}
		} else if (Input.GetKeyDown("right")) {
			if (player1.gameObject.transform.position.x < borderDistance) {
				player1.MoveX(playerSpeed);
			} else if(CanMoveRight()) {
				MoveFloor(2);
			}
		}

		if (Input.GetKeyDown("a")) {
			if (player2.gameObject.transform.position.x > -borderDistance) {
				player2.MoveX(-playerSpeed);
			}
		} else if (Input.GetKeyDown("d")) {
			if (player2.gameObject.transform.position.x < borderDistance) {
				player2.MoveX(playerSpeed);
			} else if(CanMoveRight()) {	
				MoveFloor(1);
			}
		}
	}

	private bool CanMoveRight() {
		return Mathf.Abs (player1.transform.position.x - player2.transform.position.x) < borderDistance;
	}
	
	private void MoveFloor(int level) {
		MoveFloors (1);
		MoveFloors (2);

		if (level == 1) {
			player1.MoveX(-playerSpeed);
		} else {
			player2.MoveX(-playerSpeed);
		}

		DestroyFloorsOutside (1);
		DestroyFloorsOutside (2);
	}
	
	private void MoveFloors(int level){
		ArrayList floorObjects = FloorObjectsForLevel(level);
		foreach(GameObject floorObject in floorObjects) {
			if (floorObject) {
				Vector2 pos = floorObject.transform.position;
				pos.x -= playerSpeed;
				floorObject.transform.position = pos;
			}
		}
	}

	private void DestroyFloorsOutside(int level){
		ArrayList floorObjects = FloorObjectsForLevel(level);
		foreach (GameObject floorObject in floorObjects) {
			if (floorObject) {
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
