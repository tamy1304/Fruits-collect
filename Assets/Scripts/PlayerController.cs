using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class PlayerController : MonoBehaviour
{
    [Header("VALORES CONFIGURABLES")]
    [SerializeField] private int vida = 3;
    [SerializeField] private float velocidad;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private bool saltoMejorado;
    [SerializeField] private float saltoLargo = 1.5f;
    [SerializeField] private float saltoCorto = 1f;
    [SerializeField] private Transform checkGround;
    [SerializeField] private float checkGroundRadio;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float fuerzaToque;

    [Header("BARRA DE VIDA")]
    [SerializeField] private GameObject barraVida;
    [SerializeField] private Sprite vida3, vida2, vida1, vida0;


        [Header("VALORES INFORMATIVOS")]
    [SerializeField] private bool colPies = false;
    private Rigidbody2D rPlayer;
    private SpriteRenderer sPlayer;
    private CapsuleCollider2D ccPlayer;
    private float h;
    private Animator aPlayer;
    private bool miraDerecha = true;
    private bool saltando = false;
    private bool tocaSuelo = false;
    private bool enPlataforma = false;
    private bool puedoSaltar = false;
    private bool tocado = false;
    private bool muerto = false;
    private Vector2 nuevaVelocidad;
    private Vector3 posIni;
    private Color colorOriginal;
    private Camera camara;
    private float posPlayer, altoCam, altoPlayer;


    // Start is called before the first frame update
    void Start()
    {
        posIni = transform.position;
        rPlayer = GetComponent<Rigidbody2D>();
        aPlayer = GetComponent<Animator>();
        sPlayer = GetComponent<SpriteRenderer>();
        ccPlayer = GetComponent<CapsuleCollider2D>();
        colorOriginal = sPlayer.color;
        camara = Camera.main;

        
        altoCam = camara.orthographicSize * 2;
        altoPlayer = GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameOn)
        {
            recibePulsaciones();
            variablesAnimador();
        }
        if (muerto)
        {
            posPlayer = camara.transform.InverseTransformDirection(transform.position - camara.transform.position).y;
            if (posPlayer < ((altoCam / 2) * -1) - (altoPlayer / 2)){
                Invoke("llamaRecarga", 0.5f);
                muerto = false;
            }
        }
    }

    private void llamaRecarga()
    {
        GameController.playerMuerto = true;
    }

    void FixedUpdate()
    {
        if (GameController.gameOn)
        {
            checkTocaSuelo();
            if (!tocado) movimientoPlayer();
        }

    }

    private void movimientoPlayer()
    {
        if (tocaSuelo && !saltando)
        {
            nuevaVelocidad.Set(velocidad * h, rPlayer.velocity.y);
            rPlayer.velocity = nuevaVelocidad;
        }

        else
        {
            if (!tocaSuelo)
            {
                nuevaVelocidad.Set(velocidad * h, rPlayer.velocity.y);
                rPlayer.velocity = nuevaVelocidad;
            }
        }
    }

    private void recibePulsaciones()
    {
        if (Input.GetKey(KeyCode.R)) GameController.playerMuerto = true;
        h = Input.GetAxisRaw("Horizontal");
        if ((h > 0 && !miraDerecha) || h < 0 && miraDerecha) girar();
        if (Input.GetButtonDown("Jump") && puedoSaltar && tocaSuelo) salto();
        if (saltoMejorado) SaltoMejorado();
    }

    private void salto()
    {
        saltando = true;
        puedoSaltar = false;
        rPlayer.velocity = new Vector2(rPlayer.velocity.x, 0f);
        rPlayer.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
    }

    private void SaltoMejorado()
    {
        if (rPlayer.velocity.y < 0)
        {
            rPlayer.velocity += Vector2.up * Physics2D.gravity.y * saltoLargo * Time.deltaTime;
        }
        else if (rPlayer.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rPlayer.velocity += Vector2.up * Physics2D.gravity.y * saltoCorto * Time.deltaTime;
        }
    }

    private void checkTocaSuelo()
    {

        tocaSuelo = Physics2D.OverlapCircle(checkGround.position, checkGroundRadio, capaSuelo);
        if (rPlayer.velocity.y <= 0f)
        {
            saltando = false;
            if (tocado && tocaSuelo)
            {
                rPlayer.velocity = Vector2.zero;
                tocado = false;
                sPlayer.color = colorOriginal;
            }
        }

        if (tocaSuelo && !saltando)
        {
            puedoSaltar = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma" && !muerto )
        {
            rPlayer.velocity = Vector3.zero;

            transform.parent = collision.transform;
            enPlataforma = true;
            tocaSuelo = true;
        }

        if (collision.gameObject.tag == "Enemigo" && !muerto)
        {
            toque(collision.transform.position.x);
        }

        if (collision.gameObject.tag == "SobreEnemigo" && !tocado && !muerto)
        {
            collision.gameObject.SendMessage("muere");
        }
    }

    private void toque(float posX)
    {
        if (!tocado)
        {
            if (vida > 1)
            {
                Color nuevoColor = new Color(255f / 255f, 153f / 255f, 153f / 255f);
                sPlayer.color = nuevoColor;
                tocado = true;
                float lado = Mathf.Sign(posX - transform.position.x);
                rPlayer.velocity = Vector2.zero;
                rPlayer.AddForce(new Vector2(fuerzaToque * -lado, fuerzaToque), ForceMode2D.Impulse);
                vida--;
                BarraVida(vida);
            }
            else
            {
                muertePlayer();
            }

        }

    }

    private void BarraVida(int salud)
    {
        if(salud == 2) barraVida.GetComponent<Image>().sprite = vida2;
        if(salud == 1) barraVida.GetComponent<Image>().sprite = vida1;
    }


    private void muertePlayer()
    {
        barraVida.GetComponent<Image>().sprite = vida0;
        aPlayer.Play("Muerto");
        GameController.gameOn = false;
        rPlayer.velocity = Vector2.zero;
        rPlayer.AddForce(new Vector2(0.0f, fuerzaSalto), ForceMode2D.Impulse);
        ccPlayer.enabled = false;
        muerto = true;
    } 

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma" && !muerto)
        {
            transform.parent = null;
            enPlataforma = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Pinchos" && !muerto)
        {
            Debug.Log("Quita salud");
            muertePlayer();

        }
        if (collision.gameObject.tag == "CaidaAlVacio")
        {
            Debug.Log("Muerte");
            Invoke("llamaRecarga", 0.5f);
        }
    }
         
    private void variablesAnimador()
    {
        aPlayer.SetFloat("VelocidadX", Mathf.Abs(rPlayer.velocity.x));
        aPlayer.SetFloat("VelocidadY", rPlayer.velocity.y);
        aPlayer.SetBool("Saltando", saltando);
        aPlayer.SetBool("TocaSuelo", tocaSuelo);    
    }


    void girar()
    {
        miraDerecha = !miraDerecha;
        Vector3 escalaGiro = transform.localScale;
        escalaGiro.x = escalaGiro.x * -1;
        transform.localScale = escalaGiro;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);

    }
}
