using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MovingState : FSMState<EnemyController>
    {
        //Definimos las Variables adicionales que necesitemos en cada
        //determinado Estado
        private Vector3 mDirection;

        public MovingState(EnemyController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<EnemyController>(
                isValid : () => { // <-- Definición mediante LAMBDA

                    //Comprueba si la Distancia entre el Enemigo y
                    //el Player es la mayor a la ncesaria para Despertar
                    return Vector3.Distance(
                        mController.transform.position,
                        mController.Player.transform.position
                    ) >= mController.WakeDistance;
                },

                //Funcion que retorna el Sigueinte Estado, en casos e cumpla la validación anterior
                getNextState: () => { // <-- Definición mediante LAMBDA

                    //Retornamos el EstadoAttacking usando su Constructor
                    return new IdleState(mController);
                }
            ));

            Transitions.Add(new FSMTransition<EnemyController>(
                isValid : () => { // <-- Definición mediante LAMBDA

                    //Comprueba si la Distancia entre el Enemigo y
                    //el Player es la mínima para Atacar
                    return Vector3.Distance(
                        mController.transform.position,
                        mController.Player.transform.position
                    ) <= mController.AttackDistance;
                },

                //Funcion que retorna el Sigueinte Estado, en casos e cumpla la validación anterior
                getNextState: () => { // <-- Definición mediante LAMBDA
                    //Retornamos el EstadoAttacking usando su Constructor
                    return new AttackingState(mController);
                }
            ));
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter MovingState");
            mController.animator.SetBool("IsMoving", true);
        }

        public override void OnExit()
        {
            Debug.Log("OnExit MovingState");
        }

        public override void OnUpdate(float deltaTime)
        {
            //Obtenemos la direccion de Movimiento en base a la posicion
            //del enemigo y la del jugador

            var playerPosition = mController.Player.transform.position;
            var enemyPosition = mController.transform.position;

            mDirection = (playerPosition - enemyPosition).normalized;

            //Si hay una direccion a la cual moverse
            if (mDirection != Vector3.zero)
            {
                //Actualizamos los parametros de Animacion
                mController.animator.SetFloat("Horizontal", mDirection.x);
                mController.animator.SetFloat("Vertical", mDirection.y);
            }

            //Ejecutamos el Movimiento hacia el jugador
            mController.rb.MovePosition(
                mController.transform.position + (mDirection * mController.Speed * deltaTime)
            );
        }
    }
}
