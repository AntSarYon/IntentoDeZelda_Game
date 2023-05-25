using MonkEnemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Boss
{
    public class BossMoveState : FSMState<BossController>
    {
        //Direccion en que debera moverse el NPC
        private Vector3 moveDirection;
        public BossMoveState(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {

                    //Si la distancia entre el Boss y el jugador es mayor a la distancia minima para despertar
                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) >= mController.FollowDistance;
                },

                //Construccion del Sigueinte Estado, en caso se cumpla la validación anterior
                getNextState: () => {

                    //Retornamos el Estado IDLE usando su Constructor, e ingresando el controller que asignamos
                    return new BossIdleState(mController);

                }));

            //--------------------------
            //TRANSICION AL simple attack
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {

                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) <= mController.AttackDistance &&
                        Mathf.Abs(mController.PosicionRelativa.y - mController.Player.position.y) < 0.75f &&
                        !mController.EspecialDisponible;
                },

                getNextState: () =>
                {
                    return new BossSimpleAttackState(mController);
                }));

            //--------------------------
            //TRANSICION AL ESTADO "HURT"
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () =>
                {
                    //Si el Flag de Recibir Daño se activó
                    return mController.BeingHit;
                },

                getNextState: () =>
                {
                    return new BossHurtState(mController);
                }));

            //--------------------------
            //TRANSICION AL ESTADO "special AttaCK"
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () =>
                {
                    //Si la distancia entre el NPC y el jugador es menor a la distancia minima de ataque
                    return mController.EspecialDisponible;
                },

                getNextState: () =>
                {
                    return new BossSpecialAttack(mController);
                }));
        }

        public override void OnEnter()
        {
            //Activamos el Flag de Animacion de Movimiento
            mController.MAnimator.SetBool("IsMoving", true);

            //Iniciamos con el Flag de Ataque Finalizado para prevenir errores
            mController.AtaqueSimpleFinalizado = false;
            mController.AtaqueEspecialFinalizado = false;
        }
        //--------------------------------------------------
        public override void OnExit()
        {

        }
        //---------------------------------------------------
        public override void OnUpdate(float deltaTime)
        {
            //Obtenemos la direccion de Movimiento en base a la posicion
            //del enemigo y la del jugador

            var playerPosition = mController.Player.transform.position;
            var enemyPosition = mController.PosicionRelativa;

            moveDirection = (playerPosition - enemyPosition).normalized;

            //Si hay una direccion a la cual moverse
            if (moveDirection != Vector3.zero)
            {
                //Controlamos la orientacion del Sprite en X
                ControlarOrientacionEnX();
            }

            //Ejecutamos el Movimiento hacia el jugador
            mController.MRb.MovePosition(
                mController.transform.position + (moveDirection * mController.Speed * deltaTime)
            );
        }

        //*******************************************************************************
        private void ControlarOrientacionEnX()
        {
            //Controlamos hacia donde estará mirando el Jugador
            if (mController.Player.position.x < mController.transform.position.x)
            {
                mController.transform.localScale = new Vector3(
                    -1,
                    mController.transform.localScale.y,
                    mController.transform.localScale.z);

            }
            else
            {
                mController.transform.localScale = new Vector3(
                    1,
                    mController.transform.localScale.y,
                    mController.transform.localScale.z);

            }
        }

    }
}