using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesList : MonoBehaviour
{
  public static SpritesList instance;

  public Sprite[] spritesAr = new Sprite[ 6 ];

  void Awake( )
  {
    MakeInstance();
    InitializeVariables();
  }

  void MakeInstance( )
  {
    if ( instance == null ) instance = this;
  }

  void InitializeVariables( )
  {
  }
}