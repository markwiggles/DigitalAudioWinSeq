using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalAudioConsole
   {
   public partial class FormNoteData : Form
      {
      public FormNoteData()
         {
         InitializeComponent();
         }

      public void DisplayNoteData( List<MusicNote> musicNoteList )
         {
         foreach (MusicNote musicNote in musicNoteList)
            {
            string[] items = { musicNote.m_Frequency.ToString(), musicNote.m_DurationInSamples.ToString() };
            listViewNoteData.Items.Add( musicNote.m_NoteName + (musicNote.m_IsFlat ? "b" : " ") ).SubItems.AddRange( items );
            }
         }
      }
   }
