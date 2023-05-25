using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace MonkEnemy
{
    public class MonkAttackState : FSMState<MonkEnemyController>
    {
        private int indexAtaque;

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

            Transitions.Add(new FSMTransition<MonkEnemyController>(
                isValid: () =>
                {
                    //Si el Flag de Recibir Daño se activó
                    return mController.BeingHit;
                },

                getNextState: () =>
                {
                    return new MonkHurtState(mController);
                }));
        }

        //-------------------------------------------------------------------

        public override void OnEnter()
        {
            //Disparamos uno de los eventos de Ataque de forma aleatoria
            indexAtaque = UnityEngine.Random.Range(1, 4);
            if (indexAtaque == 3)
            {
                //Activamos el UnestopableBox
                mController.UnestopableBox.gameObject.SetActive(true);
            }
            //Activamos el HitBox
            mController.HitBox.gameObject.SetActive(true);
            
            //Disparamos el Trigger para la Animacion de Ataque
            mController.MAnimator.SetTrigger($"Attack{indexAtaque}");

            
        }
        // - - - - - - - - - - - - - - - - - - - - - - -
        public override void OnExit()
        {
            //Desactivamos los Objetos de HitBox
            mController.HitBox.gameObject.SetActive(false);
            mController.UnestopableBox.gameObject.SetActive(false);
        }

        // - - - - - - - - - - - - - - - - - - - - - - -

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

