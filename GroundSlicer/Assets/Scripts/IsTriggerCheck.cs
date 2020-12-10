using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTriggerCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Deformable"))
            return;

        if (other.gameObject.GetComponent<Rigidbody>() == null)
        {
            other.gameObject.transform.localScale *= 0.95f;
            other.gameObject.AddComponent<Rigidbody>();
            Destroy(other.gameObject, 2f);

        }

        Destroy(gameObject, 2f);
        
    }
}
