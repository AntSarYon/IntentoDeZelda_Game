using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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
    [SerializeField] private AudioClip[] clipsGolpes = new AudioClip[2];
    [SerializeField] private AudioClip clipTierra;
    [SerializeField] private AudioClip clipMuriendo;

    //Variables parametros
    [SerializeField] private float wakeDistance = 3.5f;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float attackDistance = 0.50f;

    [SerializeField] private int vida = 3;  //3 ataques para morir
    [SerializeField] private int ataque = 1; //Atque quita 1 corazon

    //Posicion del Sprite en relacion con el centro del GameObject
    private Vector3 posicionRelativa;
    private Vector3 distanciaRelativa = new Vector3(0, 0.86f, 0);

    //Flags de Estado
    private bool ataqueFinalizado = false;
    private bool hitFinalizado = false;
    private bool vivo = true;
    private bool beingHit = false;

    //Variable de referencia al jugador
    [SerializeField] private Transform player;

    //Referencia al HitBox
    private Transform hitBox;
    private Transform unestopableBox;

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
    public int Ataque { get => ataque; set => ataque = value; }
    public Transform UnestopableBox { get => unestopableBox; set => unestopableBox = value; }
    public bool HitFinalizado { get => hitFinalizado; set => hitFinalizado = value; }
    public bool Vivo { get => vivo; set => vivo = value; }
    public bool BeingHit { get => beingHit; set => beingHit = value; }
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

        player = GameObject.Find("Player").transform;

        //Obtenemos referencia al transform del HitBox
        hitBox = transform.Find("HitBox");
        unestopableBox = transform.Find("UnestopableBox");
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

        //Desactivamos los HitBoxes
        HitBox.gameObject.SetActive(false);
        UnestopableBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Actualizamos constantemente la PosicionRelativa
        posicionRelativa = transform.position - distanciaRelativa;
        
        //Si la Vida del Enemigo llega a 0
        if (vida <= 0)
        {
            //Desactivamos el Flag de Vivo -> "MURIO"
            vivo = false;
        }
    }

    //------------------------------------------------------------------

    private void FixedUpdate()
    {
        //Le pasamos el FixedDeltaTime para no Afectar el rendimiento
        mFSM.Tick(Time.fixedDeltaTime);
    }

    //-------------------------------------------------------------------

    public void ReducirVida(int ataqueRecibido)
    {
        //Reducimos la vida en base al AtaqueRecibido
        vida -= ataqueRecibido;

        //Activamos el flag de Golpe recibido
        beingHit = true;
        hitFinalizado = false;
    }

    //-------------------------------------------------------------------------
    // Funcion Evento para indicar el Fin del Ataque tras completar la animacion
    public void SetAttackingEnd()
    {
        //Activamos el Flag de AtaqueFinalizado
        ataqueFinalizado = true;
    }

    //-------------------------------------------------------------------------
    // Funcion Evento para indicar el Fin del Recibimiento de Daño
    public void SetHitEnd()
    {   
        //Activamos el Flag de Daño dinalizado
        hitFinalizado = true;

        //Desactivamos el Flag de "Golpeado"
        beingHit = false;
    }

    public void MorirYDestruir()
    {
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------

    public void ReproducirGolpe()
    {
        //Reproducimos uno de los golpes de forma aleatoria
        mAudioSource.PlayOneShot(clipsGolpes[UnityEngine.Random.Range(0, 1)], 0.75f);
    }

    //----------------------------------------------------------------------------

    public void ReproducirAtaqueDeTierra()
    {
        //Reproducimos el sonido de ataque de Tierra
        mAudioSource.PlayOneShot(clipTierra, 0.75f);
    }

    //--------------------------------------------------------------------------
    public void ReproducirMuerte()
    {
        //Reproducimos el sonido de ataque de Tierra
        mAudioSource.PlayOneShot(clipMuriendo, 0.60f);
    }
}
