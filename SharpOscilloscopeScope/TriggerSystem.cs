/*
 * Sharp Oscilloscope - A windows 2 channel oscilloscope which uses
 * audio inputs.
 * 
 * Copyright (C) 2024  Mark McCarron
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, see
 * <https://www.gnu.org/licenses/>.
 */

namespace SharpOscilloscopeScope
{
    public enum TriggerType { RisingEdge, FallingEdge, Level, Pulse, Slope }
    public enum TriggerMode { None, Auto, Normal, Single }

    internal class TriggerSystem
    {
        public TriggerType TriggerTypeChannel1 { get; set; } = TriggerType.RisingEdge;
        public TriggerType TriggerTypeChannel2 { get; set; } = TriggerType.RisingEdge;
        public TriggerMode TriggerModeChannel1 { get; set; } = TriggerMode.None;
        public TriggerMode TriggerModeChannel2 { get; set; } = TriggerMode.None;
        public float TriggerLevelChannel1 { get; set; } = 0.3f;
        public float TriggerLevelChannel2 { get; set; } = 0.3f;

        public bool IsTriggeredChannel1 { get; set; } = false;
        public bool IsTriggeredChannel2 { get; set; } = false;


        private DateTime pulseStartTimeChannel1;
        private DateTime pulseEndTimeChannel1;
        private bool pulseInProgressChannel1 = false;
        public double MinPulseDurationChannel1 = 1; // Min pulse duration in milliseconds
        public double MaxPulseDurationChannel1 = 1000; // Max pulse duration in milliseconds

        public float SlopeThresholdChannel1 = 0.5f; // Example threshold for slope detection
        public float TimeBetweenSamplesChannel1 = 0.001f; // Time between samples in seconds


        private DateTime pulseStartTimeChannel2;
        private DateTime pulseEndTimeChannel2;
        private bool pulseInProgressChannel2 = false;
        public double MinPulseDurationChannel2 = 1; // Min pulse duration in milliseconds
        public double MaxPulseDurationChannel2 = 1000; // Max pulse duration in milliseconds

        public float SlopeThresholdChannel2 = 0.5f; // Example threshold for slope detection
        public float TimeBetweenSamplesChannel2 = 0.001f; // Time between samples in seconds

        public bool CheckTriggerChannel1(float currentSample, float previousSample)
        {
            if (IsTriggeredChannel1) return false; // Avoid re-triggering while already triggered

            switch (TriggerTypeChannel1)
            {
                case TriggerType.RisingEdge:
                    {
                        // Trigger on a rising edge crossing the trigger level
                        if (previousSample < TriggerLevelChannel1 && currentSample >= TriggerLevelChannel1)
                        {
                            IsTriggeredChannel1 = true;
                        }

                        break;
                    }
                case TriggerType.FallingEdge:
                    {
                        // Trigger on a falling edge crossing the trigger level
                        if (previousSample > TriggerLevelChannel1 && currentSample <= TriggerLevelChannel1)
                        {
                            IsTriggeredChannel1 = true;
                        }

                        break;
                    }
                case TriggerType.Level:
                    {
                        if (currentSample >= TriggerLevelChannel1)
                        {
                            IsTriggeredChannel1 = true;
                        }

                        break;
                    }
                case TriggerType.Pulse:
                    {
                        // Detect a pulse with a specific duration
                        if (previousSample < TriggerLevelChannel1 && currentSample >= TriggerLevelChannel1)
                        {
                            // Start counting the pulse duration
                            pulseStartTimeChannel1 = DateTime.Now;
                            pulseInProgressChannel1 = true;
                        }

                        if (pulseInProgressChannel1 && currentSample < TriggerLevelChannel1)
                        {
                            // If the pulse drops below the trigger level, calculate the duration
                            pulseEndTimeChannel1 = DateTime.Now;
                            double pulseDuration = (pulseEndTimeChannel1 - pulseStartTimeChannel1).TotalMilliseconds;

                            if (pulseDuration >= MinPulseDurationChannel1 && pulseDuration <= MaxPulseDurationChannel1)
                            {
                                IsTriggeredChannel1 = true;
                            }

                            pulseInProgressChannel1 = false; // Reset the pulse
                        }

                        break;
                    }
                case TriggerType.Slope:
                    {
                        // Detect if the slope exceeds a threshold
                        float slope = (currentSample - previousSample) / TimeBetweenSamplesChannel1;

                        if (Math.Abs(slope) >= SlopeThresholdChannel1)
                        {
                            IsTriggeredChannel1 = true;
                        }

                        break;
                    }
                default:
                    break;
            }

            return IsTriggeredChannel1;
        }

        public bool CheckTriggerChannel2(float currentSample, float previousSample)
        {
            if (IsTriggeredChannel2) return false; // Avoid re-triggering while already triggered

            switch (TriggerTypeChannel1)
            {
                case TriggerType.RisingEdge:
                    {
                        // Trigger on a rising edge crossing the trigger level
                        if (previousSample < TriggerLevelChannel2 && currentSample >= TriggerLevelChannel2)
                        {
                            IsTriggeredChannel2 = true;
                        }

                        break;
                    }
                case TriggerType.FallingEdge:
                    {
                        // Trigger on a falling edge crossing the trigger level
                        if (previousSample > TriggerLevelChannel2 && currentSample <= TriggerLevelChannel2)
                        {
                            IsTriggeredChannel2 = true;
                        }

                        break;
                    }
                case TriggerType.Level:
                    {
                        if (currentSample >= TriggerLevelChannel2)
                        {
                            IsTriggeredChannel2 = true;
                        }

                        break;
                    }
                case TriggerType.Pulse:
                    {
                        // Detect a pulse with a specific duration
                        if (previousSample < TriggerLevelChannel2 && currentSample >= TriggerLevelChannel2)
                        {
                            // Start counting the pulse duration
                            pulseStartTimeChannel2 = DateTime.Now;
                            pulseInProgressChannel2 = true;
                        }

                        if (pulseInProgressChannel2 && currentSample < TriggerLevelChannel2)
                        {
                            // If the pulse drops below the trigger level, calculate the duration
                            pulseEndTimeChannel2 = DateTime.Now;
                            double pulseDuration = (pulseEndTimeChannel2 - pulseStartTimeChannel2).TotalMilliseconds;

                            if (pulseDuration >= MinPulseDurationChannel2 && pulseDuration <= MaxPulseDurationChannel2)
                            {
                                IsTriggeredChannel2 = true;
                            }

                            pulseInProgressChannel2 = false; // Reset the pulse
                        }

                        break;
                    }
                case TriggerType.Slope:
                    {
                        // Detect if the slope exceeds a threshold
                        float slope = (currentSample - previousSample) / TimeBetweenSamplesChannel2;

                        if (Math.Abs(slope) >= SlopeThresholdChannel2)
                        {
                            IsTriggeredChannel2 = true;
                        }

                        break;
                    }
                default:
                    break;
            }

            return IsTriggeredChannel2;
        }

        public void ResetTriggerChannel1()
        {
            IsTriggeredChannel1 = false;
        }

        public void ResetTriggerChannel2()
        {
            IsTriggeredChannel2 = false;
        }
    }
}
