﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Binding<TSourceIndex, TTargetIndex> : MonoBehaviour
{
    [SerializeField] private TSourceIndex _sourceIndex;
    [SerializeField] private TTargetIndex _targetIndex;

    [SerializeField] private BindableMonoBehaviour _source;
    [SerializeField] private UIBehaviour _target;

    public BindableMonoBehaviour Source
    {
        get => _source;
        set => _source = value;
    }

    public UIBehaviour Target
    {
        get => _target;
        set => _target = value;
    }

    public bool HasSource => _source != null;

    public bool HasTarget => _target != null;

    public bool HasSourceAndTarget => HasSource && HasTarget;

    public Type SourceType => _source?.GetType();

    public Type TargetType => _target?.GetType();

    public TSourceIndex SourceIndex
    {
        get => _sourceIndex;
        set => _sourceIndex = value;
    }

    public TTargetIndex TargetIndex
    {
        get => _targetIndex;
        set => _targetIndex = value;
    }
}
