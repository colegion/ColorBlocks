using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        if(EventBus.Instance != null)
            EventBus.Instance.Unregister<CommonFields.LevelFinishedEvent>(HandleLevelCompletion);
    }
}
