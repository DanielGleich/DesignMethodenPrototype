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
    [SerializeField] bool canShoot = true;
    [SerializeField] float shootCooldown;
    [SerializeField] float shootDelay;


    [Header("Sonstiges")]
    [SerializeField] List<Material> damageMaterials;
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
        if (hp == 0) Destroy(gameObject); else { 
            GetComponent<MeshRenderer>().material = damageMaterials[hp];
        }
    }

    IEnumerator ShootBullet()
    {
        canShoot = false;
        Vector3 pos = gameObject.transform.position;
        Quaternion rot = Quaternion.identity;
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        bullet.transform.LookAt(player.transform.position);
        yield return new WaitForSeconds(shootDelay);
        bullet.GetComponent<ProjectileScript>().Shoot();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
