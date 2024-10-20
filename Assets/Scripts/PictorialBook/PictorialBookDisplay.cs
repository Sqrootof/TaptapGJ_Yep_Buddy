using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PictorialBookDisplay : MonoBehaviour
{
    public GameObject pictorialEntryPrefab; // ���ڴ���ͼ����Ŀ��Prefab
    public Transform contentPanel; // UI����ʾ��Ŀ�ĸ�����

    private void Start()
    {
        DisplayPictorialBooks();
    }

    public void DisplayPictorialBooks()
    {
        //// ������е�UIԪ��
        //foreach (Transform child in contentPanel)
        //{
        //    Destroy(child.gameObject);
        //}

        // ��FillPictorialBook��ȡͼ���б�
        List<PictorialBook> pictorialBooks = Whole.pictorialBooks;
        foreach (var book in pictorialBooks)
        {
            if (book.entryName != "")
            {
                // ʵ����ͼ����Ŀ
                GameObject entry = Instantiate(pictorialEntryPrefab, contentPanel);
                // ��ȡImage��Text���
                Image entryImage = entry.transform.Find("Image").GetComponent<Image>();
                Text entryText = entry.transform.Find("Text").GetComponent<Text>();

                // ����ͼ�����ƺ�ͼƬ
                if (book.isUnlocked)
                {
                    entryText.text = book.entryName;

                    // ����Sprite����ֵ��Image���
                    entryImage.sprite = book.entryImage;
                }
                else
                {
                    entryText.text = "XXXX"; // δ����״̬
                    entryImage.sprite = null; // ������ʾһ��Ĭ��ͼ��
                }
            }

        }
    }
}
