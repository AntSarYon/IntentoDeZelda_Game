using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class BossController : MonoBehaviour
{
    //Referencia a componentes
    [HideInInspector] public Rigidbody2D MRb { get; set; }
    [HideInInspector] public Collider2D MCollider { get; set; }
    [HideInInspector] public SpriteRenderer MSpriteRenderer { get; set; }
    [HideInInspector] public Animator MAnimator { get; set; }
    [HideInInspector] public AudioSource MAudioSource { get; set; }

    //Lista de Clips de Audio que usará el enemigo
    [SerializeField] private AudioClip clipGolpe;
    [SerializeField] private AudioClip clipGolpeTierra;
    [SerializeField] private AudioClip clipMuriendo;

    //Variables parametros
    public float WakeDistance { get; set; } = 5f;
    public float FollowDistance { get; set; } = 3.5f;
    public float Speed { get; set; } = 1f;
    public float AttackDistance { get; set; } = 2.5f;

    [HideInInspector] public int Vida { get; set; } = 15;  //3 ataques para morir
    [HideInInspector] public int Ataque { get; set; } = 1; //Atque quita 1 corazon

    //Contador de Ataques ejecutados
    [HideInInspector] public int AtaquesEjecutados { get; set; } = 0;

    //Flags de Estado
    [HideInInspector] public bool Despierto { get; set; } = false;
    [HideInInspector] public bool AtaqueSimpleFinalizado { get; set; } = false;
    [HideInInspector] public bool AtaqueEspecialFinalizado { get; set; } = false;
    [HideInInspector] public bool HitFinalizado { get; set; } = false;
    [HideInInspector] public bool Vivo { get; set; } = true;
    [HideInInspector] public bool BeingHit { get; set; } = false;

    [HideInInspector] public bool EspecialDisponible { get; set; } = false;

    //Posicion del Sprite en relacion con el centro del GameObject
    [HideInInspector] public Vector3 PosicionRelativa { get; set; }

    private Vector3 distanciaRelativa = new Vector3(0, 0.72f, 0);

    //Variable de referencia al jugador
    [HideInInspector] public Transform Player;

    //Referencia al HitBox
    [HideInInspector] public Transform HitBox { get; set; }
    [HideInInspector] public Transform UnestopableBox { get; set; }

    //Tendremos una Maquina de Estados Finita (FSM)
    private FSM<BossController> mFSM;

    //-----------------------------------------------------------------------

    private void Awake()
    {
        //Obtenemos referencias
        MSpriteRenderer = GetComponent<SpriteRenderer>();
        MRb = GetComponent<Rigidbody2D>();
        MAnimator = GetComponent<Animator>();
        MAudioSource = GetComponent<AudioSource>();
        MCollider = GetComponent<Collider2D>();

        Player = GameObject.Find("Player").transform;

        //Obtenemos referencia al transform del HitBox
        UnestopableBox = transform.Find("UnestopableBox");
    }

    //-----------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //Creamos un FSM indicando este Script como principal componente
        mFSM = new FSM<BossController>(
            //El Estado inicial será IDLE
            new Boss.BossMeditateState(this)
            );

        // Activamos la máquina de estados
        mFSM.Begin();

        //Desactivamos los HitBoxes
        UnestopableBox.gameObject.SetActive(false);
    }

    //-----------------------------------------------------------------------

    // Update is called once per frame
    private void Update()
    {
        //Actualizamos constantemente la PosicionRelativa
        PosicionRelativa = transform.position - distanciaRelativa;

        if (AtaquesEjecutados == 3)
        {
            EspecialDisponible = true;
        }else EspecialDisponible = false;


        //Si la Vida del Enemigo llega a 0
        if (Vida <= 0)
        {
            //Desactivamos el Flag de Vivo -> "MURIO"
            Vivo = false;
        }
    }

    //------------------------------------------------------------------

    private void FixedUpdate()
    {
        //Le pasamos el FixedDeltaTime para no Afectar el rendimiento
        mFSM.Tick(Time.fixedDeltaTime);
    }

    //-----------------------------------------------------------
    public void ReducirVida(int ataqueRecibido)
    {
        //Reducimos la vida en base al AtaqueRecibido
        Vida -= ataqueRecibido;

        //Activamos el flag de Golpe recibido
        BeingHit = true;
        HitFinalizado = false;
    }

    //----------------------------------------------------------------
    public void Despertar()
    {
        Despierto = true;
    }

    //-----------------------------------------------------------------------
    // Funcion Evento para indicar el Fin del Ataque tras completar la animacion
    public void SetSimpleAttackEnd()
    {
        //Activamos el Flag de AtaqueFinalizado
        AtaqueSimpleFinalizado = true;
    }

    public void SetSpecialAttackEnd()
    {
        //Activamos el Flag de AtaqueFinalizado
        AtaqueEspecialFinalizado = true;
    }

    //-------------------------------------------------------------------------
    // Funcion Evento para indicar el Fin del Recibimiento de Daño
    public void SetHitEnd()
    {
        //Activamos el Flag de Daño dinalizado
        HitFinalizado = true;

        //Desactivamos el Flag de "Golpeado"
        BeingHit = false;
    }

    public void MorirYDestruir()
    {
        gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------

    public void ReproducirGolpes()
    {
        //Reproducimos uno de los golpes de forma aleatoria
        MAudioSource.PlayOneShot(clipGolpe, 0.75f);
    }

    public void ReproducirGolpeTierra()
    {
        //Reproducimos uno de los golpes de forma aleatoria
        MAudioSource.PlayOneShot(clipGolpeTierra, 0.75f);
    }

    //----------------------------------------------------------------------------

    public void ReproducirTransformacion()
    {
        //Reproducimos el sonido de ataque de Tierra
        MAudioSource.PlayOneShot(clipMuriendo, 0.60f);
    }

    public void ReproducirExplosion()
    {
        //Reproducimos el sonido de ataque de Tierra
        MAudioSource.PlayOneShot(clipMuriendo, 0.60f);
    }

    public void ReproducirMuerte()
    {
        //Reproducimos el sonido de ataque de Tierra
        MAudioSource.PlayOneShot(clipMuriendo, 0.60f);
    }



    //-----------------------------------------------------------------------



    //-----------------------------------------------------------------------
}
