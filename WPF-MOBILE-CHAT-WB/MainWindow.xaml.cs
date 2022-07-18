using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AIMLbot;
using System.Speech.Synthesis;
using System.Threading;
using System.Diagnostics;

namespace WPF_MOBILE_CHAT_WB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User ReceiverUser { get; set; }
        public User SenderUser { get; set; }
        public string Aisp { get; set; }
        private List<Grid> _chatGrids;
        public MainWindow()
        {
            InitializeComponent();
            _chatGrids = new List<Grid>();
            GetBotResponse();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void GetBotResponse()
        {
            Bot AI = new Bot();
            //AI.loadSettings();
            //AI.loadAIMLFromFiles();
           // AI.isAcceptingUserInput = false;
            //User myuser = new User("Boss", AI);
            //AI.isAcceptingUserInput = true;
            //Request r = new Request(t2.Text, myuser, AI);
            //Result res = AI.Chat(r);
            //Aisp = res.Output;
            //SpeechSynthesizer reader = new SpeechSynthesizer();
            //reader.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Senior);
            //reader.Speak(res.Output);

        }


        private void t2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                //SendBtn.
            }
        }
        private Grid CreateMessagePanel(bool sender)
        {
            var grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition()
                    {
                        Width = new GridLength(50),
                    },
                    new ColumnDefinition()
                    {
                        Width = new GridLength(),
                    },
                },
            };

            var stackPanel = new StackPanel();

            var ellipse = new Ellipse()
            {
                StrokeThickness = 1,
                Height = 50,
                Width = 50,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5),
            };
            stackPanel.Children.Add(ellipse);
            grid.Children.Add(stackPanel);
            Grid.SetColumn(stackPanel, 0);
            var border = new Border()
            {
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
            };

            border.Background = sender
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#48435C"))
                : Brushes.White;
            var stackPanel2 = new StackPanel() { Orientation = Orientation.Vertical };

            var textBlock = new TextBlock()
            {
                Foreground = sender ? Brushes.White : Brushes.Black,
                MaxWidth = 90,
                
                TextWrapping = TextWrapping.Wrap
            };

            textBlock.Text = "Data";

            stackPanel2.Children.Add(textBlock);

            border.Child = stackPanel2;

            grid.Children.Add(border);

            Grid.SetColumn(border, 1);
            return grid;
        }



        private void RefreshChat()
        {
            ListBoxChat.ItemsSource = null;

            ListBoxChat.ItemsSource = _chatGrids;
        }
        private void LoadMessageData(User user, string message, Grid grid)
        {
            var border = grid.Children[1] as Border;
            border.CornerRadius = new CornerRadius(0,15,15,15);
            var stackPanel = border.Child as StackPanel;
            var textBlock = stackPanel.Children[0] as TextBlock;
            textBlock.Text = message + "\n"+ DateTime.Now.ToShortTimeString().PadLeft(message.Length);
            border.Margin =new Thickness(0,5,50,5);
        }

        private void LoadMessageData2(User user, string message, Grid grid)
        {
            var border = grid.Children[1] as Border;
            border.CornerRadius = new CornerRadius(15, 15, 0, 15);
            var stackPanel = border.Child as StackPanel;
            var textBlock = stackPanel.Children[0] as TextBlock;
            textBlock.Text = message +"\n" + DateTime.Now.ToShortTimeString().PadRight(message.Length);
            border.Margin = new Thickness(135,5,0,5);
        }
        private void RecevieMessage()
        {
            Task.Run(() =>
            {
               

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var newGrid = CreateMessagePanel(false);

                    LoadMessageData(SenderUser, Aisp , newGrid);

                    _chatGrids.Add(newGrid);

                    RefreshChat();
                });
            });
        }


        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(t2.Text)))
            {

            GetBotResponse();
            var newGrid = CreateMessagePanel(true);

            LoadMessageData2(ReceiverUser, t2.Text.ToString(), newGrid);

            _chatGrids.Add(newGrid);

            t2.Text = string.Empty;
            RefreshChat();

            RecevieMessage();
            }
            else
            {
                MessageBox.Show(" You did not write anything, please write text", "False Input",MessageBoxButton.OK , MessageBoxImage.Information);
            }
        }

        private void Attach_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                t2.Text = openFileDlg.SelectedPath;
            }
        }
    }
}
