using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField]
    public int seed;

    private void Awake()
    {
        CreateWorld();
    }

    #region World Creation
    public void CreateWorld()
    {
        GenerateNoise();
    }

    private void GenerateNoise()
    {
        
    }

    #endregion

    #region Save/Load Data 
    public void SaveWorld()
    {
        SaveSystem.SaveWorld(this);
    }

    public void LoadWorld()
    {
        WorldData data = SaveSystem.LoadWorld();
        seed = data.seed;
    }
    #endregion
}
