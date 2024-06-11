using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void puertaAbierta()
    {
        GameController.permitePaso();
    }
}
