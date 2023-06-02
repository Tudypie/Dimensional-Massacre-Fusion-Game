using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class Health : MonoBehaviour
{   
    private bool isDead;
    [SerializeField] public float totalHp = 400f;
    [SerializeField] public float currentHp;
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDie;

    [Header("ONLY FOR PLAYER")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Image bloodImage;
    [SerializeField] private Image healthBar;

    private void Start()
    {
        currentHp = PlayerPrefs.GetFloat("Health");
    }

    private void Update()
    {       
        if(hpText != null)
            hpText.text = currentHp.ToString("F0");

        if(healthBar != null)
            healthBar.fillAmount = currentHp / totalHp;

        if(bloodImage != null && currentHp < 100f)
            bloodImage.color = new Color(1, 1, 1, Mathf.Min(1 - (currentHp / 100f), 0.7f));
        else if(bloodImage != null && currentHp >= 100f)
            bloodImage.color = new Color(1, 1, 1, 0);
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
        Debug.Log(gameObject.name + " took " + damage + " damage");
        
        if (currentHp <= 0)
        {
            isDead = true;
            OnDie?.Invoke();

            if(gameObject.tag != "Player")
                return;
            
            AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.playerDie);
            PlayerStats.Instance.AddDeath();
            SceneLoader.Instance.RestartScene(4f);
            
        }
    }

    public void GetHealth(float amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, totalHp);
    }

    public void SaveHealthAmount()
    {
        PlayerPrefs.SetFloat("Health", currentHp);
    }

}
