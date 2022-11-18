using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Enemy Position", menuName = "Enemy Position")]

public class EnemyPosition : ScriptableObject
{
    [SerializeField] public GameObject _enemyPrefab;
    [SerializeField] public Vector2Int[] _characterStartingTiles;
    [SerializeField] public Vector2[] _characterStartingOrientation;
}
