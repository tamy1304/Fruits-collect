using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PiedraPinchos : MonoBehaviour
{
    [SerializeField] private Transform destino;
    [SerializeField] private float velocidad;
    private Vector3 posIni, posFin;

    // Start is called before the first frame update
    void Start()
    {
        destino.parent = null;
        posIni = transform.position;
        posFin = destino.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);
        if (transform.position == destino.position) {
            destino.position = (destino.position == posFin) ? posIni : posFin;
        }

    }
}
