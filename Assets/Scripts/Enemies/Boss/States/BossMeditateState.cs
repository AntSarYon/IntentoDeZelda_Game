using MonkEnemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossMeditateState : FSMState<BossController>
    {
        public BossMeditateState(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    //Si el Enemigo se acercó más de la cuenta
                    return Vector3.Distance(
                        mController.PosicionRelativa,
                        mController.Player.position) <= mController.WakeDistance;
                },

                getNextState: () => {
                    //Ingresmaos al Estado IDDLE
                    return new BossAwakingState(mController);
                }));
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            //Al salir, activará el Flag de Animacion para Transformarse
            mController.MAnimator.SetTrigger("Transform");
        }

        public override void OnUpdate(float deltaTime)
        {

        }

    }
}
