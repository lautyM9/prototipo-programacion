using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arena : MonoBehaviour
{
    public float moverse = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position += Vector3.down * moverse * Time.deltaTime;


    }
}
