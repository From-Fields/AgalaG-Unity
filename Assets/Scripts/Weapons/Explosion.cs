using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Min(1)]public int explosionDamage = 1;
    public float expansion = .02f;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClip;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        transform.localScale = Vector3.one * .6f;

        if(_audioClip != null) {
            if(_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            _audioSource.PlayOneShot(_audioClip);
        }
    }

    void Update()
    {
        transform.localScale += Vector3.one * expansion * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();
        entity?.TakeDamage(explosionDamage);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
