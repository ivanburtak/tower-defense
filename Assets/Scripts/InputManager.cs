using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private Tile lastHovered;

    void Update()
    {
        if (GameManager.Instance.State != GameState.Preparation)
        {
            return;
        }

        if (TowerShop.Instance.IsActive)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                TowerShop.Instance.CloseShop();
            }
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        Tile currentTile = null;

        if (hit.collider != null)
            currentTile = hit.collider.GetComponent<Tile>();

        if (currentTile != lastHovered)
        {
            if (lastHovered != null)
                lastHovered.SetHover(false);

            if (currentTile != null)
                currentTile.SetHover(true);

            lastHovered = currentTile;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && currentTile != null)
        {
            if (currentTile.IsOccupied) return;

            TowerShop.Instance.OpenShop(currentTile);
        }
    }
}