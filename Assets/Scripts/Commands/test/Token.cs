using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour, IBuffable
{
    [SerializeField] MeshRenderer _rend;
    [SerializeField] Color _initialColor = Color.green;
    [SerializeField] Color _buffColor = Color.red;

    // Start is called before the first frame update
    private void Awake()
    {
        _rend = GetComponentInChildren<MeshRenderer>();
        _rend.material.color = _initialColor;
    }

    public void Buff()
    {
        _rend.material.color = _buffColor;
    }

    public void Unbuff()
    {
        _rend.material.color = _initialColor;
    }
}
