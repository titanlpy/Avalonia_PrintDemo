using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KYSharp.SM;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace PrintDemo
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BtnPrintTest(object sender, RoutedEventArgs e)
        {
            SkConsume("{\"shopName\":\"承康小儿推拿总店\",\"cardNumber\":\"0004867702\",\"customer\":\"梁玲\",\"productName\":\"1407-推拿专用粉\",\"number\":\"1\",\"price\":\"3\",\"total\":\"3\",\"totalPrice\":\"3\",\"operation\":\"梁玲\",\"oddNumber\":\"000201704221745\",\"telephone\":\"0531-67818167\",\"staff\":\"1024-王超\",\"address\":\"二环东路与华龙路交叉口发展大厦B座16a\",\"remark\":\"祝宝宝快乐成长！\",\"payMethods\":[{\"payMethod\":\"现金\",\"price\":\"1\"},{\"payMethod\":\"支付宝\",\"price\":\"2\"}],\"storedMoney\":\"100\",\"giveMoney\":\"20\"}");
        }

        private void BtnSm2Test(object sender, RoutedEventArgs e)
        {
            //公钥
            string publickey = "";
            //私钥
            string privatekey = "";
            //生成公钥和私钥
            SM2Utils.GenerateKeyPair(out publickey, out privatekey);

            System.Console.Out.WriteLine("加密明文: " + "000000");
            System.Console.Out.WriteLine("publickey：" + publickey);
            //开始加密
            string cipherText = SM2Utils.Encrypt(publickey, "000000");
            System.Console.Out.WriteLine("密文: " + cipherText);
            System.Console.Out.WriteLine("privatekey：" + privatekey);
            //解密
            string plainText = SM2Utils.Decrypt(privatekey, cipherText);
            System.Console.Out.WriteLine("明文: " + plainText);
            Console.ReadLine();
        }

        BillInfo model = new BillInfo();
        /// <summary> 散客消费
        /// 散客消费
        /// </summary>
        public void SkConsume(string billInfo)
        {
            model = JsonConvert.DeserializeObject<BillInfo>(billInfo);
            //初始化打印设备
            PrintDocument printDocument = new PrintDocument();
            //设置打印用的纸张 当设置为Custom的时候，可以自定义纸张的大小，还可以选择A4,A5等常用纸型  
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 250, 400);
            printDocument.PrintPage += new PrintPageEventHandler(this.SkPrintDocument_PrintPage);
            printDocument.Print();
        }

        /// <summary> 散客消费打印的格式  
        /// 散客消费打印的格式  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void SkPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //头部设置 100    250         
            e.Graphics.DrawString(model.shopName, new Font(new FontFamily("黑体"), 11, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 58, 0);
            e.Graphics.DrawString("消费凭证", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Blue, 99, 17);
            //信息的名称                  
            e.Graphics.DrawString("会员卡号：" + model.cardNumber, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 50);

            //左 上   长   上(x,y,x1,y1:y=y1)+15
            e.Graphics.DrawLine(Pens.Black, 0, 65, 245, 65);
            e.Graphics.DrawString("项目/产品", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 72);
            e.Graphics.DrawString("数量", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 129, 72);
            e.Graphics.DrawString("单价", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 169, 72);
            e.Graphics.DrawString("总金额", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 209, 72);

            //左 上   长   上(x,y,x1,y1:y=y1)
            e.Graphics.DrawLine(Pens.Black, 0, 87, 245, 87);
            int shopFlag = 0;
            foreach (Shop shopModel in model.shops)
            {
                e.Graphics.DrawString(shopModel.productName, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 94 + shopFlag);
                e.Graphics.DrawString(shopModel.number.ToString(), new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 137, 94 + shopFlag);
                e.Graphics.DrawString(double.Parse(shopModel.price.ToString()).ToString(), new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 173, 94 + shopFlag);
                e.Graphics.DrawString(double.Parse(shopModel.total.ToString()).ToString(), new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 214, 94 + shopFlag);
                shopFlag += 12;
            }
            shopFlag = shopFlag >= 12 ? shopFlag - 12 : 0;
            //服务人员
            int isOperationEmpty = model.operation == "" ? 0 : 12;
            e.Graphics.DrawString(model.operation, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 108 + shopFlag);

            //左 上   长   上(x,y,x1,y1:y=y1)
            e.Graphics.DrawLine(Pens.Black, 0, 109 + isOperationEmpty + shopFlag, 245, 109 + isOperationEmpty + shopFlag);
            e.Graphics.DrawString("合计：", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 116 + isOperationEmpty + shopFlag);
            e.Graphics.DrawString(double.Parse(model.totalPrice.ToString()).ToString(), new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 214, 116 + isOperationEmpty + shopFlag);

            //左 上   长   上(x,y,x1,y1:y=y1)
            e.Graphics.DrawLine(Pens.Black, 0, 131 + isOperationEmpty + shopFlag, 245, 131 + isOperationEmpty + shopFlag);
            e.Graphics.DrawString("付款：", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 138 + isOperationEmpty + shopFlag);
            int i = 0;
            //付款方式
            foreach (Paymethod payMoney in model.payMethods)
            {
                e.Graphics.DrawString(payMoney.payMethod, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 129, 138 + i + isOperationEmpty + shopFlag);
                e.Graphics.DrawString(double.Parse(payMoney.price.ToString()).ToString(), new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 214, 138 + i + isOperationEmpty + shopFlag);
                i += 12;
            }
            //流水信息 
            e.Graphics.DrawString("流水单号：" + model.oddNumber, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 170 + isOperationEmpty + shopFlag);

            e.Graphics.DrawString("单据时间:" + DateTime.Now.ToString(), new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 190 + isOperationEmpty + shopFlag);

            e.Graphics.DrawString("收银员:" + model.staff, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 210 + isOperationEmpty + shopFlag);
            //地址
            if (model.address.Length > 19)
            {
                model.address = model.address.Insert(19, "\r\n");
                e.Graphics.DrawString("地址:" + model.address, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 230 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString("电话:" + "0531-67818167", new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 262 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString(model.remark, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 282 + isOperationEmpty);
            }
            else
            {
                e.Graphics.DrawString("地址:" + model.address, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 230 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString("电话:" + model.telephone, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 250 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString(model.remark, new Font(new FontFamily("黑体"), 8), System.Drawing.Brushes.Black, 0, 270 + isOperationEmpty + shopFlag);
            }
        }


    }
}
