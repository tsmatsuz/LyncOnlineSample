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
using LyncHelpDeskCommLib;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace LyncCWEProductDemo
{
    public partial class MainPage : UserControl
    {
        private Conversation con;
        private int selectedId;
        private bool contextInitialized = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Conversation の取得
            con = (Conversation)LyncClient.GetHostingConversation();

            // 選択されたアイテムの取得
            string argString = con.GetApplicationData(LyncProductHelpDesk.ProductCWEGuid);
            XmlSerializer serializer = new XmlSerializer(typeof(QuestionInfo));
            TextReader reader = new StringReader(argString);
            QuestionInfo qinfo = (QuestionInfo)serializer.Deserialize(reader);
            this.selectedId = qinfo.selecedId;
            QuestionTxt.Text = qinfo.questionTxt;

            // 表示を更新
            this.Update();

            // コンテキストの継続的受信
            con.ContextDataReceived += new EventHandler<ContextEventArgs>(con_ContextDataReceived);
        }

        void con_ContextDataReceived(object sender, ContextEventArgs e)
        {
            selectedId = int.Parse(e.ContextData);
            this.Update();
        }

        private void ItemBackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId > 1) selectedId--;

            // 相手に通知
            this.SendContextData();

            // 表示を更新
            this.Update();
        }

        private void ItemForwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId < LyncProductHelpDesk.ProductList.Count) selectedId++;

            // 相手に通知
            this.SendContextData();

            // 表示を更新
            this.Update();
        }

        private void SendContextData()
        {
            if (!this.contextInitialized)
            {
                Dictionary<ContextType, object> ctx = new Dictionary<ContextType, object>();
                ctx.Add(ContextType.ApplicationId, LyncProductHelpDesk.ProductCWEGuid);
                //ctx.Add(ContextType.ApplicationData, selectedId.ToString());
                con.BeginSendInitialContext(
                    ctx,
                    BeginSendInitialContextCallback,
                    null);
                this.contextInitialized = true;
            }

            con.BeginSendContextData(
                LyncProductHelpDesk.ProductCWEGuid,
                @"text/plain",
                selectedId.ToString(),
                BeginSendContextDataCallback,
                null);
        }

        private void BeginSendInitialContextCallback(IAsyncResult res)
        {
            if (res.IsCompleted == true)
                con.EndSendInitialContext(res);
        }

        private void BeginSendContextDataCallback(IAsyncResult res)
        {
            if (res.IsCompleted == true)
                con.EndSendContextData(res);
        }

        private void Update()
        {
            // アイテムを取得
            var selectedItem = (from p in LyncProductHelpDesk.ProductList
                                where p.Id == selectedId
                                select p).FirstOrDefault<ProductItem>();

            // 写真 / 名前の表示
            Uri uriSource = new Uri(
                selectedItem.PhotoUrl,
                UriKind.RelativeOrAbsolute);
            productImage.Source = new BitmapImage(uriSource);
            productName.Text = selectedItem.Name;
        }
    }
}
