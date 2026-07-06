using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countShips;
    [SerializeField] private int size;


    public int GetSize()
    {
        return size;
    }


    public void SetCountShips(int count)
    {
        countShips.text = $"x{count}";
    }

}
