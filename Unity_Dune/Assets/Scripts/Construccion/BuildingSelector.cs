using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    public BuildingPlacer buildingPlacer;
    public GameObject buildingPrefab;

    public void SelectThisBuilding()
    {
        buildingPlacer.SelectBuilding(buildingPrefab);
    }
}