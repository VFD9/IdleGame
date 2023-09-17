using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Transform UI;
    [Header("UserInfo")]
    public Player player;
    public DamageText numberText;
    public Text currentHp;
    public Text fullHp;
    public Image Hpbar;
    public float userSpeed;
    public Inventory inventory;
    public GameGold gameGold;
    [Header("Skill")]
    public Thunder Thunder;
    public Transform thunderPos;
    [Header("Sound")]
    public AudioSource buttonClickSound;
    public AudioSource statusClickSound;
}
