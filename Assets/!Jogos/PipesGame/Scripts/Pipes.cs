using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pipes : MonoBehaviour
{
    private static readonly int[] possibleRotations = { 0, 90, 180, 270 };

    public int[] correctRotations;
    private bool isPlaced = false;

    private Controller gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<Controller>();
    }

    private void Start()
    {
        int rand = Random.Range(0, possibleRotations.Length);
        transform.rotation = Quaternion.Euler(0, 0, possibleRotations[rand]);

        foreach (int rotation in correctRotations)
        {
            if (Mathf.Approximately(transform.eulerAngles.z, rotation))
            {
                isPlaced = true;
                gameManager.CorrectMove();
                break;
            }
        }
    }

    public void OnButtonClick()
    {
        transform.Rotate(new Vector3(0, 0, 90));

        bool isRotationCorrect = false;
        foreach (int rotation in correctRotations)
        {
            if (Mathf.Approximately(transform.eulerAngles.z, rotation))
            {
                isRotationCorrect = true;
                break;
            }
        }

        if (isRotationCorrect && !isPlaced)
        {
            isPlaced = true;
            gameManager.CorrectMove();
        }
        else if (!isRotationCorrect && isPlaced)
        {
            isPlaced = false;
            gameManager.WrongMove();
        }
    }
}
