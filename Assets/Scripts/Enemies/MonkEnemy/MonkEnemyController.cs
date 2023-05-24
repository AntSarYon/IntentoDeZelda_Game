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
    [SerializeField] private float wakeDistance = 3.5f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float attackDistance = 0.50f;

    //Posicion del Sprite en relacion con el centro del GameObject
    private Vector3 posicionRelativa;
    private Vector3 distanciaRelativa = new Vector3(0, 0.86f, 0);

    //Flags de Estado
    private bool ataqueFinalizado = false;

    //Variable de referencia al jugador
    [SerializeField] private Transform player;

    //Referencia al HitBox
    private Transform hitBox;

    //Tendremos una Maquina de Estados Finita (FSM)
    private FSM<MonkEnemyController> mFSM;

    //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    #region GETTERS Y SETTERS
    public Transform Player { get => player; set => player = value; }
    public float WakeDistance { get => wakeDistance; set => wakeDistance = value; }
    public Vector3 PosicionRelativa { get => posicionRelativa; set => posicionRelativa = value; }
    public AudioSource MAudioSource { get => mAudioSource; set => mAudioSource = value; }
    public Animator MAnimator { get => mAnimator; set => mAnimator = value; }
    public SpriteRenderer MSpriteRenderer { get => mSpriteRenderer; set => mSpriteRenderer = value; }
    public Collider2D MCollider { get => mCollider; set => mCollider = value; }
    public Rigidbody2D MRb { get => mRb; set => mRb = value; }
    public float Speed { get => speed; set => speed = value; }
    public float AttackDistance { get => attackDistance; set => attackDistance = value; }
    public bool AtaqueFinalizado { get => ataqueFinalizado; set => ataqueFinalizado = value; }
    public Transform HitBox { get => hitBox; set => hitBox = value; }
    #endregion
    
    //-------------------------------------------------------------------------------

    private void Awake()
    {
        //Obtenemos referencias
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mAudioSource = GetComponent<AudioSource>();
        mCollider = GetComponent<Collider2D>();

        hitBox = transform.Find("HitBox");
    }

    //-------------------------------------------------------------------------------

    private void Start()
    {
        //Creamos un FSM indicando este Script como principal componente
        mFSM = new FSM<MonkEnemyController>(
            //El Estado inicial será IDLE
            new MonkEnemy.MonkIdleState(this)
            );

        // Activamos la máquina de estados
        mFSM.Begin();  
    }

    private void Update()
    {
        posicionRelativa = transform.position - distanciaRelativa;
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
        ataqueFinalizado = true;
    }


}
