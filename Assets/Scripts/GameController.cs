using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject fundidoNegro;

    public static bool gameOn = false;
    private SpriteRenderer sprFundido;
    public static bool playerMuerto;


    private void Awake()
    {
        fundidoNegro.SetActive(true);
    }

    private void Start()
    {
        sprFundido = fundidoNegro.GetComponent<SpriteRenderer>();
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
            sprFundido.material.color = new Color(sprFundido.material.color.r, sprFundido.material.color.g, sprFundido.material.color.b, alpha);

            yield return null;

        }
        gameOn = true;

    }

    IEnumerator PonF()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundido.material.color = new Color(sprFundido.material.color.r, sprFundido.material.color.g, sprFundido.material.color.b, alpha);

            yield return null;

        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }


}
