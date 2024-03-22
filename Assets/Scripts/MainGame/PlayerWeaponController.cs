using Fusion;
using UnityEngine;

public class PlayerWeaponController : NetworkBehaviour, IBeforeUpdate
{
    public Quaternion LocalQuaternionPivotRotate { get; set; }
    [SerializeField] Camera localCamera;
    [SerializeField] Transform pivotToRotate;

    [Networked] Quaternion currentPlayerPivotRotation { get; set; }

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
            currentPlayerPivotRotation = input.GunPivotRotate;
        }
        pivotToRotate.rotation = currentPlayerPivotRotation;
    }
}
