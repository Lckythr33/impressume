﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
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
        
    }
}