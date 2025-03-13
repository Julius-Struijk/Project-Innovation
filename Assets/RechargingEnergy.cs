using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargingEnergy : MonoBehaviour
{
    UIManager uiManager;
    bool recharging = false;
    [SerializeField]
    float rechargingInterval = 0;
    [SerializeField]
    float amountOfEnergy = 0;

    private void Awake()
    {
        uiManager= GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    public void ToggleRecharging()
    {
        if (recharging)
        {
            recharging = false;
            return;
        }
        else
        {
            recharging = true;
        }
    }

    float lastRechargeTime = 0;
    float enableTime = 0;

    private void Update()
    {
        if (recharging)
        {
            if (lastRechargeTime == 0 && Time.time - enableTime > rechargingInterval)
            {
                uiManager.AddEnergy(amountOfEnergy);
                lastRechargeTime = Time.time;
                return;
            }

            if (Time.time - lastRechargeTime > rechargingInterval)
            {
                uiManager.AddEnergy(amountOfEnergy);
                lastRechargeTime = Time.time;
            }
        }
    }

    private void OnEnable()
    {
        lastRechargeTime = 0;
        enableTime = Time.time;
    }
}
