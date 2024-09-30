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
    public class FifoFloatBuffer
    {
        private float[] buffer;
        private int currentSize;
        private int bufferCapacity;

        public FifoFloatBuffer(int capacity = 48000)
        {
            buffer = new float[capacity];
            currentSize = 0;
            bufferCapacity = capacity;
        }

        // Method to append a new block of floats to the buffer
        public void AppendBlock(float[] block)
        {
            if (block.Length != 4800)
            {
                throw new ArgumentException("Block size must be 4800 floats.");
            }

            // If adding the new block exceeds capacity, shift the buffer contents
            if (currentSize + block.Length > bufferCapacity)
            {
                // Calculate how many samples to discard (oldest data)
                int excess = (currentSize + block.Length) - bufferCapacity;

                // Shift the buffer to discard the oldest 'excess' samples
                Array.Copy(buffer, excess, buffer, 0, currentSize - excess);
                currentSize -= excess; // Update current size after discard
            }

            // Copy the block into the buffer
            Array.Copy(block, 0, buffer, currentSize, block.Length);
            currentSize += block.Length;
        }

        // Optionally, return the current state of the buffer (filled portion)
        public float[] GetBufferContents()
        {
            return buffer.Take(currentSize).ToArray();
        }
    }
}
