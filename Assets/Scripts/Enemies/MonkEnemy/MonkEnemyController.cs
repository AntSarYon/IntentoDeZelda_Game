using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonkEnemyController : MonoBehaviour
{
    //Referencia a componentes
    private Rigidbody2D mRb;
    private Collider2D mCollider;
    private SpriteRenderer mSpriteRenderer;
    private Animator mAnimator;
    private AudioSource mAudioSource;

    //Lista de Clips de Audio que usará el enemigo
    private List<AudioClip> listaClips;

    //Variables parametros
    [SerializeField] private float WakeDistance = 3.5f;
    [SerializeField] private float Speed = 1.5f;
    [SerializeField] private float AttackDistance = 1f;

    //Flags de Estado
    private bool AtaqueFinalizado = false;

    //Variable de referencia al jugador
    [SerializeField] private Transform player;

    //Referencia al HitBox
    private Transform hitBox;

    //Tendremos una Maquina de Estados Finita (FSM)
    private FSM<MonkEnemyController> mFSM;

    //-------------------------------------------------------------------------------

    private void Awake()
    {
        //Obtenemos referencias
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();

        hitBox = transform.Find("HitBox");
    }

    //-------------------------------------------------------------------------------

    private void Start()
    {
        mFSM = new FSM<MonkEnemyController>(new MonkEnemy.MonkIdleState(this));
        mFSM.Begin();  // prendo la máquina de estados
    }

    //------------------------------------------------------------------

    private void FixedUpdate()
    {
        //Le pasamos el FixedDeltaTime para no Afectar el rendimiento
        mFSM.Tick(Time.fixedDeltaTime);
    }

    //-------------------------------------------------------------------------
    // Funcion Evento para indicar el Fin del Ataque tras completar la animacion

    public void SetAttackingEnd()
    {
        AtaqueFinalizado = true;
    }


}
