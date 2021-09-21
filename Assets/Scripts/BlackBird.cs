using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBird : Bird
{
    public float fieldOfImpact;
    public LayerMask LayerToHit;
    public float force = 100;
    public bool isExplode = false;
    public GameObject ExplosionEffect;

    public void Explode()
    {
        if (isExplode == false)
        {
            isExplode = true;
            GameObject ExplosionEffectIns = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
            Destroy(ExplosionEffectIns, 10);

            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, LayerToHit);
        
            foreach (Collider2D obj in objects)
            {
                Vector2 direction = obj.transform.position - transform.position;

                obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
            }
            
        }

    }

    void OnCollisionEnter2D(Collision2D _other)
    {
        Explode();
    }
}
