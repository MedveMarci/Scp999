using RoleAPI.API.Abilities;
using UnityEngine;

namespace Scp999.Features.Abilities;

public class YippeeAbility : AbilityBase
{
    public override string Name => "Yippee";

    public override string Description => "Play just a funny Yippee sound";

    public override KeyCode DefaultKey => KeyCode.Q;

    public override float Cooldown => 3f;

    protected override void OnExecute(AbilityExecutionContext context)
    {
        context.LocksDuringExecution = true;
        int value = 1;
        int chance = Random.Range(0, 100);
        if (chance >= 60) value = 2;
        context.SoundResource = $"Audio.yippee-tbh{value}.ogg";
    }
}