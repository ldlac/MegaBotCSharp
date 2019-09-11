using Godot;
using System;

public class Explosion : Area2D
{
  public override void _Ready()
  {
    var anim = GetNode<AnimatedSprite>("AnimatedSprite");
    anim.Play("Default");
  }

  public void _OnAnimatedSpriteAnimationFinished()
  {
    QueueFree();
  }
}
