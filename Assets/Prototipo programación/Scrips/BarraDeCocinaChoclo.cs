using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarraCocinaChurro : MonoBehaviour
{
    [Header("Visuales")]
    public Image barraRelleno;             // Imagen tipo Filled
    public GameObject borde;               // Borde de la barra
    public GameObject cartelChurroAgarrado;
    public GameObject cartelChurroEnMaquina;
    public GameObject cartelChurroListo;
    public GameObject cartelTiempo;        // Cartel que muestra el tiempo de cocción
    public float tiempoCoccion = 3f;

    private bool cocinando = false;

    public void EmpezarCocina()
    {
        if (cocinando) return; // evita que se ejecute dos veces
        cocinando = true;

        // Resetear visuales
        if (cartelChurroAgarrado != null) cartelChurroAgarrado.SetActive(true);
        if (cartelChurroEnMaquina != null) cartelChurroEnMaquina.SetActive(false);
        if (cartelChurroListo != null) cartelChurroListo.SetActive(false);
        if (cartelTiempo != null) cartelTiempo.SetActive(false);

        if (barraRelleno != null) barraRelleno.fillAmount = 0f;
        if (barraRelleno != null) barraRelleno.gameObject.SetActive(false);
        if (borde != null) borde.SetActive(false);

        StartCoroutine(ProcesoCoccion());
    }

    private IEnumerator ProcesoCoccion()
    {
        // Paso 1: Mostrar ingrediente agarrado por 0.8 segundos
        yield return new WaitForSeconds(1f);
        if (cartelChurroAgarrado != null) cartelChurroAgarrado.SetActive(false);

        // Paso 2: Mostrar ingrediente en máquina
        if (cartelChurroEnMaquina != null) cartelChurroEnMaquina.SetActive(true);

        // Paso 3: Aparecen JUNTOS el borde, la barra y el cartel de tiempo
        if (borde != null) borde.SetActive(true);
        if (barraRelleno != null) barraRelleno.gameObject.SetActive(true);
        if (cartelTiempo != null) cartelTiempo.SetActive(true);

        // Paso 4: Animar la barra mientras se cocina
        float tiempo = 0f;
        while (tiempo < tiempoCoccion)
        {
            tiempo += Time.deltaTime;
            if (barraRelleno != null)
                barraRelleno.fillAmount = Mathf.Clamp01(tiempo / tiempoCoccion);
            yield return null;
        }

        // Paso 5: Cocción terminada
        if (cartelChurroEnMaquina != null) cartelChurroEnMaquina.SetActive(false);
        if (cartelChurroListo != null) cartelChurroListo.SetActive(true);
        if (cartelTiempo != null) cartelTiempo.SetActive(false);
        if (borde != null) borde.SetActive(false);
        if (barraRelleno != null) barraRelleno.gameObject.SetActive(false);

        cocinando = false;
    }
}

