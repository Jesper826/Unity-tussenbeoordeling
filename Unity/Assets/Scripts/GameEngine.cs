using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEngine : MonoBehaviour
{
    public GameObject pigModel;
    public GameObject tileModel;
    public GameObject towerModel;

    private Tower tower;
    private Enemy enemy;

    private GameObject[] path;

    // Define the RelAdd struct to hold the relative movement (x, z)
    [System.Serializable]
    public struct RelAdd
    {
        public float x;
        public float z;
    }

    void Start()
    {
        // Declare local variables for coordinates and size
        int x = 0;
        int z = 0;
        int size = 2;

        // Updated pathplus array using relative movements
        RelAdd[] pathplus = new RelAdd[] {
            new RelAdd() {x=0, z=0 },
            new RelAdd() {x=0, z=1 },
            new RelAdd() {x=1, z=0},
            new RelAdd() {x=1, z=0},
            new RelAdd() {x=1, z=0},
            new RelAdd() {x=0, z=-1 },
            new RelAdd() {x=0, z=-1 },
            new RelAdd() {x=0, z=-1 },
            new RelAdd() {x=1, z=0},
            new RelAdd() {x=1, z=0},
            new RelAdd() {x=1, z=0},
        };

        // Initialize the path array based on the relative movements
        path = new GameObject[pathplus.Length];

        // Loop through each step in pathplus to create tiles
        for (int i = 0; i < pathplus.Length; i++)
        {
            // Get the current RelAdd for the current index i
            RelAdd step = pathplus[i];

            // Update x and z based on the current RelAdd values and apply the size
            x += (int)(step.x * size);
            z += (int)(step.z * size);

            // Instantiate the tile at the updated position
            path[i] = Instantiate(tileModel, new Vector3(x, 0, z), Quaternion.identity);
        }

        // Set up the enemy at the first path tile
        GameObject enemyStart = path[0];
        GameObject enemyObj = Instantiate(pigModel, enemyStart.transform.position, Quaternion.identity);
        enemy = new Enemy(enemyObj);
        enemy.from = 0;
        enemy.to = 1;

        // Set up the tower
        GameObject towerPlace = path[4];
        GameObject onTile = Instantiate(tileModel, towerPlace.transform.position + new Vector3(0, 0, 2), Quaternion.identity);
        GameObject towerObj = Instantiate(towerModel, onTile.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        tower = new Tower(towerObj, 5, onTile);
    }

    void Update()
    {
        MoveEnemy(enemy);

        if (GetDist(tower.obj, enemy.obj) <= tower.detectRange)
        {
            Debug.Log("near!");
        }
    }

    public void MoveEnemy(Enemy enemy)
    {
        if (enemy.to >= path.Length)
        {
            return;
        }

        // Haal de begin- en eindtegel op uit het pad
        GameObject from = path[enemy.from];
        GameObject to = path[enemy.to];

        // Bepaal de delta tussen de begin- en eindtegel
        float dx = to.transform.position.x - from.transform.position.x;
        float dy = to.transform.position.y - from.transform.position.y;
        float dz = to.transform.position.z - from.transform.position.z;

        Debug.Log(dx + " " + dy + " " + dz);

        // Verplaats de vijand volgens de delta
        enemy.obj.transform.position += new Vector3(dx, dy, dz) * Time.deltaTime;

        // Controleer of de vijand de volgende tegel heeft bereikt
        if (Vector3.Distance(enemy.obj.transform.position, to.transform.position) < 0.1f)
        {
            enemy.from = enemy.to;
            enemy.to++;
        }
    }

    public double GetDist(GameObject a, GameObject b)
    {
        float dx = a.transform.position.x - b.transform.position.x;
        float dy = a.transform.position.y - b.transform.position.y;
        float dz = a.transform.position.z - b.transform.position.z;

        float powered = (dx * dx) + (dy * dy) + (dz * dz);
        double dist = Math.Sqrt(powered);
        Debug.Log(dist);
        return dist;
    }
}
