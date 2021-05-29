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
       Initialize();
       //
       gameState = GameState.MENU;

   }
   //
   public void Initialize() 
   {
       // EnemySpawn = false;  
       enemyObject.SetActive(false);
   }
   //
}