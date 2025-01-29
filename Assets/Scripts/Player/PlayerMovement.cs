
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //Velocidad
    [SerializeField]
    private float speed = 4f;
    public int Ataque { get; set; }

    //Referencias a componentes
    private Rigidbody2D mRb;
    private Animator mAnimator;
    private AudioSource mAudioSource;
    private PlayerInput mPlayerInput;

    //Lista de Clips de voz
    [SerializeField] private List<AudioClip> listaVoces;
    [SerializeField] private List<AudioClip> listaGolpes;

    //Direccion de movimiento
    private Vector3 mDirection = Vector3.zero;

    //Transform del Hitbox
    private Transform hitBox;

    //Flags de Estado
    private bool isTalking;
    private bool isAttacking;
    private bool isBeingHurt;

    //-------------------------------------------------------------------------------------------------
    private void Start()
    {
        //Obtenemos referencias a componentes
        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mPlayerInput = GetComponent<PlayerInput>();
        mAudioSource = GetComponent<AudioSource>();

        //Obtenemos referencia al HitBox Hijo
        hitBox = transform.Find("HitBox");

        //Inicializamos Flags
        isTalking = false;
        isAttacking = false;
        isBeingHurt = false;

        //Inicializamos Ataque en 2
        Ataque = 2;

        //Declaramos el Script como Delegado de los Eventos Evento OnConversationStop
        if (ConversationManager.Instance != null)
        {
            ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;
        }
        


        GameManager.Instance.OnPlayerDamage += OnPlayerDamageDelegate;
        GameManager.Instance.OnChangeAttack += OnChangeAttackDelegate;

    }

    //------------------------------------------------------------------------------

    private void OnPlayerDamageDelegate(int damage)
    {
        //Desactivaci�n del Ataque (Porsiacaso)
        DisableHitBox();

        //Actualizamos el Flag de RecibiendoDa�o
        isBeingHurt = true;

        //Activamos el Trigger de Animacion de Da�o
        mAnimator.SetTrigger("Hurt");

        //Reproducimos voz de da�o
        mAudioSource.PlayOneShot(listaVoces[UnityEngine.Random.Range(0, listaVoces.Count - 1)], 0.75f);

    }

    //-------------------------------------------------------------------------------------------------

    private void OnChangeAttackDelegate(int current){
        switch(current){
            case 0:
                Ataque = 2;
                break;
            case 1:
                Ataque = 1;
                break;
            case 2:
                Ataque = 3;
                break;
        }
    }

     //-------------------------------------------------------------------------------------------------

    private void OnConversationStopDelegate()
    {
        //Cambiamos el Mapa de Accion a Player -> Devolvemos la capacidad para moverse y atacar
        mPlayerInput.SwitchCurrentActionMap("Player");
    }

    //-------------------------------------------------------------------------------------------------

    private void Update()
    {
        //Si se est� recibiendo un input de Direccion para el movimiento
        if (mDirection != Vector3.zero)
        {
            //Activmaos el Flag de Animacion para Movimiento
            mAnimator.SetBool("IsMoving", true);

            //Actualizamos los parametros H y V para la animacion
            mAnimator.SetFloat("Horizontal", mDirection.x);
            mAnimator.SetFloat("Vertical", mDirection.y);
        }
        else
        {   //En caso no se est� recibiendo Input de Direccion
            //Desactivamos el Flag de Animacion -> QUIETO
            mAnimator.SetBool("IsMoving", false);
        }
    }

    //-------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {
        //Movemos el RigidBody hacia una direcci�n en base al Input y la velocidad
        mRb.MovePosition(
            transform.position + (mDirection * speed * Time.fixedDeltaTime)
        );
    }

    //-------------------------------------------------------------------------------------------------

    public void OnMove(InputValue value)
    {
        //Almacenamos el input de Direccion para el movimiento
        mDirection = value.Get<Vector2>().normalized;
    }

    public void OnNext(InputValue value)
    {
        //Si se oprime el boton para pasar al siguiente di�logo
        if (value.isPressed)
        {
            //Hacemos que el Manager de COnversacion pase a la siguente conversacion
            ConversationManager.Instance.NextConversation();
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void OnCancel(InputValue value)
    {
        //Si se oprime el boton para detener el di�logo
        if (value.isPressed)
        {
            //Hacemos que el Manager de Conversacion detenga todo
            ConversationManager.Instance.StopConversation();
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void OnChangeAttack(InputValue value)
    {
        //Si se oprime el boton de cambiar arma
        if (value.isPressed)
        {
            switch(GameManager.Instance.currentAttack)
            {
                case 0:
                    GameManager.Instance.currentAttack = 1;
                    break;
                case 1:
                    GameManager.Instance.currentAttack = 2;
                    break;
                case 2:
                    GameManager.Instance.currentAttack = 0;
                    break;                
            }
            //Evento de cambiar ataque
            GameManager.Instance.CambiarAtaque(GameManager.Instance.currentAttack);

        }
    }

    //-------------------------------------------------------------------------------------------------


    public void OnAttack(InputValue value)
    {
        //Si se oprime el boton de Ataque
        if (value.isPressed)
        {
            switch(GameManager.Instance.currentAttack)
            {
                case 0:
                    //Disparamos el Trigger de Attack
                    mAnimator.SetTrigger("Attack");
                    //Activamos el HitBox
                    hitBox.gameObject.SetActive(true);
                    //Reproducimos el sonido de ataque
                    mAudioSource.PlayOneShot(listaGolpes[0], 0.75f);
                    break;
                case 1:
                    //Disparamos el Trigger de Attack
                    mAnimator.SetTrigger("Attack1");
                    //Activamos el HitBox
                    hitBox.gameObject.SetActive(true);
                    //Reproducimos el sonido de ataque
                    mAudioSource.PlayOneShot(listaGolpes[1], 0.75f);
                    break;
                 case 2:
                    //Disparamos el Trigger de Attack
                    mAnimator.SetTrigger("Attack2");
                    //Activamos el HitBox
                    hitBox.gameObject.SetActive(true);
                    //Reproducimos el sonido de ataque
                    mAudioSource.PlayOneShot(listaGolpes[2], 0.75f);
                    break;               
            }
            //Activamos el Flag de Ataque
            isAttacking = true;
            //Debug
            print(Ataque);
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void OnInteract(InputValue value)
    {
        //Si se oprime el boton de interacci�n; y la iteraccion esta habilitada
        if (value.isPressed)
        {
            if (GameManager.Instance.InteraccionDisponible)
            {
                //Cambiamos el Mapa de Acci�n al de Conversaci�n
                mPlayerInput.SwitchCurrentActionMap("Conversation");

                //Hacemos que el Manager inicie dicha conversacion
                ConversationManager.Instance.StartConversation(GameManager.Instance.ConversacionDisponible);

                //Desactivamos el Flag de Interaccion, pues ya activams el dialogo
                GameManager.Instance.InteraccionDisponible = false;
            }
            else
            {
                //Reproducimos una de sus voces de manera aleatoria
                mAudioSource.PlayOneShot(listaVoces[UnityEngine.Random.Range(0, listaVoces.Count-1)],0.75f);
            }

        }
    }

    //-------------------------------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D other)
    {
        Conversation conversation;
        //Si el bjeto con el que impactamos posee una Conversacion para mostrar
        if (other.transform.TryGetComponent<Conversation>(out conversation))
        {
            //Almacenamos dicha conversacion en la Variable correspondiente
            GameManager.Instance.ConversacionDisponible = conversation;

            //Activamos el Flag de DialogoDisponible
            GameManager.Instance.InteraccionDisponible = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el atacante es un enemigo comun...
        if (collision.transform.CompareTag("Enemy"))
        {
            //Obtenemos referencia al atacante
            MonkEnemyController atacante = collision.transform.GetComponentInParent<MonkEnemyController>();

            //Si el impacto sucede cuando no estamos atacando
            if (!isAttacking)
            {
                //Invocamos al evento de PlayerDamage ingresando el ataque del enemigo
                GameManager.Instance.PlayerDamage(atacante.Ataque);
            }

            //Si estamos atacando, pero es un ataque impenetrable
            else if (collision.transform.CompareTag("UnestopableAttack"))
            {
                //Invocamos al evento de PlayerDamage ingresando el ataque del enemigo
                GameManager.Instance.PlayerDamage(atacante.Ataque);
            }
        }

        //Si el atacante es el Boss
        else if (collision.transform.CompareTag("Boss")) 
        {
            //Obtenemos referencia al Boss
            BossController atacante = collision.transform.GetComponentInParent<BossController>();

            //Invocamos al evento de daño directamente
            GameManager.Instance.PlayerDamage(atacante.Ataque);
        }
    }
    

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Si el objeto con el que dej� de chocar era un NPC
        if (collision.transform.CompareTag("NPC"))
        {
            //Desactivamos el Flag de Interaccion Disponible
            GameManager.Instance.InteraccionDisponible = false;

            //Devolvemos a Null la referencia de ConversacionDisponible
            GameManager.Instance.ConversacionDisponible = null;
        }
    }

    //------------------------------------------------------------------------------------------

    //Funcion llamada mediante un Evento al final de cada Animacion de Ataque
    public void DisableHitBox()
    {
        //Desactivaci�n de la HitBox
        hitBox.gameObject.SetActive(false);

        //Desactivamos el flag de Ataque
        isAttacking = false;
    }

    //------------------------------------------------------------------------------------------

    public void SetEndHurt()
    {
        isBeingHurt = false;

        //Desactivaci�n del Ataque (Porsiacaso)
        DisableHitBox();
    }
}
