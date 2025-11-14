using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TowerBuildMenu : MonoBehaviour
{
    [Header("UI Prefab Reference")]
    public GameObject towerMenuPrefab;   // assign prefab here (TowerBuildPanel)

    [Header("Tower Prefabs")]
    public GameObject archerPrefab;
    public GameObject cannonPrefab;
    public GameObject wizardPrefab;

    private GameObject panelInstance;
    private RectTransform panelRect;
    private Transform currentSpot;

    private Button archerButton, cannonButton, wizardButton;

    void Start()
    {
        // Create panel instance at runtime
        GameObject canvas = FindObjectOfType<Canvas>().gameObject;
        panelInstance = Instantiate(towerMenuPrefab, canvas.transform);
        panelRect = panelInstance.GetComponent<RectTransform>();
        panelInstance.SetActive(false);

        //  Auto-assign buttons by name
        archerButton = panelInstance.transform.Find("ArcherButton").GetComponent<Button>();
        cannonButton = panelInstance.transform.Find("CannonButton").GetComponent<Button>();
        wizardButton = panelInstance.transform.Find("WizardButton").GetComponent<Button>();

        //  Add listeners
        archerButton.onClick.AddListener(() => OnBuildSelected(archerPrefab));
        cannonButton.onClick.AddListener(() => OnBuildSelected(cannonPrefab));
        wizardButton.onClick.AddListener(() => OnBuildSelected(wizardPrefab));
    }

    public void OpenMenu(Transform spot)
    {
        currentSpot = spot;
        StartCoroutine(OpenMenuWithDelay(spot));
    }

    private IEnumerator OpenMenuWithDelay(Transform spot)
    {
        yield return new WaitForEndOfFrame();  // Wait one frame so mouse click is released

        panelInstance.SetActive(true);
        panelInstance.transform.SetAsLastSibling();

        Vector3 screenPos = Camera.main.WorldToScreenPoint(spot.position);
        panelRect.position = screenPos + new Vector3(0, 100f, 0);
    }

    private void OnBuildSelected(GameObject prefab)
    {
        if (currentSpot == null || prefab == null) return;

        Instantiate(prefab, currentSpot.position, Quaternion.identity);
        currentSpot.gameObject.SetActive(false);
        CloseMenu();
    }

    public void CloseMenu()
    {
        panelInstance.SetActive(false);
        currentSpot = null;
    }
}
