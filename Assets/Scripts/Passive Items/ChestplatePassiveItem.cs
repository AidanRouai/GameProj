using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestplatePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentRecovery *= 1 + passiveItemData.Multiplier / 100f;
    }
}
