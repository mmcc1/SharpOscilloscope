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

using NAudio.Wave;

namespace SharpOscilloscopeLib
{
    public class AudioCapture
    {
        private WaveInEvent wavChannel1;
        private WaveInEvent wavChannel2;
        private Action<float[], float[]> processAudioChannel1Callback;
        private Action<float[], float[]> processAudioChannel2Callback;
        private float[] channel1Buffer;
        private float[] channel2Buffer;

        public AudioCapture()
        {
        }

        public void AudioCaptureChannel1(int channel1DeviceIndex, int sampleRate, int bitRate, Action<float[], float[]> processAudioChannel1Callback)
        {
            this.processAudioChannel1Callback = processAudioChannel1Callback;

            wavChannel1 = new WaveInEvent
            {
                DeviceNumber = channel1DeviceIndex,
                WaveFormat = new WaveFormat(sampleRate, bitRate, 1)
            };
            
            wavChannel1.DataAvailable += OnDataAvailableChannel1;
        }

        public void AudioCaptureChannel2(int channel1DeviceIndex, int sampleRate, int bitRate, Action<float[], float[]> processAudioChannel1Callback)
        {
            this.processAudioChannel2Callback = processAudioChannel1Callback;

            wavChannel2 = new WaveInEvent
            {
                DeviceNumber = channel1DeviceIndex,
                WaveFormat = new WaveFormat(sampleRate, bitRate, 1)
            };

            wavChannel2.DataAvailable += OnDataAvailableChannel2;
        }

        private void OnDataAvailableChannel1(object sender, WaveInEventArgs e)
        {
            channel1Buffer = ConvertToFloatArray(e.Buffer, e.BytesRecorded);
            SendDataToProcessChannel1();
        }

        private void OnDataAvailableChannel2(object sender, WaveInEventArgs e)
        {
            channel2Buffer = ConvertToFloatArray(e.Buffer, e.BytesRecorded);
            SendDataToProcessChannel2();
        }

        private void SendDataToProcessChannel1()
        {
            if (channel1Buffer != null)
            {
                processAudioChannel1Callback(channel1Buffer, channel1Buffer);
                channel1Buffer = null;
            }
        }

        private void SendDataToProcessChannel2()
        {
            if (channel2Buffer != null)
            {
                processAudioChannel2Callback(channel2Buffer, channel2Buffer);
                channel2Buffer = null;
            }
        }

        private float[] ConvertToFloatArray(byte[] buffer, int bytesRecorded)
        {
            int samples = bytesRecorded / 2; // 16-bit PCM
            float[] floatBuffer = new float[samples];
            for (int i = 0; i < samples; i++)
            {
                floatBuffer[i] = BitConverter.ToInt16(buffer, i * 2) / 32768f; // Normalize
            }
            return floatBuffer;
        }

        public void StartRecordingChannel1()
        {
            wavChannel1.StartRecording();
        }

        public void StartRecordingChannel2()
        {
            wavChannel2.StartRecording();
        }

        public void StopRecordingChannel1()
        {
            wavChannel1.StopRecording();
        }

        public void StopRecordingChannel2()
        {
            wavChannel2.StopRecording();
        }
    }

}
