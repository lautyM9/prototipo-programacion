using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChoclero : MonoBehaviour
{
    public int estado = 1;
    public GameObject cartelGritarChoclos;
    public GameObject cartelClienteChoclo;
    public Transform clienteChoclo;
    public Transform puntoCarrito;
    public float velocidadCliente = 2f;

    void Start()
    {

        if (clienteChoclo == null)
            clienteChoclo = GameObject.Find("ClienteChurro").transform;

        if (puntoCarrito == null)
            puntoCarrito = GameObject.Find("PuntoCarritoChurro").transform;

        cartelGritarChoclos.SetActive(true);
        cartelClienteChoclo.SetActive(false);
    }

    void Update()
    {
        if (estado == 1 && Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Gritó choclos - pasamos al estado 2");

            if (cartelGritarChoclos != null)
                cartelGritarChoclos.SetActive(false);

            StartCoroutine(MoverCliente());
            estado = 2;
        }
    }

    IEnumerator MoverCliente()
    {
        if (clienteChoclo == null || puntoCarrito == null)
        {
            Debug.LogError("Cliente o punto de carrito no encontrados");
            yield break;
        }

        // Hacer kinematic si tiene Rigidbody2D
        Rigidbody2D rb = clienteChoclo.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true;

        clienteChoclo.gameObject.SetActive(true);

        // Mantener Z = 0 para que se vea
        Vector3 targetPos = new Vector3(puntoCarrito.position.x, puntoCarrito.position.y, 0f);
        clienteChoclo.position = new Vector3(clienteChoclo.position.x, clienteChoclo.position.y, 0f);

        Debug.Log("Cliente empieza a moverse hacia el carrito");

        while (Vector3.Distance(clienteChoclo.position, targetPos) > 0.05f)
        {
            clienteChoclo.position = Vector3.MoveTowards(
                clienteChoclo.position,
                targetPos,
                velocidadCliente * Time.deltaTime
            );
            yield return null;
        }

        Debug.Log("Cliente llegó al carrito");

        if (cartelClienteChoclo != null)
            cartelClienteChoclo.SetActive(true);

        // Restaurar Rigidbody2D si existía
        if (rb != null) rb.isKinematic = false;
    }
}
