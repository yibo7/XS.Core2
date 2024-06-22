using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
namespace XS.Core2
{
    /// <summary>
    /// 执行Cmd相关的命令
    /// </summary>
    public class CmdHelper
    {
        //private Process proc = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        //public CmdHelper()
        //{
        //    proc = new Process();
        //}

        /// <summary>
        /// 执行CMD语句
        /// </summary>
        /// <param name="cmd">要执行的CMD命令</param>
        static public string RunCmd(string cmd)
        {
            List<string> cmds = new List<string>();
            cmds.Add(cmd);
            return RunCmd(cmds);
        }
        static public string RunCmd(List<string> cmds)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            foreach (string cmd in cmds)
            {
                proc.StandardInput.WriteLine(cmd);
            }

            proc.StandardInput.WriteLine("exit");
            //string outStr = proc.StandardOutput.ReadToEnd();

            if (proc != null)
            {
                proc.Close();
            }
            return "";
        }
        /// <summary>
        /// 打开软件并执行命令
        /// </summary>
        /// <param name="programName">软件路径加名称（.exe文件）</param>
        /// <param name="cmd">要执行的命令</param>
        static public void RunProgram(string programName, string cmd)
        {
            Process proc = new Process();
            //Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = programName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (cmd.Length != 0)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            proc.Close();
        }
        /// <summary>
        /// 打开软件
        /// </summary>
        /// <param name="programName">软件路径加名称（.exe文件）</param>
        static public void RunProgram(string programName)
        {            
            RunProgram(programName, "");
        }

        /// <summary>
        /// 监测特定名称的服务是否存在
        /// </summary>
        /// <param name="serviceName">要检测的服务名称</param>
        /// <returns>存在true，不存在false</returns>
        static public bool CheckService(string serviceName)
        {
            bool bCheck = false;

            //获取windows服务列表
            ServiceController[] serviceList = ServiceController.GetServices();

            //循环查找该名称的服务
            for (int i = 0; i < serviceList.Length; i++)
            {
                if (serviceList[i].DisplayName.ToString() == serviceName)
                {
                    bCheck = true;
                    break;
                }
            }
            return bCheck;
        }
        /// <summary>
        /// 使用默认浏览器打开连接
        /// </summary>
        /// <param name="sUrl"></param>
        static public void OpenUrl(string sUrl)
        {
            // 启动默认浏览器并打开指定的URL
            Process.Start(new ProcessStartInfo
            {
                FileName = sUrl,
                UseShellExecute = true // 这允许使用操作系统的功能来打开链接
            });
        }

        /// <summary>
        /// 执行cmd命令，并返回结果
        /// </summary>
        /// <param name="BackFun">返回执行过程中的消息</param>
        /// <param name="Arguments">要执行的命令</param>
        /// <param name="exePath">要打开的exe文件路径，可以是相对路径，比如@".\\ffmpeg\\ffmpeg.exe" </param>
        /// <returns></returns>
        static public string Run(Action<string> BackFun,string Arguments,string exePath = "")
        {
            Process proc = new Process();

            try
            {
                proc.StartInfo.FileName = string.IsNullOrWhiteSpace(exePath)?"cmd.exe":exePath;

                proc.StartInfo.Arguments = Arguments;  

                // 配置Process对象
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true; // FFMPEG的输出信息在错误流中
                proc.StartInfo.CreateNoWindow = true; // 不打开窗口
                //TimeSpan totalDuration = TimeSpan.Zero;
                // 定义事件处理程序来读取输出信息
                proc.OutputDataReceived += (sender, output) =>
                {
                    string? msg = output.Data;
                    if (!string.IsNullOrEmpty(msg))
                    {
                        BackFun(msg);
                    }
                };
                proc.ErrorDataReceived += (sender, output) =>
                {
                    string? msg = output.Data;
                    if (!string.IsNullOrEmpty(msg))
                    {
                        BackFun(msg);
                    }
                };

                // 启动进程
                proc.Start();
                proc.BeginErrorReadLine(); // 开始异步读取错误流

                // 等待进程完成
                proc.WaitForExit();
 

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                // 清理资源
                proc.Close();
                proc.Dispose();
            }

            return string.Empty;
        }
    }
}

    /* 以下是一个安装windows服务的示例
     * CmdRun cmd = new CmdRun(); 
            string sPath = AppDomain.CurrentDomain.BaseDirectory;
            //SC Delete MongoDB  --删除服务 
            string sPan = Path.GetPathRoot(sPath) 

            cmd.RunCmd("SC Delete MongoDB");

            List<string> cmds = new List<string>();
            cmds.Add(string.Format("cd {0}", sPan));
            cmds.Add(string.Format(@"cd {0}mdb3\bin", sPath));
            cmds.Add(string.Format(@"mongod --dbpath {0}mdb3\data", sPath));
           
            cmd.RunCmd(cmds);

            cmds.Clear();
            cmds.Add(string.Format("cd {0}", sPan));
            cmds.Add(string.Format(@"cd {0}mdb3\bin", sPath)); 
            cmds.Add(string.Format(@"mongod.exe --dbpath={0}mdb3\data --logpath={0}mdb3\logs\mongodb.log --install --serviceName ""MongoDB""", sPath));

             cmd.RunCmd(cmds);

            cmd.RunCmd("net start MongoDB");
     */

