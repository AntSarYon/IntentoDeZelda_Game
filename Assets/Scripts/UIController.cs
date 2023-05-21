using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //Referencia al PlayerMovement del jugador
    [SerializeField] private GameObject interaccionPanel;

    //---------------------------------------------------------------------------
    private void Start()
    {
        //Desactivamos el Objeto de UI, pues inicialmente no existirá ningún dialogo abierto
        interaccionPanel.SetActive(false);
    }

    private void Update()
    {
        //Mostramos o no el Panel de Interaccion en base al Flag del GameManager
        interaccionPanel.SetActive(GameManager.Instance.InteraccionDisponible);
    }
}
