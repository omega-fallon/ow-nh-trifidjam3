﻿using NewHorizons;
using UnityEngine;

namespace TrifidJam3
{
    public class BeaconCore : MonoBehaviour
    {
        public static float FadeTime = 1.0f;

        public int Id;
        public Light[] Lights;
        public MeshRenderer[] Emissions;
        public Collider Collider;

        private float[] _lightIntensities;
        private Color[] _emissionColors;
        private bool _isLit;
        private float _fade;

        private void Awake()
        {
            _isLit = true;
            _fade = FadeTime;
        }

        private void Start()
        {
            _lightIntensities = new float[Lights.Length];
            _emissionColors = new Color[Emissions.Length];

            for (int i = 0; i < Lights.Length; i++)
            {
                _lightIntensities[i] = Lights[i].intensity;
            }
            for (int i = 0; i < Emissions.Length; i++)
            {
                var material = Emissions[i].material;
                _emissionColors[i] = material.GetColor("_EmissionColor");
            }
        }

        private void FixedUpdate()
        {
            var change = false;

            if (_isLit && _fade < FadeTime)
            {
                _fade = Mathf.Min(FadeTime, _fade + Time.fixedDeltaTime);
                change = true;

            }
            else if (!_isLit && _fade > 0f)
            {
                _fade = Mathf.Max(0f, _fade - Time.fixedDeltaTime);
                change = true;
            }

            if (change)
            {
                for (int i = 0; i < Lights.Length; i++)
                {
                    Lights[i].intensity = _lightIntensities[i] * (_fade / FadeTime);
                }
                for (int i = 0; i < Emissions.Length; i++)
                {
                    Emissions[i].material.SetColor("_EmissionColor", _emissionColors[i] * (_fade / FadeTime));
                }
            }
        }

        public void Activate()
        {
            _isLit = true;
        }

        public void Deactivate()
        {
            _isLit = false;
        }

        public bool IsLit() => _isLit;
    }
}