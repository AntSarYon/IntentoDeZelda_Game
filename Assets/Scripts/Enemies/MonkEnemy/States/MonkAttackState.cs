using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkEnemy
{
    public class MonkAttackState : FSMState<MonkEnemyController>
    {
        public MonkAttackState(MonkEnemyController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<MonkEnemyController>(
                isValid: () => {
                    //Si ya se ha finzalizado el ataque
                    return mController.AtaqueFinalizado;
                },
                
                getNextState: () => {
                    //Retornamos al Estado IDDLE
                    return new MonkIdleState(mController);
                }));
        }

        public override void OnEnter()
        {
            //Disparamos el trigger de Ataque
            mController.MAnimator.SetTrigger("Attack");

            //Activamos el HitBox
            mController.HitBox.gameObject.SetActive(true);
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            //ControlarOrientacionEnX();
        }

        //-----------------------------------------------------------------------------
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

