using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//Definimos la Clase Interaction que será utilizada por este Script
public class Interaction
{
    public Sprite CharacterImage;
    public string CharacterName;
    public string CharacterText;
}

//-------------------------------------------------------------------------------------------------
//Una conversación se componde de una o varias interacciones entre el PLayer y el NPC
//Estas interacciones equivalen a cada cuadro de dialogo.}

public class Conversation : MonoBehaviour
{
    //La conversacion se compone basicamente por una Lista de Interacciones (Cuadros de dialogo)
    public List<Interaction> Interactions;
}