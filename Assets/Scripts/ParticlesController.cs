using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] confetti;

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void HandleLevelCompletion(CommonFields.LevelFinishedEvent eventData)
    {
        if (eventData.IsSuccessful)
        {
            PlayParticles();
        }
    }

    private void PlayParticles()
    {
        foreach (var element in confetti)
        {
            element.Play();
        }
    }

    private void AddListeners()
    {
        EventBus.Instance.Register<CommonFields.LevelFinishedEvent>(HandleLevelCompletion);
    }

    private void RemoveListeners()
    {
        EventBus.Instance.Unregister<CommonFields.LevelFinishedEvent>(HandleLevelCompletion);
    }
}
