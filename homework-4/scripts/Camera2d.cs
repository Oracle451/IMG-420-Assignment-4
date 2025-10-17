using Godot;
using System;

public partial class Camera2d : Camera2D
{
	[Export] public TileMapLayer GroundLayer { get; set; }

	public override void _Ready()
	{
		Node world = GetParent().GetParent();
		GroundLayer = world.GetNodeOrNull<TileMapLayer>("Ground");

		if (GroundLayer == null)
		{
			GD.PrintErr("GroundLayer is not assigned!");
			return;
		}

		// Get area of used tiles (in tile coordinates)
		Rect2I usedRect = GroundLayer.GetUsedRect();

		// Convert tile coordinates to world space (this accounts for map position)
		Vector2 mapStart = GroundLayer.ToGlobal(GroundLayer.MapToLocal(usedRect.Position));
		Vector2 mapEnd = GroundLayer.ToGlobal(GroundLayer.MapToLocal(usedRect.End));

		// Set camera limits in world coordinates
		LimitLeft = (int)mapStart.X -8;
		LimitTop = (int)mapStart.Y -8;
		LimitRight = (int)mapEnd.X -8;
		LimitBottom = (int)mapEnd.Y -8;

		LimitSmoothed = true;

		GD.Print($"Camera limits set: L{LimitLeft}, R{LimitRight}, T{LimitTop}, B{LimitBottom}");
	}

}
