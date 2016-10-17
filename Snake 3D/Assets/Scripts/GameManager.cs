using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float tickDuration = 1f;
    float time;

    public GameObject snakeBodiesInstance;
    public GameObject foodPrefab;

    List<GameObject> bodyList;
    GameObject[] food;

    enum Direction { Up, Down, Right, Left};
    Direction deniedDirection;

    Vector3 moveDir;





    void Start() {
        moveDir = new Vector3(-1, 0, 0);
        deniedDirection = Direction.Right;
    }

    void Update() {
        time += Time.deltaTime;

        Controls();
        FoodEaten();

        while (time > tickDuration) {
            snakeBodiesInstance.transform.position += moveDir;
            time -= tickDuration;
        }
    }

    
    
    
    
    //Player input - arrow keys to move, can't move backwards
    void Controls() {
        if (Input.GetKey("up") && deniedDirection != Direction.Up) {
            moveDir = new Vector3(0, 0, 1);
            deniedDirection = Direction.Down;
        }
        if (Input.GetKey("down") && deniedDirection != Direction.Down) {
            moveDir = new Vector3(0, 0, -1);
            deniedDirection = Direction.Up;
        }
        if (Input.GetKey("right") && deniedDirection != Direction.Right) {
            moveDir = new Vector3(1, 0, 0);
            deniedDirection = Direction.Left;
        }
        if (Input.GetKey("left") && deniedDirection != Direction.Left) {
            moveDir = new Vector3(-1, 0, 0);
            deniedDirection = Direction.Right;
        }
    }

    //Food is moved around when touched by the snake, snake grows in size
    void FoodEaten() {
        food = GameObject.FindGameObjectsWithTag("Food");

        if (Mathf.Round(Vector3.Distance(snakeBodiesInstance.transform.position, food[0].transform.position)) == 0) {
            food[0].transform.position = new Vector3(Random.Range(-14, 14), 0.001f, Random.Range(-12, 12));
        }
    }
}
