using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [Header("Health Fill")]
    [SerializeField] private Image _redHealthFill;
    [SerializeField] private Image _whiteHealthFill;

    [Header("Health Change")]
    [SerializeField] private float _whiteDelay;
    [SerializeField] private float _whiteSpeed;

    private float _targetFill;
    private float _redCurrent;
    private float _whiteCurrent;
    private float _whiteDelayTimer;

    private void Awake()
    {
        _redCurrent = _redHealthFill.fillAmount;
        _whiteCurrent = _whiteHealthFill.fillAmount;
        _targetFill = _redCurrent;
    }

    public void SetHealth(float healthPercentage)
    {
        healthPercentage = Mathf.Clamp01(healthPercentage);

        if(healthPercentage < _targetFill)
        {
            _whiteDelayTimer = _whiteDelay;
            _whiteCurrent = _redCurrent;
        }
        
        _targetFill = healthPercentage;
    }

    void Update()
    {
        _redCurrent = _targetFill;

        _redHealthFill.fillAmount = _redCurrent;

        if (_whiteDelayTimer > 0f)
        {
            _whiteDelayTimer -= Time.deltaTime;
        }
        else
        {
            _whiteCurrent = Mathf.MoveTowards(
                _whiteCurrent,
                _redCurrent,
                _whiteSpeed * Time.deltaTime
            );

            _whiteHealthFill.fillAmount = _whiteCurrent;
        }
    }
}
