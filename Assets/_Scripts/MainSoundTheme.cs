using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSoundTheme : MonoBehaviour
{
    private static MainSoundTheme Instance;
    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
