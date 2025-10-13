using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arenaSpawner : MonoBehaviour
{
    public GameObject arena;
    public float spawnRate = 0f;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnArena();
    }
    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            Instantiate(arena, transform.position, transform.rotation);
            timer = 0;
        }

    }
    void spawnArena()
    {
        Instantiate(arena, transform.position, transform.rotation);
    }
}
