using System.Collections;
using UnityEngine;

public class TutorialChoclero : MonoBehaviour
{
    public int estado = 1;

    [Header("Carteles")]
    public GameObject cartelGritarChoclos;
    public GameObject cartelClienteChoclo;
    public GameObject cartelHervir;
    public GameObject cartelSacarChoclo;
    public GameObject cartelPasarSal;
    public GameObject cartelChocloSalado;
    public GameObject cartelChocloConManteca;
    public GameObject cartelBandeja;
    public GameObject cartelChocloListo;
    public GameObject chocloEnBandeja;
    public GameObject chocloSacado;
    public GameObject chocloConManteca;
    public GameObject monedasGanadas;
    public GameObject cartelPasarAderezo;
    public GameObject chocloConAderezo;
    public GameObject salCancelada;
    public GameObject cartelSalNo;

    [Header("Referencias")]
    public Transform puntoCarrito;
    public float velocidadCliente = 2f;
    public BarraCocinaChoclo barraCocina;

    [Header("Clientes múltiples")]
    public Transform[] clientes;
    private int clienteActual = 0;
    private GameObject cartelPasarActual;
private GameObject cartelIngredienteActual;
private GameObject objetoIngredienteActual;

    private bool chocloListo = false;

    void Start()
    {
        if (puntoCarrito == null)
            puntoCarrito = GameObject.Find("PuntoCarritoChoclo").transform;

        cartelGritarChoclos.SetActive(true);
        cartelClienteChoclo.SetActive(false);
        cartelHervir.SetActive(false);
        cartelSacarChoclo.SetActive(false);
        cartelPasarSal.SetActive(false);
        cartelChocloSalado.SetActive(false);
        cartelChocloConManteca.SetActive(false);
        cartelBandeja.SetActive(false);
        cartelChocloListo.SetActive(false);
        monedasGanadas.SetActive(false);
        chocloConManteca.SetActive(false);
        chocloSacado.SetActive(false);
        chocloEnBandeja.SetActive(false);

        foreach (Transform c in clientes) if (c != null) c.gameObject.SetActive(false);
    }

   void Update()
{
    switch (estado)
    {
        case 1:
            if (Input.GetKeyDown(KeyCode.C))
            {
                cartelGritarChoclos.SetActive(false);
                StartCoroutine(MoverCliente());
                estado = 2;
            }
            break;

        case 3:
            if (Input.GetKeyDown(KeyCode.A))
            {
                cartelHervir.SetActive(false);
                cartelClienteChoclo.SetActive(true);

                if (barraCocina != null)
                    barraCocina.EmpezarCocina();

                StartCoroutine(EsperarCoccion());
                estado = 4;
            }
            break;

        case 5:
            if (Input.GetKeyDown(KeyCode.S))
            {
                chocloListo = false;
                cartelChocloListo.SetActive(false);
                cartelSacarChoclo.SetActive(false);
                chocloSacado.SetActive(true);

                if (clienteActual == 1)
                {
                    cartelSalNo.SetActive(true);
                    salCancelada.SetActive(true);
                    cartelChocloConManteca.SetActive(true);
                    estado = 10;
                }
                else
                {
                    cartelPasarSal.SetActive(true);
                    estado = 6;
                }
            }
            break;

        case 6:
            if (Input.GetKeyDown(KeyCode.B))
            {
                chocloSacado.SetActive(false);
                cartelPasarSal.SetActive(false);
                cartelChocloSalado.SetActive(true);
                cartelChocloConManteca.SetActive(true);
                estado = 7;

                if (clienteActual + 1 < clientes.Length)
                    StartCoroutine(PrepararSiguienteCliente());
            }
            break;

        case 7:
            if (Input.GetKeyDown(KeyCode.L))
            {
                cartelChocloConManteca.SetActive(false);
                cartelChocloSalado.SetActive(false);
                chocloConManteca.SetActive(true);
                cartelBandeja.SetActive(true);
                estado = 8;
            }
            break;

        case 8:
            if (Input.GetKeyDown(KeyCode.K))
            {
                cartelBandeja.SetActive(false);
                    chocloConManteca.SetActive(false);
                chocloSacado.SetActive(false);
                    chocloEnBandeja.SetActive(true);
                chocloConAderezo.SetActive(false);
                monedasGanadas.SetActive(true);
                cartelClienteChoclo.SetActive(false);
                estado = 9;
                StartCoroutine(SalirCliente());
            }
            break;

        case 10:
            if (Input.GetKeyDown(KeyCode.L))
            {
                cartelChocloConManteca.SetActive(false);
                salCancelada.SetActive(false);
                cartelSalNo.SetActive(false);
                chocloConManteca.SetActive(true);
                cartelPasarAderezo.SetActive(true);
                estado = 11;
            }
            break;

        case 11:
            if (Input.GetKeyDown(KeyCode.V))
            {
                cartelPasarAderezo.SetActive(false);
                    chocloConManteca.SetActive(false);
                
                chocloConAderezo.SetActive(true);
                cartelBandeja.SetActive(true);
                estado = 8;
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

        cartelClienteChoclo.SetActive(true);
        yield return new WaitForSeconds(1f);
        cartelHervir.SetActive(true);

        estado = 3;

        if (rb != null) rb.isKinematic = false;
    }

    IEnumerator EsperarCoccion()
    {
        yield return new WaitForSeconds(barraCocina.tiempoCoccion + 0.5f);
        chocloListo = true;
        cartelSacarChoclo.SetActive(true);
        cartelChocloListo.SetActive(true);
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
 chocloEnBandeja.SetActive(false);
                    monedasGanadas.SetActive(false);
        clienteActual++;
        if (clienteActual < clientes.Length)
        {
            yield return new WaitForSeconds(1.5f);
               chocloEnBandeja.SetActive(false);
                    monedasGanadas.SetActive(false);
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
