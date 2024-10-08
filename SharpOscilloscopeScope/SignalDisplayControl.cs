﻿/*
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
    public partial class SignalDisplayControl : UserControl
    {
        private float[] channel1Data = new float[0];   // Data for left channel
        private float[] channel2Data = new float[0];  // Data for right channel
        private object dataLock = new object();           // Ensure thread-safety

        private int sampleRate = 48000;                   // Default sample rate
        private float timeScale = 0.05f;                  // Time scale in seconds (default: 50ms)
        private float amplitudeScale = 1.0f;              // Amplitude scale (vertical zoom)
        private bool displayLeftChannel = true;           // Toggle left channel display
        private bool displayRightChannel = true;          // Toggle right channel display

        private float maxTimeRange = 10.0f;               // Maximum time range in seconds
        private float minTimeRange = 0.001f;              // Minimum time range in seconds

        private int gridSpacingTime = 100;                // Time grid spacing in milliseconds
        private float gridSpacingVoltage = 0.5f;          // Voltage grid spacing (in volts)

        private float elapsedTime = 0f;                   // Elapsed time for incoming data
        private int totalSamplesToDisplay = 0;            // Total samples to display based on time range

        //Trigger support
        private TriggerSystem triggerSystem = new TriggerSystem();
        private bool preTriggered = false;

        // Pre-trigger and post-trigger buffers (based on time)
        private int preTriggerSamples = 0;
        private int postTriggerSamples = 0;
        private float preTriggerTime = 0.01f; // Default 10ms of pre-trigger data


        // Cursor variables
        private bool showTimeCursor = false;
        private bool showAmplitudeCursor = false;
        private float timeCursorPosition = 0f;   // X-coordinate
        private float amplitudeCursorPosition = 0f; // Y-coordinate

        //private bool bypassTriggerCheck = false; // Flag to bypass trigger checks
        private System.Timers.Timer holdTimerChannel1;
        private bool holdingWaveformChannel1 = false;
        private int currentSampleIndexChannel1 = 0;  // Tracks where new data should be appended

        private System.Timers.Timer holdTimerChannel2;
        private bool holdingWaveformChannel2 = false;
        private int currentSampleIndexChannel2 = 0;  // Tracks where new data should be appended
        private bool singleTriggeredChannel1 = false;
        private bool singleTriggeredChannel2 = false;


        public SignalDisplayControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.Black;
            SetTimeScale(1f);

            // Initialize the hold timer with a 3-second interval
            holdTimerChannel1 = new System.Timers.Timer(3000);
            holdTimerChannel1.Elapsed += (s, e) => EndHoldChannel1();
            holdTimerChannel1.AutoReset = false; // We want a single execution, no repeat

            holdTimerChannel2 = new System.Timers.Timer(3000);
            holdTimerChannel2.Elapsed += (s, e) => EndHoldChannel2();
            holdTimerChannel2.AutoReset = false; // We want a single execution, no repeat

            triggerSystem.TriggerModeChannel1 = TriggerMode.None;
            triggerSystem.TriggerModeChannel2 = TriggerMode.None;
        }

        

        private void AppendSignalData(ref float[] channelData, float[] newData)
        {
            int newDataLength = newData.Length;

            // If the existing data is smaller than the number of samples to display, expand it
            if (channelData.Length < totalSamplesToDisplay)
            {
                float[] expandedData = new float[totalSamplesToDisplay];
                Array.Copy(channelData, expandedData, channelData.Length);
                channelData = expandedData;
            }

            // If the new data exceeds the display buffer, we need to "scroll" the data
            if (newDataLength >= totalSamplesToDisplay)
            {
                // Only keep the latest portion of the new data
                channelData = newData[^totalSamplesToDisplay..];
            }
            else
            {
                // Shift existing data to the left to make room for new data
                int shiftAmount = totalSamplesToDisplay - newDataLength;
                Array.Copy(channelData, newDataLength, channelData, 0, shiftAmount);

                // Add the new data at the end
                Array.Copy(newData, 0, channelData, shiftAmount, newDataLength);
            }
        }

        // Method to set the time scale (time range to display) from 1ms to 10s
        public void SetTimeScale(float timeRangeInSeconds)
        {
            if (timeRangeInSeconds < minTimeRange || timeRangeInSeconds > maxTimeRange)
            {
                throw new ArgumentOutOfRangeException($"Time range must be between {minTimeRange} and {maxTimeRange} seconds.");
            }

            timeScale = timeRangeInSeconds;
            totalSamplesToDisplay = (int)(timeScale * sampleRate);  // Recalculate samples based on time range
            Invalidate();  // Redraw the control
        }

        public void SetAmplitudeScale(float scale)
        {
            amplitudeScale = scale;
            Invalidate();  // Redraw with new amplitude scale
        }

        // Method to toggle left channel display
        public void ToggleLeftChannelDisplay(bool enable)
        {
            displayLeftChannel = enable;
            Invalidate();  // Redraw to reflect the change
        }

        // Method to toggle right channel display
        public void ToggleRightChannelDisplay(bool enable)
        {
            displayRightChannel = enable;
            Invalidate();  // Redraw to reflect the change
        }

        // Method to draw the time/voltage grid
        private void DrawGrid(Graphics g)
        {
            int width = ClientSize.Width;
            int height = ClientSize.Height;

            // Set up drawing settings for the grid
            Pen gridPen = new Pen(Color.Yellow, 1)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
            };
            Brush labelBrush = new SolidBrush(Color.Yellow);

            // Calculate time and voltage per pixel
            float timePerPixel = timeScale / width;
            float voltagePerPixel = (2 * amplitudeScale) / height;  // Amplitude spans both positive and negative

            // Draw vertical (time) grid lines and labels
            int gridLineCount = 10; // We will always have 10 divisions for time
            for (int x = 0; x <= gridLineCount; x++)
            {
                int posX = (x * width) / gridLineCount;
                g.DrawLine(gridPen, posX, 0, posX, height);
                // Label time in milliseconds or seconds
                float timeInMs = (x * timeScale * 1000) / gridLineCount;
                g.DrawString($"{timeInMs:F0} ms", this.Font, labelBrush, posX + 2, height - 20);
            }

            // Draw horizontal (voltage) grid lines and labels
            int midY = height / 2;
            for (int y = 0; y < height; y += (int)(gridSpacingVoltage / voltagePerPixel))
            {
                int yPosAbove = midY - y;
                int yPosBelow = midY + y;

                // Draw positive and negative voltage grid lines
                g.DrawLine(gridPen, 0, yPosAbove, width, yPosAbove);
                g.DrawLine(gridPen, 0, yPosBelow, width, yPosBelow);

                // Label positive and negative voltage values
                float voltageValue = y * voltagePerPixel;
                if (yPosAbove > 0)
                    g.DrawString($"{voltageValue:F1} V", this.Font, labelBrush, 2, yPosAbove - 10);
                if (yPosBelow < height)
                    g.DrawString($"-{voltageValue:F1} V", this.Font, labelBrush, 2, yPosBelow - 10);
            }

            gridPen.Dispose();
            labelBrush.Dispose();
        }

        // Method to draw the waveform for both channels
        private void DrawSignal(Graphics g)
        {
            int width = ClientSize.Width;
            int height = ClientSize.Height;

            Pen leftChannelPen = new Pen(Color.Lime, 1);
            Pen rightChannelPen = new Pen(Color.Red, 1);

            // Draw left channel signal if enabled
            if (displayLeftChannel)
            {
                DrawWaveform(g, leftChannelPen, channel1Data, width, height, amplitudeScale);
            }

            // Draw right channel signal if enabled
            if (displayRightChannel)
            {
                DrawWaveform(g, rightChannelPen, channel2Data, width, height, amplitudeScale * 0.75f);  // Slightly offset amplitude for visibility
            }

            leftChannelPen.Dispose();
            rightChannelPen.Dispose();
        }

        // Helper method to draw a waveform
        private void DrawWaveform(Graphics g, Pen pen, float[] data, int width, int height, float amplitudeScale)
        {
            if (data.Length == 0) return;

            int samplesToDisplay = Math.Min(data.Length, totalSamplesToDisplay);
            float xScale = (float)width / samplesToDisplay;
            float yScale = (float)(height / 2) / amplitudeScale;

            for (int i = 1; i < samplesToDisplay; i++)
            {
                int x1 = (int)((i - 1) * xScale);
                int x2 = (int)(i * xScale);
                int y1 = (int)(height / 2 - data[i - 1] * yScale);
                int y2 = (int)(height / 2 - data[i] * yScale);

                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        //############### Trigger Support ###############
        public void SetTriggerLevelChannel1(float level)
        {
            triggerSystem.TriggerLevelChannel1 = level;
        }

        public void SetTriggerLevelChannel2(float level)
        {
            triggerSystem.TriggerLevelChannel2 = level;
        }

        public void SetModeChannel1(int mode)
        {
            switch (mode)
            {
                case 0:
                    {
                        triggerSystem.TriggerModeChannel1 = TriggerMode.None;
                        break;
                    }
                case 1:
                    {
                        triggerSystem.TriggerModeChannel1 = TriggerMode.Auto;
                        break;
                    }
                case 2:
                    {
                        triggerSystem.TriggerModeChannel1 = TriggerMode.Normal;
                        break;
                    }
                case 3:
                    {
                        triggerSystem.TriggerModeChannel1 = TriggerMode.Single;
                        break;
                    }
                default:
                    break;
            }
        }

        public void SetModeChannel2(int mode)
        {
            switch (mode)
            {
                case 0:
                    {
                        triggerSystem.TriggerModeChannel2 = TriggerMode.None;
                        break;
                    }
                case 1:
                    {
                        triggerSystem.TriggerModeChannel2 = TriggerMode.Auto;
                        break;
                    }
                case 2:
                    {
                        triggerSystem.TriggerModeChannel2 = TriggerMode.Normal;
                        break;
                    }
                case 3:
                    {
                        triggerSystem.TriggerModeChannel2 = TriggerMode.Single;
                        break;
                    }
                default:
                    break;
            }
        }

        public void SetTriggerTypeChannel1(int type)
        {
            switch(type)
            {
                case 0:
                    {
                        triggerSystem.TriggerTypeChannel1 = TriggerType.RisingEdge;
                        break;
                    }

                case 1:
                    {
                        triggerSystem.TriggerTypeChannel1 = TriggerType.FallingEdge;
                        break;
                    }

                case 2:
                    {
                        triggerSystem.TriggerTypeChannel1 = TriggerType.Level;
                        break;
                    }

                case 3:
                    {
                        triggerSystem.TriggerTypeChannel1 = TriggerType.Pulse;
                        break;
                    }

                case 4:
                    {
                        triggerSystem.TriggerTypeChannel1 = TriggerType.Slope;
                        break;
                    }
                default:
                    break;
            }
        }

        public void SetTriggerTypeChannel2(int type)
        {
            switch (type)
            {
                case 0:
                    {
                        triggerSystem.TriggerTypeChannel2 = TriggerType.RisingEdge;
                        break;
                    }

                case 1:
                    {
                        triggerSystem.TriggerTypeChannel2 = TriggerType.FallingEdge;
                        break;
                    }

                case 2:
                    {
                        triggerSystem.TriggerTypeChannel2 = TriggerType.Level;
                        break;
                    }

                case 3:
                    {
                        triggerSystem.TriggerTypeChannel2 = TriggerType.Pulse;
                        break;
                    }

                case 4:
                    {
                        triggerSystem.TriggerTypeChannel2 = TriggerType.Slope;
                        break;
                    }
                default:
                    break;
            }
        }

        /*
        public void SetPreTrigger(float time)
        {
            preTriggerSamples = (int)(time * sampleRate);
        }

        public void SetPostTrigger(float time)
        {
            postTriggerSamples = (int)(time * sampleRate);
        }
        */

        public void UpdateChannel1Data(float[] newSignalData)
        {
            lock (dataLock)
            {
                int samplesPerScreen = (int)(sampleRate * timeScale); // Total samples to display on the screen
                bool triggered = false;
                int triggerIndex = -1;

                switch (triggerSystem.TriggerModeChannel1)
                {
                    case TriggerMode.None:
                        {
                            // Smoothly scroll the waveform when there's no trigger mode
                            AppendSignalData(ref channel1Data, newSignalData);
                            ScrollWaveformChannel1(samplesPerScreen);
                            break;
                        }
                    case TriggerMode.Auto:
                        {
                            // Check if a trigger occurred
                            for (int i = 1; i < newSignalData.Length; i++)
                            {
                                if (newSignalData[i] >= triggerSystem.TriggerLevelChannel1 && newSignalData[i - 1] < triggerSystem.TriggerLevelChannel1)
                                {
                                    triggerIndex = i;
                                    triggered = true;
                                    break;
                                }
                            }

                            if (triggered && triggerIndex != -1 && !holdingWaveformChannel1)
                            {
                                // Align waveform to the left after the trigger
                                AlignWaveformToLeftChannel1(triggerIndex, samplesPerScreen, newSignalData);
                                triggerSystem.ResetTriggerChannel1(); // Reset for next event

                                // Start hold timer to prevent overwriting new data for 3 seconds
                                holdingWaveformChannel1 = true;
                                holdTimerChannel1.Start();
                            }
                            else if (holdingWaveformChannel1)
                            {
                                // While holding the waveform, append new data progressively
                                int spaceAvailable = samplesPerScreen - currentSampleIndexChannel1;
                                int samplesToAdd = Math.Min(spaceAvailable, newSignalData.Length);

                                if (samplesToAdd > 0)
                                {
                                    Array.Copy(newSignalData, 0, channel1Data, currentSampleIndexChannel1, samplesToAdd);
                                    currentSampleIndexChannel1 += samplesToAdd;
                                }
                            }
                            else
                            {
                                // Scroll the waveform smoothly if not holding
                                AppendSignalData(ref channel1Data, newSignalData);
                                ScrollWaveformChannel1(samplesPerScreen);
                            }

                            break;
                        }
                    case TriggerMode.Normal:
                        {
                            // Check if a trigger occurred
                            for (int i = 1; i < newSignalData.Length; i++)
                            {
                                if (newSignalData[i] >= triggerSystem.TriggerLevelChannel1 && newSignalData[i - 1] < triggerSystem.TriggerLevelChannel1)
                                {
                                    triggerIndex = i;
                                    triggered = true;
                                    break;
                                }
                            }

                            if (triggered && triggerIndex != -1 && !holdingWaveformChannel1)
                            {
                                // Align waveform to the left after the trigger
                                AlignWaveformToLeftChannel1(triggerIndex, samplesPerScreen, newSignalData);
                                triggerSystem.ResetTriggerChannel1(); // Reset for next event

                                // Start hold timer to prevent overwriting new data for 3 seconds
                                holdingWaveformChannel1 = true;
                                holdTimerChannel1.Start();
                            }
                            else if (holdingWaveformChannel1)
                            {
                                // While holding the waveform, append new data progressively
                                int spaceAvailable = samplesPerScreen - currentSampleIndexChannel1;
                                int samplesToAdd = Math.Min(spaceAvailable, newSignalData.Length);

                                if (samplesToAdd > 0)
                                {
                                    Array.Copy(newSignalData, 0, channel1Data, currentSampleIndexChannel1, samplesToAdd);
                                    currentSampleIndexChannel1 += samplesToAdd;
                                }
                            }

                            break;
                        }
                    case TriggerMode.Single:
                        {
                            if (!singleTriggeredChannel1)
                            {
                                // Check if a trigger occurred
                                for (int i = 1; i < newSignalData.Length; i++)
                                {
                                    if (newSignalData[i] >= triggerSystem.TriggerLevelChannel1 && newSignalData[i - 1] < triggerSystem.TriggerLevelChannel1)
                                    {
                                        triggerIndex = i;
                                        triggered = true;
                                        singleTriggeredChannel1 = true;
                                        break;
                                    }
                                }

                                if (triggered && triggerIndex != -1 && !holdingWaveformChannel1)
                                {
                                    // Align waveform to the left after the trigger
                                    AlignWaveformToLeftChannel1(triggerIndex, samplesPerScreen, newSignalData);
                                    triggerSystem.ResetTriggerChannel1(); // Reset for next event

                                    // Start hold timer to prevent overwriting new data for 3 seconds
                                    holdingWaveformChannel1 = true;
                                    holdTimerChannel1.Start();
                                }
                            }
                            
                            if (holdingWaveformChannel1)
                            {
                                // While holding the waveform, append new data progressively
                                int spaceAvailable = samplesPerScreen - currentSampleIndexChannel1;
                                int samplesToAdd = Math.Min(spaceAvailable, newSignalData.Length);

                                if (samplesToAdd > 0)
                                {
                                    Array.Copy(newSignalData, 0, channel1Data, currentSampleIndexChannel1, samplesToAdd);
                                    currentSampleIndexChannel1 += samplesToAdd;
                                }
                            }

                            break;
                        }
                    default:
                        break;
                }
            }

            // Refresh the control to display the updated waveform
            Invalidate();
        }

        // Method to end the hold period
        private void EndHoldChannel1()
        {
            holdingWaveformChannel1 = false; // Allow new data to be processed
            Invalidate(); // Redraw the waveform after hold ends
        }

        private void EndHoldChannel2()
        {
            holdingWaveformChannel2 = false; // Allow new data to be processed
            Invalidate(); // Redraw the waveform after hold ends
        }

        private void AlignWaveformToLeftChannel1(int triggerIndex, int samplesPerScreen, float[] newSignalData)
        {
            // Create a buffer for the full screen
            channel1Data = new float[samplesPerScreen];

            // Number of samples after the trigger
            int samplesAfterTrigger = newSignalData.Length - triggerIndex;

            // Copy data starting from the trigger point
            int samplesToCopy = Math.Min(samplesAfterTrigger, samplesPerScreen);
            Array.Copy(newSignalData, triggerIndex, channel1Data, 0, samplesToCopy);

            // The rest of the screen will be filled progressively with new data
            currentSampleIndexChannel1 = samplesToCopy; // Keep track of where the next data should be appended
        }

        private void AlignWaveformToLeftChannel2(int triggerIndex, int samplesPerScreen, float[] newSignalData)
        {
            // Create a buffer for the full screen
            channel2Data = new float[samplesPerScreen];

            // Number of samples after the trigger
            int samplesAfterTrigger = newSignalData.Length - triggerIndex;

            // Copy data starting from the trigger point
            int samplesToCopy = Math.Min(samplesAfterTrigger, samplesPerScreen);
            Array.Copy(newSignalData, triggerIndex, channel2Data, 0, samplesToCopy);

            // The rest of the screen will be filled progressively with new data
            currentSampleIndexChannel2 = samplesToCopy; // Keep track of where the next data should be appended
        }


        // Handles smooth scrolling when no trigger is detected or in bypass mode
        private void ScrollWaveformChannel1(int samplesPerScreen)
        {
            if (channel1Data.Length > samplesPerScreen)
            {
                // Truncate old data, retain only the most recent samples to fit the screen
                channel1Data = channel1Data.Skip(channel1Data.Length - samplesPerScreen).ToArray();
            }
        }

        private void ScrollWaveformChannel2(int samplesPerScreen)
        {
            if (channel2Data.Length > samplesPerScreen)
            {
                // Truncate old data, retain only the most recent samples to fit the screen
                channel2Data = channel2Data.Skip(channel2Data.Length - samplesPerScreen).ToArray();
            }
        }

        public void UpdateChannel2Data(float[] newSignalData)
        {
            lock (dataLock)
            {
                int samplesPerScreen = (int)(sampleRate * timeScale); // Total samples to display on the screen
                bool triggered = false;
                int triggerIndex = -1;

                switch (triggerSystem.TriggerModeChannel2)
                {
                    case TriggerMode.None:
                        {
                            // Smoothly scroll the waveform when there's no trigger mode
                            AppendSignalData(ref channel2Data, newSignalData);
                            ScrollWaveformChannel2(samplesPerScreen);
                            break;
                        }
                    case TriggerMode.Auto:
                        {
                            // Check if a trigger occurred
                            for (int i = 1; i < newSignalData.Length; i++)
                            {
                                if (newSignalData[i] >= triggerSystem.TriggerLevelChannel2 && newSignalData[i - 1] < triggerSystem.TriggerLevelChannel2)
                                {
                                    triggerIndex = i;
                                    triggered = true;
                                    break;
                                }
                            }

                            if (triggered && triggerIndex != -1 && !holdingWaveformChannel2)
                            {
                                // Align waveform to the left after the trigger
                                AlignWaveformToLeftChannel2(triggerIndex, samplesPerScreen, newSignalData);
                                triggerSystem.ResetTriggerChannel2(); // Reset for next event

                                // Start hold timer to prevent overwriting new data for 3 seconds
                                holdingWaveformChannel2 = true;
                                holdTimerChannel2.Start();
                            }
                            else if (holdingWaveformChannel2)
                            {
                                // While holding the waveform, append new data progressively
                                int spaceAvailable = samplesPerScreen - currentSampleIndexChannel2;
                                int samplesToAdd = Math.Min(spaceAvailable, newSignalData.Length);

                                if (samplesToAdd > 0)
                                {
                                    Array.Copy(newSignalData, 0, channel2Data, currentSampleIndexChannel2, samplesToAdd);
                                    currentSampleIndexChannel2 += samplesToAdd;
                                }
                            }
                            else
                            {
                                // Scroll the waveform smoothly if not holding
                                AppendSignalData(ref channel2Data, newSignalData);
                                ScrollWaveformChannel2(samplesPerScreen);
                            }

                            break;
                        }
                    case TriggerMode.Normal:
                        {
                            // Check if a trigger occurred
                            for (int i = 1; i < newSignalData.Length; i++)
                            {
                                if (newSignalData[i] >= triggerSystem.TriggerLevelChannel2 && newSignalData[i - 1] < triggerSystem.TriggerLevelChannel2)
                                {
                                    triggerIndex = i;
                                    triggered = true;
                                    break;
                                }
                            }

                            if (triggered && triggerIndex != -1 && !holdingWaveformChannel2)
                            {
                                // Align waveform to the left after the trigger
                                AlignWaveformToLeftChannel2(triggerIndex, samplesPerScreen, newSignalData);
                                triggerSystem.ResetTriggerChannel2(); // Reset for next event

                                // Start hold timer to prevent overwriting new data for 3 seconds
                                holdingWaveformChannel2 = true;
                                holdTimerChannel2.Start();
                            }
                            
                            if (holdingWaveformChannel2)
                            {
                                // While holding the waveform, append new data progressively
                                int spaceAvailable = samplesPerScreen - currentSampleIndexChannel2;
                                int samplesToAdd = Math.Min(spaceAvailable, newSignalData.Length);

                                if (samplesToAdd > 0)
                                {
                                    Array.Copy(newSignalData, 0, channel2Data, currentSampleIndexChannel2, samplesToAdd);
                                    currentSampleIndexChannel2 += samplesToAdd;
                                }
                            }

                            break;
                        }
                    case TriggerMode.Single:
                        {
                            if (!singleTriggeredChannel2)
                            {
                                // Check if a trigger occurred
                                for (int i = 1; i < newSignalData.Length; i++)
                                {
                                    if (newSignalData[i] >= triggerSystem.TriggerLevelChannel2 && newSignalData[i - 1] < triggerSystem.TriggerLevelChannel2)
                                    {
                                        triggerIndex = i;
                                        triggered = true;
                                        singleTriggeredChannel2 = true;
                                        break;
                                    }
                                }

                                if (triggered && triggerIndex != -1 && !holdingWaveformChannel2)
                                {
                                    // Align waveform to the left after the trigger
                                    AlignWaveformToLeftChannel2(triggerIndex, samplesPerScreen, newSignalData);
                                    triggerSystem.ResetTriggerChannel2(); // Reset for next event

                                    // Start hold timer to prevent overwriting new data for 3 seconds
                                    holdingWaveformChannel2 = true;
                                    holdTimerChannel2.Start();
                                }
                            }

                            if (holdingWaveformChannel2)
                            {
                                // While holding the waveform, append new data progressively
                                int spaceAvailable = samplesPerScreen - currentSampleIndexChannel2;
                                int samplesToAdd = Math.Min(spaceAvailable, newSignalData.Length);

                                if (samplesToAdd > 0)
                                {
                                    Array.Copy(newSignalData, 0, channel2Data, currentSampleIndexChannel2, samplesToAdd);
                                    currentSampleIndexChannel2 += samplesToAdd;
                                }
                            }
                            
                            break;
                        }
                    default:
                        break;
                }
            }

            // Refresh the control to display the updated waveform
            Invalidate();
        }

        /*
        // Handle appending pre-trigger data
        private void AppendPreTriggerData(ref float[] channelData, float[] newData, int triggerIndex)
        {
            int preTriggerIndex = Math.Max(triggerIndex - preTriggerSamples, 0);
            float[] preTriggerData = newData[preTriggerIndex..triggerIndex];
            AppendSignalData(ref channelData, preTriggerData);
        }
        */

        // Draw the time and amplitude cursors
        private void DrawCursors(Graphics g)
        {
            Pen cursorPen = new Pen(Color.Green, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
            Brush cursorBrush = new SolidBrush(Color.White);

            if (showTimeCursor)
            {
                int xPos = (int)(timeCursorPosition * ClientSize.Width);
                g.DrawLine(cursorPen, xPos, 0, xPos, ClientSize.Height);
                g.DrawString($"{timeCursorPosition:F3}s", this.Font, cursorBrush, xPos + 5, ClientSize.Height - 20);
            }

            if (showAmplitudeCursor)
            {
                int yPos = (int)(amplitudeCursorPosition * ClientSize.Height);
                g.DrawLine(cursorPen, 0, yPos, ClientSize.Width, yPos);
                g.DrawString($"{amplitudeCursorPosition:F3}V", this.Font, cursorBrush, 5, yPos - 20);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGrid(e.Graphics);
            DrawSignal(e.Graphics);
            DrawCursors(e.Graphics);  // Draw cursors over the signal
        }

        /*
        public void SetTimeCursor(float position)
        {
            showTimeCursor = true;
            timeCursorPosition = position;
            Invalidate();
        }

        public void SetAmplitudeCursor(float position)
        {
            showAmplitudeCursor = true;
            amplitudeCursorPosition = position;
            Invalidate();
        }
        */

        public void SetIntervalChannel1(int interval)
        {
            holdTimerChannel1.Interval = interval;
        }

        public void SetIntervalChannel2(int interval)
        {
            holdTimerChannel2.Interval = interval;
        }

        public void SetLevelChannel1(float level)
        {
            triggerSystem.TriggerLevelChannel1 = level;
        }

        public void SetLevelChannel2(float level)
        {;
            triggerSystem.TriggerLevelChannel2 = level;
        }

        public void ResetSingleChannel1()
        {
            singleTriggeredChannel1 = false;
            holdTimerChannel1.Enabled = false;
            holdingWaveformChannel1 = false;
        }

        public void ResetSingleChannel2()
        {
            singleTriggeredChannel2 = false;
            holdTimerChannel2.Enabled = false;
            holdingWaveformChannel2 = false;
        }

        public void SetMinPulseDurationChannel1(double duration)
        {
            triggerSystem.MinPulseDurationChannel1 = duration;
        }

        public void SetMinPulseDurationChannel2(double duration)
        {
            triggerSystem.MinPulseDurationChannel2 = duration;
        }

        public void SetMaxPulseDurationChannel1(double duration)
        {
            triggerSystem.MaxPulseDurationChannel1 = duration;
        }

        public void SetMaxPulseDurationChannel2(double duration)
        {
            triggerSystem.MaxPulseDurationChannel2 = duration;
        }

        public void SetSlopeThresholdChannel1(float threshold)
        {
            triggerSystem.SlopeThresholdChannel1 = threshold;
        }

        public void SetSlopeThresholdChannel2(float threshold)
        {
            triggerSystem.SlopeThresholdChannel2 = threshold;
        }

        public void SetTimeBetweenSamplesChannel1(float tbs)
        {
            triggerSystem.TimeBetweenSamplesChannel1 = tbs;
        }

        public void SetTimeBetweenSamplesChannel2(float tbs)
        {
            triggerSystem.TimeBetweenSamplesChannel2 = tbs;
        }
    }
}