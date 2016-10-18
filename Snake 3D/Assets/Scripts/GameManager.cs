using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float tickDuration = 1f;
    float time;

    public int boardLength;
    public int boardHeight;

    public GameObject snakeBodiesPrefab;
    public GameObject foodInstance;
    public List<GameObject> bodyList;

    enum Direction { Up, Down, Right, Left };
    Direction deniedDirection;

    Vector3 moveDir;
    Vector3 snakeEnd;

    bool foodEaten;





    void Start() {
        moveDir = new Vector3(-1, 0, 0);
        deniedDirection = Direction.Right;
    }

    void Update() {
        time += Time.deltaTime;

        Controls();
        FoodEater();

        while (time > tickDuration) {
            BodyMover();
            BodyGrower();
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

    void FoodEater() {
        if (Mathf.Round(Vector3.Distance(bodyList[0].transform.position, foodInstance.transform.position)) == 0) {                      //Are Food and Snake close together?
            foodInstance.transform.position = RandomPosition();
            foodEaten = true;
        }
    }

    void BodyMover() {
        //Move the first part of the snake
        bodyList[0].transform.position += moveDir;

        //Move the rest of the parts
        snakeEnd = bodyList[bodyList.Count - 1].transform.position;

        bodyList[bodyList.Count - 1].transform.position = bodyList[0].transform.position - moveDir;
        bodyList.Insert(1, bodyList[bodyList.Count - 1]);
        bodyList.RemoveAt(bodyList.Count - 1);
    }

    void BodyGrower() {
        if (foodEaten == true) {
            GameObject newPart = Instantiate(snakeBodiesPrefab, snakeEnd, Quaternion.identity) as GameObject;                           //Increase snake length upon eating food
            bodyList.Add(newPart);
            foodEaten = false;
        }
    }

    Vector3 RandomPosition() {
        int randomX = Random.Range(-boardLength / 2, boardLength / 2);
        int randomY = Random.Range(-boardHeight / 2, boardHeight / 2);

        Vector3 pos = new Vector3(randomX, 0, randomY);
        return pos;
    }
}
