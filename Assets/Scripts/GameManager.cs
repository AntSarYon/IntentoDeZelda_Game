using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int corazonesJugador;

    private bool interaccionDisponible;
    private Conversation conversacionDisponible;

    //EVENTOS
    public event UnityAction<int> OnPlayerDamage;

    //GETTERS Y SETTERS
    public bool InteraccionDisponible { get => interaccionDisponible; set => interaccionDisponible = value; }
    public Conversation ConversacionDisponible { get => conversacionDisponible; set => conversacionDisponible = value; }

    //------------------------------------------------------------

    private void Awake()
    {
        ControlarUnicaInstancia();

        //Inicializamos las vidas del jugador a 6
        corazonesJugador = 6;
    }

    //-----------------------------------------------------------------

    public void PlayerDamage(int damage)
    {
        //Disparamos el eventoDamage con el Daño correspondiente que recibe el jugador
        OnPlayerDamage?.Invoke(damage);
    }

    //--------------------------------------------------

    private void ControlarUnicaInstancia()
    {
        //Si no hay una Instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
