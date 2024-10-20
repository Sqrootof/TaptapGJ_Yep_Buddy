using UnityEngine;
using System.Collections.Generic;

public class PictorialBookManager : MonoBehaviour
{
    public void UnlockPictorialBook(string entryName)
    {
        // 查找图鉴条目
        PictorialBook bookToUnlock = Whole.pictorialBooks.Find(book => book.entryName == entryName);

        // 如果找到图鉴条目，解锁它
        if (bookToUnlock != null)
        {
            bookToUnlock.isUnlocked = true;
            Debug.Log($"{entryName} has been unlocked!");
        }
        else
        {
            Debug.LogWarning($"PictorialBook with name '{entryName}' not found.");
        }
    }
}
