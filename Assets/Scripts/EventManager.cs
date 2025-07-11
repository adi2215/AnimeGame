using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    // Currency
    public int Stardust { get; private set; }
    public TextMeshProUGUI StardustDisplay;

    // Shops: ShopType -> (Level, IncomePerInterval)
    public Dictionary<string, (int level, float income)> Shops = new Dictionary<string, (int level, float income)>()
    {
        { "PotionStall", (0, 1f)},
        { "WandSmith", (0, 2f) }
    };

    // Timers
    private float _lastIncomeTime;
    [Header("Orb Settings")]
    [SerializeField] private GameObject _orb; // Assign in Inspector
    [SerializeField] private float _baseRotationSpeed = -10f;
    [SerializeField] private float _rotationSpeedMultiplier = 1f;
    [SerializeField] private float _basePulseInterval = 3f;
    [SerializeField] private float _pulseSizeMultiplier = 1.5f;

    private float _currentPulseTimer;
    private Vector3 _originalOrbScale;
    private const float INCOME_INTERVAL = 2f; // Every 2 seconds

    void Awake() => Instance = this;

    void Start()
    {
        _originalOrbScale = _orb.transform.localScale;
        _currentPulseTimer = _basePulseInterval;
        StardustDisplay.text = $"Stardust: {Stardust}";
        Stardust = 100; // Starting currency for testing
        _lastIncomeTime = Time.time;
    }

    void Update()
    {
        StardustDisplay.text = $"Stardust: {Stardust}";
        // Generate passive income
        if (Time.time - _lastIncomeTime >= INCOME_INTERVAL)
        {
            AddPassiveIncome();
            _lastIncomeTime = Time.time;
        }

        if (_orb == null) return;

        // Rotate orb based on current income speed
        float incomeSpeed = CalculateTotalIncomePerSecond();
        _orb.transform.Rotate(Vector3.forward,
            (_baseRotationSpeed + incomeSpeed * _rotationSpeedMultiplier) * Time.deltaTime);

        // Pulsing logic
        _currentPulseTimer -= Time.deltaTime;
        if (_currentPulseTimer <= 0)
        {
            PulseOrb();
            _currentPulseTimer = Mathf.Max(0.5f, _basePulseInterval - incomeSpeed * 0.1f);
        }
    }

    private void IncreaseStardust(int delta)
    {
        Stardust += delta;
        // Some Animation
    }

    private void DecreaseStardust(int delta)
    {
        Stardust -= delta;
        // Some Animation
    }

    private void AddPassiveIncome()
    {
        foreach (var shop in Shops)
        {
            IncreaseStardust(Mathf.RoundToInt(shop.Value.income * shop.Value.level));
        }
    }

    private void BuyShop(string shopType)
    {
        int cost = shopType switch
        {
            "PotionStall" => 100,
            "WandSmith" => 250,
            _ => 0
        };

        if (Stardust >= cost)
        {
            DecreaseStardust(cost);
            var shop = Shops[shopType];
            shop.level++;
            Shops[shopType] = shop;
        }
    }

    private float CalculateTotalIncomePerSecond()
    {
        float total = 0f;
        foreach (var shop in Shops)
        {
            total += shop.Value.income * shop.Value.level;
        }
        return total / INCOME_INTERVAL; // Convert to per-second
    }

    //private void PulseOrb()
    //{
    //    LeanTween.scale(_orb, _originalOrbScale * _pulseSizeMultiplier, 0.3f)
    //        .setEase(LeanTweenType.easeOutQuad)
    //        .setOnComplete(() => {
    //            LeanTween.scale(_orb, _originalOrbScale, 0.3f)
    //                .setEase(LeanTweenType.easeInQuad);
    //        });
    //}

    private void PulseOrb()
    {
        StartCoroutine(IPulse());
    }

    IEnumerator IPulse()
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 targetScale = _originalOrbScale * _pulseSizeMultiplier;

        while (elapsed < duration)
        {
            _orb.transform.localScale = Vector3.Lerp(_originalOrbScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            _orb.transform.localScale = Vector3.Lerp(targetScale, _originalOrbScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void AddStardust(int amount) => Stardust += amount;
    public void BuyItem(string itemName) => BuyShop(itemName);
    public void StartRace() => SceneManager.LoadScene("RaceScene");
}