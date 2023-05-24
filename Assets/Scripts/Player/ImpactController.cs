using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    //Referencias a componentes
    private Collider2D mCollider;
    private AudioSource mAudioSource;

    //Lista para almacenar los Clips de Audio para impacto de la espada
    [SerializeField] private List<AudioClip> sonidosImpacto;

    //----------------------------------------------------------

    private void OnEnable()
    {
        //Obtenemos referencia a componentes
        mCollider = GetComponent<Collider2D>();
        mAudioSource = GetComponent<AudioSource>();
    }

    //------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el HitBox entra en contacto con un Objeto etiquetado como enemigo
        if (collision.transform.CompareTag("EnemyBoss"))
        {
            //Reproducimos uno de los sonidos de Impacto de espada
            mAudioSource.PlayOneShot(sonidosImpacto[Random.Range(0, 1)], 0.90f);
        }
    }


}
