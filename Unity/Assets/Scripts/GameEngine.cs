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

    void Start()
    {
        GameObject[] pathplus = new GameObject[10];
        path = new GameObject[pathplus.Length];

        for (int i = 0; i < pathplus.Length; i++)
        {
            path[i] = Instantiate(tileModel, new Vector3(i * 2, 0, 0), Quaternion.identity);
        }

        GameObject enemyStart = path[0];
        GameObject enemyObj = Instantiate(pigModel, enemyStart.transform.position, Quaternion.identity);
        enemy = new Enemy(enemyObj);
        enemy.from = 0;
        enemy.to = 1;

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

        // Beweeg de vijand naar de volgende tegel
        Vector3 targetPosition = path[enemy.to].transform.position;
        enemy.obj.transform.position = Vector3.MoveTowards(enemy.obj.transform.position, targetPosition, Time.deltaTime * 2);

        // Controleer of de vijand de volgende tegel heeft bereikt
        if (Vector3.Distance(enemy.obj.transform.position, targetPosition) < 0.1f)
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
