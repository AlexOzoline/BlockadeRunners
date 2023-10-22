using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleOnComplete : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetParticleSystem;

    private void Update()
    {
        // Poll if system if alive on every update
        if (!this.targetParticleSystem.IsAlive())
            Destroy(this.targetParticleSystem.gameObject);
    }
}
