using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private BananaCatCollisionHandler _bananaCat;

    private int _score = 0;

    private void OnEnable()
    {
        BananaCatCollisionHandler.FruitTakenEvent += OnFruitTaken;
    }

    private void OnDisable()
    {
        BananaCatCollisionHandler.FruitTakenEvent -= OnFruitTaken;
    }

    private void OnFruitTaken()
    {
        _score++;
      //  Debug.Log(_score);
    }
}
