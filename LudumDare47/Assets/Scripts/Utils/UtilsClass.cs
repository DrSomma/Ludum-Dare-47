using UnityEngine;

namespace Utils
{
    //Class for static functions
    public static class UtilsClass
    {
        // Hier können wir uns Hilfsfunktionen schreiben. Zum Beispiel ein $-Symbol, welches an beliebiger Stelle
        // aufploppen kann.


        // private const int SortingOrderDefault = 5000;

        // Create Text in the World
        // public static TextMesh CreateWorldText(
        //     string text,
        //     Transform parent = null,
        //     Vector3 localPosition = default(Vector3),
        //     int fontSize = 85,
        //     float characterSize = 0.05f,
        //     Color? color = null,
        //     TextAnchor textAnchor = TextAnchor.UpperLeft,
        //     TextAlignment textAlignment = TextAlignment.Left,
        //     int sortingOrder = SortingOrderDefault)
        // {
        //     if (color == null)
        //     {
        //         color = Color.white;
        //     }
        //
        //     return CreateWorldText(parent: parent,
        //                            text: text,
        //                            localPosition: localPosition,
        //                            fontSize: fontSize,
        //                            characterSize: characterSize,
        //                            color: (Color) color,
        //                            textAnchor: textAnchor,
        //                            textAlignment: textAlignment,
        //                            sortingOrder: sortingOrder);
        // }
        //
        // // Create Text in the World
        // public static TextMesh CreateWorldText(
        //     Transform parent,
        //     string text,
        //     Vector3 localPosition,
        //     int fontSize,
        //     float characterSize,
        //     Color color,
        //     TextAnchor textAnchor,
        //     TextAlignment textAlignment,
        //     int sortingOrder)
        // {
        //     GameObject gameObject = new GameObject(name: "World_Text", typeof(TextMesh));
        //     Transform transform = gameObject.transform;
        //     transform.SetParent(parent: parent, worldPositionStays: false);
        //     transform.localPosition = localPosition;
        //     TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        //     textMesh.anchor = textAnchor;
        //     textMesh.alignment = textAlignment;
        //     textMesh.text = text;
        //     textMesh.fontSize = fontSize;
        //     textMesh.characterSize = characterSize;
        //     textMesh.color = color;
        //     textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        //     return textMesh;
        // }
    }
}