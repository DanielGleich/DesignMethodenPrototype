using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool canMove = false;
    [SerializeField] float maxFlightTime;
    [SerializeField] float currentFlightTime;

    Rigidbody rb;
    bool reflected = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentFlightTime += Time.deltaTime;
        if (currentFlightTime > maxFlightTime)
            Destroy(gameObject);
        if (canMove)
            transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void Shoot() {
        canMove = true;
    }

    public void Reflect() {
        Vector3 dir = GameObject.FindGameObjectWithTag("Player").transform.forward;
        dir = dir * Camera.main.transform.eulerAngles.x;
        dir = dir * Camera.main.transform.eulerAngles.y;
        transform.LookAt(dir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (reflected && other.gameObject.layer != LayerMask.NameToLayer("Boss")) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Reflector"))
        {
            rb.excludeLayers = 0;
            reflected = true;
            Reflect();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player")) { 
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boss")) {
            other.gameObject.GetComponent<BossScript>().TakeDamage();
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Plattform")) {
            other.gameObject.GetComponent<TerrainScript>().SimulateProjectileHit();
            Destroy(gameObject);
        }
        else { 
            Destroy(gameObject);
        }  
    }
}

