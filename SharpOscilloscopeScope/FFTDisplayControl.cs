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

using System.Drawing.Drawing2D;

namespace SharpOscilloscopeScope
{
    public partial class FFTDisplayControl : UserControl
    {
        private float[] fftData;
        private float maxFrequency = 24000f; // Maximum frequency to display (Hz)
        private int sampleRate = 48000; // Default sample rate for scaling purposes
        private Color fftColor = Color.FromArgb(150, 100, 255, 100); // Shaded green
        private Pen linePen = new Pen(Color.LightBlue, 1);
        private Brush fftBrush;
        private int binCount = 4096;
        public bool fftEmpty = true;
        private Brush textBrush = Brushes.White;

        public FFTDisplayControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Helps with smooth drawing
            fftBrush = new SolidBrush(fftColor);
            fftData = new float[binCount];
        }

        // Method to update the FFT data to display
        public void UpdateFFTData(float[] newFftData)
        {
            fftData = newFftData.Take(binCount).ToArray(); // Ensure only the correct amount of bins are used
            fftEmpty = false;
            Invalidate(); // Force redraw of the control
        }

        // Set sample rate for correct scaling
        public void SetSampleRate(int newSampleRate)
        {
            sampleRate = newSampleRate;
            Invalidate();
        }

        // Override the OnPaint method to draw the FFT
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int width = ClientSize.Width;
            int height = ClientSize.Height;

            // Define the margin for the scale labels
            const int scaleMargin = 20; // Space for scale labels

            // Ensure fftData has values to plot
            if (fftEmpty)
                return;

            // Compute the maximum FFT magnitude (based on a predefined constant or normalized value)
            float MAX_FFT_MAGNITUDE = fftData.Max();// 0.1f; // For example, 16-bit audio
            float xStep = (float)width / fftData.Length;

            // Prepare the points for the FFT plot, offsetting the drawing area by the scaleMargin
            PointF[] points = new PointF[fftData.Length];
            for (int i = 0; i < fftData.Length; i++)
            {
                float x = i * xStep;
                float y = height - scaleMargin - (fftData[i] / MAX_FFT_MAGNITUDE * (height - scaleMargin) * 0.9f); // Adjust for scale margin
                points[i] = new PointF(x, y);
            }

            // Draw the FFT curve as a line graph
            if (points.Length > 1)
            {
                using (Pen fftPen = new Pen(Color.Blue, 1.5f))
                {
                    g.DrawLines(fftPen, points);
                }

                // Optionally fill under the curve for shading effect
                using (Brush fftBrush = new SolidBrush(Color.FromArgb(100, Color.Blue)))
                {
                    g.FillPolygon(fftBrush, points.Concat(new[] { new PointF(width, height - scaleMargin), new PointF(0, height - scaleMargin) }).ToArray());
                }
            }

            // Draw the frequency scale
            DrawFrequencyScale(g, width, height);
        }

        // Method to draw the frequency scale along the bottom
        private void DrawFrequencyScale(Graphics g, int width, int height)
        {
            int labelStepHz = 4000; // Step between frequency labels in Hz
            int labelCount = (int)(maxFrequency / labelStepHz); // Number of labels to draw

            Font font = new Font("Arial", 10);
            Pen scalePen = new Pen(Color.Gray, 1);
            int scaleHeight = 20; // Height of the scale area at the bottom

            // Calculate positions for the frequency labels
            for (int i = 0; i <= labelCount; i++)
            {
                float frequency = i * labelStepHz;
                float xPos = (frequency / maxFrequency) * width;
                float yPos = height - scaleHeight;

                // Draw vertical lines for the scale
                g.DrawLine(scalePen, xPos, yPos, xPos, height);

                // Draw the frequency labels (centered under the lines)
                string label = frequency.ToString("0") + " Hz";
                SizeF labelSize = g.MeasureString(label, font);
                g.DrawString(label, font, textBrush, xPos - labelSize.Width / 2, yPos + 2);
            }

            // Draw horizontal baseline for the scale
            g.DrawLine(scalePen, 0, height - scaleHeight, width, height - scaleHeight);
        }
    }
}
