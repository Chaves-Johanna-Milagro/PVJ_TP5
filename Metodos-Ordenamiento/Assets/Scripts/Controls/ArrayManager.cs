using UnityEngine;
using System;

public class ArrayManager : MonoBehaviour
{
    [SerializeField] private Element elementPrefab;
     private int arraySize = 10;
     private float posX = -7.8f;
     private float posY = 2.15f;

    public Element[] Array { get; private set; }

    private void Awake()
    {
        Array = new Element[arraySize];
        CreateArray();
    }

    public void CreateArray(bool random = false)
    {
        float elementPosX = posX;
        System.Random rnd = new System.Random();

        for (int i = 0; i < Array.Length; i++)
        {
            if (Array[i] != null) Destroy(Array[i].gameObject);

            Array[i] = Instantiate(elementPrefab, new Vector3(elementPosX, posY, 0f), Quaternion.identity);

            float scaleValue = random ? rnd.Next(1, 9) : (i + 1);
            Array[i].transform.localScale = new Vector3(scaleValue, scaleValue, 1f);

            Array[i].InitColor = new Color(i / 10f, i / 10f, i / 10f);

            elementPosX += 1.4f;
        }

        ShowArray();
    }

    public void ShowArray()
    {
        float elementPosX = posX;
        float offsetPosX = Math.Abs(posX * 2) / (Array.Length - 1);

        for (int i = 0; i < Array.Length; i++)
        {
            Array[i].transform.localPosition = new Vector3(elementPosX, posY, 0f);
            elementPosX += offsetPosX;
        }
    }

    public void ShuffleArray()
    {
        System.Random rnd = new System.Random();
        int n = Array.Length;
        while (n > 1)
        {
            int k = rnd.Next(n--);
            Swap(ref Array[n], ref Array[k]);
        }

        ShowArray();
    }

    public void Swap(ref Element element1, ref Element element2)
    {
        byte tempID = element1.elementID;
        Element temp = element1;

        element1.elementID = element2.elementID;
        element1 = element2;

        element2.elementID = tempID;
        element2 = temp;
    }
}
