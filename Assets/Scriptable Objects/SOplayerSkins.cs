using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkinData", menuName = "ScriptableObjects/SkinData", order = 1)]
public class SOplayerSkins : ScriptableObject
{
    public Sprite characterSprite;
    public RuntimeAnimatorController animatorController;
}
