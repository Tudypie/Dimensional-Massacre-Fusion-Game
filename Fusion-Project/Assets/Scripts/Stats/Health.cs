using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class Health : MonoBehaviour
{   
    private bool isDead;
    [SerializeField] private float totalHp = 400f;
    [SerializeField] private float currentHp;
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnDie;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Image bloodImage;

    private void Update()
    {   
        if(hpText != null)
            hpText.text = currentHp.ToString("F0");

        if(bloodImage != null && currentHp < 100f)
            bloodImage.color = new Color(1, 1, 1, 1 - (currentHp / totalHp));
        else
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
            SceneLoader.Instance.RestartScene(4f);
            
        }
    }

    public void GetHealth(float amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, totalHp);
    }

}
