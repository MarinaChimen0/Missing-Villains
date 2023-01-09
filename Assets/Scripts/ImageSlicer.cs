using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageSlicer {

    public static Texture2D[,] GetSlices(Texture2D image, int blocksPerLine)
    {
        //Tamaño de la imagen
        int imageSize = Mathf.Min(image.width, image.height);
        //Tamaño del bloque
        int blockSize = imageSize / blocksPerLine;

        //Texturas (partes de la imagen) de los bloques 
        Texture2D[,] blocks = new Texture2D[blocksPerLine, blocksPerLine];

        for (int i = 0; i < blocksPerLine; i++) {
            for (int j = 0; j < blocksPerLine; j++) {
                //Textura para el bloque i,j
                Texture2D block = new Texture2D(blockSize, blockSize);
                block.wrapMode = TextureWrapMode.Clamp;
                block.SetPixels(image.GetPixels(j*blockSize,i*blockSize, blockSize, blockSize));
                block.Apply();
                blocks[j, i] = block;
            }
        }

        return blocks;
    }

}
