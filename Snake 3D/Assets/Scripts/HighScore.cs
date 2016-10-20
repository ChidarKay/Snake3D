using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

    public int highScore;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
