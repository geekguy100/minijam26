using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Controls")]
public class PlayerCoreControls : ScriptableObject
{
    public KeyCode TabMenuKey => tabMenuKey;


    [SerializeField] private KeyCode tabMenuKey;
}
