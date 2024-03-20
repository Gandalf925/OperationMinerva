using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] GameObject camera;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 100f;

    [Networked(OnChanged = nameof(OnNickNameChanged))] public NetworkString<_8> playerName { get; set; }
    [Networked] public NetworkButtons buttonPrev { get; set; }
    float horizontal;
    Rigidbody2D rb2d;

    enum PlayerInput
    {
        None,
        Jump
    }



    public override void Spawned()
    {
        rb2d = GetComponent<Rigidbody2D>();

        SetLocalObject();
    }

    static void OnNickNameChanged(Changed<PlayerController> changed)
    {
        var nickName = changed.Behaviour.playerName;
        changed.Behaviour.SetPlayerNickName(nickName);
    }

    void SetPlayerNickName(NetworkString<_8> nickName)
    {
        playerName = nickName + " " + Object.InputAuthority.PlayerId;
    }

    public void BeforeUpdate()
    {
        if (Runner.LocalPlayer == Object.HasInputAuthority)
        {
            const string HORIZONTAL = "Horizontal";
            horizontal = Input.GetAxis(HORIZONTAL);
        }
    }

    void SetLocalObject()
    {
        if (Runner.LocalPlayer == Object.HasInputAuthority)
        {
            camera.SetActive(true);
            playerNameText.text = GlobalManager.Instance.networkRunnerController.LocalPlayerNickName;
        }
    }

    //FUN
    public override void FixedUpdateNetwork()
    {
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input))
        {
            rb2d.velocity = new Vector2(input.HorizontalInput * moveSpeed, rb2d.velocity.y);

            CheckJumpInput(input);
        }
    }

    void CheckJumpInput(PlayerData input)
    {
        var jumpButton = input.NetworkButtons.GetPressed(buttonPrev);
        if (jumpButton.WasPressed(buttonPrev, PlayerInput.Jump))
        {
            //Jump
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }

        buttonPrev = input.NetworkButtons;
    }

    public PlayerData GetPlayerNetworkInput()
    {
        PlayerData data = new PlayerData();
        data.HorizontalInput = horizontal;
        data.NetworkButtons.Set(PlayerInput.Jump, Input.GetKeyDown(KeyCode.Space));
        return data;
    }
}
