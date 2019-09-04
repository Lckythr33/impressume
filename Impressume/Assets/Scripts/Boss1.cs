using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = .2f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;  // 3/4 of max. volume
    [SerializeField] int scoreValue = 150;

    [Header("Projectile")]
    [SerializeField] float shotCounter;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectile;
    [SerializeField] float minTimeBetweenShots = .2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;  // 1/4 of max. volume

    [Header("Shotgun")]
    [SerializeField] GameObject shellprefab1;
    [SerializeField] GameObject shellprefab2;
    [SerializeField] GameObject shellprefab3;
    [SerializeField] GameObject shellprefab4;
    [SerializeField] GameObject shellprefab5;


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Shotgun();
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
            projectile,
            firePoint.position,
            Quaternion.Euler(0, 0, -90)
            ) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

        GameObject laser1 = Instantiate(
           projectile,
           firePoint.position,
           Quaternion.Euler(0, 0, -90)
           ) as GameObject;
        laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-8,8), -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

        GameObject laser2 = Instantiate(
           projectile,
           firePoint.position,
           Quaternion.Euler(0, 0, -90)
           ) as GameObject;
        laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-8, 8), -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    public void Shotgun()
    {
            GameObject shell1 = Instantiate(shellprefab1, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            shell1.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-10,10), -Random.Range(5,18));
            
            GameObject shell2 = Instantiate(shellprefab2, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            shell2.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3, 3), Random.Range(-5, 18));
           
            GameObject shell3 = Instantiate(shellprefab3, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            shell3.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-5, 5), -Random.Range(5, 18));
           
            GameObject shell4 = Instantiate(shellprefab4, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            shell4.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-7, 7), Random.Range(-5, 18));
       
            GameObject shell5 = Instantiate(shellprefab5, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            shell5.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-15, 15), -Random.Range(5, 18));
           
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
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        FindObjectOfType<Level>().LoadResume();

    }
}