using UnityEngine;
using UnityEngine.UI;

public class ExperienceView : MonoBehaviour
{
    public static ExperienceView Instance { get; private set; }

    [SerializeField] private Image _experienceFill;

    [SerializeField] private float _fillSpeed;

    private float _targetFill;
    private float _currentFill;

    private void Awake()
    {
        Instance = this;

        _currentFill = _experienceFill.fillAmount;
        _targetFill = _currentFill;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SetExperience(float experiencePercentage)
    {
        _targetFill = Mathf.Clamp01(experiencePercentage);
    }

    public void SetExperienceInstant(float experiencePercentage)
    {
        _targetFill = Mathf.Clamp01(experiencePercentage);
        _currentFill = _targetFill;
        _experienceFill.fillAmount = _currentFill;
    }

    private void Update()
    {
        _currentFill = Mathf.MoveTowards(
            _currentFill,
            _targetFill,
            _fillSpeed * Time.deltaTime
        );

        _experienceFill.fillAmount = _currentFill;
    }
}