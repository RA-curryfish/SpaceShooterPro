using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PostProcessVolume _postProcessVolume;
    private ChromaticAberration _ca;
    [SerializeField] private bool _isBoost;

    void Start()
    {
        _postProcessVolume = GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _ca);
        _ca.intensity.value = 0;
    }

    private void Update() 
    {
        PostProcFX();
    }

    void PostProcFX()
    {
        if(_isBoost)
        {
            _ca.active = true;
            _ca.intensity.value = 0.4f;
        }
        else
        {
            _ca.active=false;
            _ca.intensity.value = 0;
        }    
    }
    public void IsBoost(bool val)
    {
        _isBoost=val;
    }

}
