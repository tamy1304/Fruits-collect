using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class PlayerController : MonoBehaviour
{
    [Header("VALORES CONFIGURABLES")]
    [SerializeField] private float velocidad;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private bool saltoMejorado;
    [SerializeField] private float saltoLargo = 1.5f;
    [SerializeField] private float saltoCorto = 1f;
    [SerializeField] private Transform checkGround;
    [SerializeField] private float checkGroundRadio;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float fuerzaToque;

    [Header("VALORES INFORMATIVOS")]
    [SerializeField] private bool colPies = false;
    private Rigidbody2D rPlayer;
    private SpriteRenderer sPlayer;
    private float h;
    private Animator aPlayer;
    private bool miraDerecha = true;
    private bool saltando = false;
    private bool tocaSuelo = false;
    private bool enPlataforma = false;
    private bool puedoSaltar = false;
    private bool tocado = false; 
    private Vector2 nuevaVelocidad;
    private Vector3 posIni;
    private Color colorOriginal;

    // Start is called before the first frame update
    void Start()
    {
        posIni = transform.position;
        rPlayer = GetComponent<Rigidbody2D>();
        aPlayer = GetComponent<Animator>();
        sPlayer = GetComponent<SpriteRenderer>();
        colorOriginal = sPlayer.color;
    }

    // Update is called once per frame
    void Update()
    {
        recibePulsaciones();
        variablesAnimador();

    }

    void FixedUpdate()
    {
        checkTocaSuelo();
        if(!tocado )movimientoPlayer();

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
        if (Input.GetKey(KeyCode.R)) reaparece();
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
            if (tocado)
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
        if (collision.gameObject.tag == "Plataforma")
        {
            rPlayer.velocity = Vector3.zero;   
            
            transform.parent = collision.transform;
            enPlataforma = true;
            tocaSuelo = true;
        }

        if (collision.gameObject.tag == "Enemigo")
        {
            toque(collision.transform.position.x);
        }

        if (collision.gameObject.tag == "SobreEnemigo")
        {
            collision.gameObject.SendMessage("muere");
        }
    

    }

    private void toque(float posX)
    {
        if (!tocado)
        {
            Color nuevoColor = new Color(255f / 255f, 153f / 255f, 153f / 255f);
            sPlayer.color = nuevoColor;
            tocado = true;
            float lado = Mathf.Sign(posX - transform.position.x);
            rPlayer.velocity = Vector2.zero;
            rPlayer.AddForce(new Vector2(fuerzaToque * -lado, fuerzaToque), ForceMode2D.Impulse);
            

        }
        
    }
    


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataforma")
        {
            transform.parent = null;
            enPlataforma = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Pinchos")
        {
            Debug.Log("Quita salud");
            pierdeVida();
        }
        if (collision.gameObject.tag == "CaidaAlVacio")
        {
            Debug.Log("Muerte");
            pierdeVida();
        }
    }
    private void pierdeVida() {
        Debug.Log("P1erde vida");
        reaparece();
    }
    private void reaparece()
    {
        rPlayer.velocity = Vector3.zero;
        transform.position = posIni;
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
