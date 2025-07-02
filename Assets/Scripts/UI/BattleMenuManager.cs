using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.Windows;



public class BattleMenuManager : MonoBehaviour
{
    public static BattleMenuManager Instance;
    private List<CharacterStats> allies;

    public GameObject menuPanel;
    public GameObject targetPanel;

    public GameObject confirmCancelPanel;

    public Button attackButton;
    public Button magicButton;
    public Button itemsButton;
    public Button technicksButton;
    public Button specialButton;

    public TMP_Text categoryText;
    public TMP_Text[] commandOptions;
    public TMP_Text targetText;

    public string[] categories = { "main", "Attack", "Magic", "Items", "technicks", "Special Skills", "Gambits", "Confirm", "Cancel" };

    private int selectedCategoryIndex = 0;
    private Dictionary<string, System.Action> categoryActions;
    private ICombatHandler combatHandler;


    private CharacterStats currentCharacter; // this one to keep ?
    private bool isMenuOpen = false;


    [SerializeField] private PlayerInputActions playerInputActions; // it seems to be the way to go for the input system using the created InputSystem_Actions.cs file



    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialisation de l'instance et des autres composants
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple instances of BattleMenuManager detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        allies = new List<CharacterStats>(); // Crée une liste vide temporaire
        /*        combatHandler = new CombatHandler(allies, currentCharacter.Faction); // Instanciation de CombatHandler avec la liste vide
                // Une fois que CombatHandler est prêt, récupérez les alliés
                allies = combatHandler.GetAllies();*/
        // Récupérer une instance globale de CombatHandler
        combatHandler = CombatHandler.Instance; // cette ligne suppose que CombatHandler est un singleton
        if (combatHandler == null)
        {
            Debug.LogError("CombatHandler.Instance is null. Make sure CombatHandler is initialized before BattleMenuManager.");
            return;
        }

        allies = combatHandler.GetAllies(); // Récupérer les alliés via CombatHandler

        playerInputActions = new PlayerInputActions();

        if (playerInputActions != null)
        {
            playerInputActions.UI.OpenMenu.performed += ctx => ToggleMenu();
            playerInputActions.Enable();
        }

        // InitializeCategories permet de lier les catégories aux actions
        InitializeCategories();
    }


    // Start is called before the first frame update
    private void Start()
    {
        // Hide menu by default
        menuPanel.SetActive(false);

        // Hook up buttons
        attackButton.onClick.AddListener(OnAttack);
        magicButton.onClick.AddListener(OnMagic);
        itemsButton.onClick.AddListener(OnItems);
        technicksButton.onClick.AddListener(OnTechnicks);
        specialButton.onClick.AddListener(OnSpecial);
    }


    // Check if the menu is open and enable input actions
    private bool inputActionsEnabled = false;
    private void Update()
    {
        // Activer les actions d'input si elles ne le sont pas déjà
        if (!inputActionsEnabled)
        {
            EnableInputActions();
            inputActionsEnabled = true;
        }

        if (!isMenuOpen) return;

    }


    // Toggle the menu open/close state
    private void ToggleMenu()
    {
        SetMenuState(!isMenuOpen);
    }
    private void SetMenuState(bool isOpen)
    {
        isMenuOpen = isOpen;
        menuPanel.SetActive(isMenuOpen);
        targetPanel.SetActive(false);
        confirmCancelPanel.SetActive(false);
        selectedCategoryIndex = 0;
        UpdateMenuDisplay();
    }

    // Change the selected category based on input direction
    private void ChangeSelection(int dir)
    {
        selectedCategoryIndex += dir;
        if (selectedCategoryIndex < 0) selectedCategoryIndex = categories.Length - 1;
        if (selectedCategoryIndex >= categories.Length) selectedCategoryIndex = 0;
        UpdateMenuDisplay();
    }

    // Update the menu display based on the selected category
    private void UpdateMenuDisplay()
    {
        categoryText.text = "Select Action";

        for (int i = 0; i < commandOptions.Length; i++)
        {
            if (i < categories.Length)
            {
                commandOptions[i].text = (i == selectedCategoryIndex ? "> " : "  ") + categories[i];
                commandOptions[i].gameObject.SetActive(true);
            }
            else
            {
                commandOptions[i].gameObject.SetActive(false);
            }
        }
    }

    // Confirm the selection and perform the action
    private void ConfirmSelection()
    {
        string choice = categories[selectedCategoryIndex];
        if (categoryActions.ContainsKey(choice))
        {
            categoryActions[choice].Invoke();
        }
        else
        {
            Debug.LogWarning($"No action defined for category: {choice}");
        }
    }
    private void InitializeCategories()
    {
        categoryActions = new Dictionary<string, System.Action>
    {
        { "Attack", () => OpenTargeting("Choose your target for Attack!") },
        { "Magic", () => Debug.Log("Magic selected") },
        { "Items", () => Debug.Log("Items selected") },
        { "Technicks", () => Debug.Log("Technicks selected") },
        { "Special Skills", () => Debug.Log("Special Skills selected") },
        { "Gambit", () => Debug.Log("Gambit selected") },
        { "Confirm", ConfirmSelection },
        { "Cancel", CloseMenu }
    };
    }

    // Toggle On/Off the Gambits. When off, the character will not use his gambits.
    public void ToggleGambits()
    {
        if (currentCharacter == null)
        {
            Debug.LogWarning("Aucun personnage sélectionné pour basculer les Gambits.");
            return;
        }

        currentCharacter.AreGambitsEnabled = !currentCharacter.AreGambitsEnabled;
        Debug.Log($"{currentCharacter.CharacterName} Gambits {(currentCharacter.AreGambitsEnabled ? "activés" : "désactivés")}.");
    }

    // Open the targeting panel and set the message
    private void OpenTargeting(string message)
    {
        targetPanel.SetActive(true);
        targetText.text = message;

        // Utilisation de combatHandler pour récupérer les ennemis
        var enemies = combatHandler.GetEnemies();
        if (enemies.Count > 0 && currentCharacter != null)
        {
            var target = enemies[0]; // Sélection automatique pour l'instant
            combatHandler.PerformAttack(currentCharacter, target);
        }
        else
        {
            Debug.LogWarning("No enemies found or current character is null!");
        }

        CloseMenu();
    }


    // Open the menu and set the current character
    public void OpenMenu(CharacterStats character)
    {
        if (character == null)
        {
            Debug.LogError("Character is null. Cannot open menu.");
            return;
        }

        currentCharacter = character;
        menuPanel.SetActive(true);

        if (EventSystem.current == null)
        {
            Debug.LogError("Erreur : EventSystem est null.");
        }
        else if (attackButton == null)
        {
            Debug.LogError("Erreur : attackButton est null.");
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
        }
    }
    private void CloseMenu()
    {
        isMenuOpen = false; // do i keep this ?
        menuPanel.SetActive(false);
        targetPanel.SetActive(false); // do i keep this ?
    }

    void OnAttack()
    {
        var enemies = combatHandler.GetEnemies(); // use the combatHandler to get the enemies.
        if (enemies.Count > 0)
        {
            var target = enemies[0]; // future UI target selection
            combatHandler.PerformAttack(currentCharacter, target);
            //int damage = (int)DamageCalculator.CalculatePhysicalDamage(currentCharacter, target);
            //target.TakeDamage(damage);
            //FloatingTextManager.Instance.ShowText(damage.ToString(), target.transform.position + Vector3.up * 2, Color.red);
        }
        else
        {
            Debug.LogWarning("No enemies found!");
        }

        CloseMenu();
    }

    // Button Callbacks
    private void OnEnable()
    {
        EnableInputActions();
    }
    private void OnDisable()
    {
        DisableInputActions();
    }
    // Destroy is called when we don't need this object anymore
    private void DestroyGo(GameObject GO) // Where can i use this ? I can use it to destroy enemy or ally gameobject.
    {
        if (GO != null)
        {
            Destroy(GO);
        }
        else
        {
            Debug.LogWarning("GameObject to destroy is null.");
        }

    }
    private void OnValidate()
    {
        if (menuPanel == null)
        {
            menuPanel = GameObject.Find("BattleMenu");
        }
        if (targetPanel == null)
        {
            targetPanel = GameObject.Find("TargetingPanel");
        }
        if (confirmCancelPanel == null)
        {
            confirmCancelPanel = GameObject.Find("ConfirmCancelPanel");
        }
    }


    // Button Callbacks
    void OnMagic() { Debug.Log("Magic pressed"); CloseMenu(); }
    void OnItems() { Debug.Log("Items pressed"); CloseMenu(); }
    void OnTechnicks() { Debug.Log("Technicks pressed"); CloseMenu(); }
    void OnSpecial() { Debug.Log("Special pressed"); CloseMenu(); }


    // Input Handling
    private void EnableInputActions()
    {
        // Utilisation des actions définies dans le système d'input
        playerInputActions.UI.Navigate.performed += ctx =>
        {
            Vector2 navigation = ctx.ReadValue<Vector2>();
            if (navigation.y > 0) ChangeSelection(-1); // Haut
            else if (navigation.y < 0) ChangeSelection(1); // Bas
        };
        playerInputActions.UI.Navigate.performed += OnNavigate;
        playerInputActions.UI.Submit.performed += ctx => ConfirmSelection(); // enter
        playerInputActions.UI.Cancel.performed += ctx => CloseMenu(); // escape
        playerInputActions.UI.OpenMenu.performed += ctx => ToggleMenu(); // open menu / close menu
        playerInputActions.Enable();
    }
    private void DisableInputActions()
    {
        playerInputActions.UI.Navigate.performed -= OnNavigate;
        playerInputActions.UI.Submit.performed -= ctx => ConfirmSelection();
        playerInputActions.UI.Cancel.performed -= ctx => CloseMenu();
        playerInputActions.UI.OpenMenu.performed -= ctx => ToggleMenu();
        playerInputActions.Disable();
    }
    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        Vector2 navigation = ctx.ReadValue<Vector2>();
        if (navigation.y > 0) ChangeSelection(-1); // Haut
        else if (navigation.y < 0) ChangeSelection(1); // Bas
    }
}
