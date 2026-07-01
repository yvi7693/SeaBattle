using UnityEngine;
using UnityEngine.InputSystem;

public class DeployShip : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask cellLayer;
    [SerializeField] private Transform snapPoint;
    private Vector3 lastValidPosition;
    private bool hasValidPosition;
    private Vector3 offset;
    private bool isClicked;

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

        Collider2D target = Physics2D.OverlapPoint(snapPoint.position, cellLayer);

        if (target != null)
        {
            Vector3 offsetSnap = target.transform.position - snapPoint.position;
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

        if (!(hasValidPosition))
            transform.position = lastValidPosition;

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}