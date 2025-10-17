using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 70;
	[Export] public int MaxHealth { get; set; } = 100;
	public int CurrentHealth { get; private set; }

	private AnimationPlayer _animations;
	private Hud _hud;

	public override void _Ready()
	{
		_animations = GetNode<AnimationPlayer>("AnimationPlayer");
		CurrentHealth = MaxHealth/2;
		
		CallDeferred(nameof(InitializeHUD));
	}
	
	private void InitializeHUD()
	{
		_hud = GetNode<Hud>("../Hud");
		_hud.UpdateHealth(CurrentHealth, MaxHealth);
	}

	public void Heal(int amount)
	{
		CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
		GD.Print($"Health restored to {CurrentHealth}/{MaxHealth}");
		_hud.UpdateHealth(CurrentHealth, MaxHealth);
	}
	
	public void TakeDamage(int amount)
	{
		CurrentHealth = Math.Max(CurrentHealth - amount, 0);
		GD.Print($"Player took {amount} damage! Health: {CurrentHealth}");
		_hud.UpdateHealth(CurrentHealth, MaxHealth);
	}

	private void HandleInput()
	{
		Vector2 moveDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = moveDirection * Speed;
	}

	private void UpdateAnimation()
	{
		if (Velocity.Length() == 0)
		{
			if (_animations.IsPlaying())
				_animations.Stop();
		}
		else
		{
			string direction = "down";

			if (Velocity.X < 0)
				direction = "left";
			else if (Velocity.X > 0)
				direction = "right";
			else if (Velocity.Y < 0)
				direction = "up";

			_animations.Play("walk_" + direction);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleInput();
		MoveAndSlide();
		UpdateAnimation();
	}
}
