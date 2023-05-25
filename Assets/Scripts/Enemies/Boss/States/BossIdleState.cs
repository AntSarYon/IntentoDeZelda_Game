using MonkEnemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossIdleState : FSMState<BossController>
    {
        public BossIdleState(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    //Si el Enemigo se acercó más de la cuenta
                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) <= mController.FollowDistance;
                },

                getNextState: () => {
                    //Ingresmaos al Estado IDDLE
                    return new BossMoveState(mController);
                }));

            //--------------------------
            //TRANSICION AL ESTADO "ATTACK"
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () =>
                {
                    //Si la distancia entre el NPC y el jugador es menor a la distancia minima de ataque
                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) < mController.AttackDistance &&
                        Mathf.Abs(mController.PosicionRelativa.y - mController.Player.position.y) < 0.75f &&
                        !mController.EspecialDisponible;
                },

                getNextState: () =>
                {
                    return new BossSimpleAttackState(mController);
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

        }

        public override void OnEnter()
        {
            //Desactivamos el Flag de animacion de Movimiento
            mController.MAnimator.SetBool("IsMoving", false);

            //Iniciamos con el Flag de Ataque Simpe Finalizado desactivado para prevenir errores
            mController.AtaqueSimpleFinalizado = false;
            mController.AtaqueEspecialFinalizado = false;
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            ControlarOrientacionEnX();
        }

        //---------------------------------------------------------------------------

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

