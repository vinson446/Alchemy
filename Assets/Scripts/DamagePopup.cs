using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float duration;
    TextMeshProUGUI _textMesh;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    public void SetupDamage(int damageAmt, float duration)
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();

        if (damageAmt < 0)
        {
            _textMesh.color = Color.red;
            _textMesh.text = damageAmt.ToString();
        }
        else if (damageAmt > 0)
        {
            _textMesh.color = Color.green;
            _textMesh.text = "+" + damageAmt.ToString();
        }
    }

    public void SetupMessage(string message, float duration)
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _textMesh.color = Color.green;
        _textMesh.text = message;
    }
}
