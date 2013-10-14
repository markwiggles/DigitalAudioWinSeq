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
using System.Xml;

namespace DigitalAudioConsole
   {
   public enum PitchConversion { C, Db, D, Eb, E, F, Gb, G, Ab, A, Bb, B };

   public partial class FormMain : Form
      {
      public WaveFile m_WaveIn;
      public TimeFrequency m_TimeFrequency;
      public MusicNote[] m_SheetMusic;
      public List<MusicNote> m_MusicNoteList;
      public double m_BeatsPerMinute = 70;

      private const double m_BaseFrequency = 16.351625;  // 16.351625 "C" in Hertz

      public FormMain()
         {
         InitializeComponent();
         }

      /// <summary>
      /// Do Stuff...
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void buttonStart_Click( object sender, EventArgs e )
         {
         string audioFileName = textBoxAudioFile.Text;
         string xmlFileName = textBoxXmlFile.Text;

         // Load the WAV file and create and populate a WaveFile object. Assign that object to m_WaveIn.
         LoadWave( audioFileName );

         // Call FrequencyDomain and display a graph of the results
         if (checkBoxDisplayGraph.Checked)
            {
            // Create a "Spectrum Analysis" type map of the WAV file and set m_TimeFrequency with the resulting data.
            FrequencyDomain();

            FormGraph formGraph = new FormGraph();
            formGraph.Show();
            formGraph.DisplayGraph( m_TimeFrequency, audioFileName );
            }

         // Call ParseMusicXML and display the parsed Xml
         if (checkBoxDisplayParsedXml.Checked)
            {
            // Read and parse a MusicXML file.
            ParseMusicXML( xmlFileName );

            FormNoteData formNoteData = new FormNoteData();
            formNoteData.Show();
            formNoteData.DisplayNoteData( m_MusicNoteList );
            }
         }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void buttonLoadAudio_Click( object sender, EventArgs e )
         {
         if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
            textBoxAudioFile.Text = openFileDialog.FileName;
            }
         }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void buttonLoadXml_Click( object sender, EventArgs e )
         {
         if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
            textBoxXmlFile.Text = openFileDialog.FileName;
            }
         }

      /// <summary>
      /// Reads and parses a MusicXML file. The note data from the file is converted to data suitable for printing a score. (New version)
      /// </summary>
      /// <param name="filename"></param>
      /// <returns>A List of MusicNote objects. Each object represents one note in the MusicXML file.</returns>
      private void ParseMusicXML( string filename )
         {
         XmlDocument xmlDocument = new XmlDocument();
         XmlNodeList xmlMeasureList = null;
         XmlNodeList xmlNoteList = null;

         m_MusicNoteList = new List<MusicNote>();

         try
            {
            xmlDocument.Load( filename );
            }
         catch (Exception ex)
            {
            MessageBox.Show( "There was a problem loading the file '" + filename + "'.\n" + ex.Message, "Error" );
            }

         xmlMeasureList = xmlDocument.SelectNodes( "score-partwise/part/measure" );
         foreach (XmlNode xmlMeasureNode in xmlMeasureList)
            {
            xmlNoteList = xmlMeasureNode.SelectNodes( "note" );
            foreach (XmlNode xmlNoteNode in xmlNoteList)
               {
               if (xmlNoteNode.SelectSingleNode( "rest" ) == null)
                  {
                  XmlNode pitchNode = xmlNoteNode.SelectSingleNode( "pitch" );
                  XmlNode stepNode = pitchNode.SelectSingleNode( "step" );
                  XmlNode octaveNode = pitchNode.SelectSingleNode( "octave" );
                  XmlNode alterNode = pitchNode.SelectSingleNode( "alter" );

                  XmlNode durationNode = xmlNoteNode.SelectSingleNode( "duration" );

                  AddMusicNote( stepNode.InnerText, int.Parse( octaveNode.InnerText ), int.Parse( durationNode.InnerText ), (alterNode != null ? int.Parse( alterNode.InnerText ) : 0) ); 
                  }
               }
            }
         }


      /// <summary>
      /// Add a MusicNote object to the m_MusicNoteList
      /// </summary>
      /// <param name="step"></param>
      /// <param name="octave"></param>
      /// <param name="duration"></param>
      /// <param name="alter"></param>
      public void AddMusicNote( string step, int octave, int duration, int alter )
         {
         // Create and populate a MusicNote object for this note.
         // Convert the "letter name" of each note to a note number using the PitchConversion enum.
         int noteNum = (int)Enum.Parse( typeof( PitchConversion ), step );

         // Calculate the frequency of the note (in Hertz), correct the frequency if the alterList element for this note is != 0
         double frequency = m_BaseFrequency * Math.Pow( 2, octave ) * (Math.Pow( 2, ((double)noteNum + (double)alter) / 12 ));

         // Now create a new MusicNote object. Pass the frequency of the note and the calculated duration.
         // The duration is re-calculated to represent the number of samples per time interval. For example:
         // In "Jupiter.xml", the duration values are as follows,
         // 1=1/16
         // 2=1/8
         // 3=1/8 Dotted
         // 4=1/4
         //
         // For a sample rate of 44100 and a tempo of 70, a quarter note (duration = 4) would be converted to 37,800 which is the number of samples that equal a 1/4 note.
         // An 1/8 note would be converted to 18,900, etc.
         m_MusicNoteList.Add( new MusicNote( frequency, (double)duration * 60 * m_WaveIn.m_SampleRate / (4 * m_BeatsPerMinute), step ) );
         }

      
      /// <summary>
      /// This is used to create a "Spectrum Analysis" of the WAV, could be used to graph wave frequency over time.
      /// The array produced (m_PixelArray) is not currently being used by the application.
      /// </summary>
      public void FrequencyDomain()
         {
         // Create a new TimeFrequency object, passing the actual sound data (m_WaveIn.m_Wave) as an array of floats and setting Sample Window to 2048
         // The Sample Window tells the Fourier Transform function how many samples to aggregate into one result. The window of samples is analyzed to determine
         // the predominant frequency in that particular range of samples.
         m_TimeFrequency = new TimeFrequency( m_WaveIn.m_Wave, 2048 );
         }


      /// <summary>
      /// Loads the WAV file (filename) and parses relevant information (header, etc.)  into a WaveFile object. Sets member variable m_WaveIn.
      /// </summary>
      /// <param name="filename"></param>
      public void LoadWave( string filename )
         {
         // Sound File
         FileStream fileStream = new FileStream( filename, FileMode.Open, FileAccess.Read );
         if (fileStream == null)
            {
            MessageBox.Show( "Failed to Open File!" );
            }
         else
            {
            m_WaveIn = new WaveFile( fileStream );
            }
         }
      }
   }
