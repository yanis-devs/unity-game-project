using UnityEngine;

public class Checkerboard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create a 100x100 texture
        Texture2D texture = new Texture2D(100, 100, TextureFormat.RGB24, false);

        int squareSize = 10; // Size of each checkerboard square

        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                int squareX = x / squareSize;
                int squareY = y / squareSize;

                if ((squareX + squareY) % 2 == 0)
                    texture.SetPixel(x, y, Color.white);
                else
                    texture.SetPixel(x, y, Color.black);
            }
        }

        texture.Apply();
        texture.filterMode = FilterMode.Point; // Sharp pixelated edges

        // 0.5f, 0.5f anchors the pivot at the center
        // 100f pixelsPerUnit ensures the 100x100 texture spans 1 Unity world unit
        Sprite checkerSprite = Sprite.Create(
            texture,
            new Rect(0, 0, 100, 100),
            new Vector2(0.5f, 0.5f),
            100f
        );

        GetComponent<SpriteRenderer>().sprite = checkerSprite;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
