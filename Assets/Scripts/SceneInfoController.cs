using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInfoController : MonoBehaviour
{
    [SerializeField]private PlayerMode playMode;

    public Vector3 sceneCameraBoundUpperDefault;
    public Vector3 sceneCameraBoundLowerDefault;
    public float sceneCameraSpeedDefault;

    // Start is called before the first frame update
    void Start()
    {
        WorldStateInfo.Instance.SetPlayMode(playMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        WorldStateInfo.Instance.InitializeLevel(playMode, sceneCameraBoundUpperDefault, sceneCameraBoundLowerDefault, sceneCameraSpeedDefault);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
}
