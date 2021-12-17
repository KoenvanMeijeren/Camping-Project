using System;
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


        public ChatPage()
        {
            this.InitializeComponent();
            ChatPageViewModel.SendChatEvent += this.CreateChatTextBlocKEvent;

            this.CreateChatTextBlock(MessageSender.Sender, "ewaa, strijder ik heb een vraagje..");
            this.CreateChatTextBlock(MessageSender.Reciever, "Wat is je vraag bro?");
            this.CreateChatTextBlock(MessageSender.Sender, "Mag ik m'n geld terug, kut camping bro.");
            this.CreateChatTextBlock(MessageSender.Reciever, "Bel Alberto Stegeman maar");
            this.CreateChatTextBlock(MessageSender.Sender, "Dit is Alberto Stegeman, en wij gaan Nederland voor jou waarschuwen >:(");
        }

        private void CreateChatTextBlocKEvent(object sender, ChatEventArgs e)
        {
            var textblock = new TextBlock();
            var brushConverter = new BrushConverter();

            textblock.Text = e.message;
            textblock.Padding = new System.Windows.Thickness(7);
            textblock.FontFamily = new FontFamily("Century Gothic");

            if (e.messageSender.Equals(MessageSender.Reciever))
            {
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f2f2f2");
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }

            if (e.messageSender.Equals(MessageSender.Sender))
            {
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f0f8ff");
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            }

            ChatField.Children.Add(textblock);
        }
        public void CreateChatTextBlock(MessageSender messageType, string message)
        {
            var textblock = new TextBlock();
            var brushConverter = new BrushConverter();

            textblock.Text = message;
            textblock.Padding = new System.Windows.Thickness(7);
            textblock.FontFamily = new FontFamily("Century Gothic");

            if (messageType.Equals(MessageSender.Reciever))
            {
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f2f2f2");
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }

            if (messageType.Equals(MessageSender.Sender))
            {
                textblock.Background = (Brush)brushConverter.ConvertFrom("#f0f8ff");
                textblock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            }

            ChatField.Children.Add(textblock);
        }
    }
}
