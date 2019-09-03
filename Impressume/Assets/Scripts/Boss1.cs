using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = .2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = .2f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;  // 3/4 of max. volume
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;  // 1/4 of max. volume
    [SerializeField] int scoreValue = 150;
    [SerializeField] float projectileFiringPeriod= 0.1f;

    [Header("code shot")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject codeShotPrefab1;
    [SerializeField] GameObject codeShotPrefab2;
    [SerializeField] GameObject codeShotPrefab3;
    [SerializeField] GameObject codeShotPrefab4;
    [SerializeField] GameObject codeShotPrefab5;


    Coroutine codeShotCoroutine;

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
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
            projectile,
            transform.position,
            Quaternion.Euler(0, 0, -90)
            ) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }

    IEnumerator fireCodeShot()
    {
        while (true)
        {
            GameObject codeShot1 = Instantiate(codeShotPrefab1, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            codeShot1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(.5f);
            GameObject codeShot2 = Instantiate(codeShotPrefab2, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            codeShot2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(.5f);
            GameObject codeShot3 = Instantiate(codeShotPrefab3, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            codeShot3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(.5f);
            GameObject codeShot4 = Instantiate(codeShotPrefab4, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            codeShot4.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(.5f);
            GameObject codeShot5 = Instantiate(codeShotPrefab5, transform.position, Quaternion.Euler(0, 0, 90)) as GameObject;
            codeShot5.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
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