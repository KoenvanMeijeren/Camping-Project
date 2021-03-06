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
using ViewModel;
using ViewModel.EventArguments;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for MultipleChatPage.xaml
    /// </summary>
    public partial class MultipleChatPage : Page
    {
        private const double _maxWidthTextblock = 390.00;
        public MultipleChatPage()
        {
            InitializeComponent();
            MultipleChatPageViewModel.NewChatContentEvent += this.NewSelectedChatEvent;
            MultipleChatPageViewModel.SendChatEvent += this.CreateChatTextBlocKEvent;
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
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f0f8ff");
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            }

            if (e.MessageSender.Equals(MessageSender.Sender))
            {
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f2f2f2"); 
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                ChatTextBox.Clear();
            }

            ChatField.Children.Add(textblock);
        }

        private void NewSelectedChatEvent(object sender, ChatEventArgs e)
        {
            ChatField.Children.Clear();
        }
    }
}
