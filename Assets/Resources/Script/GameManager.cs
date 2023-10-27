using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("UserInfo")]
    public Player player;
    public DamageText numberText;
    public Text currentHp;
    public Text fullHp;
    public Image hpBar;
    public float userSpeed;
    public GameGold gameGold;
    public Inventory inventory;
    [Header("Skill")]
    public Thunder thunder;
    public Transform thunderPos;
}
