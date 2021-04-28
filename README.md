# Avalonia_PrintDemo
可在windows和Linux实现打印

Linux下需安装虚拟PDF打印机
1、安装CPUS-PDF
sudo apt install -y cups-pdf
2、需要设置 cups-pdf 的所有者为 root,且权限为 0700
sudo chown root:root /usr/lib/cups/backend/cups-pdf
sudo chmod 0700 /usr/lib/cups/backend/cups-pdf
3、重启CUPS服务
sudo systemctl restart cups.service
4、可以通过修改 /etc/cups/cups-pdf.conf 的 OUT 参数来设置pdf的输出位置
Out ${HOME}/PDF      //这是默认位置
5、在“打印设置”中就可以看到虚拟pdf打印机了或者使用命令lpstat -p -d查看
