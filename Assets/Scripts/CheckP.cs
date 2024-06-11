using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckP : MonoBehaviour
{
    public delegate void MiDelegado();
    public static event MiDelegado checkPoint;

    [SerializeField] Sprite spriteOn;
    [SerializeField] GameObject posPlayer;

    private SpriteRenderer spr;
    private BoxCollider2D boxCol;

    public GameObject[] zanahorias;



    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        zanahorias = GameObject.FindGameObjectsWithTag("Zanahorias");


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            boxCol.enabled = false;
            spr.sprite= spriteOn;
            checkPoint?.Invoke();
            PlayerController.posIni = posPlayer.transform.position;
            DestruyeObjetos();
        }
    }

    void DestruyeObjetos() {
        foreach (GameObject zanahoria in zanahorias)
        {
            if (!zanahoria.activeSelf) Destroy(zanahoria);
         }
    }
}
