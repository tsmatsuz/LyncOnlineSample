using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace LyncHelpDeskCommLib
{
    public class ProductItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class QuestionInfo
    {
        public int selecedId;
        public string questionTxt;
    }

    public class LyncProductHelpDesk
    {
        public static string CWEProductInstallLink = @"http://tsmatsuz1.blob.core.windows.net/share/DemoCWESetup.msi";
        public static string ProductCWEGuid = "{E817B894-BA6D-4C61-865B-F20F74B6B3DD}";
        public static List<ProductItem> ProductList;

        static LyncProductHelpDesk()
        {
            ProductList = new List<ProductItem>();
            ProductList.Add(new ProductItem()
            {
                Id = 1,
                Name = "ノート PC",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/notepc.jpg"
            });
            ProductList.Add(new ProductItem()
            {
                Id = 2,
                Name = "デスクトップ PC",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/desktoppc.jpg"
            });
            ProductList.Add(new ProductItem()
            {
                Id = 3,
                Name = "DVD",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/dvd.jpg"
            });
            ProductList.Add(new ProductItem()
            {
                Id = 4,
                Name = "フロッピー",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/floppy.jpg"
            });
            ProductList.Add(new ProductItem()
            {
                Id = 5,
                Name = "キーボード",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/keyboard.jpg"
            });
            ProductList.Add(new ProductItem()
            {
                Id = 6,
                Name = "マウス",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/mouse.jpg"
            });
            ProductList.Add(new ProductItem()
            {
                Id = 7,
                Name = "Web カメラ",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/webcam.jpg"
            });
            ProductList.Add(new ProductItem()
            {
                Id = 8,
                Name = "プロセッサー",
                PhotoUrl = @"http://i1155.photobucket.com/albums/p551/tsmatsuz/demo/processor.jpg"
            });
        }
    }
}
