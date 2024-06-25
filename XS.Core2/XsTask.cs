

namespace XS.Core2
{
    /// <summary>
    /// 实现Task 的取消终止方法-使用方法参考热词挖掘
    /// </summary>
    public class XsTask
    {
        private CancellationTokenSource cts;
        private CancellationToken token;
        public Task task;
        public XsTask(Action<CancellationToken, object> fun, object prams) {
            cts = new CancellationTokenSource();
            token = cts.Token;
            task = new Task(() => fun(token, prams), token);
        }
        public XsTask(Action<CancellationToken> fun)
        {
            cts = new CancellationTokenSource();
            token = cts.Token;
            task = new Task(() => fun(token), token);
        }
        

        public void Start()
        {
            task.Start();
        }

        public void Cancel()
        { 
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
            } 
        }
        public void Dispose()
        {
            cts.Dispose();
        }
    }
}
