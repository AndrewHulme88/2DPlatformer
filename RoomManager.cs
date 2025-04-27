using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] List<GameObject> allRooms = new List<GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActiveRoom(string roomName)
    {
        foreach(var room in allRooms)
        {
            room.SetActive(room.name == roomName);
        }
    }
}
