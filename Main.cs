using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int _score;



    public void NewGame()
	{
        _score = 0;
        
        GetNode<Timer>("StartTimer").Start();

        HUD hud = GetNode<HUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");
  
        
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        GetTree().CallGroup("Mobs", Node.MethodName.QueueFree);
        GetNode<AudioStreamPlayer>("Music").Play();
    }

    public void GameOver()
    {
        
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<Timer>("MobTimer").Stop();
        GetNode<AudioStreamPlayer>("Music").Stop();
        GetNode<AudioStreamPlayer>("DeathSound").Play();
        GetNode<HUD>("HUD").ShowGameOver();

    }


    private void OnScoreTimerTimeout()
	{
		_score++;
        GetNode<HUD>("HUD").UpdateScore(_score);
    }

	private void OnStartTimerTimeout()
	{
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    private void OnMobTimerTimeout()
	{
        Mob mob = MobScene.Instantiate<Mob>();

        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
     
        mob.Position = mobSpawnLocation.Position;
       
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

        AddChild(mob);

    }
}
