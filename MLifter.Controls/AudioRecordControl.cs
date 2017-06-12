using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LameDOTnet;
using MLifter.AudioTools;
using MLifter.Controls.Properties;

namespace MLifter.Controls
{
    public partial class AudioRecordControl : UserControl
    {
        public delegate void UserControlClickHandler(object sender, EventArgs e);
        public event UserControlClickHandler buttonPlayClick;
        public event UserControlClickHandler buttonRecordClick;
        public event UserControlClickHandler buttonDeleteClick;
        public AudioRecordControl()
        {
            InitializeComponent();
            recorder = new Recorder(Settings.Default.audioChannels, Settings.Default.audioSamplingrate);
        }
        private AudioPlayer player;
        private Recorder recorder;

        private string path = string.Empty;
        public string RecordPath
        {
            get { return path; }
            set
            {
                path = value;
                buttonPlay.Enabled = (path != string.Empty && SoundDevicesAvailable.SoundOutDeviceAvailable());
            }
        }


        private void buttonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (player == null)
                {
                    player = new AudioPlayer();
                    player.OnEnd += PlayerOnOnEnd;
                }

                if (!player.IsPlaying)
                {
                    buttonRecord.Enabled = false;
                    buttonRemove.Enabled = false;

                    buttonPlay.Image = Resources.mediaPlaybackStop;
                    buttonPlay.Text = Resources.AUDIO_STOP;

                    player.Play(RecordPath);
                }
                else
                {
                    player.Stop();
                    buttonRecord.Enabled = SoundDevicesAvailable.SoundInDeviceAvailable();
                    buttonRemove.Enabled = true;

                    buttonPlay.Image = Resources.mediaPlaybackStart;
                    buttonPlay.Text = Resources.AUDIO_PLAY;

                    if (player != null)
                        player.Stop();  // Stop the current audio played
                }

            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(Properties.Resources.AUDIOPLAYER_CRASHED_TEXT, Properties.Resources.AUDIOPLAYER_CRASHED_CAPTION,
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                System.Diagnostics.Trace.WriteLine("AudioDialog Player crashed: " + exp.ToString());
            }
        }
        private delegate void SetControlPropertyThreadSafeDelegate(
    Control control,
    string propertyName,
    object propertyValue);

        public static void SetControlPropertyThreadSafe(
            Control control,
            string propertyName,
            object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate
                (SetControlPropertyThreadSafe),
                new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(
                    propertyName,
                    BindingFlags.SetProperty,
                    null,
                    control,
                    new object[] { propertyValue });
            }
        }
        private void PlayerOnOnEnd(EventArgs eventArgs)
        {
            SetControlPropertyThreadSafe(buttonRecord, "Enabled", SoundDevicesAvailable.SoundInDeviceAvailable());
            SetControlPropertyThreadSafe(buttonRemove, "Enabled", true);
            SetControlPropertyThreadSafe(buttonPlay, "Image", Resources.mediaPlaybackStart);
            SetControlPropertyThreadSafe(buttonPlay, "Text", Resources.AUDIO_PLAY);
        }

        private void buttonRecord_Click(object sender, EventArgs e)
        {
            if (!recorder.Recording)
            {
                RecordPath = System.IO.Path.GetTempFileName();
                RecordPath = System.IO.Path.ChangeExtension(RecordPath, ".mp3");

                //buttonBrowse.Enabled = false;
                buttonPlay.Enabled = false;
                buttonRemove.Enabled = false;

                buttonRecord.Image = Resources.mediaPlaybackStop;
                buttonRecord.Text = Resources.AUDIO_STOP;
                recorder.StartRecording(RecordPath, -1, Settings.Default.audioPregap, Settings.Default.audioEndgap);
            }
            else
            {
                recorder.StopRecording();

                //buttonBrowse.Enabled = true;
                buttonPlay.Enabled = SoundDevicesAvailable.SoundOutDeviceAvailable();
                buttonRemove.Enabled = true;

                buttonRecord.Image = Resources.mediaRecord;
                buttonRecord.Text = Resources.AUDIO_RECORD;
                if (this.buttonRecordClick != null)
                {
                    this.buttonRecordClick(sender, e);
                }
            }

        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            RecordPath = string.Empty;
            if (this.buttonDeleteClick != null)
            {
                this.buttonDeleteClick(sender, e);
            }
        }
    }
}
