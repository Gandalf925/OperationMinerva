using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform pivotGunTransform;
    [SerializeField] Transform pivotCanvasTransform;
    readonly int isMovingHash = Animator.StringToHash("isWalking");
    readonly int isShootingHash = Animator.StringToHash("isShooting");
    bool init;
    bool isFacingRight = true;
    Vector3 originalPlayerScale;
    Vector3 originalCanvasScale;
    Vector3 originalGunPivotScale;

    private void Start()
    {
        originalPlayerScale = transform.localScale;
        originalCanvasScale = pivotCanvasTransform.transform.localScale;
        originalGunPivotScale = pivotGunTransform.transform.localScale;

        const int SHOOTING_LAYER_INDEX = 1;
        animator.SetLayerWeight(SHOOTING_LAYER_INDEX, 1);

        init = true;
    }

    public void RendererVisuals(Vector2 velocity, bool isShooting)
    {
        if (!init) return;

        var isMoving = velocity.x > 0.1f || velocity.x < -0.1f;

        animator.SetBool(isMovingHash, isMoving);
        animator.SetBool(isShootingHash, isShooting);
    }

    public void UpdateScaleTransforms(Vector2 velocity)
    {
        if (!init) return;

        if (velocity.x > 0.1f)
        {
            isFacingRight = true;

        }
        else if (velocity.x < -0.1f)
        {

            isFacingRight = false;
        }

        SetObjectLocalScaleBaseOnDir(gameObject, originalPlayerScale);
        SetObjectLocalScaleBaseOnDir(pivotCanvasTransform.gameObject, originalCanvasScale);
        SetObjectLocalScaleBaseOnDir(pivotGunTransform.gameObject, originalGunPivotScale);
    }

    void SetObjectLocalScaleBaseOnDir(GameObject obj, Vector3 originalScale)
    {
        var yValue = originalScale.y;
        var zValue = originalScale.z;
        var xValue = isFacingRight ? originalScale.x : -originalScale.x;
        obj.transform.localScale = new Vector3(xValue, yValue, zValue);
    }

}
