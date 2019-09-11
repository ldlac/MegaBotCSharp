using Godot;
using System;

public class Enemy : Area2D
{
  Player Player;
  PackedScene EXPLOSION = ResourceLoader.Load("res://Scenes/Explosion.tscn") as PackedScene;
  Vector2 motion = new Vector2();
  float vel = 20;
  float xPad = 150;
  float yPad = 150;

  public override void _Ready()
  {
    Player = GetParent().GetNode<Player>("Player");
  }

  public override void _Process(float delta)
  {
    var anim = GetNode<AnimatedSprite>("AnimatedSprite");
    if (Player.Position.x < Position.x)
    {
      motion.x = -vel;
      anim.FlipH = false;
    }
    else if (Player.Position.x > Position.x)
    {
      motion.x = vel * 3;
      anim.FlipH = true;
    }

    if (Player.Position.y < Position.y)
    {
      motion.y = -vel;
    }
    else if (Player.Position.y > Position.y)
    {
      motion.y = vel;
    }

    if (Position.x < Player.Position.x - 300)
      Position = new Vector2(Player.Position.x + xPad, Position.y);

    Translate(motion * delta);
  }

  public void Kill()
  {
    var explosion = EXPLOSION.Instance();
    GetParent().CallDeferred("add_child", explosion);
    (explosion as Explosion).Position = Position;

    if (randf() > 0.5)
      Position = new Vector2(Player.Position.x + xPad, Position.y);
    else
      Position = new Vector2(Player.Position.x + xPad, Position.y);

    if (randf() > 0.5)
      Position = new Vector2(Position.x, Player.Position.y + yPad);
    else
      Position = new Vector2(Position.x, Player.Position.y - yPad);
  }

  public float randf()
  {
    return new Random().Next(0, 100) / 100;
  }

  public void _OnAread2DBodyEntered(PhysicsBody2D body)
  {
    if (body.GetName() == "Player")
    {
      (body as Player).Kill();
    }

    Kill();
  }
}
