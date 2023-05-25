using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossDyingState : FSMState<BossController>
    {
        public BossDyingState(BossController controller) : base(controller)
        {}

        public override void OnEnter()
        {
            //Disparamos el Trigger para la Animacion de Muerte
            mController.MAnimator.SetTrigger("Death");
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate(float deltaTime)
        {

        }
    }
}

