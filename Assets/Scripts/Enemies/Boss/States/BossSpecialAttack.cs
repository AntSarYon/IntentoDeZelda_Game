
using UnityEngine;

namespace Boss
{
    public class BossSpecialAttack : FSMState<BossController>
    {
        public BossSpecialAttack(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    //Si ya se ha finzalizado el ataque
                    return mController.AtaqueEspecialFinalizado;
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
            mController.UnestopableBox.gameObject.SetActive(true);
            //Disparamos el Trigger para la Animacion de Ataque
            mController.MAnimator.SetTrigger("SpecialAttack");

            //Devolvemos el contador de ataques a 0
            mController.AtaquesEjecutados = 0;
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

