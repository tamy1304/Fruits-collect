using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    [Header("Opciones Generales")]
    [SerializeField] GameObject pantallaMenu;
    [SerializeField] float tiempoCambiaOpcion;

    [Header("Elementos de menu")]
    [SerializeField] SpriteRenderer comenzar;
    [SerializeField] SpriteRenderer salir;

    [Header("Sprites de menu")]
    [SerializeField] Sprite comenzar_off;
    [SerializeField] Sprite comenzar_on;
    [SerializeField] Sprite salir_off;
    [SerializeField] Sprite salir_on;

    int pantalla;
    int opcionMenu, opcionMenuAnt;
    bool pulsadoSubmit;
    float v, h;
    float tiempoV, tiempoH;


    void Awake()
    {
        pantalla = 0;
        tiempoV = tiempoH = 0;
        opcionMenu = opcionMenuAnt = 1;
        AjustaOpciones();

    }

    void AjustaOpciones()
    {

    }

    void Update()
    {
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonUp("Submit")) pulsadoSubmit = false;
        if (v == 0) tiempoV = 0;
        if (pantalla == 0) MenuPrincipal();

    }

    void MenuPrincipal()
    {
        if (v != 0)
        {
            if (tiempoV == 0 || tiempoV > tiempoCambiaOpcion)
            {
                if (v == 1 && opcionMenu > 1) SeleccionaMenu(opcionMenu - 1);
                if (v == -1 && opcionMenu < 2) SeleccionaMenu(opcionMenu + 1);
                if (tiempoV > tiempoCambiaOpcion) tiempoV = 0;
            }
            tiempoV += Time.deltaTime;
        }
        if (Input.GetButtonDown("Submit") && !pulsadoSubmit)
        {
            if (opcionMenu == 1) SceneManager.LoadScene("SampleScene");
            if (opcionMenu == 2) Application.Quit();
        }

    }

    void SeleccionaMenu(int op)
    {
        opcionMenu = op;
        if (op == 1) comenzar.sprite = comenzar_on;
        if (op == 2) salir.sprite = salir_on;
        if (opcionMenuAnt == 1) comenzar.sprite = comenzar_off;
        if (opcionMenuAnt == 2) salir.sprite = salir_off;
        opcionMenuAnt = op;
    }
}