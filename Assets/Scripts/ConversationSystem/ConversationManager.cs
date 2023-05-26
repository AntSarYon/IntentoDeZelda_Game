using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConversationManager : MonoBehaviour
{
    //Definimos INSTANCIA
    public static ConversationManager Instance {private set; get;}

    //Definimos Eventos para controlar la Conversacion
    public event UnityAction<Interaction> OnConversationStart;  // Recibirá como prámetro una Interacction
    public event UnityAction<Interaction> OnConversationNext;   // Recibirá como prámetro una Interacction
    public event UnityAction OnConversationStop;

    //Definimos un indice inicial para la lista de Interacciones
    private int mInteractionIndex = 0;

    //Referencia a la Conversacion en turno -> Inicializada en Null
    private Conversation mActiveConversation = null;

    //---------------------------------------------------------------------

    private void Awake()
    {
        //Asignamos el Script como Instancia
        Instance = this;
        DontDestroyOnLoad(this);
    }

    //---------------------------------------------------------------------

    public void StartConversation(Conversation conversation)
    {
        //Asignamos la Conversacion ingresada como Conversacion en turno
        mActiveConversation = conversation; 

        //Invocamos al Evento de Inicial Conversacion indicando la interacción de inicio 
        //en base a la lista de interacciones, y el índice correspondiente.

        OnConversationStart?.Invoke(
            mActiveConversation.Interactions[mInteractionIndex++]

            //Luego de invocar la interaccion, incrementamos el Indice de interaccion en 1
            // -> Apunta a la supuesta siguiente interaccion de la lista
        );
    }

    //---------------------------------------------------------------------

    public void NextConversation()
    {
        //Si el indice de interaccion se mantiene dentro del tamaño de la lista
        if (mInteractionIndex < mActiveConversation.Interactions.Count)
        {
            //Invocamos al evento de Siguiente interaccion, ingresando la
            //interaccion correspondiete en base al índice

            OnConversationNext?.Invoke(
                mActiveConversation.Interactions[mInteractionIndex++]

                //Incrementamos el índice para que apunte a la supuesta siguiente interaccion en lISTA
            );

        //Si el indice de interaccion excede el tamaño de la lista
        }else
        {
            //Detenemos la conversacion
            StopConversation();
        }
    }

    //----------------------------------------------------

    public void StopConversation()
    {
        //Asignamos a Null la referencia de Conversacion en turno
        mActiveConversation = null;

        //Devolvemos el indice de interaccion a 0
        mInteractionIndex = 0;

        //Invocamos al Evento de StopConversation --> Hará que  la UI oculte el cuadro de dialogo
        OnConversationStop?.Invoke();
    }
}
