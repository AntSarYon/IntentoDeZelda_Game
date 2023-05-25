using MonkEnemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Boss
{
    public class BossSimpleAttackState : FSMState<BossController>
    {
        private int indexAtaque;

        public BossSimpleAttackState(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    //Si ya se ha finzalizado el ataque
                    return mController.AtaqueSimpleFinalizado;
                },

                getNextState: () => {
                    //Retornamos al Estado IDDLE
                    return new BossIdleState(mController);
                }));

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
            //Disparamos uno de los eventos de Ataque de forma aleatoria
            indexAtaque = UnityEngine.Random.Range(1, 4);
            //Activamos el UnestopableBox
            mController.UnestopableBox.gameObject.SetActive(true);
            //Disparamos el Trigger para la Animacion de Ataque
            mController.MAnimator.SetTrigger($"SimpleAttack{indexAtaque}");

            //Incrementamos el contador de Ataques ejecutados
            mController.AtaquesEjecutados++;
        }

        public override void OnExit()
        {
            //Desactivamos el HitBox
            mController.UnestopableBox.gameObject.SetActive(false);
        }

        public override void OnUpdate(float deltaTime)
        {

        }
    }
}