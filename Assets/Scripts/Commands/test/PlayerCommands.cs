using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommands : MonoBehaviour
{
    public BoardSpawner _boardSpawner;

    Camera _camera;
    RaycastHit _hitInfo;
    CommandInvoker _commandInvoker = new CommandInvoker();

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetNewMouseHit();
            SpawnToken();
        }
        if (Input.GetMouseButtonDown(1))
        {
            GetNewMouseHit();
            BuffToken();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Undo();
        }
    }

    void GetNewMouseHit()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hitInfo, Mathf.Infinity))
        {
            Debug.Log("Ray hit: " + _hitInfo.transform.name);
        }
    }

    void SpawnToken()
    {
        // create command
        ICommand spawnTokenCommand = new SpawnTokenCommand(_boardSpawner, _hitInfo.point);

        // perform command
        _commandInvoker.ExecuteCommand(spawnTokenCommand);
    }

    void BuffToken()
    {
        IBuffable buffableUnit = _hitInfo.transform.GetComponentInParent<IBuffable>();

        if (buffableUnit != null)
        {
            ICommand buffCommand = new BuffCommand(buffableUnit);
            _commandInvoker.ExecuteCommand(buffCommand);
        }
    }

    void Undo()
    {
        _commandInvoker.UndoCommand();
    }
}
