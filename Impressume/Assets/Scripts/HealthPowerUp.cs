using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Z.M.

public class HealthPowerUp : MonoBehaviour
{
    int incVal = 100;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        FindObjectOfType<Player>().AddHealth(incVal);
    }
}
