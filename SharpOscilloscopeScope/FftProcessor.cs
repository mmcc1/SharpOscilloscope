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

using MathNet.Numerics.IntegralTransforms;

namespace SharpOscilloscopeScope
{
    public class FftProcessor
    {
        private int fftSize;

        public FftProcessor(int fftSize = 1024)
        {
            this.fftSize = fftSize;
        }

        // Process the input signal (float samples) and perform FFT
        public float[] ProcessFft(float[] samples)
        {
            // Ensure the input samples match the FFT size (zero-pad or truncate as needed)
            float[] processedSamples = PrepareSamples(samples);

            // Convert float array to complex array (needed for FFT)
            var complexSignal = processedSamples.Select(s => new System.Numerics.Complex(s, 0)).ToArray();

            // Perform the FFT in-place
            Fourier.Forward(complexSignal, FourierOptions.Default);

            // Extract the magnitude of the FFT (only take the positive frequencies)
            float[] fftMagnitudes = new float[fftSize / 2];
            for (int i = 0; i < fftMagnitudes.Length; i++)
            {
                fftMagnitudes[i] = (float)complexSignal[i].Magnitude;
            }

            // Send the FFT data to the display method
            return fftMagnitudes;
        }

        // This method prepares the samples (zero-padding or truncating as needed)
        private float[] PrepareSamples(float[] samples)
        {
            // If the input signal is shorter than the FFT size, zero-pad it
            if (samples.Length < fftSize)
            {
                var paddedSamples = new float[fftSize];
                Array.Copy(samples, paddedSamples, samples.Length);
                return paddedSamples;
            }

            // If the input signal is longer than the FFT size, truncate it
            return samples.Take(fftSize).ToArray();
        }
    }
}
