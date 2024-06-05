using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TocarSuelo : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool colPies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag == "Suelo") { colPies = true; }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Suelo") { colPies = false; }
    }
}
