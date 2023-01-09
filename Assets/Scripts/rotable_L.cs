using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class rotable_L : MonoBehaviour {

    private int posiciones_posibles = 4;
    public int posicion_correcta;
    public int posicion_inicial;
    public int posicion;

    // Use this for initialization
    void Start () {
        // rotacion inicial
        transform.Rotate(0, 0, anguloRotacion(posicion_inicial));
        posicion = posicion_inicial;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // cambia la imagen cuando haces click
    void OnMouseDown() {
        transform.Rotate(0,0,-90);
        posicion = (posicion + 1) % posiciones_posibles;
    }

    // decide el angulo de giro
    int anguloRotacion(int posActual) {
        int angulo = 0;
        switch (posicion_inicial)
        {
            case 1:
                angulo = -90;
                break;
            case 2:
                angulo = 180;
                break;
            case 3:
                angulo = 90;
                break;
            default:
                break;
        }
        return angulo;
    }

}
