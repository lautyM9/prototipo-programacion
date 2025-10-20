using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChurrero : MonoBehaviour
{
    public int estado = 1;
    public GameObject cartelGritarChurros;
    public GameObject cartelClienteChurro;
    public Transform clienteChurro;
    public Transform puntoCarrito;
    public float velocidadCliente = 2f;

    void Start()
    {
   
        if (clienteChurro == null)
            clienteChurro = GameObject.Find("ClienteChurro").transform;

        if (puntoCarrito == null)
            puntoCarrito = GameObject.Find("PuntoCarritoChurro").transform;

        cartelGritarChurros.SetActive(true);
        cartelClienteChurro.SetActive(false);
    }

    void Update()
    {
        if (estado == 1 && Input.GetKeyDown(KeyCode.C))
{
    Debug.Log("Gritó churros - pasamos al estado 2");
    cartelGritarChurros.SetActive(false);
    Debug.Log("Llamando a StartCoroutine");
    StartCoroutine(MoverCliente());
    estado = 2;
}

    }

   IEnumerator MoverCliente()
{
    if (clienteChurro == null || puntoCarrito == null)
    {
        Debug.LogError("Cliente o punto de carrito no encontrados");
        yield break;
    }

    Rigidbody2D rb = clienteChurro.GetComponent<Rigidbody2D>();
    if (rb != null) rb.isKinematic = true;

    clienteChurro.gameObject.SetActive(true);

   
    Vector3 targetPos = new Vector3(puntoCarrito.position.x, puntoCarrito.position.y, clienteChurro.position.z);

    Debug.Log("Cliente empieza a moverse hacia el carrito");

    while (Vector3.Distance(clienteChurro.position, targetPos) > 0.05f)
    {
        clienteChurro.position = Vector3.MoveTowards(
            clienteChurro.position,
            targetPos,
            velocidadCliente * Time.deltaTime
        );
        yield return null;
    }

    Debug.Log("Cliente llegó al carrito");
    cartelClienteChurro.SetActive(true);

   
    if (rb != null) rb.isKinematic = false;
}

}
