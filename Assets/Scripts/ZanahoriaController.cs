using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanahoriaController : MonoBehaviour
{
    private bool activa = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && activa)
        {
            GameController.sumaZanahoria();
            Destroy(gameObject);
            activa = false;
        }
            
    }
}
