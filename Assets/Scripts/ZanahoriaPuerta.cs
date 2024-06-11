using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanahoriaPuerta : MonoBehaviour
{
    [SerializeField] private float velocidad;
    private GameObject destino;
    private Vector3 destinoFix;
    private ParticleSystem particulas;
    private SpriteRenderer spr;

    private bool fin = false;
    // Start is called before the first frame update
    void Start()
    {
        destino = GameObject.FindGameObjectWithTag("ZanahoriaP");
        spr = gameObject.GetComponent<SpriteRenderer>();
        particulas = gameObject.GetComponent<ParticleSystem>();
        destinoFix = new Vector3(destino.transform.position.x, destino.transform.position.y, -1);

    }

    // Update is called once per frame
    void Update()
    {
        if (!fin && GameController.zanahoriasPuerta > 0)
        {

            if (transform.position != destinoFix)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinoFix, velocidad);
            }
            else
            {
                spr.enabled = false;
                particulas.Play();
                fin = true;
                GameController.restaZanahoria();
                GameController.restaZanahoriaPuerta();
                if (GameController.zanahoriasPuerta == 0) Destroy(destino);

            }
        }
    }

    private void OnDestroy()
    {
        GameController.soltandoZanahoria = false;
        if (GameController.zanahoriasPuerta == 0)  GameController.abrePuerta();

    }

}
