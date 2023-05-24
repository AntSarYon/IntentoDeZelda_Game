using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //Referencia al Panel de Interaccion
    [SerializeField] private GameObject interaccionPanel;

    //Lista que contiene las imagenes de corazones
    [SerializeField] private GameObject[] arrCorazones = new GameObject[6];

    //Referencia al Panel de Ataque elegido
    [SerializeField] private GameObject attackPanel;

    //---------------------------------------------------------------------------
    private void Start()
    {
        //Desactivamos el Objeto de UI, pues inicialmente no existir� ning�n dialogo abierto
        interaccionPanel.SetActive(false);

        //Activamos el objeto de attack panel
        attackPanel.SetActive(true);

        GameManager.Instance.OnPlayerDamage += OnPlayerDamageDelegate;
        GameManager.Instance.OnChangeAttack += OnChangeAttackDelegate;
    }

    private void OnChangeAttackDelegate(int current){
        print (current);
        attackPanel.SetActive(false);
    }

    private void OnPlayerDamageDelegate(int damage)
    {
        //Obtenemos los corazones restantes del jugador
        int corazonesRestantes = GameManager.Instance.CorazonesJugador;
        int corazonesEliminados = 0;

        //Si el da�o es mayor a la cantidad de corazones restantes
        if (damage > corazonesRestantes)
        {
            //Recorremos la lista de corazones (De adelante hacia atras)
            for (int c = arrCorazones.Length - 1; c >= 0; c--)
            {
                //Desactivamos cada Corazon
                arrCorazones[c].SetActive(false);
            }

            //Reducimos el numero de Corazones a 0
            GameManager.Instance.CorazonesJugador = 0;
        }

        //Caso contrario
        else
        {
            //Recorremos la lista de corazones (De adelante hacia atras)
            for (int c = arrCorazones.Length - 1; c >= 0; c--)
            {
                //Si el corazon esta activo...
                if (arrCorazones[c].activeInHierarchy)
                {
                    //Desactivamos el Corazon
                    arrCorazones[c].SetActive(false);

                    //Aumentamos el contador de corazones eliminados
                    corazonesEliminados++;

                    //Si el numero de corazones eliminados equivale al da�o recibido
                    if (corazonesEliminados == damage)
                    {
                        //Terminamos el Bucle
                        break;
                    }
                }
            }
            //Reducimos el numero de corazones
            GameManager.Instance.CorazonesJugador-=corazonesEliminados;
        }

        
    }

    private void Update()
    {
        //Mostramos o no el Panel de Interaccion en base al Flag del GameManager
        interaccionPanel.SetActive(GameManager.Instance.InteraccionDisponible);

        attackPanel.SetActive(GameManager.Instance);
    }
}
