using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Win Panel")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text coinsText;

    [Header("Roulette")]
    [SerializeField] private GameObject roulettePanel;
    
    public void EnableWinPanel(int coins)
    {
        title.text = "You Win!";
        winPanel.SetActive(true);
        coinsText.text = coins.ToString();
    }

    public void EnableRoulettePanel()
    {
        roulettePanel.SetActive(true);
    }

    public void DisableRoulettePanel()
    {
        roulettePanel.SetActive(false);
    }
}
