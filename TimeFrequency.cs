using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DigitalAudioConsole
   {
   /// <summary>
   /// This class performs a Time/Frequency analysis of audio data passed in as an array of floats using a Fast Fourier Transformation algorithm.
   /// </summary>
   public class TimeFrequency
      {
      public float[][] m_TimeFrequencyData;
      public int m_WindowSampleSize; // The number of sound samples to aggregate into one result
      private Complex[] m_Twiddles;


      /// <summary>
      /// Initialize the object
      /// </summary>
      /// <param name="originalWaveDataArray">The actual sound data as an array of floats between -1.0 and 1.0</param>
      /// <param name="windowSampleSize">The number of sound samples to aggregate into one result</param>
      public TimeFrequency( float[] originalWaveDataArray, int windowSampleSize )
         {
         Complex imaginaryOne = Complex.ImaginaryOne;
         m_WindowSampleSize = windowSampleSize;

         // Twiddles are used in the FFT to repeatedly perform an operation on each element of an array using a pre-calculated series of values.
         // Like most other parts of the algorithm, just assume they're there for some magical purpose :)
         m_Twiddles = new Complex[m_WindowSampleSize];

         for (int sampleIndex = 0; sampleIndex < m_WindowSampleSize; sampleIndex++)
            {
            double a = 2F * Math.PI * (double)sampleIndex / (double)m_WindowSampleSize;  // Replaced the defined constant with the built-in. More accurate :)
            m_Twiddles[sampleIndex] = Complex.Pow( Complex.Exp( -imaginaryOne ), (float)a );
            }

         // 'nearestSampleWindowCount' is an int that represents a rounded-up value of how many "Sample Windows" the sound data will be divided in to.
         // In the case of "Jupiter.wav", the sound data in the WAV file is 2,382,848 bytes. With a Sample Window of 2048 bytes, we'll have 1164 blocks of data.
         int nearestSampleWindowCount = (int)Math.Ceiling( (double)originalWaveDataArray.Length / (double)m_WindowSampleSize );

         // Now we multiply that rounded value by the Window Sample size to get a nice tidy length for the array. In this case, 2,383,872  - Slightly larger
         // than the actual sound data length, so the sound data is guaranteed to fit in the array.
         int complexDataArraySize = nearestSampleWindowCount * m_WindowSampleSize;

         // Create the complexDataArray and copy the wave data in to it.
         Complex[] complexDataArray = new Complex[complexDataArraySize];
         for (int index = 0; index < complexDataArraySize; index++)
            {
            if (index < originalWaveDataArray.Length)
               {
               complexDataArray[index] = originalWaveDataArray[index];
               }
            else
               {
               // If the complexDataArray is larger than the actual wave data, just fill the extra space with zeroes.
               complexDataArray[index] = Complex.Zero;
               }
            }

         // Now start the Voodoo math :)
         ShortTimeFourierTransform( complexDataArray );
         }


      /// <summary>
      /// This will determine the frequency and phase of the WAV audio data (Complex[] sourceComplexDataArray)
      /// </summary>
      /// <param name="sourceComplexDataArray">An array of Complex objects representing the wave data</param>
      void ShortTimeFourierTransform( Complex[] sourceComplexDataArray )
         {
         Complex[] transformedComplexArray;
         int sourceComplexDataArrayLength = sourceComplexDataArray.Length;
         int halfWindowSampleSize = m_WindowSampleSize / 2;

         m_TimeFrequencyData = new float[halfWindowSampleSize][];

         // Create the second dimension of the m_TimeFrequencyData array. With our default settings ("Jupiter.wav", etc.)
         // the m_TimeFrequencyData array will contain 1024 outer floats, and each of those will have an array of 2328 floats: m_TimeFrequencyData[1024][2328]
         // This "grid" will store the Time/Frequency data of "Jupiter.wav"
         int columns = 2 * sourceComplexDataArrayLength / m_WindowSampleSize;
         for (int index = 0; index < halfWindowSampleSize; index++)
            {
            m_TimeFrequencyData[index] = new float[columns];
            }

         // This starts the actual FFT. Each block of WAV data (2048 blocks of 2328 bytes) will be passed to FastFourierTransformation() when processing "Jupiter.wav".
         // This data is processed recursively by FastFourierTransformation() (the function calls itself), so virtual all of the required processing time is used in that function.
         Complex[] untransformedComplexArray = new Complex[m_WindowSampleSize];
         for (int windowIndex = 0; windowIndex < columns - 1; windowIndex++)
            {
            for (int sampleIndex = 0; sampleIndex < m_WindowSampleSize; sampleIndex++)
               {
               untransformedComplexArray[sampleIndex] = sourceComplexDataArray[windowIndex * halfWindowSampleSize + sampleIndex];
               }

            // Process this "window" of data
            transformedComplexArray = FastFourierTransformation( untransformedComplexArray );

            // Now we have a processed "window" of data. Copy the Complex data array into the final 2-dimensional array of floats
            for (int sampleIndex = 0; sampleIndex < halfWindowSampleSize; sampleIndex++)
               {
               m_TimeFrequencyData[sampleIndex][windowIndex] = (float)Complex.Abs( transformedComplexArray[sampleIndex] );
               }
            }
         }


      /// <summary>
      /// Calculates Time/Frequency of the wave data. This is a recursive function. It splits the input array into two arays
      /// conatining the ODD and EVEN entries of the array. It then calls itself again to process the new array. Each time
      /// it re-calls itself, the array passed in is half the size, so the first time it's called, the array length will be
      /// equal to our defined "sample window" size (we're using 2048), the second time the size will be 1024, third will be 512, etc.
      /// When the passed in array size = 1, the recursive calls terminate.
      /// </summary>
      /// <param name="untransformedComplexArray">A prepared array containing wave data</param>
      /// <returns>The transformed array of frequencies over time</returns>
      Complex[] FastFourierTransformation( Complex[] untransformedComplexArray )
         {
         int untransformedComplexArrayLength = untransformedComplexArray.Length;

         Complex[] finalComplexDataArray = new Complex[untransformedComplexArrayLength];

         if (untransformedComplexArrayLength == 1)
            {
            finalComplexDataArray[0] = untransformedComplexArray[0];
            }
         else
            {
            Complex[] evenTransformedComplexArray = new Complex[untransformedComplexArrayLength / 2];
            Complex[] oddTransformedComplexArray = new Complex[untransformedComplexArrayLength / 2];
            Complex[] evenTempComplexArray = new Complex[untransformedComplexArrayLength / 2];
            Complex[] oddTempComplexArray = new Complex[untransformedComplexArrayLength / 2];

            for (int index = 0; index < untransformedComplexArrayLength; index++)
               {
               if (index % 2 == 0)
                  {
                  evenTempComplexArray[index / 2] = untransformedComplexArray[index];
                  }

               if (index % 2 == 1)
                  {
                  oddTempComplexArray[(index - 1) / 2] = untransformedComplexArray[index];
                  }
               }

            // Recursive calls to reprocess EVEN and ODD entries in the array. Array will be dived by 2 each iteration.
            evenTransformedComplexArray = FastFourierTransformation( evenTempComplexArray );
            oddTransformedComplexArray = FastFourierTransformation( oddTempComplexArray );

            for (int index = 0; index < untransformedComplexArrayLength; index++)
               {
               finalComplexDataArray[index] = evenTransformedComplexArray[index % (untransformedComplexArrayLength / 2)] + oddTransformedComplexArray[index % (untransformedComplexArrayLength / 2)] * m_Twiddles[index * m_WindowSampleSize / untransformedComplexArrayLength];
               }
            }

         return finalComplexDataArray;
         }
      }
   }
