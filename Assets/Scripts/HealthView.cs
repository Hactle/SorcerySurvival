using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthView : MonoBehaviour
{
    private Slider _healthSlider;

    private void Awake()
    {
        _healthSlider = GetComponent<Slider>();
    }

    public void SetHealth(float healthPercentage)
    {
        _healthSlider.value = healthPercentage;
    }
}
