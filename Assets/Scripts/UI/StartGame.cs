
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void EmpezarJuego()
    {
        //Cargamos la Escena de Town
        SceneManager.LoadScene("Town");
    }
}
