using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkEnemy
{
    public class MonkMoveState : FSMState<MonkEnemyController>
    {
        //Direccion en que debera moverse el NPC
        private Vector3 moveDirection;

        //-------------------------------------------------------------------------

        public MonkMoveState(MonkEnemyController controller) : base(controller)
        {
            //Ya se asignó el controlador y se creó la Lista vacia de transiciones
            //Ahora corresponde crearlas y añadirlas indicand el siguiente Estado

            Transitions.Add(new FSMTransition<MonkEnemyController>(
                isValid: () => {

                    //Si la distancia entre el NPC y el jugador es mayor a la distancia minima para despertar
                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) >= mController.WakeDistance;
                },

                //Construccion del Sigueinte Estado, en caso se cumpla la validación anterior
                getNextState: () => {

                    //Retornamos el Estado IDLE usando su Constructor, e ingresando el controller que asignamos
                    return new MonkIdleState(mController);

                }));

            Transitions.Add(new FSMTransition<MonkEnemyController>(
                isValid: () =>{

                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) <= mController.AttackDistance;
                },

                getNextState: () =>
                {
                    return new MonkAttackState(mController);
                }));
        }

        //-----------------------------------------------------
        // Desde aqui controlamos al NPC utilizando mController
        public override void OnEnter()
        {
            //Activamos el Flag de Animacion de Movimiento
            mController.MAnimator.SetBool("IsMoving", true);

            //Iniciamos con el Flag de Ataque Finalizado para prevenir errores
            mController.AtaqueFinalizado = false;
        }

        public override void OnExit()
        {

        }

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
