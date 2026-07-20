using UnityEngine;

public class ShipBattle : MonoBehaviour
{
    [SerializeField] private Transform[] deckPoints;
    [SerializeField] private int size;

    public int GetSize()
    {
        return size;
    }


    public Transform[] GetDeckPoints()
    {
        return deckPoints;
    }


    public void Magnet(Vector3 sectorPosition)
    {
        Vector3 deckPosition = deckPoints[0].position;

        Vector3 offsetPosition = sectorPosition - deckPosition;

        transform.position += offsetPosition; 
    }


}