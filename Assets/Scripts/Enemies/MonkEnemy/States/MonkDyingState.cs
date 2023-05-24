using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkEnemy
{
    public class MonkDyingState : FSMState<MonkEnemyController>
    {
        public MonkDyingState(MonkEnemyController controller) : base(controller)
        {
        }

        //-----------------------------------------------------
        // Desde aqui controlamos al NPC utilizando mController
        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

    }
}

