using System.Xml.Serialization;
using UnityEngine;

public class Door : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(false);
    }

    public void Deactivate()
    {
        gameObject.SetActive(true);
    }
}
