using NAudio.Wave;

namespace SharpOscilloscope
{
    internal class FuncGenerator : WaveProvider32
    {
        public enum WaveType { Sine, Square, Triangle, Sawtooth, Pulse }

        private WaveType waveType;
        private float frequency;
        private float amplitude;
        private int sampleRate;
        private int sample;
        private float pulseWidth;

        public FuncGenerator(int sampleRate = 48000, WaveType type = WaveType.Sine, float freq = 440f, float amp = 0.5f, float pulseWidth = 0.5f)
        {
            this.waveType = type;
            this.frequency = freq;
            this.amplitude = amp;
            this.sampleRate = sampleRate;
            this.pulseWidth = pulseWidth; // For pulse wave
            this.SetWaveFormat(sampleRate, 1); // 1 channel
        }

        public void SetWaveParameters(WaveType type, float freq, float amp, float pulseWidth = 0.5f)
        {
            this.waveType = type;
            this.frequency = freq;
            this.amplitude = amp;
            this.pulseWidth = pulseWidth;
        }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                float time = (float)sample / sampleRate;
                float sampleValue = 0f;

                switch (waveType)
                {
                    case WaveType.Sine:
                        sampleValue = amplitude * (float)Math.Sin(2 * Math.PI * frequency * time);
                        break;

                    case WaveType.Square:
                        sampleValue = amplitude * Math.Sign(Math.Sin(2 * Math.PI * frequency * time));
                        break;

                    case WaveType.Triangle:
                        sampleValue = amplitude * (2 * (float)(Math.Asin(Math.Sin(2 * Math.PI * frequency * time)) / Math.PI));
                        break;

                    case WaveType.Sawtooth:
                        sampleValue = amplitude * (2 * (time * frequency - (float)Math.Floor(time * frequency + 0.5f)));
                        break;

                    case WaveType.Pulse:
                        float sineValue = (float)Math.Sin(2 * Math.PI * frequency * time);
                        sampleValue = amplitude * (sineValue > Math.Cos(2 * Math.PI * pulseWidth) ? 1f : -1f);
                        break;
                }

                buffer[offset + i] = sampleValue;
                sample++;

                if (sample >= sampleRate)
                    sample = 0; // Reset sample position after each second to avoid overflow
            }

            return sampleCount;
        }
    }
}
