using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
  const float SPEED = 100;
  Vector2 UP = new Vector2(0, -1);
  const float GRAVITY = 15;
  const float JUMP_SPEED = -300;

  PackedScene SHOT = ResourceLoader.Load("res://Scenes/Shot.tscn") as PackedScene;
  Vector2 motion = new Vector2();
  int side = 1;
  bool is_attacking = false;
  float shootDelay = 0;
  Vector2 savedPos = new Vector2();

  public override void _Ready()
  {
    Global.Player = this;
    savedPos = Position;
  }

  public override void _Process(float delta)
  {
    UpdateAnimation();
  }

  public override void _PhysicsProcess(float delta)
  {
    Fall(delta);
    Run();
    Jump();
    Shoot();
    MoveAndSlide(motion, UP);

    if (shootDelay > 0)
      shootDelay -= 1;

    if (Position.y > 300)
      Kill();

    if (Position.x > 2108)
      Kill();
  }

  public void Shoot()
  {
    if (Input.IsActionPressed("ui_shoot"))
    {
      is_attacking = true;
      if (shootDelay <= 0)
      {
        shootDelay = 20;
        Area2D shot = SHOT.Instance() as Area2D;
        if (side == 1)
          (shot as Shot).SetDirection(1);
        else
          (shot as Shot).SetDirection(-1);

        GetParent().CallDeferred("add_child", shot);
        shot.Position = GetNode<Position2D>("Position2D").GetGlobalPosition();
      }
    }
    else
    {
      if (shootDelay <= 0)
        is_attacking = false;
    }
  }

  public void Fall(float delta)
  {
    if (IsOnCeiling())
      motion.y = 0;
    if (IsOnFloor())
      motion.y = 0;

    motion.y += GRAVITY;
  }

  public void Jump()
  {
    if (Input.IsActionJustPressed("ui_select") && IsOnFloor())
      motion.y = JUMP_SPEED;
  }

  public void Run()
  {
    var posX = GetNode<Position2D>("Position2D").Position.x;
    var posY = GetNode<Position2D>("Position2D").Position.y;
    if (Input.IsActionPressed("ui_right"))
    {
      motion.x = SPEED;
      if (side == -1) GetNode<Position2D>("Position2D").Position = new Vector2(posX * -1, posY);
      side = 1;
    }
    else if (Input.IsActionPressed("ui_left"))
    {
      motion.x = -SPEED;
      if (side == 1) GetNode<Position2D>("Position2D").Position = new Vector2(posX * -1, posY);
      side = -1;
    }
    else
    {
      motion.x = 0;
    }
  }


  public void Kill()
  {
    Position = savedPos;
  }


  public void UpdateAnimation()
  {
    var anim = GetNode<AnimatedSprite>("AnimatedSprite");
    if (motion.x > 0)
    {
      if (!IsOnFloor())
      {
        anim.Play("Jump");
        anim.FlipH = false;
      }
      else
      {
        if (is_attacking)
          anim.Play("RunShoot");
        else
          anim.Play("Run");
        anim.FlipH = false;
      }

    }
    else if (motion.x < 0)
    {
      if (!IsOnFloor())
      {
        anim.Play("Jump");
        anim.FlipH = true;
      }
      else
      {
        if (is_attacking)
          anim.Play("RunShoot");
        else
          anim.Play("Run");
        anim.FlipH = true;
      }
    }
    else
    {
      if (is_attacking)
      {
        anim.Play("Shoot");
      }
      else
      {
        if (!IsOnFloor() && side == 1)
        {
          anim.Play("Jump");
          anim.FlipH = false;
        }
        else if (!IsOnFloor() && side == -1)
        {
          anim.Play("Jump");
          anim.FlipH = true;
        }
        else
          anim.Play("Idle");
      }
    }

  }


}
