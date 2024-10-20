using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillPictorialBook : MonoBehaviour
{
    public List<PictorialBook> pictorialBooks = new List<PictorialBook>();
    // Start is called before the first frame update
    void Start()
    {
        Whole.pictorialBooks.AddRange(pictorialBooks);
    }
}
