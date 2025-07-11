using UnityEngine;
using System.Collections.Generic;
using System;

public class StardustLogic : MonoBehaviour
{
    public int Stardust { get; private set; } = 100;
    public Dictionary<string, (int level, float income)> Shops { get; private set; } = new()
    {
        { "PotionStall", (0, 1f) },
        { "WandSmith", (0, 2f) }
    };

    // Events
    public event Action<int> OnStardustChanged;
    public event Action<string, int> OnShopPurchased;

    public const float INCOME_INTERVAL = 2f;
    private float _lastIncomeTime;

    void Start()
    {
        _lastIncomeTime = Time.time;
        OnStardustChanged?.Invoke(Stardust);
    }

    public StardustAnimator _animator;

    void Update()
    {
        if (Time.time - _lastIncomeTime >= INCOME_INTERVAL)
        {
            AddPassiveIncome();
            _lastIncomeTime = Time.time;
        }
    }

    public void AddStardust(int amount)
    {
        Stardust += amount;
        OnStardustChanged?.Invoke(Stardust);
        _animator.ShowFloatingText(amount, true);
    }

    public void BuyShop(string shopType)
    {
        int cost = shopType switch
        {
            "PotionStall" => 100,
            "WandSmith" => 250,
            _ => 0
        };

        if (Stardust >= cost)
        {
            Stardust -= cost;
            var shop = Shops[shopType];
            shop.level++;
            Shops[shopType] = shop;

            OnStardustChanged?.Invoke(Stardust);
            OnShopPurchased?.Invoke(shopType, cost);
        }
    }

    private void AddPassiveIncome()
    {
        foreach (var shop in Shops)
        {
            if (shop.Value.level > 0)
                AddStardust(Mathf.RoundToInt(shop.Value.income * shop.Value.level));
        }
    }
}