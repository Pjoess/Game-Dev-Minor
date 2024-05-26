using UnityEngine;
using UnityEngine.InputSystem;

public class AllControlls
{
    public PlayerInput playerInput;


    public Controll Attack;
    public Controll Mortar;

    public Controll Forward;
    public Controll Backward;
    public Controll Left;
    public Controll Right;

    public Controll Dodge;
    public Controll Sprint;
    public Controll ToggleSprint;

    public Controll MoveCamera;
    public Controll Zoom;

    // public AllControlls(PlayerInput playerInput){
    //     this.playerInput = playerInput;
    // }

    public AllControlls(){
        Attack = new Controll("Attack", playerInput);
        Mortar = new Controll("Mortar", playerInput);
        Forward = new Controll("Forward", playerInput);
        Backward = new Controll("Backward", playerInput);
        Left = new Controll("Left", playerInput);
        Right = new Controll("Right", playerInput);
        Dodge = new Controll("Dodge", playerInput);
        Sprint = new Controll("Sprint", playerInput);
        ToggleSprint = new Controll("ToggleSprint", playerInput);
        MoveCamera = new Controll("MoveCamera", playerInput);
        Zoom = new Controll("Zoom", playerInput);
    }

    public void Update(){
        Attack.UpdateVariables();
        Mortar.UpdateVariables();
        Forward.UpdateVariables();
        Backward.UpdateVariables();
        Left.UpdateVariables();
        Right.UpdateVariables();
        Dodge.UpdateVariables();
        Sprint.UpdateVariables();
        ToggleSprint.UpdateVariables();
        MoveCamera.UpdateVariables();
        Zoom.UpdateVariables();
    }
}