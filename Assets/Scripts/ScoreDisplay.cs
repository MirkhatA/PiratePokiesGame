using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text[] scoreTexts;

    private void Start()
    {
        UpdateScoreList();
    }

    private void UpdateScoreList()
    {
        List<int> scores = ScoreManager.GetScores();

        // Отображаем первые 3 рекорда, если они есть
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = scores[i].ToString();
            }
            else
            {
                scoreTexts[i].text = "0";  // Если рекордов меньше, чем доступных текстов, заполняем нулями
            }
        }
    }
}