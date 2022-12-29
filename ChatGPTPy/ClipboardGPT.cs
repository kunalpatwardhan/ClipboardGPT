using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChatGPTPy
{

    public partial class ClipboardGPT : Form
    {

        CmdShell cmdShell = new CmdShell();
        string receivedString;
        public ClipboardGPT()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmdShell.onData += test_onData;
            cmdShell.Exited += Test_Exited;

            cmdShell.ExecuteOnPythonShell("\"" + Application.StartupPath + "t1.py" + "\"");
        }

 
        async void ScrollToBottom()
        {
            // update below c# code so that it will scroll to half of text
            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.SelectionLength = 0;
            textBox2.ScrollToCaret();

            clipboardText = textBox2.Text;
            Clipboard.Clear();
            Clipboard.SetText(clipboardText);

            var md = textBox2.Text;
            var html = Markdig.Markdown.ToHtml(md);
            await webView21.EnsureCoreWebView2Async();
            webView21.NavigateToString( html);

        }
        void test_onData(CmdShell sender, string e)
        {
            string message;
            // Parse the JSON string into a JObject
            if (e.Contains("message"))
            {
                e = e.Replace("Enter a string: ", "");
                JObject obj = JObject.Parse(e);

                // Extract the value of the message field
                message = (string)obj["message"];
                message = Environment.NewLine + message;
                message = message.Replace("\n", Environment.NewLine);
            }
            else
            {
                message = e;
            }
            //Console.WriteLine(message);  // outputs: "Hello! How
            if (e != null)
            {
                if (textBox2.InvokeRequired)
                {
                    textBox2.Invoke((MethodInvoker)delegate
                    {
                        textBox2.Text += message;
                        ScrollToBottom();
                    });
                }
                else
                {
                    textBox2.Text += message;
                    ScrollToBottom();
                }

            }

        }
        bool exited = false;
        private async void Test_Exited(object? sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmdShell.sortStreamWriter.Write(textBox1.Text + "\r\n");
        }

        string clipboardText = "";
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (clipboardText == Clipboard.GetText())
                return;

            if (clipboardText == "")
                return;


            clipboardText = Clipboard.GetText();
            textBox1.Text = clipboardText;
            cmdShell.sortStreamWriter.Write(textBox1.Text + "\r\n");

            textBox2.Text = "Asking Chat GPT" + Environment.NewLine;
            textBox2.Text += textBox1.Text;
            textBox2.Text = textBox2.Text.Replace(Environment.NewLine, "\n");
            ScrollToBottom();
        }
    }
}