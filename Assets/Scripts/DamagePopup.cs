using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshProUGUI _textMesh;

    // Start is called before the first frame update
    void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();

        Destroy(gameObject, 3);
    }

    public void Setup(int damageAmt)
    {
        if (damageAmt > 0)
            _textMesh.text = "=" + damageAmt.ToString();
    }
}
