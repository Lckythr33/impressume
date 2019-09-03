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

    [Header("Death")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = .2f;

    [Header("Sound")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;  // 3/4 of max. volume
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;  // 1/4 of max. volume

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
               laserPrefab, transform.position, Quaternion.Euler(0, 0, 90)
               ) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
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

    public int GetHealth()
    {
        return health;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);

    }



    private void MoveBoundaries()
    {
        Camera gameCam = Camera.main;
        xMin = gameCam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 2f;
        xMax = gameCam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 2f;
        yMin = gameCam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
