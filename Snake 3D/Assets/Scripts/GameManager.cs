using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float tickDuration = 1f;
    float time;

    public int boardLength;
    public int boardHeight;

    public GameObject snakeBodiesPrefab;
    public GameObject foodInstance;
    public GameObject highScoreContainerPrefab;
    public List<GameObject> bodyList;

    GameObject highScoreContainerInstance;
    HighScore hs;

    public Text scoreText;
    public Text highScoreText;
    public Text gameOverText;
    int score;

    enum Direction { Up, Down, Right, Left };
    Direction deniedDirection;
    Direction oldDeniedDirection;

    Vector3 moveDir;
    Vector3 snakeEnd;

    public bool isGameOver;
    bool foodEaten;




    List<Vector3> inputList;
    Direction lastPress;
    float time2;
    bool hasInputOccurred;





    void Awake() {
        if (GameObject.FindGameObjectWithTag("HighScore") == null) {
            GameObject g = Instantiate(highScoreContainerPrefab);
        }
    }

    void Start() {
        highScoreContainerInstance = GameObject.FindGameObjectWithTag("HighScore");
        hs = highScoreContainerInstance.GetComponent<HighScore>();

        moveDir = new Vector3(-1, 0, 0);
        deniedDirection = Direction.Right;
        oldDeniedDirection = deniedDirection;
        gameOverText.enabled = false;

        highScoreText.text = "High score: " + hs.highScore.ToString();

        inputList = new List<Vector3>();
        lastPress = Direction.Left;
    }

    void Update() {
        if (Input.GetKey("space")) {
            SceneManager.LoadScene(0);
        }
        if (isGameOver == true) {
            gameOverText.enabled = true;
            return;
        }

        time += Time.deltaTime;

        Controls();
        FoodEater();
        //ResetInputList(3f);

        while (time > tickDuration) {
            Movement();
            BodyGrower();
            time -= tickDuration;
        }
    }





    //Player input - arrow keys to move, can't move backwards
    //void Controls() {
    //    if (Input.GetKey("up") && deniedDirection != Direction.Up && oldDeniedDirection != Direction.Up) {
    //        moveDir = new Vector3(0, 0, 1);
    //        deniedDirection = Direction.Down;
    //    }
    //    if (Input.GetKey("down") && deniedDirection != Direction.Down && oldDeniedDirection != Direction.Down) {
    //        moveDir = new Vector3(0, 0, -1);
    //        deniedDirection = Direction.Up;
    //    }
    //    if (Input.GetKey("right") && deniedDirection != Direction.Right && oldDeniedDirection != Direction.Right) {
    //        moveDir = new Vector3(1, 0, 0);
    //        deniedDirection = Direction.Left;

    //    }
    //    if (Input.GetKey("left") && deniedDirection != Direction.Left && oldDeniedDirection != Direction.Left) {
    //        moveDir = new Vector3(-1, 0, 0);
    //        deniedDirection = Direction.Right;
    //    }
    //}

    void FoodEater() {
        if (Mathf.Round(Vector3.Distance(bodyList[0].transform.position, foodInstance.transform.position)) == 0) {                      //Are Food and Snake close together?
            foodInstance.transform.position = RandomValidPosition();
            foodEaten = true;

            ScoreIncrementer();
        }
    }

    void BodyMover() {
        //Move the first part of the snake
        bodyList[0].transform.position += moveDir;
        oldDeniedDirection = deniedDirection;

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

    //Returns a random Vector3 position that is on board but not on the snake
    Vector3 RandomValidPosition() {
        int randomX = Random.Range(-boardLength / 2, boardLength / 2);
        int randomY = Random.Range(-boardHeight / 2, boardHeight / 2);

        Vector3 pos = new Vector3(randomX, 0, randomY);

        for (int i = 0; i < bodyList.Count; i++) {
            Vector3 temp = bodyList[i].transform.position;
            if (Vector3.Distance(temp, pos) < Mathf.Epsilon) {
                pos = RandomValidPosition();
            }
        }
        return pos;
    }

    void ScoreIncrementer() {
        score++;
        scoreText.text = "Score: " + score.ToString();
        if (score > hs.highScore) {
            hs.highScore++;
        }
        highScoreText.text = "High score: " + hs.highScore.ToString();
    }

    void Movement() {
        if (inputList.Count != 0) {
            moveDir = inputList[0];
            bodyList[0].transform.position += moveDir;
            inputList.RemoveAt(0);
        }
        else {
            bodyList[0].transform.position += moveDir;
        }
    }

    void Controls() {
        if (Input.GetKey("up") && lastPress != Direction.Up) {
            moveDir = new Vector3(0, 0, 1);
            inputList.Add(moveDir);

            lastPress = Direction.Up;
            hasInputOccurred = true;
        }
        if (Input.GetKey("down") && lastPress != Direction.Down) {
            moveDir = new Vector3(0, 0, -1);
            inputList.Add(moveDir);

            lastPress = Direction.Down;
            hasInputOccurred = true;
        }
        if (Input.GetKey("left") && lastPress != Direction.Left) {
            moveDir = new Vector3(-1, 0, 0);
            inputList.Add(moveDir);

            lastPress = Direction.Left;
            hasInputOccurred = true;
        }
        if (Input.GetKey("right") && lastPress != Direction.Right) {
            moveDir = new Vector3(1, 0, 0);
            inputList.Add(moveDir);

            lastPress = Direction.Right;
            hasInputOccurred = true;
        }
    }

    void ResetInputList(float resetTime) {

        if (hasInputOccurred == true) {
            time2 += Time.deltaTime;
        }


        while (time2 > resetTime) {
            inputList.Clear();
            time2 -= resetTime;
            hasInputOccurred = false;
        }
        print(time2);
    }
}
