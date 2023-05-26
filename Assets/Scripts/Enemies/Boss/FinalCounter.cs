using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCounter : MonoBehaviour
{
    public int flagMuerto; 

    void Start()
    {
        flagMuerto=0;
    }

    public void AumentarFlag()
    {
        flagMuerto = 1;
    }
}
