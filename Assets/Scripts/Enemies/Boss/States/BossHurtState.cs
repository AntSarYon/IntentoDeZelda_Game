using MonkEnemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossHurtState : FSMState<BossController>
    {
        public BossHurtState(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {

                    //Cuando el recibiminto de Daño termine
                    return mController.HitFinalizado;
                },

                //Construccion del Sigueinte Estado, en caso se cumpla la validación anterior
                getNextState: () => {

                    //Retornamos el Estado IDLE 
                    return new BossIdleState(mController);

                }));

            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {

                    //Cuando ya no le quede vida
                    return mController.Vivo == false;
                },

                //Construccion del Sigueinte Estado, en caso se cumpla la validación anterior
                getNextState: () => {

                    //Retornamos el Estado IDLE 
                    return new BossDyingState(mController);

                }));
        }

        public override void OnEnter()
        {
            //Disparamos el Trigger para la Animacion de Daño
            mController.MAnimator.SetTrigger("TakeHit");
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            ControlarOrientacionEnX();
        }

        //----------------------------------------------------------------------
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

