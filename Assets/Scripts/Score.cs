using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    private int blueScore = 0;
    private int redScore = 0;
    
    void Update()
    {
        GetComponentInChildren<Text>().text = "Red: " + redScore + "\n\nBlue: " + blueScore;
    }
    
    public bool addPointRed() {
        redScore++;
        if(redScore == 10) {
            return true;
        }
        return false;
    }

    public bool addPointBlue() {
        blueScore++;
        if(blueScore == 10) {
            return true;
        }
        return false;
    }
}
