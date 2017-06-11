using System;
using System.IO;
using System.Windows.Forms;

namespace FileBooster
{
    public partial class Form1 : Form
    {
        string filePath;
        long nFileLength;
        public Form1(string filePath)
        {
            InitializeComponent();

            this.filePath = filePath;
            var fileInfo = new FileInfo(filePath);
            nFileLength = fileInfo.Length;
            fileNameLabel.Text = Path.GetFileName(filePath);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OKButton_Click(sender, e);
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (textBox1.Text.Length == 0)
            {
                return;
            }
            SetFileLength(Convert.ToInt64(textBox1.Text));
        }

        private void SetFileLength(long length)
        {
            if (length < nFileLength)
            {
                MessageBox.Show("파일 크기는 현재 크기보다 커야 합니다.");
                return;
            }

            BinaryReader binaryReader;
            BinaryWriter binaryWriter;
            try
            {
                binaryReader = new BinaryReader(new FileStream(filePath, FileMode.Open));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message + "\n Cannot open file (R)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var saveFilePath = filePath;
            string ext = "";
            if (saveFilePath.LastIndexOf('\\') < saveFilePath.LastIndexOf('.'))
            {
                ext = saveFilePath.Substring(saveFilePath.LastIndexOf('.'));
                saveFilePath = saveFilePath.Substring(0, saveFilePath.LastIndexOf('.'));
            }
            saveFilePath += "_mod" + ext;

            try
            {
                binaryWriter = new BinaryWriter(new FileStream(saveFilePath, FileMode.Create));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message + "\n Cannot open file (W)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                binaryReader.Close();
                return;
            }

            try
            {
                for (int i = 0; i < nFileLength; i++)
                {
                    var b = binaryReader.ReadByte();
                    binaryWriter.Write(b);
                }
                for (int i = 0; i < length - nFileLength; i++)
                {
                    binaryWriter.Write('\x00');
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message + "\n Cannot read or write file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                binaryReader.Close();
                binaryWriter.Close();
                return;
            }
            binaryReader.Close();
            binaryWriter.Close();

            MessageBox.Show($"{Path.GetFileName(filePath)} 파일이 {length} 바이트로 늘어났습니다!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
