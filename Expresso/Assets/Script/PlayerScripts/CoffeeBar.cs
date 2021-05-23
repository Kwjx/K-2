using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeBar : MonoBehaviour
{
    public Slider m_CoffeeSlider;
    public float coffee = 0;

    PlayerAbilityState m_PlayerAbilityState;

    private void Start()
    {
        m_PlayerAbilityState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilityState>();
    }

    private void Update()
    {
        m_CoffeeSlider.value = coffee;
        StartCaffineRush();
    }
    public void StartCaffineRush()
    {

        if (coffee <= 0.1)
        {
            m_PlayerAbilityState.abilityActive = PlayerAbilityState.AbilityActive.notActive;
        }

        else if (m_PlayerAbilityState.abilityActive == PlayerAbilityState.AbilityActive.isActive)
        {
            if (coffee >= 0.1)
            {
                coffee -= 5 * Time.deltaTime;
            }
        }
    }
}
