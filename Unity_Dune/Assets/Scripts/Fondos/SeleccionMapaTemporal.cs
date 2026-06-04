using UnityEngine;
using UnityEngine.SceneManagement;

public class SeleccionMapaTemporal : MonoBehaviour
{
    [SerializeField] private int indiceEscenaJuego = 1;

    public void ProbarArrakeen()
    {
        VisualSceneData.EscenarioSeleccionado = EscenarioJuego.Arrakeen;
        Debug.Log("Seleccionado temporalmente: Arrakeen");
        SceneManager.LoadScene(indiceEscenaJuego);
    }

    public void ProbarCaladan()
    {
        VisualSceneData.EscenarioSeleccionado = EscenarioJuego.Caladan;
        Debug.Log("Seleccionado temporalmente: Caladan");
        SceneManager.LoadScene(indiceEscenaJuego);
    }

    public void ProbarGiediPrime()
    {
        VisualSceneData.EscenarioSeleccionado = EscenarioJuego.GiediPrime;
        Debug.Log("Seleccionado temporalmente: Giedi Prime");
        SceneManager.LoadScene(indiceEscenaJuego);
    }
}