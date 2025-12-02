using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemmeController : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerInventory playerInventory = collision.gameObject.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddGemme();
            }
            // Ajouter des points au score du joueur ici
            Debug.Log("Gemme collectée !");
            Destroy(gameObject);
          
        }
    }
}
