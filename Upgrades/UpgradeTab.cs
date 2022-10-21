using System;
using Game.Scripts.UtilsSubmodule.Upgrades;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UI.ResourcesView;

public class UpgradeTab : MonoBehaviour
{
    public static UpgradeTab Instance;

    private void Awake()
    {
        Instance = this;
    }
}