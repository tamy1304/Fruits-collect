using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static GameController current;

    [SerializeField] private GameObject fundidoNegro;
    [SerializeField] private Text contadorZanahorias;

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
        if (current != null && current != this){
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
        fundidoNegro.SetActive(true);
    }

    private void Start()
    {
        sprFundido = fundidoNegro.GetComponent<Image>();
        Invoke("QuitaFundido", 0.5f);

    }

    private void Update()
    {
        if (playerMuerto)
        {
            StartCoroutine("PonF");
            playerMuerto = false;
        }       
    }

    private void QuitaFundido()
    {
        StartCoroutine("QuitaF");
    }

    IEnumerator QuitaF()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * 2f)
        {
            sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, alpha);

            yield return null;

        }
        gameOn = true;

    }

    IEnumerator PonF()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundido.color = new Color(sprFundido.color.r, sprFundido.color.g, sprFundido.color.b, alpha);

            yield return null;

        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }


}
