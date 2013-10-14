using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DigitalAudioConsole
   {
   /// <summary>
   /// This class parses a standard WAV file and populates its member variables. It assumes a specific ordering of header information (which will work), but I would recommend using a library such as NAudio.
   /// In addition to simplifying your code, you would also have the ability to read and process multiple types of audio files if you'd like:
   /// http://naudio.codeplex.com/
   /// </summary>
   public class WaveFile
      {
      public float[] m_Wave; // The actual sound data as a FLOAT array
      public byte[] m_Data; // The actual sound data as a BYTE array
      public char[] m_ChunkID = new char[4];
      public int m_ChunkSize;
      public char[] m_Format = new char[4];
      public char[] m_SubChunk1ID = new char[4];
      public int m_SubChunk1Size;
      public char[] m_SubChunk2ID = new char[4];
      public int m_SubChunk2Size;
      public short m_AudioFormat;
      public short m_NumChannels;
      public int m_SampleRate;
      public int m_ByteRate;
      public short m_BlockAlign;
      public short m_BitsPerSample;

      public WaveFile( FileStream file )
         {
         BinaryReader binaryReader = new BinaryReader( file );

         // Pull out blocks of info for this WAV file sequentially and populate the member variables.
         m_ChunkID = binaryReader.ReadChars( 4 );
         m_ChunkSize = binaryReader.ReadInt32();
         m_Format = binaryReader.ReadChars( 4 );
         m_SubChunk1ID = binaryReader.ReadChars( 4 );
         m_SubChunk1Size = binaryReader.ReadInt32();
         m_AudioFormat = binaryReader.ReadInt16();
         m_NumChannels = binaryReader.ReadInt16();
         m_SampleRate = binaryReader.ReadInt32();
         m_ByteRate = binaryReader.ReadInt32();
         m_BlockAlign = binaryReader.ReadInt16();
         m_BitsPerSample = binaryReader.ReadInt16();
         m_SubChunk2ID = binaryReader.ReadChars( 4 );
         m_SubChunk2Size = binaryReader.ReadInt32();

         // Read the actual sound data as an array of bytes
         m_Data = binaryReader.ReadBytes( m_SubChunk2Size );

         // Convert the BYTE array to a FLOAT array (16 bits samples are handled the same way, but in two byte pairs... see the 'else if' below)
         // Each float will be in the range of -1.0 to 1.0 so,
         // BYTE     FLOAT
         // ----     -----
         // 0        -1.0
         // 128       0.0
         // 255       1.0    (technically, it will be .992, but close enough. 256 would be be exactly 1.0 but a byte ranges from 0-255 )
         if (m_BitsPerSample == 8)
            {
            m_Wave = new float[m_SubChunk2Size];

            for (int index = 0; index < m_SubChunk2Size; index++)
               {
               m_Wave[index] = ((float)m_Data[index] - 128) / 128;
               }
            }
         else if (m_BitsPerSample == 16)
            {
            int waveIndex = 0;
            m_Wave = new float[m_SubChunk2Size / 2];

            for (int index = 0; index < m_SubChunk2Size; index += 2)
               {
               m_Wave[waveIndex++] = BytesToFloat( m_Data[index], m_Data[index + 1] );
               }
            }
         }

      /// <summary>
      /// Converts 2 bytes (16 bit integer) to a float between -1 and 1
      /// </summary>
      /// <param name="firstByte"></param>
      /// <param name="secondByte"></param>
      /// <returns>A float between -1 and 1</returns>
      private float BytesToFloat( byte firstByte, byte secondByte )
         {
         Int16 twoBytes = (Int16)((secondByte << 8) | firstByte);
         return twoBytes / 32768F;
         }
      }
   }
