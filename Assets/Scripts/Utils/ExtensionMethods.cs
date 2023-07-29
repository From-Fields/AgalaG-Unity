using UnityEngine.UIElements;

public static class ExtensionMethods
{
    public static void SetActive(this UIDocument document, bool active = false) => document.rootVisualElement.SetActive(active);
    public static void SetActive(this VisualElement element, bool active = false) {
        StyleKeyword activeKeyword = (active) ? StyleKeyword.Null :  StyleKeyword.None;

        element.SetEnabled(active);
        element.style.display = activeKeyword;
    }
}