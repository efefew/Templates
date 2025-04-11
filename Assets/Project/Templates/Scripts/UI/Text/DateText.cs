using System;
using System.Globalization;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class DateText : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = DateTime.Now.ToString("dd MMMM yyyy", new CultureInfo("en-US"));
    }
}
