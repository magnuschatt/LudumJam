﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelSequence : MonoBehaviour {

    public string key;
    public GameObject[] obstacles;

    private List<Transform> activeObstacles;
    private int[] obstacleTriggers;
    private float camHeight;

    void Start () {
        obstacleTriggers = new int[obstacles.Length];
        camHeight = Camera.main.orthographicSize * 2f;
        activeObstacles = new List<Transform>();
    }

    internal void Tick() {
        for (int i = 0; i < obstacles.Length; i++) {
            obstacleTriggers[i]++;
            GameObject obstacleGO = obstacles[i];
            Obstacle obstacle = obstacleGO.GetComponent<Obstacle>();
            int freq = obstacle == null ? 10 : obstacle.GetFrequency();

            if (obstacleTriggers[i] >= freq) {
                obstacleTriggers[i] = 0;
                SpawnObstacle(obstacleGO);
            }
        }

        Debug.Log("tick");
    }

    private void SpawnObstacle(GameObject obstacle) {

        GameObject go = Instantiate(obstacle);

        float fallDistance = Camera.main.transform.position.y;
        go.transform.position = new Vector3(0, fallDistance - camHeight, 0);
        Obstacle obst = go.GetComponent<Obstacle>();
        if (obst != null) {
            obst.Spawn();
        }

        activeObstacles.Add(go.transform);
    }

    void Update() {
        float camY = Camera.main.transform.position.y;

        List<Transform> marked = new List<Transform>();
        foreach (Transform t in activeObstacles) {
            if (t.position.y > camY + camHeight) {
                marked.Add(t);
            }
        }

        foreach (Transform t in marked) {
            activeObstacles.Remove(t);
            Destroy(t.gameObject);
        }
    }
}
