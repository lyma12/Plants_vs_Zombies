using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class Cache : Singleton<Cache>
{
   private Dictionary<GameObject, IGround> dictionaryGrid1x1 = new Dictionary<GameObject, IGround>();
   private Dictionary<GameObject, Enemy> dictionaryEnemy = new Dictionary<GameObject, Enemy>();
   private Dictionary<GameObject, GameUnitListenerPointerEvent> dictionaryPointerEvent = new Dictionary<GameObject, GameUnitListenerPointerEvent>();
   public Enemy GetEnemy(GameObject otherEnemy){
      if(dictionaryEnemy.ContainsKey(otherEnemy)) return dictionaryEnemy[otherEnemy];
      if(otherEnemy.TryGetComponent<Enemy>(out Enemy enemy)){
         dictionaryEnemy[otherEnemy] = enemy;
         return enemy;
      }
      return null;

   }
   public IGround GetIGround(GameObject otherGrid)
   {
      if (dictionaryGrid1x1.ContainsKey(otherGrid)) return dictionaryGrid1x1[otherGrid];
      IGround newGrid1x1;
      if(otherGrid.CompareTag(AppContanst.TAG_GRID)){
         newGrid1x1 = otherGrid.GetComponent<Grid1x1>();
      }else{
         newGrid1x1 = otherGrid.GetComponent<Pot>();
      }
      dictionaryGrid1x1.Add(otherGrid, newGrid1x1);
      return newGrid1x1;
   }
   public GameUnitListenerPointerEvent GetGameUnitListenerPointerEvent(GameObject other){
      if(dictionaryPointerEvent.ContainsKey(other)) return dictionaryPointerEvent[other];
      GameUnitListenerPointerEvent gameUnitListener = other.GetComponent<GameUnitListenerPointerEvent>();
      dictionaryPointerEvent[other] = gameUnitListener;
      return gameUnitListener;
   }
}