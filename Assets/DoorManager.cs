using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> doors = new List<GameObject>();
    public int doorsBits;
    public int x, y;
    void Start()
    {
        //ReinitialiseDoors(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReinitialiseDoors(int doorsBits)
    {
        for(int i =0; i < 4; i++)
        {
            doors[i].SetActive((doorsBits & (1 << i)) == 0);
        }
    }
}
