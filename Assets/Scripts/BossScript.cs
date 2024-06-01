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
    [SerializeField] GameObject reflectors;
    GameObject player;

    void LoadCheckState()
    {
        hp = CheckpointState.currentCheckPoint.bossHpCount;
        shootCooldown = CheckpointState.currentCheckPoint.bossShootCooldown;
        shootDelay = CheckpointState.currentCheckPoint.bossShootDelay;

        if (hp == 1) reflectors.SetActive(true);
        else reflectors.SetActive(false);
        GetComponent<MeshRenderer>().material = damageMaterials[hp];
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        LoadCheckState();
        if (hp > 1) reflectors.SetActive(false);
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
        if (hp == 1) reflectors.SetActive(true);
        if (hp == 0) Destroy(gameObject);
        else
        {
            GetComponent<MeshRenderer>().material = damageMaterials[hp];
        }
        GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>().SetCheckPoint(hp);
        GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>().StartCutscene();
        gameObject.SetActive(false);
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
