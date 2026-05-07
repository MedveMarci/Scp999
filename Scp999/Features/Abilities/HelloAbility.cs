using System.Collections.Generic;
using System.IO;
using CustomPlayerEffects;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Paths;
using MEC;
using RoleAPI.API.Abilities;
using UnityEngine;

namespace Scp999.Features.Abilities;

public class HelloAbility : AbilityBase
{
    public override string Name => "Hello";
    public override string Description => "Greet the players and wave your paw";
    public override KeyCode DefaultKey => KeyCode.F;
    public override float Cooldown => 15f;
    public override bool AutoReleaseLock => false;

    protected override void OnExecute(AbilityExecutionContext context)
    {
        context.LocksDuringExecution = true;
        context.Player.EnableEffect<Ensnared>();
        context.PlayAnimation("HelloAnimation");

        var clipName = Random.Range(0, 2) == 0 ? "hello" : "hi";
        context.SoundFile = Path.Combine(PathManager.Configs.FullName, "Scp999", $"{clipName}.ogg");

        if (context.Animator == null) return;
        Timing.RunCoroutine(CheckEndOfAnimation(context.Player, context));
    }

    private static IEnumerator<float> CheckEndOfAnimation(Player player, AbilityExecutionContext context)
    {
        yield return Timing.WaitForSeconds(0.1f);
        while (context.IsAnimationPlaying())
            yield return Timing.WaitForSeconds(0.3f);
        player.DisableEffect<Ensnared>();
        context.CompleteAnimation();
    }
}