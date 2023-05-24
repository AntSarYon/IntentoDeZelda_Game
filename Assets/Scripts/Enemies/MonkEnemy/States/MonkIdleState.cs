using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace MonkEnemy
{
    public class MonkIdleState : FSMState<MonkEnemyController>
    {
        public MonkIdleState(MonkEnemyController meController) : base(meController)
        {
            //Ya se asignó el controlador y se creó la Lista vacia de transiciones
            //Ahora corresponde crearlas y añadirlas indicand el siguiente Estado

            //TRANSICION AL ESTADO "MOVE"
            Transitions.Add(new FSMTransition<MonkEnemyController>(
                isValid: () => {

                    //Si la distancia entre el NPC y el jugador es menor a la distancia minima
                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) <= mController.WakeDistance;
                },

                //Construccion del Sigueinte Estado, en caso se cumpla la validación anterior
                getNextState: () => {

                    //Retornamos el EstadoMoving usando su Constructor, e ingresando el controller que asignamos
                    return new MonkMoveState(mController);

                }));

            //TRANSICION AL ESTADO "ATTACK"
            Transitions.Add(new FSMTransition<MonkEnemyController>(
                isValid: () => {

                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) < mController.AttackDistance;
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
            //Desactivamos el Flag de animacion de Movimiento
            mController.MAnimator.SetBool("IsMoving", false);
            
            //Desactivamos el Flag de Ataque concluido (PORSIACASO)
            mController.AtaqueFinalizado = false;
        }

        //-------------------------------------------------------------------------------

        public override void OnExit()
        {

        }

        //-------------------------------------------------------------------------------

        public override void OnUpdate(float deltaTime)
        {
            ControlarOrientacionEnX();
        }

        //***********************************************************************************

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

