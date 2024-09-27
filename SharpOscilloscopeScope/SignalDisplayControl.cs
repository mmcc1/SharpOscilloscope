namespace SharpOscilloscopeScope
{
    public partial class SignalDisplayControl : UserControl
    {
        private float[] leftChannelData = new float[0];   // Data for left channel
        private float[] rightChannelData = new float[0];  // Data for right channel
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

        public SignalDisplayControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.Black;
            SetTimeScale(1f);
        }

        // Method to update signal data for left channel
        public void UpdateLeftChannelData(float[] newSignalData)
        {
            lock (dataLock)
            {
                AppendSignalData(ref leftChannelData, newSignalData);
            }
            Invalidate();  // Redraw the control
        }

        // Method to update signal data for right channel
        public void UpdateRightChannelData(float[] newSignalData)
        {
            lock (dataLock)
            {
                AppendSignalData(ref rightChannelData, newSignalData);
            }
            Invalidate();  // Redraw the control
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

        /*
        private void AppendSignalData(ref float[] channelData, float[] newData)
        {
            int newDataLength = newData.Length;
            int remainingSamplesToFill = totalSamplesToDisplay - channelData.Length;

            if (remainingSamplesToFill <= 0)
            {
                // If the channel data already contains enough samples, we don't append anything
                return;
            }

            // If newData has more samples than needed to fill the buffer, only take what is needed
            if (newDataLength >= remainingSamplesToFill)
            {
                // Create a new array that combines the old and new data, up to totalSamplesToDisplay
                float[] updatedData = new float[totalSamplesToDisplay];
                Array.Copy(channelData, updatedData, channelData.Length); // Copy existing data
                Array.Copy(newData, 0, updatedData, channelData.Length, remainingSamplesToFill); // Copy only what's needed from newData
                channelData = updatedData;
            }
            else
            {
                // Append the entire newData array to channelData
                float[] updatedData = new float[channelData.Length + newDataLength];
                Array.Copy(channelData, updatedData, channelData.Length); // Copy existing data
                Array.Copy(newData, 0, updatedData, channelData.Length, newDataLength); // Append new data
                channelData = updatedData;
            }
        }


        // Append incoming data to the current buffer
        private void AppendSignalData(ref float[] channelData, float[] newData)
        {
            int newDataLength = newData.Length;
            int remainingData = totalSamplesToDisplay - channelData.Length;

            if (newDataLength >= remainingData)
            {
                // If we have enough data to display the entire time period
                channelData = newData[..totalSamplesToDisplay];
            }
            else
            {
                // Append new data to the existing buffer
                float[] updatedData = new float[channelData.Length + newDataLength];
                Array.Copy(channelData, updatedData, channelData.Length);
                Array.Copy(newData, 0, updatedData, channelData.Length, newDataLength);
                channelData = updatedData;
            }
        }
        */

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

        // Custom paint method to draw the waveform and grid
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGrid(e.Graphics);  // Draw the time/voltage grid first
            DrawSignal(e.Graphics);  // Then draw the waveforms
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
                DrawWaveform(g, leftChannelPen, leftChannelData, width, height, amplitudeScale);
            }

            // Draw right channel signal if enabled
            if (displayRightChannel)
            {
                DrawWaveform(g, rightChannelPen, rightChannelData, width, height, amplitudeScale * 0.75f);  // Slightly offset amplitude for visibility
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
    }
}