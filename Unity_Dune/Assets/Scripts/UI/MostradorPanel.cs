using UnityEngine;

public class MostradorPanel : MonoBehaviour
{
    public GameObject panelInstalaciones;

    public void MostrarPanel()
    {
        panelInstalaciones.SetActive(true);
    }

    public void OcultarPanel()
    {
        panelInstalaciones.SetActive(false);
    }

    public void AlternarPanel()
    {
        panelInstalaciones.SetActive(!panelInstalaciones.activeSelf);
    }
}