using System.Collections;
using UnityEngine;

public class TutorialChurrero : MonoBehaviour
{
    public int estado = 1;

    [Header("Carteles")]
    public GameObject cartelGritarChurros;
    public GameObject cartelClienteChurro;
    public GameObject cartelHervir;
    public GameObject cartelSacarChurro;
    public GameObject cartelPasarAzucar;
    public GameObject cartelChurroAzucarado;
    public GameObject cartelChurroDulceDeLeche;
    public GameObject cartelBolsa;
    public GameObject cartelChurroListo;
    public GameObject churroSacado;
    public GameObject churroConDulceDeLeche;
    public GameObject churroEnBolsa;
    public GameObject monedasGanadas;

    [Header("Referencias")]
    public Transform puntoCarrito;
    public float velocidadCliente = 2f;
    public BarraCocinaChurro barraCocina;

    [Header("Clientes múltiples")]
    public Transform[] clientes;
    private int clienteActual = 0;

    private bool churroListo = false;

    void Start()
    {
        cartelGritarChurros.SetActive(true);
        cartelClienteChurro.SetActive(false);
        cartelHervir.SetActive(false);
        cartelSacarChurro.SetActive(false);
        cartelPasarAzucar.SetActive(false);
        cartelChurroAzucarado.SetActive(false);
        cartelChurroDulceDeLeche.SetActive(false);
        cartelBolsa.SetActive(false);
        cartelChurroListo.SetActive(false);
        monedasGanadas.SetActive(false);
        churroEnBolsa.SetActive(false);
        churroConDulceDeLeche.SetActive(false);
        churroSacado.SetActive(false);

        foreach (Transform c in clientes) if (c != null) c.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (estado)
        {
            case 1:
                if (Input.GetKeyDown(KeyCode.C))
                {
                    cartelGritarChurros.SetActive(false);
                    StartCoroutine(MoverCliente());
                    estado = 2;
                }
                break;

            case 3:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    cartelHervir.SetActive(false);
                    cartelClienteChurro.SetActive(true);

                    if (barraCocina != null)
                        barraCocina.EmpezarCocina();

                    StartCoroutine(EsperarCoccion());
                    estado = 4;
                }
                break;

            case 5:
                if (Input.GetKeyDown(KeyCode.S))
                {
                    churroListo = false;
                    cartelChurroListo.SetActive(false);
                    cartelSacarChurro.SetActive(false);
                    churroSacado.SetActive(true);
                    cartelPasarAzucar.SetActive(true);
                    estado = 6;
                }
                break;

            case 6:
                if (Input.GetKeyDown(KeyCode.B))
                {
                    cartelPasarAzucar.SetActive(false);
                    churroSacado.SetActive(false);
                    cartelChurroAzucarado.SetActive(true);
                    cartelChurroDulceDeLeche.SetActive(true);
                    estado = 7;

                    if (clienteActual + 1 < clientes.Length)
                        StartCoroutine(PrepararSiguienteCliente());
                }
                break;

            case 7:
                if (Input.GetKeyDown(KeyCode.L))
                {
                    cartelChurroDulceDeLeche.SetActive(false);
                    cartelChurroAzucarado.SetActive(false);
                    churroConDulceDeLeche.SetActive(true);
                    cartelBolsa.SetActive(true);
                    estado = 8;
                }
                break;

            case 8:
                if (Input.GetKeyDown(KeyCode.K))
                {
                    cartelBolsa.SetActive(false);
                    churroConDulceDeLeche.SetActive(false);
                    churroEnBolsa.SetActive(true);
                    monedasGanadas.SetActive(true);
                    cartelClienteChurro.SetActive(false);
                    estado = 9;
                    StartCoroutine(SalirCliente());
                }
                break;
        }
    }

    IEnumerator MoverCliente()
    {
        if (clientes.Length == 0) yield break;

        Transform cliente = clientes[clienteActual];
        cliente.gameObject.SetActive(true);

        Rigidbody2D rb = cliente.GetComponent<Rigidbody2D>();
        if (rb != null) rb.isKinematic = true;

        Vector3 targetPos = puntoCarrito.position;
        float escalaInicial = 15f;
        float escalaFinal = 15f;
        cliente.localScale = Vector3.one * escalaInicial;

        float tiempo = 0f;
        float duracion = 4f;
        Vector3 inicio = cliente.position;

        while (tiempo < duracion)
        {
            cliente.position = Vector3.Lerp(inicio, targetPos, tiempo / duracion);
            cliente.localScale = Vector3.Lerp(Vector3.one * escalaInicial, Vector3.one * escalaFinal, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        cliente.position = targetPos;
        cliente.localScale = Vector3.one * escalaFinal;

        cartelClienteChurro.SetActive(true);
        yield return new WaitForSeconds(1f);
        cartelHervir.SetActive(true);

        estado = 3;

        if (rb != null) rb.isKinematic = false;
    }

    IEnumerator EsperarCoccion()
    {
        yield return new WaitForSeconds(barraCocina.tiempoCoccion + 0.5f);
        churroListo = true;
        cartelSacarChurro.SetActive(true);
        cartelChurroListo.SetActive(true);
        estado = 5;
    }

    IEnumerator SalirCliente()
    {
        Transform cliente = clientes[clienteActual];
        Vector3 salida = cliente.position + new Vector3(5f, 0, 0);
        float velocidadSalida = 2f;

        while (Vector3.Distance(cliente.position, salida) > 0.05f)
        {
            cliente.position = Vector3.MoveTowards(cliente.position, salida, velocidadSalida * Time.deltaTime);
            yield return null;
        }

        cliente.gameObject.SetActive(false);
            churroEnBolsa.SetActive(false);
                    monedasGanadas.SetActive(false);

        clienteActual++;
        if (clienteActual < clientes.Length)
        {
            yield return new WaitForSeconds(1.5f);
          
            StartCoroutine(MoverCliente());
        }
    }

    IEnumerator PrepararSiguienteCliente()
    {
        yield return new WaitForSeconds(1.5f);
        Transform proximo = clientes[clienteActual + 1];
        if (proximo == null) yield break;

        proximo.gameObject.SetActive(true);
        Vector3 inicio = puntoCarrito.position + new Vector3(6f, 0, 0);
        proximo.position = inicio;
        float escalaInicial = 15f;
        float escalaFinal = 15f;
        proximo.localScale = Vector3.one * escalaInicial;

        Vector3 destino = puntoCarrito.position + new Vector3(1.5f, 0, 0);

        float tiempo = 0f;
        float duracion = 4f;
        while (tiempo < duracion)
        {
            proximo.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
           
            tiempo += Time.deltaTime;
            yield return null;
        }

        proximo.position = destino;
        proximo.localScale = Vector3.one * escalaFinal;
    }
}
