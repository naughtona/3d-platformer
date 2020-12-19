using System.Collections;
using UnityEngine;

public class GlobalOptions : MonoBehaviour
{
    // settings
    public static float sensitivity=0.8f;
    public static float ghostSpeed=10f;
    public static float gargoyleFireRate = 1f;

    // coins or "score"
    public static int score = 0;

    // upgrade values
    public static int maxTemp = 100;
    public static string tempText = "Temperature";
    public static float jumpHeight = 3.0f;
    public static float playerSpeed = 18.0f;

    // upgrade bools
    public static bool tempUpgrade = false;
    public static bool jumpUpgrade = false;
    public static bool speedUpgrade = false;

    // puzzle variables
    public static int currentPosition = 0;
    public static int[] puzzlePositions = new int[4];
    public static readonly int[] puzzleSolution = {2,1,3,0};

    public static void ResetStatics() {
        score = 0;
        maxTemp = 100;
        tempText = "Temperature";
        jumpHeight = 3.0f;
        playerSpeed = 18.0f;
        tempUpgrade = false;
        jumpUpgrade = false;
        speedUpgrade = false;
    } 
}
