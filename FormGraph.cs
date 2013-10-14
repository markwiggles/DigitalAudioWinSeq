using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalAudioConsole
   {
   public partial class FormGraph : Form
      {
      public FormGraph()
         {
         InitializeComponent();
         }


      /// <summary>
      /// Display a graphic representation of the TimeFrequency data for a WAV file
      /// </summary>
      /// <param name="timeFrequency"></param>
      public void DisplayGraph( TimeFrequency timeFrequency, string waveFilePath )
         {
         panelGraph.Width = timeFrequency.m_TimeFrequencyData.Length;
         panelGraph.Height = timeFrequency.m_TimeFrequencyData[0].Length;

         panelGraph.BackgroundImage = new Bitmap( timeFrequency.m_TimeFrequencyData.Length, timeFrequency.m_TimeFrequencyData[0].Length );
         Bitmap graphBitmap = (Bitmap)panelGraph.BackgroundImage;

         for (int horizontalIndex = 0; horizontalIndex < timeFrequency.m_WindowSampleSize / 2; horizontalIndex++)
            {
            for (int verticalIndex = 0; verticalIndex < timeFrequency.m_TimeFrequencyData[0].Length; verticalIndex++)
               {
               int color = (int)(Math.Log( timeFrequency.m_TimeFrequencyData[horizontalIndex][verticalIndex] + 1 ) * 48);
               if (color > 255)
                  {
                  color = 255;
                  }

               graphBitmap.SetPixel( horizontalIndex, verticalIndex, Color.FromArgb( 255, color, color, color ) );
               }
            }

         // Save this bitmap with the same name as the input WAV file.
         graphBitmap.Save( Path.GetFileNameWithoutExtension( waveFilePath ) + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp );
         }
      }
   }
