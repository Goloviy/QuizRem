using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourSingleton<GameController>
{
    //Todo: transfer to view
    [SerializeField] private Button playButton;
    private DataBaseWorker db;
    
    public void InitializeController(DataBaseWorker _dataBase)
    {
        db = _dataBase;
        //session.InitializeController();
    }
}
