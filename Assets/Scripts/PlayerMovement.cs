using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    private Rigidbody2D mRb;
    private Vector3 mDirection = Vector3.zero;
    private Animator mAnimator;
    private AudioSource mAudioSource;

    private PlayerInput mPlayerInput;
    private Transform hitBox;

    //-------------------------------------------------------------------------------------------------
    private void Start()
    {
        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mPlayerInput = GetComponent<PlayerInput>();
        mAudioSource = GetComponent<AudioSource>();

        hitBox = transform.Find("HitBox");

        ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;
        
    }

    //-------------------------------------------------------------------------------------------------

    private void OnConversationStopDelegate()
    {
        mPlayerInput.SwitchCurrentActionMap("Player");
    }

    //-------------------------------------------------------------------------------------------------

    private void Update()
    {
        if (mDirection != Vector3.zero)
        {
            mAnimator.SetBool("IsMoving", true);
            mAnimator.SetFloat("Horizontal", mDirection.x);
            mAnimator.SetFloat("Vertical", mDirection.y);
        }else
        {
            // Quieto
            mAnimator.SetBool("IsMoving", false);
        }
    }

    //-------------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {
        mRb.MovePosition(
            transform.position + (mDirection * speed * Time.fixedDeltaTime)
        );
    }

    //-------------------------------------------------------------------------------------------------

    public void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>().normalized;
    }

    public void OnNext(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.NextConversation();
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.StopConversation();
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            mAnimator.SetTrigger("Attack");
            hitBox.gameObject.SetActive(true);
            mAudioSource.Play();
        }
    }

    //-------------------------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D other)
    {
        Conversation conversation;
        if (other.transform.TryGetComponent<Conversation>(out conversation))
        {
            mPlayerInput.SwitchCurrentActionMap("Conversation");
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void DisableHitBox()
    {
        hitBox.gameObject.SetActive(false);
    }
}
