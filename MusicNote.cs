using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalAudioConsole
   {
   /// <summary>
   /// 
   /// </summary>
   public class MusicNote
      {
      //public enum NotePitch { Ab, A_, Bb, B_, C_, Db, D_, Eb, E_, F_, Gb, G_ }  // This enum is currently unused in the application. Would be used for switch/case values below.

      public int m_Pitch; // The pitch of the note represented as a number corresponding to a named note
      public double m_DurationInSamples;
      public bool m_IsFlat;
      public double m_Error;
      public int m_StaffPosition;
      public int m_Multiplier;
      public double m_Frequency;
      public string m_NoteName;

      /// <summary>
      /// Represents a note element parsed from a MusicXML file
      /// </summary>
      /// <param name="frequency">The Frequency of the Note in Hertz</param>
      /// <param name="durationInSamples">The Duration of the note (in samples)</param>
      public MusicNote( double frequency, double durationInSamples, string noteName )
         {
         m_Frequency = frequency;
         m_DurationInSamples = durationInSamples;
         m_NoteName = noteName;
         double frequencyPitch = (Math.Log( (frequency / 110), 2 ) * 12 + 1);

         // Round the pitch to the nearest integer. Store the difference between the rounded integer value and the original 'double' value in m_Error.
         if ((Math.Ceiling( frequencyPitch ) - frequencyPitch) >= (frequencyPitch - Math.Floor( frequencyPitch )))
            {
            m_Pitch = (int)Math.Floor( frequencyPitch );
            m_Error = (frequencyPitch - Math.Floor( frequencyPitch ));
            }
         else
            {
            m_Pitch = (int)Math.Ceiling( frequencyPitch );
            m_Error = (frequencyPitch - Math.Ceiling( frequencyPitch ));
            }

         // Determine if this note number is a flat.
         if (m_Pitch % 12 == 0 || m_Pitch % 12 == 2 || m_Pitch % 12 == 5 || m_Pitch % 12 == 7 || m_Pitch % 12 == 10)
            {
            m_IsFlat = true;
            }

         // Basically determines the octave of this note: A-220 has a multiplier of '1', A-440 has a multiplier of '2', etc.
         m_Multiplier = (m_Pitch - m_Pitch % 12) / 12;

         switch (m_Pitch % 12)
            {
            case 0:
               m_StaffPosition = 7 * m_Multiplier;
               break;

            case 1:
               m_StaffPosition = 7 * m_Multiplier;
               break;

            case 2:
               m_StaffPosition = 1 + 7 * m_Multiplier;
               break;

            case 3:
               m_StaffPosition = 1 + 7 * m_Multiplier;
               break;

            case 4:
               m_StaffPosition = 2 + 7 * m_Multiplier;
               break;

            case 5:
               m_StaffPosition = 3 + 7 * m_Multiplier;
               break;

            case 6:
               m_StaffPosition = 3 + 7 * m_Multiplier;
               break;

            case 7:
               m_StaffPosition = 4 + 7 * m_Multiplier;
               break;

            case 8:
               m_StaffPosition = 4 + 7 * m_Multiplier;
               break;

            case 9:
               m_StaffPosition = 5 + 7 * m_Multiplier;
               break;

            case 10:
               m_StaffPosition = 6 + 7 * m_Multiplier;
               break;

            case 11:
               m_StaffPosition = 6 + 7 * m_Multiplier;
               break;
            }
         }
      }
   }
