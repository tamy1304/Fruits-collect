using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;


public class Enemigo : MonoBehaviour
{

    [SerializeField] private Transform[] puntosMov;
    [SerializeField] private float velocidad;
    [SerializeField] GameObject padre;
    [SerializeField] private GameObject parte1;
    
    private PolygonCollider2D boxColi;
    private SpriteRenderer spr1;


    private int i=0;

    private Vector3 escalaIni, escalaTemp;
    private float miraDer = 1;

    // Start is called before the first frame update
    void Start()
    {
        escalaIni = transform.localScale;
        boxColi = parte1.GetComponent<PolygonCollider2D>();
        spr1 = parte1.GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntosMov[i].transform.position, velocidad * Time.deltaTime);
        if(Vector2.Distance(transform.position, puntosMov[i].transform.position) < 0.1f)
        {
            if (puntosMov[i] != puntosMov[puntosMov.Length - 1]) i++;
            else i = 0;
            miraDer = Mathf.Sign(puntosMov[i].transform.position.x - transform.position.x);
            gira(miraDer);

        }
    }

    private void gira(float lado)
    {
        if (miraDer == -1)
        {
            escalaTemp = transform.localScale;
            escalaTemp.x = escalaTemp.x * -1;
        }
        else escalaTemp = escalaIni;
        transform.localScale = escalaTemp;
    }
     
    public void muere()
    {
        boxColi.enabled = false;
        StartCoroutine("FadeOut");

    }

    IEnumerator FadeOut()
    {
        for (float f = 1f;  f >= 0; f -= 0.2f)
        {
            Color c1 = spr1.material.color;
            c1.a  = f;
            spr1.material.color = c1;
            yield return new WaitForSeconds(0.025f);
        }
            

        Destroy(padre);
    }

}
