using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    private const string SCORE_KEY = "Scores";

    public static void SaveScore(int score)
    {
        List<int> scores = GetScores();
        scores.Add(score);

        // Сортируем по убыванию (большее в начале списка)
        scores.Sort((a, b) => b.CompareTo(a));

        // Сохраним обновленный список
        PlayerPrefs.SetString(SCORE_KEY, string.Join(",", scores.ToArray()));
        PlayerPrefs.Save();
    }

    public static List<int> GetScores()
    {
        string scoresString = PlayerPrefs.GetString(SCORE_KEY, "");
        List<int> scores = new List<int>();

        if (!string.IsNullOrEmpty(scoresString))
        {
            string[] scoresArr = scoresString.Split(',');
            foreach (var score in scoresArr)
            {
                if (int.TryParse(score, out int result))
                {
                    scores.Add(result);
                }
            }
        }

        return scores;
    }

    public static void ClearScores()
    {
        PlayerPrefs.DeleteKey(SCORE_KEY);
    }
}