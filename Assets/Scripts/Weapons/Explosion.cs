using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Min(1)]public int explosionDamage = 1;
    public float expansion = .02f;

    void Awake()
    {
        transform.localScale = Vector3.one * .6f;
    }

    void Update()
    {
        transform.localScale += Vector3.one * expansion * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            entity.TakeDamage(explosionDamage);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
