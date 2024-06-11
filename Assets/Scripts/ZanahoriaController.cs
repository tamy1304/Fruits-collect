using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanahoriaController : MonoBehaviour
{
    private bool activa = true;
    private ParticleSystem particulas;
    private SpriteRenderer spr;

    private void Awake()
    {
        particulas = GetComponent<ParticleSystem>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if (collision.gameObject.tag == "Player" && activa)
        {
            GameController.sumaZanahoria();
            spr.enabled = false;
            particulas.Play();
            //Destroy(gameObject);
            activa = false;
        }
            
    }
}
