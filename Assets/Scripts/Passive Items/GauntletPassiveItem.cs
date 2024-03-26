using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentProjectileSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
}
