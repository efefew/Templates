using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _content;

    private void Awake()
    {
        _health.OnAddBar.AddListener(CreateBar);
    }

    private void CreateBar(HealthBar healthBar)
    {
        SliderBarUI sliderBarUI = Instantiate(healthBar.Configuration.Prefab, _content);
        sliderBarUI.Build(healthBar);
    }
}
