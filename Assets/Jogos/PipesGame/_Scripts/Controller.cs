using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject pipesHolder;
    public GameObject[] pipes;

    [SerializeField]
    private int totalpipes = 0;
    [SerializeField]
    private int correctsPipes = 0;

    // Start is called before the first frame update
    void Start()
    {
        totalpipes = pipesHolder.transform.childCount;

        pipes = new GameObject[totalpipes];

        for (int i = 0; i < pipes.Length; i++ ) {
            pipes[i] = pipesHolder.transform.GetChild(i).gameObject;
        }

    }

    public void CorrectMove() {
        correctsPipes += 1;
        Debug.Log("correct Move");

        if (correctsPipes == totalpipes) {
            Debug.Log("you win!");
        }
    }

    public void WrongMove() {
        correctsPipes -= 1;
    }

}
