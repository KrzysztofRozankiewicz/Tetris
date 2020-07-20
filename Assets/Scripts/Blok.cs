using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blok : MonoBehaviour
{

    private float prevTime; // Zmienna pomocnicza przy swobodnym opadaniu
    private float fallTime = 0.5f; // Szybkość spadania = 2 kratki/sek
    public static int width = 10; // Szerokość planszy
    public static int height = 20; // Wysokość planszy
    public Vector3 rotPoint; // Punkt odniesienia rotacji
    private static Transform[,] grid = new Transform[width, height]; // Siatka
    public static bool gameover = false; // Koniec gry? 

    void Start()
    {
        gameover = false;
    }

    void GridAdd() // Siatka, coby nie przenikało
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);   // Zaokrąglamy w razie Niemca
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;
        }
    }

    void CheckLine() // Czy wypełniono? 
    {
        for (int i = height-1; i>= 0; i--)
        {
            if(Line(i))
            {
                Delete(i);
                TetrDown(i);
            }
        }
    }

    void Fail() // Sprawdzanie faila
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i,16] != null)
            {
                gameover = true;
            }
        }
    }
    bool Line(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void Delete(int i)
    {
        for(int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;

        }
    }
    void TetrDown(int i)
    {
        for (int x = i; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (grid[y, x] != null)
                {
                    grid[y, x - 1] = grid[y, x];
                    grid[y, x] = null;
                    grid[y, x - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow)) // W prawo
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0); // Jak nie przejdzie testu to cofamy
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) // W lewo
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) // Obracanie górną strzałką
        {
            transform.RotateAround(transform.TransformPoint(rotPoint), new Vector3(0,0,1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotPoint), new Vector3(0, 0, 1), -90);
        }
        if (Time.time - prevTime > (Input.GetKeyDown(KeyCode.DownArrow) ? fallTime / 10 : fallTime)) // Opada
        {                                                                                // Po wciśnieciu strzałki szybciej
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0); // Jak nie przejdzie testu to cofamy
                Fail(); // Czy jest skucha?
                this.enabled = false; // Jak spadnie to nie ruszamy nim
                FindObjectOfType<Spawner>().NewTetr(); // I spawnujemy nowy
                GridAdd(); // Dodajemy do siatki
                CheckLine(); // Sprawdzamy czy wypełniono linię
            }
                
            prevTime = Time.time;
        }
    }
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);   // Zaokrąglamy w razie Niemca
            int roundY = Mathf.RoundToInt(children.transform.position.y);


            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)    // Czy się mieści na planszy?
            {
                return false;
            }

            if (grid[roundX, roundY] != null)
            {
                return false;
            }
        }
        return true;
    }
}
