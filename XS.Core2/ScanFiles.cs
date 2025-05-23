 

namespace XS.Core2
{
    public class ScanFiles
    {
        public Action<string> OnShowInfo;
        public Action<string> OnFile;
        public Action OnAllComp;
        private string ScanPath;
        private Thread th = null;
        private CancellationTokenSource cts = null; // 用于取消操作

        public ScanFiles(string path)
        {
            ScanPath = path;
        }

        public void Stop()
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel(); // 请求取消线程操作
            }
        }

        public void Start()
        {
            cts = new CancellationTokenSource(); // 创建新的 CancellationTokenSource
            th = new Thread(() =>
            {
                try
                {
                    if (!Equals(OnShowInfo, null))
                        OnShowInfo("文件读入中...");

                    ToScan(ScanPath, cts.Token);

                    if (!Equals(OnAllComp, null) && !cts.Token.IsCancellationRequested)
                        OnAllComp();
                }
                catch (OperationCanceledException)
                {
                    if (!Equals(OnShowInfo, null))
                        OnShowInfo("扫描已取消。");
                }
                catch (Exception ex)
                {
                    LogHelper.Error<ScanFiles>($"扫描线程发生错误:{ex}");
                }
            });
            th.Start();
        }

        private void ToScan(string filepath, CancellationToken token)
        {
            if (filepath.Trim().Length > 0)
            {
                string[] filecollect = null;
                try
                {
                    if (!Equals(OnShowInfo, null))
                        OnShowInfo("列表扫描中...");

                    token.ThrowIfCancellationRequested(); // 检查是否取消
                    filecollect = Directory.GetFileSystemEntries(filepath);
                }
                catch (Exception ex)
                {
                    LogHelper.Error<ScanFiles>($"扫描文件夹:{filepath}发生错误:{ex}");
                }

                if (!Equals(filecollect, null))
                {
                    foreach (string file in filecollect)
                    {
                        token.ThrowIfCancellationRequested(); // 检查是否取消

                        if (Directory.Exists(file))
                        {
                            ToScan(file, token); // 递归扫描子文件夹
                        }
                        else
                        {
                            try
                            {
                                if (!Equals(OnFile, null))
                                    OnFile(file);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error<ScanFiles>($"对文件:{file}的操作发生错误:{ex}");
                            }
                        }
                    }
                }
            }
        }
    }
}
