using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossAwakingState : FSMState<BossController>
    {
        public BossAwakingState(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    //Si el Enemigo se acercó más de la cuenta
                    return mController.Despierto;
                },

                getNextState: () => {
                    //Ingresmaos al Estado IDDLE
                    return new BossIdleState(mController);
                }));
        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnUpdate(float deltaTime)
        {

        }

    }
}
