using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingBlocksPuzzle : MonoBehaviour {

    //Numero de piezas por linea
    public int blocksPerLine = 4;
    //Imagen
    public Texture2D image;
    //Veces que se va a mezclar la imagen
    public int shuffleLength = 20;
    //Duracion por defecto del movimiento
    public float defaultMoveDuration = .2f;
    //Duracion del movimiento al mezclar
    public float shuffleMoveDuration = .1f;

    //Estado del puzzle
    enum PuzzleState {Solved, Shuffling, InPlay};
    PuzzleState state;

    //Bloque vacio
    Block emptyBlock;
    //Referencias de los bloques
    Block[,] blocks;
    //Cola de bloques seleccionados
    Queue<Block> inputs;
    //Verdadero si hay un bloque moviendose, falso al contrario
    bool BlockIsMoving;
    //Numero de movimientos que nos quedan por hacer de la mezcla
    int shuffleMovesRemaining;
    //Anterior offset en el proceso de la mezcla
    Vector2Int prevShuffleOffset;

    // Start is called before the first frame update
    void Start()
    {
        CreatePuzzle();
        StartShuffle();
    }

    // Update del puzzle 
    /*
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && state == PuzzleState.Solved) {
            StartShuffle();
        }
    }
    */

    //Crea el puzzle 
    void CreatePuzzle() {
        blocks = new Block[blocksPerLine, blocksPerLine];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);
        for(int i = 0; i < blocksPerLine; i++){
            for(int j = 0; j < blocksPerLine; j++){
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //Centramos los bloques
                blockObject.transform.position = -Vector2.one * (blocksPerLine - 1) *.5f + new Vector2(j, i);
                blockObject.transform.parent = transform;
                //Referencia al scrip de bloque
                Block block = blockObject.AddComponent<Block>();
                block.OnBlockPressed += PlayerMoveBlockInput;
                block.OnFinishedMoving += OnBlockFinishedMoving;
                block.Init(new Vector2Int(j, i), imageSlices[j, i]);
                blocks[j, i] = block;
                //Si es el ultimo bloque
                if(i==0 && j == blocksPerLine - 1) {
                    emptyBlock = block;
                }
            }
        }
        Camera.main.orthographicSize = blocksPerLine * .55f;
        inputs = new Queue<Block>(); 
    }

    //Encola un bloque seleccionado
    void PlayerMoveBlockInput(Block blockToMove)
    {
        if (state == PuzzleState.InPlay)
        {
            inputs.Enqueue(blockToMove);
            MakeNextPlayerMove();
        }
    }

    //Hace un nuevo movimiento
    void MakeNextPlayerMove() {
        while (inputs.Count > 0 && !BlockIsMoving) {
            MoveBlock(inputs.Dequeue(), defaultMoveDuration);
        }
    }

    //Intercambia el bloque en el que se ha pinchado con el desactivado
    void MoveBlock(Block blockToMove, float duration) {
        //Si es uno de los bloques adyacentes al vacio
        if ((blockToMove.cord - emptyBlock.cord).sqrMagnitude == 1)
        {
            blocks[blockToMove.cord.x, blockToMove.cord.y] = emptyBlock;
            blocks[emptyBlock.cord.x, emptyBlock.cord.y] = blockToMove;
            //Intercambiamos las coordenadas
            Vector2Int auxCord = emptyBlock.cord;
            emptyBlock.cord = blockToMove.cord;
            blockToMove.cord = auxCord;
            //Intercambiamos la posicion
            Vector2 auxPos = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.MoveToPosition(auxPos, duration);
            BlockIsMoving = true;
        }
    }

    //Finaliza los movimientos
    void OnBlockFinishedMoving() {

        BlockIsMoving = false;
        CheckIfSolved();

        if (state == PuzzleState.InPlay) {
            MakeNextPlayerMove();
        }
        else if (state == PuzzleState.Shuffling) {
            if (shuffleMovesRemaining > 0)
            {
                MakeNextShuffleMove();
            }
            else
            {
                state = PuzzleState.InPlay;
            }
        }
    }

    //Empieza a mezclar los bloques
    void StartShuffle() {
        state = PuzzleState.Shuffling;
        shuffleMovesRemaining = shuffleLength;
        emptyBlock.gameObject.SetActive(false);
        MakeNextShuffleMove();
    }

    //Hace el siguiente movimiento de la mezcla
    void MakeNextShuffleMove() {
        Vector2Int[] offsets = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        int randomIndex = Random.Range(0, offsets.Length);

        for (int i = 0; i < offsets.Length; i++) {
            Vector2Int offset = offsets[(randomIndex + i) % offsets.Length];

            if(offset != prevShuffleOffset * -1) {

                Vector2Int moveBlockCord = emptyBlock.cord + offset;

                if (moveBlockCord.x >= 0 && moveBlockCord.x < blocksPerLine && moveBlockCord.y >= 0 && moveBlockCord.y < blocksPerLine){
                    MoveBlock(blocks[moveBlockCord.x, moveBlockCord.y], shuffleMoveDuration);
                    shuffleMovesRemaining--;
                    prevShuffleOffset = offset;
                    break;
                }
            }
        }
    }

    //Comprueba si un block esta en la postura correcta
    void CheckIfSolved() {
        foreach (Block block in blocks) {
            if (!block.IsAtStartingCord()) {
                return;
             }
        }

        state = PuzzleState.Solved;
        emptyBlock.gameObject.SetActive(true);
    }
}
