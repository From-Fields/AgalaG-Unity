using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;

    [Space]
    public string lifePrefix;
    public string shieldPrefix;

    [Space]
    public string shotIconTag;
    public string ammunitionAmountTag;

    [Space]
    public string pointsTag;
    [SerializeField]
    private List<VisualElement> playerLifes = new List<VisualElement>();
    [SerializeField]private List<VisualElement> playerShields = new List<VisualElement>();

    private VisualElement _shotIcon;
    private Label _ammunition;

    private Label _points;

    void Awake()
    {
        _document.gameObject.SetActive(true);
        VisualElement root = _document.rootVisualElement;

        for (int i = 0; i < 3; i++)
        {
            var life = root.Q<VisualElement>(lifePrefix + (i + 1).ToString());
            if (life != null)
            {
                playerLifes.Add(life);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            var shield = root.Q<VisualElement>(shieldPrefix + (i + 1).ToString());
            if (shield != null)
            {
                playerShields.Add(shield);
                shield.visible = false;
            }
        }

        _shotIcon = root.Q<VisualElement>(shotIconTag);
        _ammunition = root.Q<Label>(ammunitionAmountTag);
        _points = root.Q<Label>(pointsTag);
        _points.text = 0.ToString();
    }

    public void UpdateLifeCount(int life)
    {
        for (int i = 0; i < 3; i++)
        {
            playerLifes[i].visible = (life - 1 >= i);
        }
    }

    public void UpdateShieldCount(int shield)
    {
        for (int i = 0; i < 6; i++)
        {
            playerShields[i].visible = (shield - 1 >= i);
        }
    }

    public void UpdateAmmunitionCount(string ammunitionAmount)
    {
        _ammunition.text = ammunitionAmount;
    }

    public void UpdateShotIcon(Sprite icon)
    {
        _shotIcon.style.backgroundImage = new StyleBackground(icon);
    }

    public void UpdatePoints(int points)
    {
        _points.text = points.ToString();
    }
}
