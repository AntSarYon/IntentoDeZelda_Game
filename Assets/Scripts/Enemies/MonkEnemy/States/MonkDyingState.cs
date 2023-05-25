using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkEnemy
{
    public class MonkDyingState : FSMState<MonkEnemyController>
    {
        public MonkDyingState(MonkEnemyController controller) : base(controller)
        {}

        //-----------------------------------------------------
        // Desde aqui controlamos al NPC utilizando mController
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

