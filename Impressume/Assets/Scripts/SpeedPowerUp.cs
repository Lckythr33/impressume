using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Z.M.
public class SpeedPowerUp : MonoBehaviour
{
    [SerializeField] private float powerUpCounter;
    [SerializeField] private int incVal = 10;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<Player>().speedUp(10, powerUpCounter);
        Destroy(gameObject);
    }


}
