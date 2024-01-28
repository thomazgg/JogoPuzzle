using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seguir : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        if (player.position.y > transform.position.y) {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }
}
