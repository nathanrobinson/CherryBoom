using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;
    public float runMultiplyer = 2.5f;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        var running = Input.GetButton("Fire1");

        var directionalInput = new Vector2((running ? runMultiplyer : 1) * Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            player.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
        }
    }
}
