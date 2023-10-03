using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public PilaTDA<Disc> towerStack;
    [SerializeField] public bool isStarterTower;
    [SerializeField] private string requiredObjectType; // Tipo de objeto requerido para esta torre

    public string RequiredObjectType => requiredObjectType;

    public int Count => throw new System.NotImplementedException();

    private HashSet<string> discTypesInTower = new HashSet<string>(); // Conjunto para rastrear tipos de objetos en la torre

    // Start is called before the first frame update
    private void Start()
    {
        towerStack = new PilaTDA<Disc>();

        if (isStarterTower)
        {
            for (int i = Level1manager.Instance.GameDiscs.Length - 1; i >= 0; i--)
            {
                Level1manager.Instance.GameDiscs[i].TowerOwner = this;
                towerStack.Push(Level1manager.Instance.GameDiscs[i]);

                // Registrar el tipo de objeto en la torre
                discTypesInTower.Add(Level1manager.Instance.GameDiscs[i].DiscType);
            }
        }
    }


    public void TryPlaceDiscInNewTower(Tower previousTower, Disc incomingDisc)
    {
        if (previousTower.towerStack.Count > 0)
        {
            Disc previousTowerUpperDisc = previousTower.towerStack.Peek();
            print("disco arriba de todo es" + previousTowerUpperDisc.name);

            if (incomingDisc.DiscSize > previousTowerUpperDisc.DiscSize)
            {
                print("No se puede sacar, hay discos por encima de este en esta torre");
            }
            else
            {
                if (towerStack.Count > 0)
                {
                    Disc upperDisc = towerStack.Peek();

                    if (upperDisc.DiscSize >= incomingDisc.DiscSize)
                    {
                        SwapDiscs(previousTower, incomingDisc);
                        print("colocando disco" + incomingDisc.name + " en nueva torre" + this.name);
                    }
                    else
                    {
                        print("No se puede");
                    }
                }
                else
                {
                    // Verifica si hay discos en la torre destino con el mismo tamaño
                    bool hasDiscsWithSameSize = towerStack.Any(disc => disc.DiscSize == incomingDisc.DiscSize);

                    if (hasDiscsWithSameSize || incomingDisc.DiscSize == previousTowerUpperDisc.DiscSize)
                    {
                        SwapDiscs(previousTower, incomingDisc);
                        print("colocando disco" + incomingDisc.name + " en nueva torre" + this.name);
                    }
                    else
                    {
                        print("No se puede");
                    }
                }
            }
        }
        else
        {
            print("No hay Discos disponibles");
        }
    }



    // Update is called once per frame
    void SwapDiscs(Tower previousTower, Disc incomingDisc)
    {
        towerStack.Push(incomingDisc);
        previousTower.towerStack.Pop();

        incomingDisc.transform.parent = transform.GetChild(0);

        incomingDisc.TowerOwner = this;
    }

}
