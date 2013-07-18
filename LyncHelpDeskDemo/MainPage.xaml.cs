using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using LyncHelpDeskCommLib;
using System.Xml.Serialization;
using System.IO;

namespace LyncHelpDeskDemo
{
    public partial class MainPage : UserControl
    {
        private LyncClient cl = null;
        private string conversationId = string.Empty;

        public MainPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox1.ItemsSource = LyncProductHelpDesk.ProductList;
            //cl = LyncClient.GetClient();

            // レジストリ設定がされていない場合は、ここで落ちるはずなので、判定 !
            try
            {
                cl = LyncClient.GetClient();
            }
            catch(Exception exp)
            {
                return;
            }
            EnableLyncIntegration();
        }

        private void EnableLyncIntegration()
        {
            cl.ConversationManager.ConversationRemoved += new EventHandler<ConversationManagerEventArgs>(ConversationManager_ConversationRemoved);

            LyncSetupPanel.Visibility = System.Windows.Visibility.Collapsed;
            AskBtn.IsEnabled = true;
            DeskAddress.IsEnabled = true;
            QuestionTxt.IsEnabled = true;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(LyncProductHelpDesk.CWEProductInstallLink), "_self");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox1.SelectedIndex == -1)
            {
                MessageBox.Show("問い合わせをおこなうアイテムを選択してください.\n(Please select a target item.)");
                return;
            }
            cl.ConversationManager.ConversationAdded += new EventHandler<ConversationManagerEventArgs>(ConversationManager_ConversationAdded);
            Conversation con = cl.ConversationManager.AddConversation();
            this.conversationId = (string) con.Properties[ConversationProperty.Id];
        }

        private void ConversationManager_ConversationAdded(object sender, ConversationManagerEventArgs e)
        {
            cl.ConversationManager.ConversationAdded -= new EventHandler<ConversationManagerEventArgs>(ConversationManager_ConversationAdded);
            Dictionary<ContextType, object> ctxInfo = new Dictionary<ContextType, object>();
            ctxInfo.Add(ContextType.ApplicationId, LyncProductHelpDesk.ProductCWEGuid);
            QuestionInfo qinfo = new QuestionInfo()
            {
                selecedId = ((ProductItem)ListBox1.SelectedItem).Id,
                questionTxt = QuestionTxt.Text
            };
            XmlSerializer serializer = new XmlSerializer(typeof(QuestionInfo));
            TextWriter writer = new StringWriter();
            serializer.Serialize(writer, qinfo);
            ctxInfo.Add(ContextType.ApplicationData, writer.ToString());
            e.Conversation.BeginSendInitialContext(ctxInfo, BeginSendInitialContextCallback, e.Conversation);

            e.Conversation.ParticipantAdded += new EventHandler<ParticipantCollectionChangedEventArgs>(Conversation_ParticipantAdded);
            e.Conversation.AddParticipant(LyncClient.GetClient().ContactManager.GetContactByUri(DeskAddress.Text));
        }

        private void BeginSendInitialContextCallback(IAsyncResult res)
        {
            if (res.IsCompleted == true)
                ((Conversation)res.AsyncState).EndSendInitialContext(res);
        }

        private void Conversation_ParticipantAdded(object sender, ParticipantCollectionChangedEventArgs e)
        {
            Conversation con = (Conversation)sender;

            // 本人と相手の 2 回到達するため、CanInvoke で確認！
            if (con.Modalities[ModalityTypes.InstantMessage].CanInvoke(ModalityAction.Connect))
            {
                con.Modalities[ModalityTypes.InstantMessage].BeginConnect(
                    BeginConnectCallback,
                    con.Modalities[ModalityTypes.InstantMessage]);
            }
        }

        private void BeginConnectCallback(IAsyncResult res)
        {
            if (res.IsCompleted == true)
                ((InstantMessageModality)res.AsyncState).EndConnect(res);
        }

        void ConversationManager_ConversationRemoved(object sender, ConversationManagerEventArgs e)
        {
            if (string.IsNullOrEmpty(this.conversationId))
                return;

            // 問い合わせ中の Conversation がなければ、アンケート実施 !
            bool IsExist = false;
            foreach (Conversation con in cl.ConversationManager.Conversations)
            {
                if ((string)con.Properties[ConversationProperty.Id] == this.conversationId)
                    IsExist = true;
            }
            if (!IsExist)
            {
                this.conversationId = string.Empty;
                NSAT_Radio_3.IsChecked = true;
                CheckPanel.Visibility = System.Windows.Visibility.Visible;
            }

            //if ((string)e.Conversation.Properties[ConversationProperty.Id] == this.conversationId)
            //{
            //    this.conversationId = string.Empty;
            //    NSAT_Radio_3.IsChecked = true;
            //    CheckPanel.Visibility = System.Windows.Visibility.Visible;
            //}
        }

        private void NSATCommitBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckPanel.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
