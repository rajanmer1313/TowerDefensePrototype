using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private TowerBuildMenu buildMenu;

    void Start()
    {
        buildMenu = FindObjectOfType<TowerBuildMenu>();
    }

    private void OnMouseDown()
    {
        if (buildMenu != null)
        {
            Debug.Log("TowerSpot Clicked");
            buildMenu.OpenMenu(this.transform);
        }
    }
}
