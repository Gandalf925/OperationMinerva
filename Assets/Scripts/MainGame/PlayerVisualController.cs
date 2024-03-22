using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] Animator animator;
    readonly int isMovingMesh = Animator.StringToHash("isWalking");

    public void RendererVisuals(Vector2 velocity)
    {
        var isMoving = velocity.x > 0.1f || velocity.x < -0.1f;

        animator.SetBool(isMovingMesh, isMoving);
    }
}
