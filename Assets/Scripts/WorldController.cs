﻿using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public Player player1;
	public Player player2;
	public GameObject badGuy;
	private float badGuySpeed = 0.005f;

	public static float floorAccel = 0;
	public static int playersCountTouchingRightWall = 0;
	private bool moveFloor = false;
	
	public static float behindWallSpeed = 0.05f;
	public static float playerSpeed = 0.1f;
	public static float playerJumpSpeed = 10f;
	public static int playersMaxDistance = 5;
	public static int distanceToDestroyObstacle = 25;
	public int obstacleCount = 4;

	public GameObject[] breakableObstacles;
	public GameObject[] jumpableObstacles;
	public GameObject[] behindWalls;
	
	public ArrayList obstacle1Objects;
	public ArrayList obstacle2Objects;
	
	public ArrayList obstacleObjectsToDestroy;
	
	public float channelingTimeLimit = 1.0f;
	public float channelingTime = 0.0f;
	public float cooldownTime = 0.0f;
	public float cooldownTimeLimit = 2.0f;

	public ChannelingBar ChannelingBar1;
	public ChannelingBar ChannelingBar2;

	void Start () {
		player1.canHitHard = true;
		player2.canJumpHigh = true;
		ChannelingBar1.Hide();
		ChannelingBar1.max = cooldownTimeLimit;
		ChannelingBar1.Reset();
		ChannelingBar2.Hide();
		ChannelingBar2.max = cooldownTimeLimit;
		ChannelingBar2.Reset();

		badGuy.transform.position = new Vector3 (-23f, 3.4f, 19f);

		obstacle1Objects = new ArrayList();
		obstacle2Objects = new ArrayList();
		obstacleObjectsToDestroy = new ArrayList();
		for (int i = 0; i < obstacleCount; i ++) {
			AddObstacleToLevel (1);
			AddObstacleToLevel (2);
		}
	}

	private void AddObstacleToLevel(int level) {
		float x = GetNewXForObstacle(level);
		float y;

		GameObject newFloor;
		if (Random.Range(0f, 1f) < 0.5f) {
			newFloor = (GameObject)GameObject.Instantiate (breakableObstacles[Random.Range(0, breakableObstacles.Length)]);
			if (level == 1) {	
				y = -8.8f;
			} else {	
				y = 4f;
			} 
		} else {
			newFloor = (GameObject)GameObject.Instantiate (jumpableObstacles[Random.Range(0, jumpableObstacles.Length)]);
			if (level == 1) {	
				y = -6.21f;
			} else {	
				y = 6.7f;
			} 
		}
		newFloor.transform.parent = gameObject.transform;
		if (level == 1) {	
			newFloor.transform.position = new Vector3 (x, y, 30);
			obstacle1Objects.Add (newFloor);
		} else {	
			newFloor.transform.position = new Vector3 (x, y, 30);
			obstacle2Objects.Add (newFloor);
		}
	}

	private float GetNewXForObstacle(int level) {
		GameObject lastObstacle;
		if (level == 1 && obstacle1Objects.Count > 0) {
			lastObstacle = ((GameObject)obstacle1Objects [obstacle1Objects.Count - 1]);
		} else if (level == 2 && obstacle2Objects.Count > 0) {
			lastObstacle = ((GameObject)obstacle2Objects [obstacle2Objects.Count - 1]);
		} else {
			return 10;
		}
		return lastObstacle.transform.position.x + Random.Range(15, 40);
	}
	
	// Update is called once per frame
	void Update () {
		CheckForMovement ();

		CheckForAbilityUse ();

		Vector3 pos = badGuy.transform.position;
		pos.x += badGuySpeed - WorldController.floorAccel;
		badGuy.transform.position = pos;

		badGuySpeed += Time.deltaTime / 1000;
	}

	private void CheckForMovement() {
		moveFloor = (player1.accel > 0 && player1.touchingRightWall) || (player2.accel > 0 && player2.touchingRightWall);
		
		if (Input.GetKeyDown("left")) {
			player1.accel = -playerSpeed;
			player1.RunLeft();
		}
		if (Input.GetKeyDown("right") && !moveFloor) {
			player1.accel = playerSpeed;
			player1.RunRight();
		}
		if (Input.GetKeyUp("left") || Input.GetKeyUp("right")) {
			player1.accel = 0;
			player1.Stand();
		}
		if (Input.GetKeyDown("up")) {
			player1.Jump();
		}
		
		if (Input.GetKeyDown("a")) {
			player2.accel = -playerSpeed;
			player2.RunLeft();
		} 
		if (Input.GetKeyDown("d") && !moveFloor) {
			player2.accel = playerSpeed;
			player2.RunRight();
		} 
		if (Input.GetKeyUp("a") || Input.GetKeyUp("d")) {
			player2.accel = 0;
			player2.Stand();
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
		if (Input.GetKeyDown ("o")) {
			player1.Hit();
		}
		
		if (Input.GetKeyDown ("c")) {
			player2.Hit();
		}

		if (cooldownTime >= cooldownTimeLimit) {
			ChannelingBar1.Hide ();
			ChannelingBar2.Hide ();
			if (Input.GetKeyDown ("p")) {
				player1.wantsToChange = true;
				channelingTime = 0f;
			} else if (Input.GetKeyUp ("p"))
				player1.wantsToChange = false;

			if (Input.GetKeyDown ("v")) {
				player2.wantsToChange = true;
				channelingTime = 0f;
			} else if (Input.GetKeyUp ("v"))
				player2.wantsToChange = false;

			if (player1.wantsToChange && player2.wantsToChange) {
				ChannelingBar1.Reset();
				ChannelingBar2.Reset();
				channelingTime += Time.deltaTime;

				if (channelingTime >= channelingTimeLimit) {
					player1.canHitHard = !player1.canHitHard;
					player1.canJumpHigh = !player1.canJumpHigh;
					player2.canHitHard = !player2.canHitHard;
					player2.canJumpHigh = !player2.canJumpHigh;
					player1.wantsToChange = false;
					player2.wantsToChange = false;

					cooldownTime = 0;
				}
			}
		} else {
			cooldownTime += Time.deltaTime;
			ChannelingBar1.Show ();
			ChannelingBar1.SetValue(cooldownTime);
			ChannelingBar2.Show ();
			ChannelingBar2.SetValue(cooldownTime);
			if (cooldownTime >= cooldownTimeLimit) Debug.Log("READY TO CHANGE");
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
				Vector3 pos = obstacleObject.transform.position;
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
