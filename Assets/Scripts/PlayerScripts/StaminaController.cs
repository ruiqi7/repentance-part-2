using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [SerializeField] private Slider staminaBar;

    public void LoadStamina(float stamina)
    {
        staminaBar.value = stamina;
    }
}
