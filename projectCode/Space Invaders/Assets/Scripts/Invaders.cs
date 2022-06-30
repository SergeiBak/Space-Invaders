using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [SerializeField]
    private Invader[] prefabs;
    [SerializeField]
    private int rows = 5;
    [SerializeField]
    private int cols = 11;

    [SerializeField]
    private float invaderSpacing = 2.0f;

    private void Awake()
    {
        for (int row = 0; row < rows; row++)
        {
            float gridWidth = invaderSpacing * (cols - 1);
            float gridHeight = invaderSpacing * (rows - 1);

            Vector2 centering = new Vector2(-gridWidth / 2, -gridHeight / 2);
            Vector3 rowPos = new Vector3(centering.x, centering.y + (row * invaderSpacing), 0.0f);

            for (int col = 0; col < cols; col++)
            {
                Invader invader = Instantiate(prefabs[row], transform);
                Vector3 pos = rowPos;
                pos.x += col * invaderSpacing;
                invader.transform.localPosition = pos;
            }
        }
    }
}
