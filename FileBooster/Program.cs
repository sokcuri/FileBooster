using System;
using System.Windows.Forms;

namespace FileBooster
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All Files(*.*)|*.*";
            fileDialog.Title = "파일을 선택하세요";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1(fileDialog.FileName));
            }

            
        }
    }
}
