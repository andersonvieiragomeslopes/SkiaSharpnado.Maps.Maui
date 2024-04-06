using System;
using System.Collections.Generic;
using System.Linq;


namespace SkiaSharpnado.Maps.Domain
{
    public class EffortComputer
    {
        private readonly List<EffortSpan> _effortSpans;

        private double _defaultMaxEffortValue;

        public EffortComputer(List<EffortSpan> effortSpans, double defaultMaxEffortValue)
        {
            if (effortSpans.Count < 2)
            {
                throw new ArgumentException("effortSpans.Count >= 2", nameof(effortSpans));
            }

            _effortSpans = effortSpans;
            _defaultMaxEffortValue = defaultMaxEffortValue;

            LastSpan = _effortSpans.Last();
        }

        public EffortSpan LastSpan { get; }

        public EffortComputer OverrideDefaultMaxValue(double defaultMaxValue)
        {
            _defaultMaxEffortValue = defaultMaxValue;
            return this;
        }

        public EffortSpan GetSpan(double? effortValue)
        {
            double currentPercentage = (effortValue ?? 0) / _defaultMaxEffortValue;

            EffortSpan previousSpan = _effortSpans[0];
            foreach (var currentSpan in _effortSpans)
            {
                if (currentPercentage < currentSpan.Threshold)
                {
                    return previousSpan;
                }

                previousSpan = currentSpan;
            }

            return LastSpan;
        }

        public Color GetColor(double? effortValue, double? maxEffortValue = null)
        {
            double currentPercentage = (effortValue ?? 0) / (maxEffortValue ?? _defaultMaxEffortValue);

            if (currentPercentage >= LastSpan.Threshold)
            {
                return LastSpan.Color;
            }

            var sourceSpan = _effortSpans[0];
            var targetSpan = _effortSpans[1];

            EffortSpan previousSpan = _effortSpans[0];
            foreach (var currentSpan in _effortSpans)
            {
                sourceSpan = previousSpan;
                targetSpan = currentSpan;

                if (currentPercentage < currentSpan.Threshold)
                {
                    break;
                }

                previousSpan = currentSpan;
            }

            double percentToTarget =
                (currentPercentage - sourceSpan.Threshold) / (targetSpan.Threshold - sourceSpan.Threshold);

            var sourceColor = sourceSpan.Color;
            var targetColor = targetSpan.Color;

            // Define color
            return Color.FromRgba(
                sourceColor.Red + (percentToTarget * (targetColor.Red - sourceColor.Red)),
                sourceColor.Green + (percentToTarget * (targetColor.Green - sourceColor.Green)),
                sourceColor.Blue + (percentToTarget * (targetColor.Blue - sourceColor.Blue)),
                sourceColor.Alpha + (percentToTarget * (targetColor.Alpha - sourceColor.Alpha)));
        }
    }
}