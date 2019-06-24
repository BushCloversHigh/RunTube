using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public static float intervalDifficulty = 0f, probabiltyDifficulty = 0, speedDifficulty = 0;

    private float timeCount = 0, timeInterval = 0;

    [SerializeField] private float maxSecond = 50;
    [SerializeField] private float maxIntervalDifficulty = 0.5f, maxProbabiltyDifficulty = 0.35f, maxSpeedDifficulty = 4f;
    private float addDeltaIntervalDifficulty, addDeltaProbabiltyDifficulty, addDeltaSpeedDifficulty;

    [SerializeField] private Material tubeMaterial;
    [SerializeField] private Color[] difficultyColor;
    private Color currentColor;

    [SerializeField] private ReflectionProbe reflectionProbe;
    [SerializeField] private float[] difficultyIntensity;
    private float currentIntensity;

    private void Awake ()
    {
        intervalDifficulty = 0f;
        probabiltyDifficulty = 0f;
        speedDifficulty = 0f;

        addDeltaIntervalDifficulty = maxIntervalDifficulty / maxSecond;
        addDeltaProbabiltyDifficulty = maxProbabiltyDifficulty / maxSecond;
        addDeltaSpeedDifficulty = maxSpeedDifficulty / maxSecond;

        currentColor = difficultyColor[0];
        tubeMaterial.SetColor ("_EmissionColor", currentColor * 5f);
    }

    private int i = 0, currentDifficulty = 0;
    private void Update ()
    {
        timeCount += Time.deltaTime;
        if (timeCount < 10f)
        {
            return;
        }
        timeInterval += Time.deltaTime;
        speedDifficulty += addDeltaSpeedDifficulty * Time.deltaTime;
        if (timeCount < 10 + maxSecond)
        {
            if (timeInterval > 1f)
            {
                timeInterval = 0;
                intervalDifficulty += addDeltaIntervalDifficulty;
                probabiltyDifficulty += addDeltaProbabiltyDifficulty;

                i++;
                if (i >= (maxSecond / 2.5f))
                {
                    i = 0;
                    if (currentDifficulty < 2)
                    {
                        currentDifficulty++;
                    }
                }
            }
            currentColor = Color.Lerp (currentColor, difficultyColor[currentDifficulty], Time.deltaTime * 0.5f);
            tubeMaterial.SetColor ("_EmissionColor", currentColor * 5f);
            currentIntensity = Mathf.Lerp (currentIntensity, difficultyIntensity[currentDifficulty], Time.deltaTime * 0.5f);
            reflectionProbe.intensity = currentIntensity;
        }
    }

    private Color DivisionColorElement(Color dividedColor, float divider, float alpha)
    {
        Color newColor;
        newColor.r = dividedColor.r / divider;
        newColor.g = dividedColor.g / divider;
        newColor.b = dividedColor.b / divider;
        newColor.a = alpha;
        return newColor;
    }
}
