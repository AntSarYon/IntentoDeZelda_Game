using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //Velocidad
    [SerializeField]
    private float speed = 4f;

    //Referencias a componentes
    private Rigidbody2D mRb;
    private Animator mAnimator;
    private AudioSource mAudioSource;
    private PlayerInput mPlayerInput;

    //Direccion de movimiento
    private Vector3 mDirection = Vector3.zero;

    //Transform del Hitbox
    private Transform hitBox;

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

        //Declaramos el Script como Delegado del Evento OnConversationStop
        ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;
        
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
        //Si se está recibiendo un input de Direccion para el movimiento
        if (mDirection != Vector3.zero)
        {
            //Activmaos el Flag de Animacion para Movimiento
            mAnimator.SetBool("IsMoving", true);

            //Actualizamos los parametros H y V para la animacion
            mAnimator.SetFloat("Horizontal", mDirection.x);
            mAnimator.SetFloat("Vertical", mDirection.y);
        }
        else
        {   //En caso no se esté recibiendo Input de Direccion
            //Desactivamos el Flag de Animacion -> QUIETO
            mAnimator.SetBool("IsMoving", false);
        }
    }

    //-------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {
        //Movemos el RigidBody hacia una dirección en base al Input y la velocidad
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
        //Si se oprime el boton para pasar al siguiente diálogo
        if (value.isPressed)
        {
            //Hacemos que el Manager de COnversacion pase a la siguente conversacion
            ConversationManager.Instance.NextConversation();
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void OnCancel(InputValue value)
    {
        //Si se oprime el boton para detener el diálogo
        if (value.isPressed)
        {
            //Hacemos que el Manager de Conversacion detenga todo
            ConversationManager.Instance.StopConversation();
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void OnAttack(InputValue value)
    {
        //Si se oprime el boton de Ataque
        if (value.isPressed)
        {
            //Disparamos el Trigger de Attack
            mAnimator.SetTrigger("Attack");

            //Activamos el HitBox
            hitBox.gameObject.SetActive(true);

            //Reproducimos el sonido de ataque
            mAudioSource.Play();
        }
    }

    //-------------------------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Definimos variable para almacenar una posible conversacion
        Conversation conversation;

        //Si ek bjeto con el que impactamos posee una Conversacion para mostrar
        if (other.transform.TryGetComponent<Conversation>(out conversation))
        {
            //Cambiamos el Mapa de Acción al de Conversación
            mPlayerInput.SwitchCurrentActionMap("Conversation");

            //Hacemos que el Manager inicie dicha conversacion
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void DisableHitBox()
    {
        //Desactivación de la HitBox
        hitBox.gameObject.SetActive(false);
    }
}
