using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] Tetris; // Tablica obiektów z kształtami

    void Start()
    {
        NewTetr();
    }


    public void NewTetr()
    {

        if (Blok.gameover == false)
        {
            Instantiate(Tetris[Random.Range(0, Tetris.Length)], transform.position, Quaternion.identity);
        }
        // Losujemy kształt i spawnujemy, jeśli nie przerwano gry
    }
}
