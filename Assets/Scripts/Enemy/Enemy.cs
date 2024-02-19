using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Movement
    [SerializeField] private MovementController mController;
    [SerializeField] private EnemyHealth healthController;
    private Vector2 moveDirection = Vector2.zero;

    // Shooting
    [SerializeField] private GameObject projectile;
    private float lastShootTime = 0;
    public float timeToShoot = 2.0f;

    private void FixedUpdate()
    {
        mController.MovementFixedUpdate(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;

        if (lastShootTime > timeToShoot) 
        {
            ShootProjectile();
            lastShootTime = 0;
        }
    }

    public void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + transform.forward.normalized;
        spawnPosition += transform.up.normalized * 0.2f;
        Instantiate(projectile, spawnPosition, transform.rotation);
    }
}