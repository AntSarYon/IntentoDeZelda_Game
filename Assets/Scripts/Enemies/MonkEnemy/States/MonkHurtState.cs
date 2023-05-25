using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonkEnemy
{
    public class MonkHurtState : FSMState<MonkEnemyController>
    {
        public MonkHurtState(MonkEnemyController controller) : base(controller)
        {
            //Ya se asignó el controlador y se creó la Lista vacia de transiciones
            //Ahora corresponde crearlas y añadirlas indicand el siguiente Estado

            Transitions.Add(new FSMTransition<MonkEnemyController>(
                isValid: () => {

                    //Cuando el recibiminto de Daño termine
                    return mController.HitFinalizado;
                },

                //Construccion del Sigueinte Estado, en caso se cumpla la validación anterior
                getNextState: () => {

                    //Retornamos el Estado IDLE 
                    return new MonkIdleState(mController);

                }));
        }

        //-----------------------------------------------------
        // Desde aqui controlamos al NPC utilizando mController
        public override void OnEnter()
        {
            //Disparamos el Trigger para la Animacion de Daño
            mController.MAnimator.SetTrigger("Hurt");
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate(float deltaTime)
        {

        }
    }

}
