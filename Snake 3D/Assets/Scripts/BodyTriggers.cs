using UnityEngine;
using System.Collections;

public class BodyTriggers : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
        GameObject.Find("GameManager").GetComponent<GameManager>().isGameOver = true;
    }
}
