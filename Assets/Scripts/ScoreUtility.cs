using System.Collections.Generic;
using UnityEngine;

public class ScoreUtility
{

    private static List<float> playerScores = new List<float> { 3, 2, 17, 8, 9, 6, 9, 7, 10, 4, 19 }; // trellosta + oma


    public static void CalculateMeanAndSD()
    {
        float mean = CalculateMean(playerScores);
        Debug.Log("Mean:" + mean);
        Debug.Log("Standard Deviation:" + CalculateStandardDeviation(playerScores, mean));
    }

    static float CalculateMean(List<float> scores)
    {
        float sum = 0;
        foreach (float score in scores) sum += score;
        return sum / scores.Count;
    }

    static float CalculateStandardDeviation(List<float> scores, float mean)
    {
        float sum = 0;
        foreach (float score in scores) sum += (score - mean) * (score - mean);
        return Mathf.Sqrt(sum / scores.Count);
    }

    public static float NormalDistributionCDF(float x, float mean, float stdDev)
    {
        float z = (x - mean) / (stdDev * Mathf.Sqrt(2));
        return 0.5f * (1 + Erf(z));
    }

    private static float Erf(float x)
    {
        float sign = Mathf.Sign(x);
        x = Mathf.Abs(x);

        float a1 = 0.254829592f, a2 = -0.284496736f, a3 = 1.421413741f;
        float a4 = -1.453152027f, a5 = 1.061405429f;
        float p = 0.3275911f;

        float t = 1.0f / (1.0f + p * x);
        float y = 1.0f - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Mathf.Exp(-x * x);

        return sign * y;
    }
}
