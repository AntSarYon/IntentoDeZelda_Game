using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    #region Public Properties
    public float WakeDistance = 5f;
    public float Speed = 2f;
    public float AttackDistance = 1f;
    #endregion

    #region Components
    public Transform Player;
    public SpriteRenderer spriteRenderer {private set; get;}
    public Rigidbody2D rb { private set; get; }
    public Animator animator { private set; get; }
    
    public bool AttackingEnd { set; get; } = false;
    public Transform hitBox { private set; get; }
    #endregion

    #region Private Properties
    //Tendremos una Maquina de Estados Finita (FSM)
    private FSM<EnemyController> mFSM;
    #endregion

    //-----------------------------------------------------------------

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitBox = transform.Find("HitBox");

        // Creo la maquina de estado finita, indicando el Componente
        // específico del que hará uso
        mFSM = new FSM<EnemyController>(new Enemy.IdleState(this));
        mFSM.Begin();  // prendo la mquina de estados
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
        AttackingEnd = true;
    }
}
