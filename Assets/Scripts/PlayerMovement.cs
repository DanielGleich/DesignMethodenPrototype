using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            rb.AddForce(-rb.transform.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey("s")) { rb.AddForce(rb.transform.forward * moveSpeed * Time.deltaTime); }
        
        
        if ( Input.GetKey("a") ) {
            rb.AddForce(rb.transform.right * moveSpeed * Time.deltaTime);
        } else if ( Input.GetKey("d") ) { rb.AddForce(-rb.transform.right * moveSpeed * Time.deltaTime);}


    }
}
