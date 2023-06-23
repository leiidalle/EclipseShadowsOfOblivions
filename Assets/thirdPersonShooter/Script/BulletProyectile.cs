using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletProyectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitEnemy;
    [SerializeField] private Transform vfxHitOther;
    [SerializeField] private float speed = 10f;

    private Rigidbody bulletRigidBody;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
        bulletRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            //Hit target
            Instantiate(vfxHitEnemy,transform.position, Quaternion.identity);
        }
        else {
            //Hit en otro target
            Instantiate(vfxHitOther, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
