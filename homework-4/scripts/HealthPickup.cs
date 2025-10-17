using Godot;
using System;

public partial class HealthPickup : Area2D
{
	[Export] public int HealAmount { get; set; } = 20;
	[Export] public float RespawnTime { get; set; } = 5f; // Seconds before respawn after pickup

	private CollisionShape2D _collisionShape;
	private Sprite2D _sprite; // Assuming you have a Sprite2D for visuals; adjust if different (e.g., AnimatedSprite2D)
	private Vector2 _originalPosition;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;

		// Cache child nodes (adjust names if your scene uses different ones)
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_sprite = GetNodeOrNull<Sprite2D>("Sprite2D"); // Or "Sprite" if that's the name

		if (_collisionShape == null)
			GD.PrintErr("HealthPickup: No CollisionShape2D found! Respawn won't disable collision properly.");

		_originalPosition = GlobalPosition;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			// If the player has a "Heal" method, call it
			if (body.HasMethod("Heal"))
			{
				body.Call("Heal", HealAmount);
			}

			// "Pick up" by hiding and disabling, then respawn
			CallDeferred(nameof(PickupAndRespawn)); // Defer to avoid physics query flush error
		}
	}

	private void PickupAndRespawn()
	{
		// Disable visibility and collision (safe now post-query)
		if (_sprite != null)
			_sprite.Visible = false;

		if (_collisionShape != null)
			_collisionShape.Disabled = true;

		// Start timer for respawn
		var timer = new Timer();
		timer.WaitTime = RespawnTime;
		timer.OneShot = true;
		timer.Connect("timeout", new Callable(this, nameof(Respawn)));
		AddChild(timer);
		timer.Start();

		GD.Print($"HealthPickup collected! Respawning in {RespawnTime} seconds.");
	}

	private void Respawn()
	{
		// Re-enable everything (also deferred if needed, but timer timeout is safe)
		if (_sprite != null)
			_sprite.Visible = true;

		if (_collisionShape != null)
			_collisionShape.Disabled = false;

		// Reset position in case it was moved
		GlobalPosition = _originalPosition;

		GD.Print("HealthPickup respawned!");
	}
}
