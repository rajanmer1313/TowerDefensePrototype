using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradePanel : MonoBehaviour
{
    public static TowerUpgradePanel Instance;

    [Header("UI")]
    public GameObject panel;
    public Text towerName;
    public Text levelText;
    public Button upgradeButton;
    public Image nextBulletPreview;

    private ITower currentTower;

    void Awake()
    {
        // FORCE-SET INSTANCE
        Instance = this;
    }

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradePressed);
        else
            Debug.LogError("UpgradeButton NOT assigned in inspector!");
    }

    public void OpenPanel(ITower tower)
    {
        currentTower = tower;

        panel.SetActive(true);

        // Tower name
        towerName.text = tower.GetTowerName();

        int level = tower.GetLevel();
        int maxLevel = tower.GetMaxLevel();

        levelText.text = "Level " + (level + 1);

        // -------- MAX LEVEL CHECK --------
        if (level >= maxLevel - 1)
        {
            // Max level UI
            nextBulletPreview.sprite = null;
            upgradeButton.interactable = false;
            upgradeButton.GetComponentInChildren<Text>().text = "MAX";
            return;
        }

        // -------- NOT MAX LEVEL --------
        upgradeButton.interactable = true;
        upgradeButton.GetComponentInChildren<Text>().text = "Upgrade";

        // Bullet preview
        GameObject nextBullet = tower.GetNextLevelBullet();
        if (nextBullet != null)
        {
            SpriteRenderer sr = nextBullet.GetComponent<SpriteRenderer>();
            nextBulletPreview.sprite = sr != null ? sr.sprite : null;
        }
        else
        {
            nextBulletPreview.sprite = null;
        }
    }


    void OnUpgradePressed()
    {
        currentTower?.Upgrade();
        ClosePanel();
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        currentTower = null;
    }
}
