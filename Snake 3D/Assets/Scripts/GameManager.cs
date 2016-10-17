using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    float time;

    public GameObject snakeBodiesInstance;

    void Update() {
        Controls();
    }

    void Controls() {
        if (Input.GetKey("up")) {
            snakeBodiesInstance.transform.position += new Vector3(0, 0, 1);
        }
        if (Input.GetKey("down")) {
            snakeBodiesInstance.transform.position += new Vector3(0, 0, -1);
        }
        if (Input.GetKey("right")) {
            snakeBodiesInstance.transform.position += new Vector3(1, 0, 0);
        }
        if (Input.GetKey("left")) {
            snakeBodiesInstance.transform.position += new Vector3(-1, 0, 0);
        }
    }
}
