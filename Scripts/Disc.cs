using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{

    public int DiscSize => _size;
    [SerializeField] private int _size;
    public string DiscType => _type;
    [SerializeField] private string _type;



    public Tower TowerOwner
    {
        get { return _towerOwner; }
        set { _towerOwner = value;}
    }

    private Tower _towerOwner;
}
