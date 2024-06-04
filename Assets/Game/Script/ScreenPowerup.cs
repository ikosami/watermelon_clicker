using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPowerup : MonoBehaviour
{
    [SerializeField] private PowerUpList powerUpList;
    private void OnEnable()
    {
        powerUpList.ChangeLock();
    }
}
