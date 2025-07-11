using UnityEngine;

public class EventBridge : MonoBehaviour
{
    [SerializeField] private StardustLogic _logic;
    [SerializeField] private StardustAnimator _animator;

    void OnEnable()
    {
        _logic.OnStardustChanged += _animator.UpdateStardustUI;
        _logic.OnShopPurchased += (shop, cost) =>
            _animator.ShowFloatingText(cost, false);
    }

    void OnDisable()
    {
        _logic.OnStardustChanged -= _animator.UpdateStardustUI;
    }
}
