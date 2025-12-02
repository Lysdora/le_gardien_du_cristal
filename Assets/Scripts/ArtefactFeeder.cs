using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactFeeder : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = collision.GetComponent<PlayerInventory>();
        }


    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory.GetGemmeCount() >= 1)
            {
                playerInventory.RemoveGemme(1);
                GetComponent<ArtefactHealth>().RestoreHealth(10);
                Debug.Log("Artefact nourri ! +10 PV");
            }
            else
            {
                Debug.Log("Pas assez de gemmes !");
            }
        }
        // Pas de else ici !
    }
}
