using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] int damage = 10;
    [SerializeField] float moveSpeed = 20;
    [SerializeField] float lifeTimeAmount = 0.8f;
    Collider2D col;
    List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

    [Networked] NetworkBool didHitSomething { get; set; }
    [Networked] TickTimer lifeTimeTimer { get; set; }

    public override void Spawned()
    {
        col = GetComponent<Collider2D>();
        lifeTimeTimer = TickTimer.CreateFromSeconds(Runner, lifeTimeAmount);
    }

    public override void FixedUpdateNetwork()
    {
        if (!didHitSomething)
        {
            CheckIfHitGround();
            ChechIfWeHitAPlayer();
        }


        if (lifeTimeTimer.ExpiredOrNotRunning(Runner) == false && !didHitSomething)
        {
            transform.Translate(transform.right * moveSpeed * Runner.DeltaTime, Space.World);
        }

        if (lifeTimeTimer.Expired(Runner) || didHitSomething)
        {
            lifeTimeTimer = TickTimer.None;
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

    void ChechIfWeHitAPlayer()
    {
        Runner.LagCompensation.OverlapBox(transform.position, col.bounds.size, Quaternion.identity, Object.InputAuthority, hits, playerLayerMask);

        if (hits.Count <= 0) return;

        foreach (var hit in hits)
        {
            if (hit.Hitbox != null)
            {
                var player = hit.Hitbox.GetComponentInParent<NetworkObject>();
                var didNotHitOurOwnPlayer = player.InputAuthority.PlayerId != Object.InputAuthority.PlayerId;

                if (Runner.IsServer && didNotHitOurOwnPlayer)
                {
                    player.GetComponent<PlayerHealth>().Rpc_ReducePlayerHealth(damage);
                }
                didHitSomething = true;
                break;
            }
        }
    }

}
