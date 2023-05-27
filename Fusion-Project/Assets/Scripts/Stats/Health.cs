using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class Health : MonoBehaviour
{   
    private bool isDead;
    [SerializeField] private float totalHp;
    [SerializeField] private float currentHp;
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDie;
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
        if(isDead)
            return;

        if(TryGetComponent(out Shield shield))
        {   
            if(shield.shieldHealth > 0)
            {
                shield.DamageShield(damage);
                damage *= shield.damageReduction;
            }
        }

        OnTakeDamage?.Invoke();
        currentHp = Mathf.Clamp(currentHp - damage, 0, totalHp);
        if (currentHp <= 0)
        {
            isDead = true;
            OnDie?.Invoke();

            if(gameObject.tag != "Player")
                return;
            
            AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.playerDie);
            SceneLoader.Instance.RestartScene(4f);
            
        }
    }

    public void GetHealth(float amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, totalHp);
    }

}
