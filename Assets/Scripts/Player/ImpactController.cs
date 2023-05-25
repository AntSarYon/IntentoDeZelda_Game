using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    //Referencias a componentes
    private Collider2D mCollider;
    private AudioSource mAudioSource;

    //Referencia la Padre y su atributo de Ataque
    private PlayerMovement player;
    private int ataque;


    //Lista para almacenar los Clips de Audio para impacto de la espada
    [SerializeField] private List<AudioClip> sonidosImpacto;

    //----------------------------------------------------------

    private void OnEnable()
    {
        //Obtenemos referencia al Player padre
        player = GetComponentInParent<PlayerMovement>();
        //Obtenemos el Ataque del Player
        ataque = player.Ataque;

        //Obtenemos referencia a componentes
        mCollider = GetComponent<Collider2D>();
        mAudioSource = GetComponent<AudioSource>();
    }

    //------------------------------------------------------------

    private void Update()
    {
        //Actualizamos que el Ataque siempre este sincroniado con el Arma que esté portando el player
        ataque = player.Ataque;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el HitBox entra en contacto con un Objeto etiquetado como enemigo
        if (collision.transform.CompareTag("Enemy"))
        {
            //Reducimos la vida del enemigo en base al Ataque del jugador
            collision.transform.GetComponent<MonkEnemyController>().ReducirVida(ataque);

            //Reproducimos uno de los sonidos de Impacto de espada
            mAudioSource.PlayOneShot(sonidosImpacto[Random.Range(0, 1)], 1);
        }

        //Si el HitBox entra en contacto con un Objeto etiquetado como enemigo
        if (collision.transform.CompareTag("Boss"))
        {
            //Reducimos la vida del enemigo en base al Ataque del jugador
            collision.transform.GetComponent<BossController>().ReducirVida(ataque);

            //Reproducimos uno de los sonidos de Impacto de espada
            mAudioSource.PlayOneShot(sonidosImpacto[Random.Range(0, 1)], 1);
        }
    }


}
