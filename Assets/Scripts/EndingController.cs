using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    [SerializeField] private GameObject[] arrBosses = new GameObject[3];
    [SerializeField] private AudioSource bmAudioSource;

    [SerializeField] private AudioSource linkinAudioSource;

    private bool yaEjecutado;
    private bool iniciarFinal;

    //---------------------------------------------------------------------

    private void Awake()
    {
        iniciarFinal = false;
        yaEjecutado = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Si los 3 Bosses estan desactivados (muertos)
        if (arrBosses[0].activeSelf == false &&
            arrBosses[1].activeSelf == false &&
            arrBosses[2].activeSelf == false && !yaEjecutado)
        {
            iniciarFinal=true;
        }

        if (iniciarFinal)
        {
            bmAudioSource.Stop();
            linkinAudioSource.Play();

            //Actualizamos Flags para no volver a caer en la ejecucion
            iniciarFinal = false;
            yaEjecutado = true;

            StartCoroutine(PasarACreditos(2));
        }
    }

    private IEnumerator PasarACreditos(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Credits");

        StopAllCoroutines();
    }
}
