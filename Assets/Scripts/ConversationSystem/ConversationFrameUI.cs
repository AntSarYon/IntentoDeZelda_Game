using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationFrameUI : MonoBehaviour
{

    private void Start()
    {
        //Declaramos el Script como Delegado de los Eventos de Conversacion
        ConversationManager.Instance.OnConversationStart += OnConversationStartDelegate;
        ConversationManager.Instance.OnConversationNext += OnConversationNextDelegate;
        ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;

        //Desactivamos el Objeto de UI, pues inicialmente no existirá ningún dialogo abierto
        transform.gameObject.SetActive(false);
    }

    //------------------------------------------------------------------------------------
    // EVENTOS DELEGADOS

    private void OnConversationStartDelegate(Interaction interaction)
    {
        //Activamos el Cuadro de Dialogo de la UI
        transform.gameObject.SetActive(true);

        //Actualizmaos los datos de la (primera) interaccion
        ShowInteraction(interaction);
    }

    private void OnConversationNextDelegate(Interaction interaction)
    {
        //Actualizamos el cuadro con los daos de la siguiente interaccion en lista
        ShowInteraction(interaction);
    }

    private void OnConversationStopDelegate()
    {
        //Desactivamos el Objeto de UI
        transform.gameObject.SetActive(false);
    }

    //--------------------------------------------------------------------------------------

    private void ShowInteraction(Interaction interaction)
    {
        //Actualizamos los Objetos de UI que componen el Cuadro de dialogo en base a la interacción en turno
        //Para esto, realizamos una Busqueda en los Hijos del Panel
        transform
            .Find("CharacterImage")
            .Find("Image")
            .GetComponent<Image>().sprite = interaction.CharacterImage;
        transform
            .Find("CharacterName")
            .GetComponent<TextMeshProUGUI>().text = interaction.CharacterName;
        transform
            .Find("CharacterText")
            .GetComponent<TextMeshProUGUI>().text = interaction.CharacterText;

    }

    //-------------------------------------------------------------------------------------------------




}
