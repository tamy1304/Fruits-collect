using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class ZanahoriaController : MonoBehaviour
{
    private bool activa = true;
    private ParticleSystem particulas;
    private SpriteRenderer spr;

    private void Awake()
    {
        particulas = GetComponent<ParticleSystem>();
        spr = GetComponent<SpriteRenderer>();
        GameController.respawn += Respawn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && activa)
        {
            GameController.sumaZanahoria();
            spr.enabled = false;
            particulas.Play();
            activa = false;
        }

    }
    void Respawn()
    {
        activa = true;
        gameObject.SetActive(true);
        spr.enabled = true;

    }

    private void OnDestroy()
    {
        GameController.respawn -= Respawn;

    }
}
