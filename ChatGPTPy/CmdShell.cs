using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTPy
{
    public class CmdShell
    {
        public Process shellProcess;

        public delegate void onDataHandler(CmdShell sender, string e);
        public event onDataHandler onData;
        public event EventHandler Exited;
        public static StringBuilder sortOutput = null;
        private static int numOutputLines = 0;
        public StreamWriter sortStreamWriter;
        //  Process sortProcess;
        public async void ExecuteOnPythonShell(string scriptPath, string command = "", string parameters = "")
        {
            try
            {
                if (shellProcess != null)
                {
                    //  write("exit()");
                    //  write("\r\n");
                    //  Thread.Sleep(20);
                    shellProcess.Kill();
                }
                shellProcess = new Process();
                string pythonPath = Convert.ToChar(34) + @"C:\Users\kunal\AppData\Local\Microsoft\WindowsApps\python3.10.exe" + Convert.ToChar(34);
              //  if (System.IO.File.Exists(pythonPath) == false)
             //       pythonPath = Convert.ToChar(34) + AppContext.BaseDirectory + @"MyPython\python.exe" + Convert.ToChar(34);
                //  pythonPath = Convert.ToChar(34) + @"C:\Users\kunal\AppData\Local\Programs\Thonny\python.exe" + Convert.ToChar(34);
                if (command == "")
                {
                    command = pythonPath;
                }
                if (parameters == "")
                {
                    parameters = " -u " + scriptPath;
                }
                ProcessStartInfo si = new ProcessStartInfo(command, parameters);


                //si.Arguments = "";
                si.RedirectStandardInput = true;
                si.RedirectStandardOutput = true;
                si.RedirectStandardError = true;

                si.UseShellExecute = false;
                si.WorkingDirectory =@"C:\Users\kunal\AppData\Local\Microsoft\WindowsApps";
                //  si.WorkingDirectory = Convert.ToChar(34) + @"C:\Users\kunal\AppData\Local\Programs\Thonny" + Convert.ToChar(34);
                si.CreateNoWindow = true;
                //si.CreateNoWindow = false;
                si.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //si.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                shellProcess.StartInfo = si;
                shellProcess.EnableRaisingEvents = true;
                shellProcess.OutputDataReceived += shellProcess_OutputDataReceived;
                shellProcess.ErrorDataReceived += shellProcess_ErrorDataReceived;
                shellProcess.Exited += ShellProcess_Exited;
                shellProcess.Start();
                shellProcess.BeginErrorReadLine();
                shellProcess.BeginOutputReadLine();
                sortStreamWriter = shellProcess.StandardInput;

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void ShellProcess_Exited(object? sender, EventArgs e)
        {
            if (Exited != null) Exited(sender, e);
        }

        void shellProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //  doOnData(e.Data);
            if (!String.IsNullOrEmpty(e.Data))
            {

                // Add the text to the collected output.
                // sortOutput.Append(Environment.NewLine +
                //      $"[{numOutputLines}] - {e.Data}");
                doOnData(Environment.NewLine +
                    e.Data);
            }
        }

        void shellProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {


                // Add the text to the collected output.
                //    sortOutput.Append(Environment.NewLine +
                //         $"[{numOutputLines}] - {e.Data}");
                doOnData(Environment.NewLine +
                    e.Data);
            }
        }

        private void doOnData(string data)
        {
            if (onData != null) onData(this, data);
        }

        public void write(string data)
        {
            try
            {
                shellProcess.StandardInput.WriteLine(data);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
