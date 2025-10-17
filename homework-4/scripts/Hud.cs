using Godot;
using System;

public partial class Hud : CanvasLayer
{
	private Label _HealthLabel;

	public override void _Ready()
	{
		_HealthLabel = GetNode<Label>("MarginContainer/Label");
	}

	public void UpdateHealth(int currentHealth, int maxHealth)
	{
		_HealthLabel.Text = $"Health: {currentHealth}/{maxHealth}";
	}
}
