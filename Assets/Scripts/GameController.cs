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

    [SerializeField] private PlayerController playerController;



    public static bool gameOn = false;
    private Image sprFundido;
    public static bool playerMuerto;

    private int zanahorias;

    public static void sumaZanahoria()
    {
        current.zanahorias++;
        if (current.zanahorias < 10) current.contadorZanahorias.text = "0" + current.zanahorias;
        else current.contadorZanahorias.text = current.zanahorias.ToString();


    }

    private void Awake()
    {
        current = this;
        fundidoNegro.SetActive(true);
    }

    private void Start()
    {
        sprFundido = fundidoNegro.GetComponent<Image>();
        playerController.PlayerMuerto += PlayerMuerto;
        StartCoroutine(FundidoNegroOff(0.5f));
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
        respawn();
        StartCoroutine(FundidoNegroOff(1f));
    }


}
