using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ViewModel;
using ViewModel.EventArguments;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        private const double _maxWidthTextblock = 390.00;
        public ChatPage()
        {
            this.InitializeComponent();
            ChatPageViewModel.OpenChatEvent += this.CreateChatTextBlocKEvent;
            ChatPageViewModel.SendChatEvent += this.CreateChatTextBlocKEvent;
        }

        /// <summary>
        /// Function that displays new chatmessage in chat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">ChatEventArgs(string, who sent it)</param>
        private void CreateChatTextBlocKEvent(object sender, ChatEventArgs e)
        {
            var textblock = new TextBlock();
            var brushConverter = new BrushConverter();

            textblock.Text = e.Message;
            textblock.Padding = new System.Windows.Thickness(7);
            textblock.FontFamily = new FontFamily("Century Gothic");
            textblock.TextWrapping = TextWrapping.Wrap;
            textblock.MaxWidth = _maxWidthTextblock;

            if (e.MessageSender.Equals(MessageSender.Receiver))
            {
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f2f2f2");
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }

            if (e.MessageSender.Equals(MessageSender.Sender))
            {
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f0f8ff");
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                ChatTextBox.Clear();
            }

            ChatField.Children.Add(textblock);
        }
    }
}
