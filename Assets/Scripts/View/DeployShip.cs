using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DeployShip : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private LayerMask shipLayer;
    [SerializeField] private Transform[] deckPoints;
    [SerializeField] private int durability;
    private Vector3 lastValidPosition;
    private bool hasValidPosition;
    private bool isDeploy;
    private Vector3 offset;
    private bool isClicked;
    private DeployPresenter deployPresenter;

    public void Init(DeployPresenter deployPresenter)
    {
        this.deployPresenter = deployPresenter;
    }


    public int GetDurability()
    {
        return durability;
    }


    public void Deploy()
    {
        List<DeploySector> coord = SearchSectors();

        deployPresenter.DeployShip(coord);
    }


    public bool IsDeploy()
    {
        return isDeploy;
    }

    public void UnDeploy()
    {
        isDeploy = false;
    }


    public void Rotate()
    {
        Vector3 deckPosition = deckPoints[0].position;
        transform.RotateAround(deckPosition, Vector3.forward, 90);
    }


    private void Start()
    {
        lastValidPosition = transform.position;
        isDeploy = false;
    }

    
    public void SyncPlace(Collider2D target, bool isVertical)
    {
        if (isVertical)
            Rotate();

        Magnet(target);

        lastValidPosition = transform.position;
        isDeploy = true;
    }

     public void Magnet(Collider2D target)
    {
        Vector3 offsetSnap = target.transform.position - deckPoints[0].position;
        offsetSnap.z = 0;
            
        transform.position += offsetSnap;
    }


    private void OnMouseDown()
    {
        isClicked = true;

        Vector3 mousePosition = GetMouseWorldPosition();
        offset = transform.position - mousePosition;

        if (isDeploy)
            SetStatusPlace(StatusSector.Empty);
    }


    private void OnMouseDrag()
    {
        if (!(isClicked))
            return;

        Vector3 shipPosition = GetMouseWorldPosition() + offset;
        shipPosition.z = transform.position.z;

        transform.position = shipPosition;

        Collider2D targetLeft = Physics2D.OverlapPoint(deckPoints[0].position, cellLayer);
        Collider2D targetRight = Physics2D.OverlapPoint(deckPoints[deckPoints.Length-1].position, cellLayer);       

        DefineValid(targetLeft, targetRight);
    }


    private void OnMouseUp()
    {
        isClicked = false;

        if (hasValidPosition)
        {
            lastValidPosition = transform.position;
            SetStatusPlace(StatusSector.Ship); 
            isDeploy = true;
        }
        
        else
            transform.position = lastValidPosition;     
    }


    private void OnMouseOver()
    {
        if(Mouse.current.rightButton.wasPressedThisFrame)
            Rotate();

    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }


    private  List<DeploySector> SearchSectors()
    {
        List<DeploySector> coord = new List<DeploySector>();

        for (int i = 0; i < deckPoints.Length; i++)
        {
            Collider2D cell = Physics2D.OverlapPoint(deckPoints[i].position, cellLayer);

            DeploySector deploySector = cell.GetComponent<DeploySector>();

            coord.Add(deploySector);
        }

        return coord;
    }


    private void SetStatusPlace(StatusSector newStatus)
    {
        List<DeploySector> sectors = SearchSectors();

        for(int i = 0; i < sectors.Count; i++)
        {
            sectors[i].SetStatus(newStatus);
        }
    }


    private void DefineValid(Collider2D targetLeft, Collider2D targetRight)
    {
        if (targetLeft != null && targetRight != null)
        {
            Magnet(targetLeft);

            List<DeploySector> sectors = SearchSectors();

            Debug.Log(deployPresenter.ValidateDeploy(sectors));

            if (deployPresenter.ValidateDeploy(sectors))
                hasValidPosition = true;
        
            else
                hasValidPosition = false;
        }

        else
            hasValidPosition = false;
    }
    
}