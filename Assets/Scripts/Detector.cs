using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public GameManager gameManager;                 // InterCommunication scripts

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Te he pillado");
            gameManager.isPlayerCaught = true;
        }
    }
}
