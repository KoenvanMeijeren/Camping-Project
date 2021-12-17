﻿using System.Windows.Controls;
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
                ChatTextBox.Clear();
            }

            ChatField.Children.Add(textblock);
        }
    }
}