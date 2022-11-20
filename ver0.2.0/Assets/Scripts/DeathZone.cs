using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().ComeBackHome();
        }
        else
        {
            other.GetComponent<Zombie>().Die();
        }
    }
}
