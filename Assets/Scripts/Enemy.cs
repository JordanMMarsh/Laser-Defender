using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] float health = 100f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject laser;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float explosionEndTime = 1f;
    [Range(0, 1)][SerializeField] float deathSoundVolume = .5f;
    [SerializeField] AudioClip enemyFiringSound;
    [SerializeField] AudioClip enemyDeathSound;
    [SerializeField] int scoreValue = 150;

    private void Start()
    {
        CalculateShotCounter();   
    }

    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            CalculateShotCounter();
        }
    }

    private void Fire()
    {
       GameObject newLaser = Instantiate(laser, transform.position, Quaternion.identity) as GameObject;
       newLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
       GetComponent<AudioSource>().PlayOneShot(enemyFiringSound);
    }

    private void CalculateShotCounter()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);       
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
            var explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity) as GameObject;
            Destroy(explosion, explosionEndTime);
            AudioSource.PlayClipAtPoint(enemyDeathSound, Camera.main.transform.position, deathSoundVolume);
            Destroy(gameObject);
        }
    }
}
