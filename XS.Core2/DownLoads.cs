using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace XS.Core2
{

    /// <summary>
    /// 下载中的文件信息
    /// </summary>
    public class DownLoadInfo
    {
        public int MaxSize;

        public string FileName;

        public int CurrentSize;

        public bool IsComplete { get; set; }
    }

    /// <summary>
    /// 多线程文件下载（分块）
    /// </summary>
    public class DownLoads
    {
        private int _threadNum;             //线程数量
        private long _fileSize;             //文件大小
        //private string _extName;            //文件扩展名
        private string _fileUrl;            //文件地址
        private string _fileName;           //文件名
        private string _savePath;           //保存路径
        private short _threadCompleteNum;   //线程完成数量
        private bool _isComplete;           //是否完成
        private volatile int _downloadSize; //当前下载大小
        private Thread[] _thread;           //线程数组
        private List<string> _tempFiles = new List<string>();

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        public long FileSize
        {
            get
            {
                return _fileSize;
            }
        }

        public int DownloadSize
        {
            get
            {
                return _downloadSize;
            }
        }

        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }
            set
            {
                _isComplete = value;
            }
        }

        public int ThreadNum
        {
            get
            {
                return _threadNum;
            }
            set
            {
                _threadNum = value;
            }
        }

        public string SavePath
        {
            get
            {
                return _savePath;
            }
            set
            {
                _savePath = value;
            }
        }

        public DownLoads(int threahNum, string fileUrl, string savePath)
        {
            this._threadNum = threahNum;
            this._thread = new Thread[threahNum];
            this._fileUrl = fileUrl;
            this._savePath = savePath;
        }

        public void Start()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_fileUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //_extName = response.ResponseUri.ToString().Substring(response.ResponseUri.ToString().LastIndexOf('.'));//获取真实扩展名
            _fileSize = response.ContentLength;
            int singelNum = (int)(_fileSize / _threadNum);      //平均分配
            int remainder = (int)(_fileSize % _threadNum);      //获取剩余的
            request.Abort();
            response.Close();
            for (int i = 0; i < _threadNum; i++)
            {
                List<int> range = new List<int>();
                range.Add(i * singelNum);
                if (remainder != 0 && (_threadNum - 1) == i)    //剩余的交给最后一个线程
                    range.Add(i * singelNum + singelNum + remainder - 1);
                else
                    range.Add(i * singelNum + singelNum - 1);
                _thread[i] = new Thread(() => { Download(range[0], range[1]); });
                _thread[i].Name = string.Format("jhxz_{0}", (i + 1));
                _thread[i].Start();
            }
        }

        private void Download(int from, int to)
        {
            Stream httpFileStream = null, localFileStram = null;
            try
            {
                string tmpFileBlock = string.Format(@"{0}\{1}_{2}.dat", _savePath, _fileName, Thread.CurrentThread.Name);
                _tempFiles.Add(tmpFileBlock);
                HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(_fileUrl);
                httprequest.AddRange(from, to);
                HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();
                httpFileStream = httpresponse.GetResponseStream();
                localFileStram = new FileStream(tmpFileBlock, FileMode.Create);
                byte[] by = new byte[5000];
                int getByteSize = httpFileStream.Read(by, 0, (int)by.Length);           //Read方法将返回读入by变量中的总字节数
                while (getByteSize > 0)
                {
                    Thread.Sleep(20);
                    _downloadSize += getByteSize;
                    localFileStram.Write(by, 0, getByteSize);
                    getByteSize = httpFileStream.Read(by, 0, (int)by.Length);
                }
                _threadCompleteNum++;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                //Application.Exit();
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (httpFileStream != null) httpFileStream.Dispose();
                if (localFileStram != null) localFileStram.Dispose();
            }
            if (_threadCompleteNum == _threadNum)
            {
                _isComplete = true;
                Complete();
            }
        }

        private void Complete()
        {
            string sSavePath = string.Format(@"{0}\{1}", _savePath, _fileName);
            Stream mergeFile = new FileStream(sSavePath, FileMode.Create);
            BinaryWriter AddWriter = new BinaryWriter(mergeFile);
            foreach (string file in _tempFiles)
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    BinaryReader TempReader = new BinaryReader(fs);
                    AddWriter.Write(TempReader.ReadBytes((int)fs.Length));
                    TempReader.Close();
                }
                File.Delete(file);
            }
            AddWriter.Close();
        }
    }
}
