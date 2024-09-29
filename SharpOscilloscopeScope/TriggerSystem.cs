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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOscilloscopeScope
{
    public enum TriggerType { RisingEdge, FallingEdge, Level, Pulse, Slope }
    public enum TriggerMode { None, Auto, Normal, Single }

    internal class TriggerSystem
    {
        public TriggerType TriggerType { get; set; } = TriggerType.RisingEdge;
        public TriggerMode TriggerMode { get; set; } = TriggerMode.None;
        public float TriggerLevelChannel1 { get; set; } = 0.3f;
        public float TriggerLevelChannel2 { get; set; } = 0.3f;
        public bool TriggerOnRisingEdge { get; set; } = true;

        public bool IsTriggeredChannel1 { get; set; } = false;
        public bool IsTriggeredChannel2 { get; set; } = false;
        
        private float preTriggerBufferChannel1;
        private float preTriggerBufferChannel2;
        private bool waitingForTriggerChannel1 = true;
        private bool waitingForTriggerChannel2 = true;

        private DateTime pulseStartTime;
        private DateTime pulseEndTime;
        private bool pulseInProgress = false;
        public double MinPulseDuration = 1; // Min pulse duration in milliseconds
        public double MaxPulseDuration = 1000; // Max pulse duration in milliseconds

        public float SlopeThreshold = 0.5f; // Example threshold for slope detection
        public float TimeBetweenSamples = 0.001f; // Time between samples in seconds

        public bool CheckTriggerChannel1(float currentSample, float previousSample)
        {
            if (IsTriggeredChannel1) return false; // Avoid re-triggering while already triggered

            switch (TriggerType)
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
                            pulseStartTime = DateTime.Now;
                            pulseInProgress = true;
                        }

                        if (pulseInProgress && currentSample < TriggerLevelChannel1)
                        {
                            // If the pulse drops below the trigger level, calculate the duration
                            pulseEndTime = DateTime.Now;
                            double pulseDuration = (pulseEndTime - pulseStartTime).TotalMilliseconds;

                            if (pulseDuration >= MinPulseDuration && pulseDuration <= MaxPulseDuration)
                            {
                                IsTriggeredChannel1 = true;
                            }

                            pulseInProgress = false; // Reset the pulse
                        }

                        break;
                    }
                case TriggerType.Slope:
                    {
                        // Detect if the slope exceeds a threshold
                        float slope = (currentSample - previousSample) / TimeBetweenSamples;

                        if (Math.Abs(slope) >= SlopeThreshold)
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

            switch (TriggerType)
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
                            pulseStartTime = DateTime.Now;
                            pulseInProgress = true;
                        }

                        if (pulseInProgress && currentSample < TriggerLevelChannel2)
                        {
                            // If the pulse drops below the trigger level, calculate the duration
                            pulseEndTime = DateTime.Now;
                            double pulseDuration = (pulseEndTime - pulseStartTime).TotalMilliseconds;

                            if (pulseDuration >= MinPulseDuration && pulseDuration <= MaxPulseDuration)
                            {
                                IsTriggeredChannel2 = true;
                            }

                            pulseInProgress = false; // Reset the pulse
                        }

                        break;
                    }
                case TriggerType.Slope:
                    {
                        // Detect if the slope exceeds a threshold
                        float slope = (currentSample - previousSample) / TimeBetweenSamples;

                        if (Math.Abs(slope) >= SlopeThreshold)
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
            waitingForTriggerChannel1 = true;
        }

        public void ResetTriggerChannel2()
        {
            IsTriggeredChannel2 = false;
            waitingForTriggerChannel2 = true;
        }
    }
}
