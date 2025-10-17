using Godot;

/// <summary>
/// A simple enemy that uses a NavigationAgent2D to follow the player.
/// To use this script:
/// - Create a CharacterBody2D with a NavigationAgent2D + AnimationPlayer child.
/// - Assign the "TargetPath" export to the Player node in the inspector.
/// - Ensure your TileMap has a NavigationRegion2D for pathfinding.
/// </summary>
public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed = 50f;
	[Export] public NodePath TargetPath;
	[Export] public int Damage = 10; // Amount of damage to deal on touch
	[Export] public float TeleportInterval = 5f; // Seconds between teleports
	[Export] public Vector2 TeleportOffset = new Vector2(100, 0); // Example offset for teleport relative to player (e.g., 100 pixels to the side)

	private NavigationAgent2D _navAgent;
	private Node2D _target;
	private AnimationPlayer _animPlayer;
	private Timer _teleportTimer;
	private CharacterBody2D _playerBody; // Cache for damage interface call

	public override void _Ready()
	{
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_animPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

		if (TargetPath != null && !TargetPath.IsEmpty)
			_target = GetNode<Node2D>(TargetPath);

		if (_target == null)
			GD.PrintErr("Enemy: TargetPath not assigned or invalid!");

		// Cache player as CharacterBody2D for potential interface calls (assuming Player has TakeDamage method)
		_playerBody = _target as CharacterBody2D;
	}

	private void TeleportNearPlayer()
	{
		if (_target == null) return;

		// Calculate a coordinate near the player (e.g., offset to avoid direct overlap)
		Vector2 teleportPos = new Vector2(574, 264);

		// Optional: Snap to navigation map if needed, but for true teleport, just set position
		GlobalPosition = teleportPos;

		// Update nav agent to recalculate path immediately
		if (_navAgent != null)
		{
			_navAgent.TargetPosition = _target.GlobalPosition;
		}

		GD.Print($"Enemy teleported to {teleportPos}");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null || _navAgent == null)
			return;

		// Update target position to player's current position
		_navAgent.TargetPosition = _target.GlobalPosition;

		// Stop if path is complete or agent reached the goal
		if (_navAgent.IsNavigationFinished())
		{
			Velocity = Vector2.Zero;
			PlayIdleAnimation();
			return;
		}

		// Get next point in path and move toward it
		Vector2 nextPoint = _navAgent.GetNextPathPosition();
		Vector2 direction = (nextPoint - GlobalPosition).Normalized();
		Velocity = direction * Speed;

		// Move and check for collisions (damage on touch)
		if (MoveAndSlide())
		{
			// Check what we collided with
			for (int i = 0; i < GetSlideCollisionCount(); i++)
			{
				CollisionObject2D collided = GetSlideCollision(i).GetCollider() as CollisionObject2D;
				if (collided == _target)
				{
					DealDamageToPlayer();
					// Optional: Teleport away after dealing damage to avoid instant re-damage
					TeleportNearPlayer();
					break;
				}
			}
		}

		// Play proper walking animation if available
		PlayWalkAnimation(direction);
	}

	private void DealDamageToPlayer()
	{
		if (_playerBody == null) return;

		// Assume Player has a TakeDamage(int damage) method
		// If using signals or groups, adjust accordingly
		if (_playerBody.HasMethod("TakeDamage"))
		{
			_playerBody.Call("TakeDamage", Damage);
			GD.Print($"Enemy dealt {Damage} damage to player");
		}
		else
		{
			GD.PrintErr("Player does not have TakeDamage method!");
		}
	}

	private void PlayWalkAnimation(Vector2 direction)
	{
		if (_animPlayer == null)
			return;

		// Decide animation based on direction
		string anim = "";

		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
			anim = direction.X > 0 ? "walk_right" : "walk_left";
		else
			anim = direction.Y > 0 ? "walk_down" : "walk_up";

		// Only play if different from current
		if (_animPlayer.CurrentAnimation != anim)
			_animPlayer.Play(anim);
	}

	private void PlayIdleAnimation()
	{
		if (_animPlayer == null)
			return;

		if (_animPlayer.CurrentAnimation != "idle")
			_animPlayer.Play("idle");
	}
}
