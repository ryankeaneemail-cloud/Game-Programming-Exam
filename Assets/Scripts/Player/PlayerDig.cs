using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDig : MonoBehaviour
{
    public KeyCode digKey;
    private Plant nearbyPlant;

    void Update()
    {
        CheckDigging();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Plant")
        {
            Debug.Log("Collide With Plant");
            nearbyPlant = collision.GetComponent<Plant>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Plant")
        {
            Debug.Log("Exit Plant");
            nearbyPlant = null;
        }
    }

    void CheckDigging()
    {
        if (Input.GetKeyDown(digKey) && nearbyPlant != null)
        {
            Debug.Log("Digging plant");
            GetComponent<Animator>().SetTrigger("Dig");
            nearbyPlant.HarvestPlant();
        }
    }
}
