using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] Image fillAmountImage;
    [SerializeField] TMP_Text healthAmountText;

    [Networked(OnChanged = nameof(HealthAmountChanged))] int currentHealthAmount { get; set; }

    const int MAX_HEALTH_AMOUNT = 100;

    public override void Spawned()
    {
        currentHealthAmount = MAX_HEALTH_AMOUNT;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
    public void Rpc_ReducePlayerHealth(int damage)
    {
        currentHealthAmount -= damage;
    }

    static void HealthAmountChanged(Changed<PlayerHealth> changed)
    {
        var currentHealth = changed.Behaviour.currentHealthAmount;

        changed.LoadOld();
        var oldHealthAmount = changed.Behaviour.currentHealthAmount;

        if (currentHealth != oldHealthAmount)
        {
            changed.Behaviour.UpdateVisuals(currentHealth);

            if (currentHealth != MAX_HEALTH_AMOUNT)
            {
                changed.Behaviour.PlayerGotHit(currentHealth);
            }
        }
    }

    void UpdateVisuals(int healthAmount)
    {
        var num = (float)healthAmount / MAX_HEALTH_AMOUNT;
        fillAmountImage.fillAmount = num;
        healthAmountText.text = $"{healthAmount}/{MAX_HEALTH_AMOUNT}";
    }

    void PlayerGotHit(int healthAmount)
    {
        var isLocalPlayer = Runner.LocalPlayer == Object.HasInputAuthority;

        if (!isLocalPlayer) return;

        Debug.Log("Player got hit");

        if (healthAmount <= 0)
        {
            Debug.Log("Player id Dead");
        }

    }
}
