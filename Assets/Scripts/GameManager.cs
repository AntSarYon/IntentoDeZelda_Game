using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Variables para controlar Vida del jugador
    private bool jugadorVivo;
    private int corazonesJugador;
    public int currentAttack;

    //Variables para controlar la sinteraaciones y cuadros de dialogo
    private bool interaccionDisponible;
    private Conversation conversacionDisponible;

    //EVENTOS
    public event UnityAction<int> OnPlayerDamage;
    public event UnityAction<int> OnChangeAttack;

    //GETTERS Y SETTERS
    public bool InteraccionDisponible { get => interaccionDisponible; set => interaccionDisponible = value; }
    public Conversation ConversacionDisponible { get => conversacionDisponible; set => conversacionDisponible = value; }
    public int CorazonesJugador { get => corazonesJugador; set => corazonesJugador = value; }

    //------------------------------------------------------------

    private void Awake()
    {
        ControlarUnicaInstancia();

        //ataque principal
        currentAttack = 0;

        //Inicializamos las vidas del jugador a 6
        corazonesJugador = 6;

        //Inicializamos el Flag de jugador Vivo como True
        jugadorVivo = true;
    }

    //-----------------------------------------------------------------

    private void Update()
    {
        ControlarEstadoDelJugador();

        //Mientras el jugador este vivo...
        if (jugadorVivo)
        {
            //FLUJO DE JUEGO
        }

        else
        {
            //GAME OVER

        }
    }

    public void PlayerDamage(int damage)
    {
        //Disparamos el eventoDamage con el Da�o correspondiente que recibe el jugador
        OnPlayerDamage?.Invoke(damage);
    }

    //-------------------------------------------------------

    private void ControlarEstadoDelJugador()
    {
        if (corazonesJugador == 0)
        {
            jugadorVivo = false;
        }
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

    //--------------------------------------------------

    //Cambiar de ataque al presionar CTRL
    public void CambiarAtaque(int current)
    {
        OnChangeAttack?.Invoke(current);
    }

}
