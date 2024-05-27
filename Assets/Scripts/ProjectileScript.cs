using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool canMove = false;
    [SerializeField] float maxFlightTime;
    [SerializeField] float currentFlightTime;
    // Start is called before the first frame update
    void Start()
    {
        
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

    public void Reflect(Vector3 dir) {
        transform.LookAt(dir);
    }
}
