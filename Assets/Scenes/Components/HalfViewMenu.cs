// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
// using System.Collections.Generic;

// public class HalfViewMenu : MonoBehaviour
// {
//     [Header("Referências do Painel")]
//     [SerializeField] private RectTransform menuPanel;
//     [SerializeField] private Canvas menuCanvas;

//     [Header("Botões da Interface")]
//     [SerializeField] private Button logoutButton;
//     [SerializeField] private Button deleteAccountButton;
//     [SerializeField] private Button closeButton;

//     [Header("Botão de Ativação")]
//     [SerializeField] private Button engineButton;

//     [Header("Configurações de Animação")]
//     [SerializeField] private float animationDuration = 0.3f;
//     [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

//     [Header("Configurações de Overlay")]
//     [SerializeField] private GameObject halfViewDarkOverlay;
//     [SerializeField] private float overlayAlpha = 0.6f;

//     // Referência ao ProfileManager
//     private ProfileManager profileManager;

//     // Variáveis privadas
//     private Vector2 hiddenPosition;
//     private Vector2 visiblePosition;
//     private bool isVisible = false;
//     private Coroutine animationCoroutine;

//     // Lista de elementos interativos a serem desabilitados
//     private List<Selectable> interactableElements = new List<Selectable>();
//     private List<bool> originalInteractableStates = new List<bool>();

//     // Variáveis para auto-detecção da bottomBar
//     private RectTransform bottomBar = null;
//     private Canvas bottomBarCanvas = null;
//     private int originalBottomBarSortingOrder = 0;

//     private void Awake()
//     {
//         profileManager = FindFirstObjectByType<ProfileManager>();
//         EnsureCanvasSetup();
//     }

//     private void OnEnable()
//     {
//         SetupButtonListeners();
//     }

//     private void Start()
//     {
//         FindBottomBar();
//         SetupPositions();

//         if (menuPanel != null)
//         {
//             menuPanel.anchoredPosition = hiddenPosition;
//         }

//         SetupButtonListeners();
//         SetupOverlay();
//         CollectInteractableElements();

//         // Only hide the menu, not the gameObject
//         if (halfViewDarkOverlay != null)
//         {
//             halfViewDarkOverlay.SetActive(false);
//         }

//         Debug.Log("[HalfViewMenu] Iniciado com sucesso");
//     }

//     private void EnsureCanvasSetup()
//     {
//         if (menuCanvas == null)
//         {
//             menuCanvas = GetComponent<Canvas>();
//             if (menuCanvas == null)
//             {
//                 menuCanvas = gameObject.AddComponent<Canvas>();
//             }
//         }

//         menuCanvas.overrideSorting = true;
//         menuCanvas.sortingOrder = 100;

//         GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
//         if (raycaster == null)
//         {
//             raycaster = gameObject.AddComponent<GraphicRaycaster>();
//             Debug.Log("[HalfViewMenu] GraphicRaycaster adicionado");
//         }
//     }

//     private void FindBottomBar()
//     {
//         Canvas[] allCanvases = FindObjectsOfType<Canvas>(true);
//         foreach (Canvas canvas in allCanvases)
//         {
//             if (canvas.gameObject.name.ToLower().Contains("bottom") ||
//                 canvas.gameObject.name.ToLower().Contains("bar"))
//             {
//                 bottomBarCanvas = canvas;
//                 bottomBar = canvas.GetComponent<RectTransform>();
//                 originalBottomBarSortingOrder = canvas.sortingOrder;
//                 break;
//             }
//         }
//     }

//     private void SetupPositions()
//     {
//         if (menuPanel == null)
//         {
//             Debug.LogError("[HalfViewMenu] Menu Panel não atribuído!");
//             return;
//         }

//         hiddenPosition = new Vector2(0, -menuPanel.rect.height);
//         visiblePosition = new Vector2(0, 0);
//     }

//     private void SetupButtonListeners()
//     {
//         if (logoutButton != null)
//         {
//             logoutButton.onClick.RemoveAllListeners();
//             logoutButton.onClick.AddListener(OnLogoutClicked);
//             logoutButton.interactable = true;

//             if (logoutButton.targetGraphic != null)
//             {
//                 logoutButton.targetGraphic.raycastTarget = true;
//             }
//             Debug.Log("[HalfViewMenu] Logout button configurado");
//         }

//         if (deleteAccountButton != null)
//         {
//             deleteAccountButton.onClick.RemoveAllListeners();
//             deleteAccountButton.onClick.AddListener(OnDeleteAccountClicked);
//             deleteAccountButton.interactable = true;

//             if (deleteAccountButton.targetGraphic != null)
//             {
//                 deleteAccountButton.targetGraphic.raycastTarget = true;
//             }
//             Debug.Log("[HalfViewMenu] Delete account button configurado");
//         }

//         if (closeButton != null)
//         {
//             closeButton.onClick.RemoveAllListeners();
//             closeButton.onClick.AddListener(HideMenu);
//             closeButton.interactable = true;

//             if (closeButton.targetGraphic != null)
//             {
//                 closeButton.targetGraphic.raycastTarget = true;
//             }
//             Debug.Log("[HalfViewMenu] Close button configurado");
//         }

//         if (engineButton != null)
//         {
//             engineButton.onClick.RemoveListener(ToggleMenu);
//             engineButton.onClick.AddListener(ToggleMenu);
//         }
//     }

//     private void SetupOverlay()
//     {
//         if (halfViewDarkOverlay == null) return;

//         halfViewDarkOverlay.SetActive(false);

//         Canvas overlayCanvas = halfViewDarkOverlay.GetComponent<Canvas>();
//         if (overlayCanvas == null)
//         {
//             overlayCanvas = halfViewDarkOverlay.AddComponent<Canvas>();
//         }
//         overlayCanvas.overrideSorting = true;
//         overlayCanvas.sortingOrder = 90;

//         Image overlayImage = halfViewDarkOverlay.GetComponent<Image>();
//         if (overlayImage != null)
//         {
//             Color color = overlayImage.color;
//             color.a = overlayAlpha;
//             overlayImage.color = color;
//         }

//         Button overlayButton = halfViewDarkOverlay.GetComponent<Button>();
//         if (overlayButton == null)
//         {
//             overlayButton = halfViewDarkOverlay.AddComponent<Button>();
//             ColorBlock colors = overlayButton.colors;
//             colors.normalColor = new Color(0, 0, 0, 0);
//             colors.highlightedColor = new Color(0, 0, 0, 0);
//             colors.pressedColor = new Color(0, 0, 0, 0);
//             colors.selectedColor = new Color(0, 0, 0, 0);
//             overlayButton.colors = colors;
//         }
//         overlayButton.onClick.RemoveAllListeners();
//         overlayButton.onClick.AddListener(HideMenu);

//         // CRÍTICO: Garante que o overlay esteja ATRÁS da half view na hierarquia
//         halfViewDarkOverlay.transform.SetSiblingIndex(0);
//     }

//     private void CollectInteractableElements()
//     {
//         interactableElements.Clear();
//         originalInteractableStates.Clear();

//         Selectable[] selectables = FindObjectsOfType<Selectable>(true);
//         foreach (Selectable selectable in selectables)
//         {
//             if (!IsPartOfHalfView(selectable.gameObject))
//             {
//                 interactableElements.Add(selectable);
//                 originalInteractableStates.Add(selectable.interactable);
//             }
//         }
//     }

//     private bool IsPartOfHalfView(GameObject obj)
//     {
//         Transform current = obj.transform;
//         while (current != null)
//         {
//             if (current.gameObject == gameObject || (halfViewDarkOverlay != null && current.gameObject == halfViewDarkOverlay))
//             {
//                 return true;
//             }
//             current = current.parent;
//         }
//         return false;
//     }

//     public void ToggleMenu()
//     {
//         if (isVisible)
//             HideMenu();
//         else
//             ShowMenu();
//     }

//     public void ShowMenu()
//     {
//         if (isVisible) return;

//         CollectInteractableElements();
//         DisableSceneInteraction();

//         EnsureCanvasSetup();

//         gameObject.SetActive(true);

//         CanvasGroup panelGroup = menuPanel.GetComponent<CanvasGroup>();
//         if (panelGroup != null)
//         {
//             panelGroup.interactable = true;
//             panelGroup.blocksRaycasts = true;
//             panelGroup.alpha = 1f;
//         }

//         if (halfViewDarkOverlay != null)
//         {
//             halfViewDarkOverlay.SetActive(true);
//             halfViewDarkOverlay.transform.SetSiblingIndex(0);
//         }

//         AdjustSortingOrderForVisibility(true);

//         if (animationCoroutine != null)
//             StopCoroutine(animationCoroutine);

//         animationCoroutine = StartCoroutine(AnimateMenu(hiddenPosition, visiblePosition));
//         isVisible = true;

//         SetupButtonListeners();
//     }

//     public void HideMenu()
//     {
//         if (!isVisible) return;

//         EnableSceneInteraction();

//         if (animationCoroutine != null)
//             StopCoroutine(animationCoroutine);

//         animationCoroutine = StartCoroutine(AnimateMenu(visiblePosition, hiddenPosition, true));
//         isVisible = false;

//         if (halfViewDarkOverlay != null)
//         {
//             halfViewDarkOverlay.SetActive(false);
//         }

//         AdjustSortingOrderForVisibility(false);
//     }

//     private void DisableSceneInteraction()
//     {
//         for (int i = 0; i < interactableElements.Count; i++)
//         {
//             if (interactableElements[i] != null)
//             {
//                 if (!IsPartOfDeleteAccountCanvas(interactableElements[i].gameObject))
//                 {
//                     originalInteractableStates[i] = interactableElements[i].interactable;
//                     interactableElements[i].interactable = false;
//                 }
//             }
//         }
//     }

//     private bool IsPartOfDeleteAccountCanvas(GameObject obj)
//     {
//         Transform current = obj.transform;
//         while (current != null)
//         {
//             if (current.name.Contains("DeleteAccountCanvas") || current.name.Contains("DeleteAccountPanel"))
//             {
//                 return true;
//             }
//             current = current.parent;
//         }
//         return false;
//     }

//     private void EnableSceneInteraction()
//     {
//         for (int i = 0; i < interactableElements.Count; i++)
//         {
//             if (interactableElements[i] != null)
//             {
//                 interactableElements[i].interactable = originalInteractableStates[i];
//             }
//         }
//     }

//     private void AdjustSortingOrderForVisibility(bool show)
//     {
//         if (menuCanvas != null)
//         {
//             if (show)
//             {
//                 menuCanvas.sortingOrder = 100;

//                 if (bottomBarCanvas != null)
//                 {
//                     bottomBarCanvas.sortingOrder = 88;
//                 }

//                 if (halfViewDarkOverlay != null)
//                 {
//                     Canvas overlayCanvas = halfViewDarkOverlay.GetComponent<Canvas>();
//                     if (overlayCanvas != null)
//                     {
//                         overlayCanvas.sortingOrder = 90;
//                     }
//                 }
//             }
//             else
//             {
//                 menuCanvas.sortingOrder = 0;

//                 if (bottomBarCanvas != null)
//                 {
//                     bottomBarCanvas.sortingOrder = originalBottomBarSortingOrder;
//                 }
//             }
//         }
//     }

//     private IEnumerator AnimateMenu(Vector2 startPos, Vector2 endPos, bool hideWhenDone = false)
//     {
//         if (menuPanel == null) yield break;

//         float elapsedTime = 0;
//         menuPanel.anchoredPosition = startPos;

//         while (elapsedTime < animationDuration)
//         {
//             elapsedTime += Time.deltaTime;
//             float normalizedTime = Mathf.Clamp01(elapsedTime / animationDuration);
//             float curveValue = animationCurve.Evaluate(normalizedTime);

//             menuPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);

//             yield return null;
//         }

//         menuPanel.anchoredPosition = endPos;

//         if (hideWhenDone)
//         {
//             gameObject.SetActive(false);
//         }
//         else
//         {
//             SetupButtonListeners();
//         }

//         animationCoroutine = null;
//     }

//     private void OnLogoutClicked()
//     {
//         Debug.Log("[HalfViewMenu] OnLogoutClicked chamado");

//         if (profileManager != null)
//         {
//             profileManager.LogoutButton();
//         }
//         else
//         {
//             Debug.LogError("[HalfViewMenu] ProfileManager não encontrado!");
//         }

//         HideMenu();
//     }

//     private void OnDeleteAccountClicked()
//     {
//         Debug.Log("OnDeleteAccountClicked chamado");
//         GameObject overlay = halfViewDarkOverlay;

//         if (profileManager != null)
//         {
//             if (animationCoroutine != null)
//                 StopCoroutine(animationCoroutine);

//             animationCoroutine = StartCoroutine(AnimateMenuWithoutHidingOverlay(visiblePosition, hiddenPosition));
//             isVisible = false;

//             StartCoroutine(DelayedDeleteAccount());
//         }
//         else
//         {
//             Debug.LogError("ProfileManager não encontrado!");
//             HideMenu();
//         }
//     }

//     private IEnumerator DelayedDeleteAccount()
//     {
//         yield return new WaitForSeconds(0.1f);
//         if (halfViewDarkOverlay != null)
//         {
//             halfViewDarkOverlay.SetActive(false);
//             Debug.Log("HalfViewDarkOverlay desativado antes de mostrar DeleteAccountPanel");
//         }

//         profileManager.StartDeleteAccount();
//         gameObject.SetActive(false);
//     }

//     private IEnumerator AnimateMenuWithoutHidingOverlay(Vector2 startPos, Vector2 endPos)
//     {
//         if (menuPanel == null) yield break;

//         float elapsedTime = 0;
//         menuPanel.anchoredPosition = startPos;

//         while (elapsedTime < animationDuration)
//         {
//             elapsedTime += Time.deltaTime;
//             float normalizedTime = Mathf.Clamp01(elapsedTime / animationDuration);
//             float curveValue = animationCurve.Evaluate(normalizedTime);

//             menuPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);

//             yield return null;
//         }

//         menuPanel.anchoredPosition = endPos;
//         animationCoroutine = null;
//     }
// }