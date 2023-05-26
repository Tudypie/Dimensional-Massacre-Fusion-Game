using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class Health : MonoBehaviour
{
    [SerializeField] private float totalHp;
    private float currentHp;

    public UnityEvent OnDie;

    [Header("UI")]

    [SerializeField] private TMP_Text hpText;



    private void Start()
    {
        currentHp = totalHp;
    }

    private void Update()
    {   
        hpText.text = Mathf.RoundToInt((currentHp / totalHp) * 100) + "%";
    }

    public void TakeDamage(float damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0, totalHp);
        if (currentHp <= 0)
        {
            OnDie?.Invoke();
        }
    }

}
