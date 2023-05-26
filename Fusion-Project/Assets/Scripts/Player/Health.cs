using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class Health : MonoBehaviour
{
    [SerializeField] private float totalHp;
    [SerializeField] private float currentHp;

    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDie;

    [Header("UI")]
    [SerializeField] private TMP_Text hpText;



    private void Start()
    {
        currentHp = totalHp;
    }

    private void Update()
    {   
        if(hpText != null)
            hpText.text = Mathf.RoundToInt((currentHp / totalHp) * 100) + "%";
    }

    public void TakeDamage(float damage)
    {   
        OnTakeDamage?.Invoke();

        currentHp = Mathf.Clamp(currentHp - damage, 0, totalHp);
        if (currentHp <= 0)
        {
            OnDie?.Invoke();
            enabled = false;
        }
    }

}
