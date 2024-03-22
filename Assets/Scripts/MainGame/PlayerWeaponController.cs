using Fusion;
using UnityEngine;

public class PlayerWeaponController : NetworkBehaviour, IBeforeUpdate
{
    public Quaternion LocalQuaternionPivotRotate { get; set; }
    [SerializeField] float delayBetweenShots = 0.18f;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] Camera localCamera;
    [SerializeField] Transform pivotToRotate;

    [Networked] public NetworkBool isHoldingShootingKey { get; private set; }
    [Networked(OnChanged = nameof(OnMuzzleEffectStateChanged))] NetworkBool playMuzzleEffect { get; set; }
    [Networked] Quaternion currentPlayerPivotRotation { get; set; }
    [Networked] NetworkButtons buttonsPrev { get; set; }
    [Networked] TickTimer shootCoolDown { get; set; }

    public void BeforeUpdate()
    {
        if (Runner.LocalPlayer == Object.HasInputAuthority)
        {
            var direction = localCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            LocalQuaternionPivotRotate = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input))
        {
            CheckShootInput(input);
            currentPlayerPivotRotation = input.GunPivotRotate;

            buttonsPrev = input.NetworkButtons;
        }
        pivotToRotate.rotation = currentPlayerPivotRotation;
    }

    void CheckShootInput(PlayerData input)
    {
        var currentBtns = input.NetworkButtons.GetPressed(buttonsPrev);

        isHoldingShootingKey = currentBtns.WasReleased(buttonsPrev, PlayerController.PlayerInput.Shoot);


        if (currentBtns.WasReleased(buttonsPrev, PlayerController.PlayerInput.Shoot) && shootCoolDown.ExpiredOrNotRunning(Runner))
        {
            playMuzzleEffect = true;
            shootCoolDown = TickTimer.CreateFromSeconds(Runner, delayBetweenShots);
        }
        else
        {
            playMuzzleEffect = false;
        }
    }

    static void OnMuzzleEffectStateChanged(Changed<PlayerWeaponController> changed)
    {
        var currentStates = changed.Behaviour.playMuzzleEffect;

        changed.LoadOld();
        var oldState = changed.Behaviour.playMuzzleEffect;
        if (oldState != currentStates)
        {
            changed.Behaviour.PlayOrStopMuzzleEffect(currentStates);
        }

    }

    void PlayOrStopMuzzleEffect(bool play)
    {
        if (play)
        {
            muzzleEffect.Play();
        }
        else
        {
            muzzleEffect.Stop();
        }
    }
}

