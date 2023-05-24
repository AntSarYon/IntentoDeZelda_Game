using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void EmpezarJuego()
    {
        //Cargamos la Escena de MainScene
        SceneManager.LoadScene("MainScene");
    }
}
