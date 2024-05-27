using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [Header("Healthparameter")]
    [SerializeField] int hp = 3;

    [Header("Angriffparameter")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletForce;
    [SerializeField] bool canShoot = true;
    [SerializeField] float shootCooldown;
    [SerializeField] float shootDelay;

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
    }

    
    void Update()
    {
        if (canShoot)
        {
            StartCoroutine(ShootBullet());
        }
    }

    public void TakeDamage()
    {
        hp--;
    }

    IEnumerator ShootBullet()
    {
        Debug.Log("Shoot");
        canShoot = false;
        Vector3 pos = gameObject.transform.position;
        Quaternion rot = Quaternion.identity;
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        bullet.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
        yield return new WaitForSeconds(shootDelay);
        bullet.GetComponent<ProjectileScript>().Shoot();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
