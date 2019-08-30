using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] int health = 200;
    [SerializeField] float moveSpeed = 11f;
    [SerializeField] float padding = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileDelay = 0.1f;

    Coroutine firingCoroutine;

    float xMin, xMax;
    float yMin, yMax;
    // Start is called before the first frame update
    void Start()
    {

        MoveBoundaries();
        
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }


    public void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           firingCoroutine = StartCoroutine(FireCont());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireCont()
    {
        while (true) { 
        GameObject laser = Instantiate(
               laserPrefab, transform.position, Quaternion.identity
               ) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        yield return new WaitForSeconds(projectileDelay);
    }
 }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; //create delta X on horizontal
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed; // create delta Y on vertical
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);  // Calculate new positions

        transform.position = new Vector2(newXPos, newYPos); //apply new positions

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }



    private void MoveBoundaries()
    {
        Camera gameCam = Camera.main;
        xMin = gameCam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
