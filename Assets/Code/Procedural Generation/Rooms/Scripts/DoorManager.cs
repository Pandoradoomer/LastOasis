using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<GameObject> doors = new List<GameObject>();
    List<bool> doorOpen;

    [SerializeField]
    List<Sprite> doorOpenSprites;
    [SerializeField]
    List<Sprite> doorClosedSprites;
    [SerializeField]
    List<Sprite> wallSprites;
    public int doorsBits;
    public int x, y;
    void Awake()
    {
        doorOpen = new List<bool>();
        for (int i = 0; i < doors.Count; i++)
            doorOpen.Add(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReinitialiseDoors(int doorsBits)
    {
        for(int i =0; i < 4; i++)
        {
            bool isDoor = (doorsBits & (1 << i)) != 0;
            bool isOpen = doorOpen[i];
            SpriteRenderer sr = doors[i].GetComponent<SpriteRenderer>();
            if(isDoor)
            {
                if (isOpen)
                {
                    sr.sprite = doorOpenSprites[i];
                    doors[i].layer = 6;
                }

                else
                {
                    sr.sprite = doorClosedSprites[i];
                    doors[i].layer = 6;
                }
            }
            else
            {
                sr.sprite = wallSprites[i];
                doors[i].layer = 8;
            }
        }
    }
}
