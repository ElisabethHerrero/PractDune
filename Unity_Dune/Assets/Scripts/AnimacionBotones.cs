using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float factorEscala = 1.2f;
    [SerializeField] float velocidad = 10f;

    private Vector3 escalaOriginal;
    private Vector3 escalaObjetivo;

    void Start()
    {
        escalaOriginal = transform.localScale;
        escalaObjetivo = escalaOriginal;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            escalaObjetivo,
            velocidad * Time.deltaTime
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal * factorEscala;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal;
    }
}