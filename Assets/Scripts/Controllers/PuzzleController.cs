using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour {
    public int position;
    public MeshRenderer otherRenderer1;
    public MeshRenderer otherRenderer2;
    public MeshRenderer otherRenderer3;
    public GameObject elevator;

    private MeshRenderer mRenderer; 
    
    private void Start() {
        mRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other) {
        if (mRenderer.material.GetColor("_Color") == Color.white) {
            mRenderer.material.SetColor("_Color", Color.black);
            GlobalOptions.puzzlePositions[GlobalOptions.currentPosition++] = position;
            CompleteCheck();
        } 
    }

    private void CompleteCheck() {
        if (GlobalOptions.currentPosition == 4) {
            if (ArrEquals()) {
                ChangeCubeColour(Color.green);
                elevator.SetActive(true);
            } else {
                StartCoroutine(ResetPuzzle());
            }
            GlobalOptions.currentPosition = 0;
        }
    }

    IEnumerator ResetPuzzle() {
        ChangeCubeColour(Color.red);
        yield return new WaitForSeconds(3.0f);
        ChangeCubeColour(Color.white);
        
    }

    private void ChangeCubeColour(Color color) {
        mRenderer.material.SetColor("_Color", color);
        otherRenderer1.material.SetColor("_Color", color);
        otherRenderer2.material.SetColor("_Color", color);
        otherRenderer3.material.SetColor("_Color", color);
    }

    private bool ArrEquals() {
        for (int i=0; i<4;i++) {
            if (GlobalOptions.puzzlePositions[i] != GlobalOptions.puzzleSolution[i]) {
                return false;
            }
        }
        return true;
    }
}   
