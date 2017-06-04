using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using System.Windows.Controls;
using System.IO;
using System.Threading;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private bool _paused = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void clearScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            scoreTextBox.Text = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            filenameTextBox.Text = "";
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var score = scoreTextBox.Text;
            var filename = filenameTextBox.Text;
            var tempo = 200f;
            try
            {
                if (tempoTextBox.Text.Length == 0)
                {
                    throw new System.ArgumentException("The tempo box cannot be empty.");
                }
                else if (!float.TryParse(tempoTextBox.Text, out tempo))
                {
                    throw new System.ArgumentException("The tempo could not be parsed.");
                }

                if (File.Exists(filename))
                    throw new System.InvalidOperationException("File named "
                        + filename + " already exists!");
                else if (File.Exists(filename + ".wav"))
                    throw new System.InvalidOperationException("File named "
                        + filename + ".wav already exists!");

                ComposerMethods.ComposerMethods.PackSounds(score, filename, tempo);
                MessageBox.Show("A file " + filename + " was created "
                    + "successfully at your current directory!", "Success!");

                if (File.Exists(filename))
                    mediaPlayerService(filename);
                else if (File.Exists(filename + ".wav"))
                    mediaPlayerService(filename + ".wav");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
            catch (InvalidOperationException ex)
            {
                fileExistsErrorHandling(sender, e, filename, ex);
            }
        }

        private void fileExistsErrorHandling(object sender, RoutedEventArgs e,
            string filename, InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Error!");
            if (MessageBox.Show("Do you want to delete the existing file?",
                "Suggestion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _mediaPlayer.Close();
                playerButtonsState(false);
                if (File.Exists(filename + ".wav"))
                    File.Delete(filename + ".wav");

                File.Delete(filename);
            }
            else return;
        }

        private void playerButtonsState(bool state)
        {
            btnPause.IsEnabled = state;
            btnPlay.IsEnabled = state;
            btnStop.IsEnabled = state;
        }

        private void mediaPlayerService(string filename)
        {
            DispatcherTimer timer = new DispatcherTimer();
            Thread.Sleep(1000); // this is very brute force, highly not recommended in any way, shape or form
            _mediaPlayer.Open(new Uri(filename, UriKind.Relative));
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            playerButtonsState(true);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer.Source != null
                && _mediaPlayer.NaturalDuration.HasTimeSpan)
                lblStatus.Content = String.Format("{0} / {1}",
                    _mediaPlayer.Position.ToString(@"mm\:ss"),
                    _mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            else
                lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (!_paused)
            {
                if (_mediaPlayer.Position.ToString(@"mm\:ss") == "00:00")
                {
                    _mediaPlayer.Play();
                }
                else if (Strings.Left((string)lblStatus.Content, 5) == _mediaPlayer
                    .NaturalDuration.TimeSpan.ToString(@"mm\:ss"))
                {
                    resetPlayer();
                }
            }
            else if (_paused && Strings
                    .Left((string)lblStatus.Content, 5) == _mediaPlayer
                    .NaturalDuration.TimeSpan.ToString(@"mm\:ss"))
            {
                resetPlayer();
                _paused = false;
            }
            else
            {
                _mediaPlayer.Play();
                _paused = false;
            }
        }

        private void resetPlayer()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Pause();
            _paused = true;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To create your own ringtone type a note score"
                + " into the designated text box, then give it a name.\n"
                + "It's considered good practice for the name to end with .wav"
                + " but if you don't give it this ending, then the program"
                + " will do it for you.", "Help");
        }

        private void dropdown_Click(object sender, RoutedEventArgs e)
        {
            dropdownContextMenu(sender);
        }

        private static void dropdownContextMenu(object sender)
        {
            (sender as MenuItem).ContextMenu.IsEnabled = true;
            (sender as MenuItem).ContextMenu.IsOpen = true;
        }

        private void newFileBtn_Click(object sender, RoutedEventArgs e)
        {
            clearScoreBtn_Click(sender, e);
            button_Click(sender, e);
        }

        private void exitBtn(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
