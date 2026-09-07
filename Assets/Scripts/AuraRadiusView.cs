using UnityEngine;

public class AuraRadiusView : MonoBehaviour
{
    public static AuraRadiusView Instance { get; private set; }

    [SerializeField] private int _segments = 32;
    [SerializeField] private float _lineWidth = 0.1f;
    [SerializeField] private Color _color = Color.cyan;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        Instance = this;

        _lineRenderer = GetComponent<LineRenderer>();

        if (_lineRenderer == null)
            _lineRenderer = gameObject.AddComponent<LineRenderer>();

        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _segments;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.endWidth = _lineWidth;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = _color;
        _lineRenderer.endColor = _color;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Show(Vector3 center, float radius)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < _segments; i++)
        {
            float angle = (i / (float)_segments) * Mathf.PI * 2f;

            Vector3 point = center + new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f);

            _lineRenderer.SetPosition(i, point);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}