using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
   private static GameManager _instance;
   public enum GameState { MENU, GAME, PAUSE, ENDGAME };
   public GameState gameState { get; private set; }
   public delegate void ChangeStateDelegate();
   public static ChangeStateDelegate changeStateDelegate;

   //
   public GameObject enemyObject;
   public bool EnemySpawn;
   // 
   public GameObject Carta1Object;
   public GameObject Carta2Object;
   public GameObject Carta3Object;
   public GameObject Carta4Object;
   public GameObject Carta5Object;
   public bool Carta1Captured;  
   public bool Carta2Captured;  
   public bool Carta3Captured;  
   public bool Carta4Captured;  
   public bool Carta5Captured; 
   public bool playerFirstUpdate; 

   public void ChangeState(GameState nextState)
   {
       gameState = nextState;
       changeStateDelegate();
   }


   public static GameManager GetInstance()
   {
       if(_instance == null)
       {
           _instance = new GameManager();
       }

       return _instance;
   }
   private GameManager()
   {
       //
       enemyObject = GameObject.Find("Inimigo");
       //
       Carta1Object = GameObject.Find("Carta-1");
       Carta2Object = GameObject.Find("Carta-2");
       Carta3Object = GameObject.Find("Carta-3");
       Carta4Object = GameObject.Find("Carta-4");
       Carta5Object = GameObject.Find("Carta-5");
       //
       Initialize();
       //
       gameState = GameState.MENU;

   }
   //
   public void Initialize() 
   {
       // EnemySpawn = false;  
       playerFirstUpdate = true;
       enemyObject.SetActive(false);
       Carta1Captured = false;  
       Carta2Captured = false;  
       Carta3Captured = false;  
       Carta4Captured = false;  
       Carta5Captured = false;  
       Carta1Object.SetActive(true);
       Carta2Object.SetActive(true);
       Carta3Object.SetActive(true);
       Carta4Object.SetActive(true);
       Carta5Object.SetActive(true);
   }
   //

    public int GetPontos()
    {
        int pontos = 0;
        if (Carta1Captured) { pontos++; };
        if (Carta2Captured) { pontos++; };
        if (Carta3Captured) { pontos++; };
        if (Carta4Captured) { pontos++; };
        if (Carta5Captured) { pontos++; };
        return pontos;
    }

}