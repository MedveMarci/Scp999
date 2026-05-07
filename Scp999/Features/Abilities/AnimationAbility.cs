using System.Collections.Generic;
using System.IO;
using CustomPlayerEffects;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Paths;
using MEC;
using RoleAPI.API.Abilities;
using UnityEngine;

namespace Scp999.Features.Abilities;

public class AnimationAbility : AbilityBase
{
    public override string Name => "Dance";
    public override string Description => "Play a random funny animation";
    public override KeyCode DefaultKey => KeyCode.T;
    public override float Cooldown => 15f;
    public override bool AutoReleaseLock => false;

    protected override void OnExecute(AbilityExecutionContext context)
    {
        context.Player.EnableEffect<Ensnared>();

        var rand = Random.Range(0, 100) + 1;
        context.LocksDuringExecution = true;
        switch (rand)
        {
            // throwing balls
            case > 0 and <= 15:
            {
                context.PlayAnimation("FunAnimation1");
                context.SoundFile = Path.Combine(PathManager.Configs.FullName, "Scp999", "circus.ogg");
            }
                break;

            // Jump x3
            case > 15 and <= 60:
            {
                context.PlayAnimation("FunAnimation2");
                context.SoundFile = Path.Combine(PathManager.Configs.FullName, "Scp999", "funnytoy.ogg");
            }
                break;

            // Shrinking
            case > 60 and <= 90:
            {
                context.PlayAnimation("FunAnimation3");
                context.SoundFile = Path.Combine(PathManager.Configs.FullName, "Scp999", "funnytoy.ogg");
            }
                break;

            // UwU - Secret animation
            case > 90:
            {
                context.PlayAnimation("FunAnimation4");
                context.SoundFile = Path.Combine(PathManager.Configs.FullName, "Scp999", "uwu.ogg");
            }
                break;
        }

        if (context.Animator == null) return;
        context.LocksDuringExecution = true;
        Timing.RunCoroutine(CheckEndOfAnimation(context.Player, context));
    }

    private static IEnumerator<float> CheckEndOfAnimation(Player player, AbilityExecutionContext context)
    {
        yield return Timing.WaitForSeconds(0.1f);
        while (context.IsAnimationPlaying())
            yield return Timing.WaitForSeconds(0.5f);
        player.DisableEffect<Ensnared>();
        context.CompleteAnimation();
    }
}