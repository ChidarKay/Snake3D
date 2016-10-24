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
    List<Vector3> inputList;

    GameObject highScoreContainerInstance;
    HighScore hs;

    public Text scoreText;
    public Text highScoreText;
    public Text gameOverText;
    int score;

    enum Direction { Up, Down, Right, Left };
    Direction previousDirection;
    Direction oppositeDirection;

    Vector3 moveDir;
    Vector3 snakeEnd;

    public bool isGameOver;
    bool foodEaten;





    void Awake() {
        if (GameObject.FindGameObjectWithTag("HighScore") == null) {
            GameObject g = Instantiate(highScoreContainerPrefab);
        }
    }

    void Start() {
        highScoreContainerInstance = GameObject.FindGameObjectWithTag("HighScore");
        hs = highScoreContainerInstance.GetComponent<HighScore>();

        moveDir = new Vector3(-1, 0, 0);
        gameOverText.enabled = false;

        highScoreText.text = "High score: " + hs.highScore.ToString();

        inputList = new List<Vector3>();
        previousDirection = Direction.Left;
        oppositeDirection = Direction.Right;
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

        //string s = " ";
        //foreach (Vector3 element in inputList) {
        //    s += element;
        //}
        //print(s);

        while (time > tickDuration) {
            Movement();
            BodyGrower();
            time -= tickDuration;
        }
    }





    void FoodEater() {
        if (Mathf.Round(Vector3.Distance(bodyList[0].transform.position, foodInstance.transform.position)) == 0) {                      //Are Food and Snake close together?
            foodInstance.transform.position = RandomValidPosition();
            foodEaten = true;

            ScoreIncrementer();
        }
    }

    void BodyGrower() {
        if (foodEaten == true) {
            GameObject newPart = Instantiate(snakeBodiesPrefab, snakeEnd, Quaternion.identity) as GameObject;                           //Increase snake length upon eating food
            bodyList.Add(newPart);
            foodEaten = false;
        }
    }

    //Returns a random Vector3 position that is on the board but not on the snake
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
        //Move head first according to the buffer then just forward
        if (inputList.Count != 0) {
            moveDir = inputList[0];
            bodyList[0].transform.position += moveDir;
            inputList.RemoveAt(0);
        }
        else {
            bodyList[0].transform.position += moveDir;
        }

        //Move the rest of the parts
        snakeEnd = bodyList[bodyList.Count - 1].transform.position;

        bodyList[bodyList.Count - 1].transform.position = bodyList[0].transform.position - moveDir;
        bodyList.Insert(1, bodyList[bodyList.Count - 1]);
        bodyList.RemoveAt(bodyList.Count - 1);
    }

    void Controls() {

        string[] keys = new string[] { "up", "down", "left", "right" };
        Direction[] directions = new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        Direction[] oppositeDirections = new Direction[] { Direction.Down, Direction.Up, Direction.Right, Direction.Left };
        Vector3[] moveDirections = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        for (int i = 0; i < 4; i++) {
            if (Input.GetKeyDown(keys[i]) && previousDirection != directions[i] && oppositeDirection != directions[i]) {
                moveDir = moveDirections[i];
                inputList.Add(moveDir);

                previousDirection = directions[i];
                oppositeDirection = oppositeDirections[i];

                //Limit the size of the buffer to 3
                if (inputList.Count > 3) {
                    inputList.RemoveAt(inputList.Count-1);
                }
            }
        }
    }
}
