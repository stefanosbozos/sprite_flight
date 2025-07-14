using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [Header("Add Sliders Here")]
    [SerializeField] private Slider slider;

    public void UpdateStatusBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}