using UnityEngine;

public class DeployBoard : MonoBehaviour
{
    [SerializeField]private GameObject sectorPrefab;
    [SerializeField]private int sizeSea = 10;
    [SerializeField]private float spacing = 0.5f;
    private SectorView [,] sectors;
    private DeployPresenter deployPresenter;

    
    public void CreateBoard()
    {
        for (int x = 0; x < sizeSea; x++)
        {
            for (int y = 0; y < sizeSea; y++)
            {
                GameObject sectorObject = Instantiate(sectorPrefab, transform);
                sectorObject.transform.localPosition = new Vector3(x * spacing, y * spacing, 0);

                
            }
        }

    }
}