using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public delegate void Respawn();
    public static event Respawn respawn;

    static GameController current;

    [SerializeField] private GameObject fundidoNegro;
    [SerializeField] private Text contadorZanahorias;
    [SerializeField] private GameObject contadorZanahoriasPuerta;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject zonaPuerta;
    [SerializeField] private GameObject puerta;

    public static bool gameOn = false;
    private Image sprFundido;
    public static bool playerMuerto;


    public static int zanahorias;
    public static int zanahoriasPuerta;
    private int zanahoriasIni;
    private TextMesh textoPuerta;

    public static bool soltandoZanahoria = false;
    private BoxCollider2D col;
    private Animator animPuerta;


    public static void sumaZanahoria()
    {
        zanahorias++;
        if (zanahorias < 10) current.contadorZanahorias.text = "0" + zanahorias;
        else current.contadorZanahorias.text = zanahorias.ToString();
    }

    public static void restaZanahoria()
    {
        zanahorias--;
        if (zanahorias < 10) current.contadorZanahorias.text = "0" + zanahorias;
        else current.contadorZanahorias.text = zanahorias.ToString();
    }

    public static void restaZanahoriaPuerta()
    {
        zanahoriasPuerta--;
        if (zanahoriasPuerta < 10) current.textoPuerta.text = "0" + zanahoriasPuerta;
        else current.textoPuerta.text = zanahoriasPuerta.ToString();

    }

    private void Awake()
    {
        current = this;
        fundidoNegro.SetActive(true);
        zanahoriasIni = 0;
    }

    private void Start()
    {
        sprFundido = fundidoNegro.GetComponent<Image>();
        textoPuerta = contadorZanahoriasPuerta.GetComponent<TextMesh>();
        col = zonaPuerta.GetComponent<BoxCollider2D>();
        animPuerta = puerta.GetComponent<Animator>();
        playerController.PlayerMuerto += PlayerMuerto;
        CheckP.checkPoint += CheckPoint;
        StartCoroutine(FundidoNegroOff(0.5f));
        zanahorias = zanahoriasIni;
        zanahoriasPuerta = 15;
        if (zanahoriasPuerta < 10) textoPuerta.text = "0" + zanahoriasPuerta;
        else textoPuerta.text = zanahoriasPuerta.ToString();

    }

    void CheckPoint()
    {
        zanahoriasIni = zanahorias;
    }

    void PlayerMuerto()
    {
        gameOn = false;
        StartCoroutine("FundidoNegroOn");
    }

    IEnumerator FundidoNegroOff(float retardo)
    {
        yield return new WaitForSeconds(retardo);

        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * 2f)
        {
            sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, alpha);
            yield return null;

        }
        sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, 0);
        gameOn = true;

    }

    IEnumerator FundidoNegroOn()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, alpha);

            yield return null;

        }
        sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, 1);
        zanahorias = zanahoriasIni - 1;
        sumaZanahoria();
        respawn();
        StartCoroutine(FundidoNegroOff(1f));
    }

    public static void permitePaso()
    {
        current.col.isTrigger = true;
    }

    public static void abrePuerta()
    {
        current.animPuerta.Play("PuertaAbre");
    }

    public static void FinJuego()
    {
        current.StartCoroutine("FundidoFinJuego");
    }

    IEnumerator FundidoFinJuego()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, alpha);
            yield return null;
        }
        sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, 1);
    }



}
