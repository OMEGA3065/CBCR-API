using DisplayKit.Elements;
using LabApi.Features.Wrappers;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = LabApi.Features.Console.Logger;

namespace CustomRoleLib.DisplayKit;

public partial class RoleIntroScreen : ICanvasReference
{
    public DisplayCanvas DisplayCanvas => Canvas;

    public RoleIntroScreen(Player owner)
    {
        Owner = owner;
        InitializeComponent();
        Canvas.Spawn(Owner.ReferenceHub);
    }

    public Player Owner { get; }

    public string Role { set => RoleName.Content = value; }
    public Color RoleColor { set => RoleName.Text.Color = value; }
    public string Description { set => RoleDescription.Content = value; }
    public object LastOwner { get; set; }
}