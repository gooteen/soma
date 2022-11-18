using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Squad", menuName = "Squad")]

public class Squad : ScriptableObject
{
    [SerializeField] public GameObject[] _listOfSquadMembers;
}
