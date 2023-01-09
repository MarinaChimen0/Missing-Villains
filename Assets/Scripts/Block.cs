using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public event System.Action<Block> OnBlockPressed;
    public event System.Action OnFinishedMoving;
    //Coordenada del bloque
    public Vector2Int cord;
    //Coordenada de inicio del bloque
    Vector2Int startingCord;

    //Inicia el bloque
    public void Init(Vector2Int startingCord, Texture2D image) {
        this.startingCord = startingCord;
        cord = startingCord;
        GetComponent<MeshRenderer>().material = Resources.Load<Material>("Block");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    //Cambio de posicion
    public void MoveToPosition(Vector2 pos, float duration) {
        StartCoroutine(AnimateMove(pos, duration));
    }

    //Selecciona con raton
    void OnMouseDown() {
        if (OnBlockPressed != null) {
            OnBlockPressed(this);
        }
    }

    //Animacion del movimiento
    IEnumerator AnimateMove(Vector2 pos, float duration) {
        Vector2 initialPos = transform.position;
        float percent = 0;

        while (percent < 1) {
            percent += Time.deltaTime / duration;
            transform.position = Vector2.Lerp(initialPos, pos, percent);
            yield return null;
        }

        if(OnFinishedMoving != null) {
            OnFinishedMoving();
        }
    }

    public bool IsAtStartingCord() {
        return cord == startingCord;
    }
}
