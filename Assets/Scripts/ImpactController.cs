using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    private Collider2D mCollider;
    private AudioSource mAudioSource;

    [SerializeField] private List<AudioClip> sonidosImpacto;

    private void OnEnable()
    {
        mCollider = GetComponent<Collider2D>();
        mAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("EnemyBoss"))
        {
            mAudioSource.PlayOneShot(sonidosImpacto[Random.Range(0, 1)], 0.90f);
        }
    }


}
