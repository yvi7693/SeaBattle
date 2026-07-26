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
    private bool isVertical = false;
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


    public bool IsDeploy()
    {
        return isDeploy;
    }


    public void UnDeploy()
    {
        isDeploy = false;
        AcceptHorizontal();
    }

    public void SetVertical(bool vertical)
    {
        if (vertical == isVertical)
            return;
        
        Rotate();
    }


    public void Rotate()
    {   
        Vector3 deckPosition = deckPoints[0].position;

        if (!isVertical)
        {
            isVertical = true;
            transform.RotateAround(deckPosition, Vector3.forward, 90);
        }

        else
        {
            isVertical = false;
            transform.RotateAround(deckPosition, Vector3.forward, -90);
        }        
    }

    public void AcceptHorizontal()
    {
        if (isVertical)
        {
            Vector3 deckPosition = deckPoints[0].position;
            transform.RotateAround(deckPosition, Vector3.forward, -90);

            isVertical = false;
        }
            
    }

    
    public void SetPlace(Collider2D target, bool vertical)
    {
        SetVertical(vertical);
  
        Magnet(target);

        lastValidPosition = transform.position;

        isDeploy = true;
    }


    public  List<DeploySector> SearchSectors()
    {
        List<DeploySector> coord = new List<DeploySector>();

        for (int i = 0; i < deckPoints.Length; i++)
        {
            Collider2D cell = Physics2D.OverlapPoint(deckPoints[i].position, cellLayer);

            if (cell == null)
            {
                deployPresenter.GetPanel().PanelOn();
                break;
            }
                
            DeploySector deploySector = cell.GetComponent<DeploySector>();
            coord.Add(deploySector);
        }

        return coord;
    }


     public void Magnet(Collider2D target)
    {
        Vector3 offsetSnap = target.transform.position - deckPoints[0].position;
        offsetSnap.z = 0;
            
        transform.position += offsetSnap;
    }


    private void Start()
    {
        lastValidPosition = transform.position;
        isDeploy = false;
    }


    private void SetStatusSector(StatusSector newStatus)
    {
        List<DeploySector> sectors = SearchSectors();
        deployPresenter.SetStatusSector(newStatus, sectors);
    }


    private void OnMouseDown()
    {
        isClicked = true;

        Vector3 mousePosition = GetMouseWorldPosition();
        offset = transform.position - mousePosition;

        if (isDeploy)
            SetStatusSector(StatusSector.Empty);
            
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

        Debug.Log(hasValidPosition);

        if (hasValidPosition)
        {
            lastValidPosition = transform.position;

            SetStatusSector(StatusSector.Ship);

            isDeploy = true;
        }

        else
        {
            transform.position = lastValidPosition;     
            SetStatusSector(StatusSector.Ship);
        }
            
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


    private void DefineValid(Collider2D targetLeft, Collider2D targetRight)
    {
        if (targetLeft != null && targetRight != null)
        {
            Magnet(targetLeft);

            List<DeploySector> sectors = SearchSectors();

            if (deployPresenter.ValidateDeploy(sectors))
                hasValidPosition = true;
        
            else
                hasValidPosition = false;
        }

        else
            hasValidPosition = false;
    }
    
}