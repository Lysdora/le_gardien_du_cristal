using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtefactHealth : MonoBehaviour
{
    public Slider healthBar;
    float health = 100f;
    float decreaseRate = 1f;
    float max_health = 100f;
    // Start is called before the first frame update
    void Start()

    {
        health = max_health;
        healthBar.value = health;
        InvokeRepeating("DecreaseHealth", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DecreaseHealth()
    {
        health -= decreaseRate;
        healthBar.value = health;
        Debug.Log("Vie de l'artefact : " + health);

        if (health <= 0)
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    public void RestoreHealth(float amount)
    {
        health += amount;

        if (health > max_health)
            health = max_health; // Ne pas dépasser le max
        healthBar.value = health;
        Debug.Log("Artefact nourri ! Vie : " + health);
    }
}
