using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [Header("Add Sliders Here")]
    [SerializeField] private Slider[] sliders;

    public void UpdateStatusBar(float currentValue, float maxValue)
    {
        foreach (Slider slider in sliders)
        {
            slider.value = currentValue / maxValue;
        }
    }

}
