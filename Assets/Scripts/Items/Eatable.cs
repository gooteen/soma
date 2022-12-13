using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Eatable", menuName = "Item/Eatable")]
public class Eatable : Item
{
    [SerializeField] private float _healthToResotre;
}
