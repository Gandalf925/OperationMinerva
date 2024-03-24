using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float moveSpeed = 20;
    [SerializeField] float lifeTimeAmount = 0.8f;
    Collider2D col;

    [Networked] NetworkBool didHitSomething { get; set; }
    [Networked] TickTimer lifeTimeTimer { get; set; }

    public override void Spawned()
    {
        col = GetComponent<Collider2D>();
        lifeTimeTimer = TickTimer.CreateFromSeconds(Runner, lifeTimeAmount);
    }

    public override void FixedUpdateNetwork()
    {
        CheckIfHitGround();

        if (lifeTimeTimer.ExpiredOrNotRunning(Runner) == false && !didHitSomething)
        {
            transform.Translate(transform.right * moveSpeed * Runner.DeltaTime, Space.World);
        }

        if (lifeTimeTimer.Expired(Runner) || didHitSomething)
        {
            Runner.Despawn(Object);
        }
    }

    void CheckIfHitGround()
    {
        var groundCollider = Runner.GetPhysicsScene2D().OverlapBox(
            transform.position, col.bounds.size, 0, groundLayerMask);

        if (groundCollider != default)
        {
            didHitSomething = true;
        }
    }
}
