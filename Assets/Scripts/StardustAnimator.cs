using TMPro;
using UnityEngine;
using System.Collections;

public class StardustAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StardustLogic _logic;
    [SerializeField] private TextMeshProUGUI _stardustDisplay;
    [SerializeField] private GameObject _orb;
    [SerializeField] private GameObject _floatingTextPrefab;
    [SerializeField] private Transform canvasTransform;

    [Header("Settings")]
    [SerializeField] private float _baseRotationSpeed = -10f;
    [SerializeField] private float _rotationSpeedMultiplier = 1f;
    [SerializeField] private float _pulseInterval = 3f;
    [SerializeField] private float _pulseSizeMultiplier = 1f;

    private Vector3 _originalOrbScale;
    private float _currentPulseTimer;

    void Start()
    {
        _originalOrbScale = _orb.transform.localScale;
        _currentPulseTimer = 0;

        _logic.OnStardustChanged += UpdateStardustUI;
    }

    void Update()
    {
        UpdateOrbRotation();
        UpdateOrbPulse();
    }

    public void UpdateStardustUI(int amount)
    {
        _stardustDisplay.text = $"Stardust: {amount}";
    }

    private void UpdateOrbRotation()
    {
        float incomeSpeed = CalculateIncomePerSecond();
        _orb.transform.Rotate(Vector3.forward,
            (_baseRotationSpeed + incomeSpeed * _rotationSpeedMultiplier) * Time.deltaTime);
    }

    private void UpdateOrbPulse()
    {
        _currentPulseTimer -= Time.deltaTime;
        if (_currentPulseTimer <= 0)
        {
            StartCoroutine(PulseOrb());
            _currentPulseTimer = _pulseInterval;
        }
    }

    private IEnumerator PulseOrb()
    {
        float duration = 1.5f;
        Vector3 targetScale = _originalOrbScale * _pulseSizeMultiplier;

        yield return ScaleObject(_orb.transform, _originalOrbScale, targetScale, duration);
        yield return ScaleObject(_orb.transform, targetScale, _originalOrbScale, duration);
    }

    private IEnumerator ScaleObject(Transform target, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = to;
    }

    private float CalculateIncomePerSecond()
    {
        float total = 0f;
        foreach (var shop in _logic.Shops)
            total += shop.Value.income * shop.Value.level;
        return total / StardustLogic.INCOME_INTERVAL;
    }

    public void ShowFloatingText(int amount, bool isPositive)
    {
        Vector3 offset = Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * Vector3.right * Random.Range(100f, 120f);

        GameObject textObj = Instantiate(_floatingTextPrefab,
            _orb.transform.position + offset,
            Quaternion.identity, canvasTransform);

        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = $"{(isPositive ? "+" : "-")}{amount}";
        text.color = isPositive ? Color.green : Color.red;

        StartCoroutine(AnimateText(textObj));
    }

    private IEnumerator AnimateText(GameObject textObj)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = textObj.transform.position;
        Vector3 endPos = startPos + Vector3.up;

        while (elapsed < duration)
        {
            textObj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(textObj);
    }
}