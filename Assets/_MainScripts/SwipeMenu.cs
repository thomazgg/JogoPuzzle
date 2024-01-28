using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    public Button nextButton;
    public Button prevButton;
    public Sprite unselectedPage;
    public Sprite selectedPage;
    public Transform pageSelectionIcons;
    private bool _showPageSelection;
    private int _pageCount;
    private int _previousPageSelectionIndex;
    private List<Image> _pageSelectionImages;

    float scroll_pos = 0;
    float[] pos;
    int currentPage = 0;

    void Start() {
        _pageCount = transform.childCount;
        nextButton.onClick.AddListener(NextButtonClick);
        prevButton.onClick.AddListener(PrevButtonClick);
        InitPageSelection();
        
        // Set initial page to 0
        SetPage(0);
    }

    private void NextButtonClick() {
        int currentIndex = GetCurrentIndex();
        if (currentIndex < pos.Length - 1)
        {
            scrollbar.GetComponent<Scrollbar>().value = pos[currentIndex + 1];
            scroll_pos = pos[currentIndex + 1];

            // Update current page
            SetPage(currentPage + 1);
        }
    }

    private void PrevButtonClick() {
        int currentIndex = GetCurrentIndex();
        if (currentIndex > 0)
        {
            scrollbar.GetComponent<Scrollbar>().value = pos[currentIndex - 1];
            scroll_pos = pos[currentIndex - 1];

            // Update current page
            SetPage(currentPage - 1);
        }
    }

    private int GetCurrentIndex() {
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                return i;
            }
        }
        return 0;
    }

    private void InitPageSelection() {
        _showPageSelection = unselectedPage != null && selectedPage != null;
        if (_showPageSelection) {
            if (pageSelectionIcons == null || pageSelectionIcons.childCount != _pageCount) {
                // Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
                _showPageSelection = false;
            } else {
                _previousPageSelectionIndex = -1;
                _pageSelectionImages = new List<Image>();

                for (int i = 0; i < pageSelectionIcons.childCount; i++) {
                    Image image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
                    if (image == null) {
                        Debug.LogWarning("Page selection icon at position " + i + " is missing Image component");
                    }
                    _pageSelectionImages.Add(image);
                }
            }
        }
    }

    // Update current page based on the scroll position
    void UpdateCurrentPage() {
        int index = GetCurrentIndex();

        if (index != currentPage) {
            SetPage(index);
        }
    }

    // Set the current page
    void SetPage(int index) {
        currentPage = index;

        // Debug.Log("Página atual: " + currentPage);

        // Update page selection
        if (_showPageSelection) {
            if (_previousPageSelectionIndex != -1) {
                _pageSelectionImages[_previousPageSelectionIndex].sprite = unselectedPage;
            }
            _pageSelectionImages[currentPage].sprite = selectedPage;
            _previousPageSelectionIndex = currentPage;
        }
    }
    
    void Update() {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++) {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0)) {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        } else {
            for (int i = 0; i < pos.Length; i++) {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);

                    // Atualiza a página atual
                    if (currentPage != i) {
                        currentPage = i;
                        SetPage(currentPage);
                        // Debug.Log("Página atual: " + currentPage);
                    }
                }
            }
        }

        for (int i = 0; i < pos.Length; i++) {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < pos.Length; a++) {
                    if (a != i) {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}