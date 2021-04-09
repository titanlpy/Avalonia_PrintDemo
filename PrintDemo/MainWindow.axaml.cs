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
            SkConsume("{\"shopName\":\"�п�С�������ܵ�\",\"cardNumber\":\"0004867702\",\"customer\":\"����\",\"productName\":\"1407-����ר�÷�\",\"number\":\"1\",\"price\":\"3\",\"total\":\"3\",\"totalPrice\":\"3\",\"operation\":\"����\",\"oddNumber\":\"000201704221745\",\"telephone\":\"0531-67818167\",\"staff\":\"1024-����\",\"address\":\"������·�뻪��·����ڷ�չ����B��16a\",\"remark\":\"ף�������ֳɳ���\",\"payMethods\":[{\"payMethod\":\"�ֽ�\",\"price\":\"1\"},{\"payMethod\":\"֧����\",\"price\":\"2\"}],\"storedMoney\":\"100\",\"giveMoney\":\"20\"}");
        }

        private void BtnSm2Test(object sender, RoutedEventArgs e)
        {
            //��Կ
            string publickey = "";
            //˽Կ
            string privatekey = "";
            //���ɹ�Կ��˽Կ
            SM2Utils.GenerateKeyPair(out publickey, out privatekey);

            System.Console.Out.WriteLine("��������: " + "000000");
            System.Console.Out.WriteLine("publickey��" + publickey);
            //��ʼ����
            string cipherText = SM2Utils.Encrypt(publickey, "000000");
            System.Console.Out.WriteLine("����: " + cipherText);
            System.Console.Out.WriteLine("privatekey��" + privatekey);
            //����
            string plainText = SM2Utils.Decrypt(privatekey, cipherText);
            System.Console.Out.WriteLine("����: " + plainText);
            Console.ReadLine();
        }

        BillInfo model = new BillInfo();
        /// <summary> ɢ������
        /// ɢ������
        /// </summary>
        public void SkConsume(string billInfo)
        {
            model = JsonConvert.DeserializeObject<BillInfo>(billInfo);
            //��ʼ����ӡ�豸
            PrintDocument printDocument = new PrintDocument();
            //���ô�ӡ�õ�ֽ�� ������ΪCustom��ʱ�򣬿����Զ���ֽ�ŵĴ�С��������ѡ��A4,A5�ȳ���ֽ��  
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 250, 400);
            printDocument.PrintPage += new PrintPageEventHandler(this.SkPrintDocument_PrintPage);
            printDocument.Print();
        }

        /// <summary> ɢ�����Ѵ�ӡ�ĸ�ʽ  
        /// ɢ�����Ѵ�ӡ�ĸ�ʽ  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        private void SkPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //ͷ������ 100    250         
            e.Graphics.DrawString(model.shopName, new Font(new FontFamily("����"), 11, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, 58, 0);
            e.Graphics.DrawString("����ƾ֤", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Blue, 99, 17);
            //��Ϣ������                  
            e.Graphics.DrawString("��Ա���ţ�" + model.cardNumber, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 50);

            //�� ��   ��   ��(x,y,x1,y1:y=y1)+15
            e.Graphics.DrawLine(Pens.Black, 0, 65, 245, 65);
            e.Graphics.DrawString("��Ŀ/��Ʒ", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 72);
            e.Graphics.DrawString("����", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 129, 72);
            e.Graphics.DrawString("����", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 169, 72);
            e.Graphics.DrawString("�ܽ��", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 209, 72);

            //�� ��   ��   ��(x,y,x1,y1:y=y1)
            e.Graphics.DrawLine(Pens.Black, 0, 87, 245, 87);
            int shopFlag = 0;
            foreach (Shop shopModel in model.shops)
            {
                e.Graphics.DrawString(shopModel.productName, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 94 + shopFlag);
                e.Graphics.DrawString(shopModel.number.ToString(), new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 137, 94 + shopFlag);
                e.Graphics.DrawString(double.Parse(shopModel.price.ToString()).ToString(), new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 173, 94 + shopFlag);
                e.Graphics.DrawString(double.Parse(shopModel.total.ToString()).ToString(), new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 214, 94 + shopFlag);
                shopFlag += 12;
            }
            shopFlag = shopFlag >= 12 ? shopFlag - 12 : 0;
            //������Ա
            int isOperationEmpty = model.operation == "" ? 0 : 12;
            e.Graphics.DrawString(model.operation, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 108 + shopFlag);

            //�� ��   ��   ��(x,y,x1,y1:y=y1)
            e.Graphics.DrawLine(Pens.Black, 0, 109 + isOperationEmpty + shopFlag, 245, 109 + isOperationEmpty + shopFlag);
            e.Graphics.DrawString("�ϼƣ�", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 116 + isOperationEmpty + shopFlag);
            e.Graphics.DrawString(double.Parse(model.totalPrice.ToString()).ToString(), new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 214, 116 + isOperationEmpty + shopFlag);

            //�� ��   ��   ��(x,y,x1,y1:y=y1)
            e.Graphics.DrawLine(Pens.Black, 0, 131 + isOperationEmpty + shopFlag, 245, 131 + isOperationEmpty + shopFlag);
            e.Graphics.DrawString("���", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 138 + isOperationEmpty + shopFlag);
            int i = 0;
            //���ʽ
            foreach (Paymethod payMoney in model.payMethods)
            {
                e.Graphics.DrawString(payMoney.payMethod, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 129, 138 + i + isOperationEmpty + shopFlag);
                e.Graphics.DrawString(double.Parse(payMoney.price.ToString()).ToString(), new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 214, 138 + i + isOperationEmpty + shopFlag);
                i += 12;
            }
            //��ˮ��Ϣ 
            e.Graphics.DrawString("��ˮ���ţ�" + model.oddNumber, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 170 + isOperationEmpty + shopFlag);

            e.Graphics.DrawString("����ʱ��:" + DateTime.Now.ToString(), new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 190 + isOperationEmpty + shopFlag);

            e.Graphics.DrawString("����Ա:" + model.staff, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 210 + isOperationEmpty + shopFlag);
            //��ַ
            if (model.address.Length > 19)
            {
                model.address = model.address.Insert(19, "\r\n");
                e.Graphics.DrawString("��ַ:" + model.address, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 230 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString("�绰:" + "0531-67818167", new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 262 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString(model.remark, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 282 + isOperationEmpty);
            }
            else
            {
                e.Graphics.DrawString("��ַ:" + model.address, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 230 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString("�绰:" + model.telephone, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 250 + isOperationEmpty + shopFlag);

                e.Graphics.DrawString(model.remark, new Font(new FontFamily("����"), 8), System.Drawing.Brushes.Black, 0, 270 + isOperationEmpty + shopFlag);
            }
        }


    }
}
