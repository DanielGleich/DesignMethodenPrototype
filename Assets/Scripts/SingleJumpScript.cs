using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleJumpScript : MonoBehaviour
{

    [SerializeField] float maxLifeTime;
    [SerializeField] Material touchedMaterial;
    [SerializeField] MeshRenderer mRenderer;
    float currentLifeTime = 0;
    bool isTouched = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouched) return;
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime > maxLifeTime) { 
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isTouched = true;
            if (mRenderer != null && touchedMaterial != null)
            {
                mRenderer.material = touchedMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isTouched = true;
            if (mRenderer != null && touchedMaterial != null)
            {
                mRenderer.material = touchedMaterial;
            }
        }
    }
}
