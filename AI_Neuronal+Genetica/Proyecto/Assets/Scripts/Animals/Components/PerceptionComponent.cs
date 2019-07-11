using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField]
    protected SphereCollider detection_area;

    public GameObject object_detected { get; set; }

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
        object_detected = other.gameObject;

    }

    private void OnCollisionEnter(Collision collision)
    {

        
    }

    private void OnTriggerExit(Collider other)
    {

    }

    public SphereCollider getDetectionArea() { return detection_area; }
}
