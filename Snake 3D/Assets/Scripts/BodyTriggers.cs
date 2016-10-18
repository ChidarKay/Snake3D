using UnityEngine;
using System.Collections;

public class BodyTriggers : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
        print("GAME OVER");
    }
}
