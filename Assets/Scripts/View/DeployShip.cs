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

    public void Rotate()
    {
        Vector3 deckPosition = deckPoints[0].position;
        transform.RotateAround(deckPosition, Vector3.forward, 90);
    }

    private void Start()
    {
        lastValidPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isClicked = true;

        Vector3 mousePosition = GetMouseWorldPosition();
        offset = transform.position - mousePosition;
    }

    private void OnMouseDrag()
    {
        if (!(isClicked))
            return;

        Vector3 shipPosition = GetMouseWorldPosition() + offset;
        shipPosition.z = transform.position.z;

        transform.position = shipPosition;

        Collider2D target = Physics2D.OverlapPoint(deckPoints[0].position, cellLayer);

        if (target != null)
        {
            Vector3 offsetSnap = target.transform.position - deckPoints[0].position;
            offsetSnap.z = 0;

            transform.position += offsetSnap;

            
            lastValidPosition = transform.position;
            hasValidPosition = true;
            
        }

        else
            hasValidPosition = false;
    }

    private void OnMouseUp()
    {
        isClicked = false;

        List<DeploySector> sectors = SearchSectors();

        if (!(hasValidPosition) || !(deployPresenter.ValidateDeploy(sectors)))
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

    
}