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

                    //Condicion que debe cunplirse para efectuar la transicion
                    return true;
                },

                //Construccion del Sigueinte Estado, en caso se cumpla la validación anterior
                getNextState: () => {

                    //Retornamos el EstadoMoving usando su Constructor, e ingresando el controller que asignamos
                    return new MonkMoveState(mController);

                }));
        }

        //-----------------------------------------------------
        // Desde aqui controlamos al NPC utilizando mController
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
