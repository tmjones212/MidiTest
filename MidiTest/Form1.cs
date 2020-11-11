using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.MusicTheory;

namespace MidiTest
{

    public partial class Form1 : Form
    {
        private long prevTime = 0;
        int startOctave = 0;
        int notesPlayed = 0;
        Boolean nextOctave = true;
        string playDirection = "asc";

        Melanchall.DryWetMidi.Interaction.Note prevNote = null;
        Melanchall.DryWetMidi.Interaction.Note rootNote = null;

        int scaleInterval = 0;
        long startTime = 0;
        long endTime = 0;


        //https://www.codeproject.com/Articles/1200014/DryWetMIDI-High-level-processing-of-MIDI-files
        public Form1()
        {
            InitializeComponent();
            TestMethod();
        } 
        public void TestMethod()
        {
            Console.WriteLine("Testing...");

            var midiFile = MidiFile.Read("C:\\Users\\trent.jones\\Documents\\midi\\test my C scales.mid");

            using (NotesManager notesManager = midiFile.GetTrackChunks() // shortcut method for
                                                                         // Chunks.OfType<TrackChunk>()
                                           .First()
                                           .ManageNotes())
            {
                // Get notes ordered by time
                NotesCollection notes = notesManager.Notes;

                // Get all C# notes
                //IEnumerable<Melanchall.DryWetMidi.Interaction.Note> cSharpNotes = notes.Where(n => n.NoteName == NoteName.C);
               


                //long prevTime = 0;
                //The first note is the start octave



                foreach (var note in notes)
                {
                    notesPlayed++;
                   
                    if(notesPlayed == 1)
                    {
                        rootNote = note;
                        startOctave = note.Octave;
                        scaleInterval = 1;
                        startTime = note.Time;
                    } else if(note.NoteNumber > prevNote.NoteNumber)
                    {
                        playDirection = "asc";
                    } else if(note.NoteNumber < prevNote.NoteNumber)
                    {
                        playDirection = "desc";
                    }


                    //Set the interval
                    //Maybe let's wait until we have all the scales plugged in
                   


                    


                    Console.WriteLine(notesPlayed + " " + note.Octave + " " + note.NoteName + "(" + note.NoteNumber + ")" + " " + (note.Time - prevTime));

                    prevTime = note.Time;
                    prevNote = note;
                    

                    //If we're back to the root note...
                    if (note.NoteNumber == rootNote.NoteNumber && notesPlayed > 1)
                    {
                        Console.WriteLine("The total time was " + (note.Time - startTime));
                        ResetScale();
                        
                    }

                    
                }

                // Add new note: C# of octave with number of 2
                // Note: DryWetMIDI uses scientific pitch notation which means middle C is C4
                //notes.Add(new Melanchall.DryWetMidi.Interaction.Note(NoteName.C, 2)
                //{
                //    Channel = (FourBitNumber)2,
                //    Velocity = (SevenBitNumber)95
                //});
            }


        }

        public void ResetScale()
        {
            prevTime = 0;
            startOctave = 0;
            notesPlayed = 0;
            prevNote = null;
            rootNote = null;
            scaleInterval = 0;
            startTime = 0;
            endTime = 0;
            notesPlayed = 0;
        }
    }
}


