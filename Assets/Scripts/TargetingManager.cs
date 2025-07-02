using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingManager : MonoBehaviour
{
    public CharacterStats selectedTarget;
    public UnitInfoPanel unitInfoPanel;

    void Update()
    {
        // If you Right-clicked an enemy -> target
        // If you Right-Clicked the floor -> move to ground
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    selectedTarget = hit.collider.GetComponent<CharacterStats>();
                    BattleLogManager.Instance.Write($"Selected target: {selectedTarget.name}");
                }
                if (hit.collider.CompareTag("Ground"))
                {
                    // Move to ground logic here
                    Vector3 targetPosition = hit.point;
                    // Move character to targetPosition
                    BattleLogManager.Instance.Write($"Moving to: {targetPosition}");
                }
            }
        }
        // If you Left-clicked an enemy -> get info
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    selectedTarget = hit.collider.GetComponent<CharacterStats>();
                    BattleLogManager.Instance.Write($"Selected target: {selectedTarget.name}");

                    // Show target info on the panel
                    unitInfoPanel.UpdatePanel(selectedTarget);
                    unitInfoPanel.ShowPanel();
                }
                if (hit.collider.CompareTag("Ground"))
                {
                    unitInfoPanel.HidePanel();
                }
            }
        }

    }
}


