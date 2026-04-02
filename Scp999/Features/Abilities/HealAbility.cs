using System.Collections.Generic;
using System.IO;
using CustomPlayerEffects;
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
        context.Animator?.Play("HealthAnimation");
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
        
        if (context.Animator == null || context.Animator.Animators.Count == 0) return;
        context.LocksDuringExecution = true;
        Timing.RunCoroutine(CheckEndOfAnimation(context.Animator.Animators[0], context));
    }
    
    private static IEnumerator<float> CheckEndOfAnimation(Animator animator,
        AbilityExecutionContext context)
    {
        yield return Timing.WaitForSeconds(0.1f);
        while (true)
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("HealthAnimation"))
            {
                context.CompleteAnimation();
                yield break;
            }

            yield return Timing.WaitForSeconds(0.3f);
        }
    }
}