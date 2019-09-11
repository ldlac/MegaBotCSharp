using Godot;
using System;

public class Shot : Area2D
{

  const float SPEED = 280;
  Vector2 velocity = new Vector2();
  int direction = 1;

  public override void _Process(float delta)
  {
    velocity.x = SPEED * delta * direction;
    Translate(velocity);
  }

  public void SetDirection(int dir)
  {
    direction = dir;
  }

  public void _OnVisibilityNotifier2DScreenExited()
  {
    QueueFree();
  }

  public void _OnArea2DAreaEntered(Area2D area)
  {
    if (area.GetName().Contains("Enemy"))
    {
      (area as Enemy).Kill();
    }

    QueueFree();
  }
}
