using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Particles : MonoBehaviour
{
    private ParticleSystem vfxParticlesSystem;
    private void Awake()
    {
        vfxParticlesSystem = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        Destroy(gameObject, vfxParticlesSystem.main.duration);
    }

}
