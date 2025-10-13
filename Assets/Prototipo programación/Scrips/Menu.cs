using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void irAlNivel1()
    {
        SceneManager.LoadScene("Escena1");
    }
     public void irAlNivel2()
    {
        SceneManager.LoadScene("Escena2");
    }
}
