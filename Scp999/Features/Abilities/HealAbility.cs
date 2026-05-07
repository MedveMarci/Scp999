using System.Collections.Generic;
using System.IO;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Paths;
using MEC;
using RoleAPI.API.Abilities;
using UnityEngine;

namespace Scp999.Features.Abilities;

public class HealAbility : AbilityBase
{
    public override string Name => "Heal";
    public override string Description => "Heal all the players near you";
    public override KeyCode DefaultKey => KeyCode.R;
    public override float Cooldown => 60f;
    public override bool LocksDuringExecution => false;

    protected override void OnExecute(AbilityExecutionContext context)
    {
        context.PlayAnimation("HealthAnimation");
        context.SoundFile = Path.Combine(PathManager.Configs.FullName, "Scp999", "health.ogg");

        // Heal all the players in the radius
        foreach (var ply in Player.ReadyList)
        {
            if (context.Player == ply)
                continue;

            if (!(Vector3.Distance(context.Player.Position, ply.Position) <
                  Scp999.Singleton.Config!.Distance)) continue;
            ply.Heal(Scp999.Singleton.Config.HealAmount);
        }

        if (context.Animator == null) return;
        context.LocksDuringExecution = true;
        Timing.RunCoroutine(CheckEndOfAnimation(context));
    }

    private static IEnumerator<float> CheckEndOfAnimation(AbilityExecutionContext context)
    {
        yield return Timing.WaitForSeconds(0.1f);
        while (context.IsAnimationPlaying())
            yield return Timing.WaitForSeconds(0.3f);
        context.CompleteAnimation();
    }
}