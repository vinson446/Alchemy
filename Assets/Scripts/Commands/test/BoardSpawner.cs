using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    public Token _tokenPrefab;

    public Token SpawnToken(Vector3 position)
    {
        Token newToken = Instantiate(_tokenPrefab, position, _tokenPrefab.transform.rotation);

        return newToken;
    }

    public void RemoveToken(Token tokenToRemove)
    {
        Destroy(tokenToRemove.gameObject);
    }
}
