using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin }

    [Header("Bullet Attributes")]
    public GameObject bulletPrefab;
    public float bulletLife = 1f;
    public float speed = 1f;
    //public int bulletPerFire = 1;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;

    private GameObject spawnedBullet;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (spawnerType == SpawnerType.Spin) transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 1);
        if(timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }

    private void Fire()
    {
        spawnedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<BulletScript>().speed = speed;
        spawnedBullet.GetComponent<BulletScript>().bulletLife = bulletLife;
        spawnedBullet.transform.rotation = transform.rotation;
    }
}
