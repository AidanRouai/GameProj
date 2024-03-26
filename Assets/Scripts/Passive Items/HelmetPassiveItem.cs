using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentStrength *= 1 + passiveItemData.Multiplier / 100f;
    }
}
