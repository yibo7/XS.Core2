using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XS.Core2
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceHelper
    {
        private static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
        private static readonly string AssemblyName = CurrentAssembly.GetName().Name;

        #region 基础文本读取方法

        /// <summary>
        /// 读取嵌入的文本资源
        /// </summary>
        /// <param name="resourcePath">
        /// 资源路径格式（两种方式均可）：
        /// 1. 完整路径："程序集名.目录名.文件名.扩展名"（如 "MyApp.Resources.Config.json"）
        /// 2. 相对路径："目录名.文件名.扩展名"（自动补全程序集名）
        /// </param>
        /// <param name="encoding">文本编码，默认UTF-8</param>
        /// <returns>资源内容字符串，资源不存在时返回空字符串</returns>
        public static string GetText(string resourcePath, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
                return string.Empty;

            string fullResourceName = GetFullResourceName(resourcePath);

            try
            {
                using (Stream stream = CurrentAssembly.GetManifestResourceStream(fullResourceName))
                {
                    if (stream == null)
                    {
                        // 尝试在调试时提供有用信息
                        DebugLogResourceNotFound(resourcePath, fullResourceName);
                        return string.Empty;
                    }

                    encoding ??= Encoding.UTF8;
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出，保持方法友好性
                System.Diagnostics.Debug.WriteLine($"读取嵌入资源失败: {fullResourceName}, 错误: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 尝试读取嵌入的文本资源
        /// </summary>
        /// <param name="resourcePath">资源路径</param>
        /// <param name="content">输出的内容</param>
        /// <param name="encoding">文本编码</param>
        /// <returns>是否成功读取</returns>
        public static bool TryGetText(string resourcePath, out string content, Encoding encoding = null)
        {
            content = GetText(resourcePath, encoding);
            return !string.IsNullOrEmpty(content);
        }

        #endregion

        #region 二进制资源读取

        /// <summary>
        /// 读取嵌入的二进制资源
        /// </summary>
        /// <param name="resourcePath">资源路径</param>
        /// <returns>资源字节数组，资源不存在时返回空数组</returns>
        public static byte[] GetBytes(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
                return Array.Empty<byte>();

            string fullResourceName = GetFullResourceName(resourcePath);

            try
            {
                using (Stream stream = CurrentAssembly.GetManifestResourceStream(fullResourceName))
                {
                    if (stream == null)
                        return Array.Empty<byte>();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读取二进制嵌入资源失败: {fullResourceName}, 错误: {ex.Message}");
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// 读取嵌入资源到流
        /// </summary>
        /// <param name="resourcePath">资源路径</param>
        /// <returns>资源流，使用后需要调用者释放</returns>
        public static Stream GetStream(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
                return null;

            string fullResourceName = GetFullResourceName(resourcePath);

            try
            {
                return CurrentAssembly.GetManifestResourceStream(fullResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取嵌入资源流失败: {fullResourceName}, 错误: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region 资源列表与检查

        /// <summary>
        /// 获取所有嵌入资源名称列表
        /// </summary>
        /// <returns>资源名称数组</returns>
        public static string[] GetAllResourceNames()
        {
            try
            {
                return CurrentAssembly.GetManifestResourceNames();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="resourcePath">资源路径</param>
        /// <returns>是否存在</returns>
        public static bool ResourceExists(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
                return false;

            string fullResourceName = GetFullResourceName(resourcePath);

            try
            {
                var allResources = GetAllResourceNames();
                return Array.Exists(allResources, name =>
                    string.Equals(name, fullResourceName, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取指定前缀的所有资源
        /// </summary>
        /// <param name="prefix">资源前缀（如 "SmartMedia.Resources."）</param>
        /// <returns>匹配的资源名称列表</returns>
        public static List<string> GetResourcesByPrefix(string prefix)
        {
            var result = new List<string>();

            try
            {
                var allResources = GetAllResourceNames();
                string searchPrefix = prefix?.Trim().ToLowerInvariant() ?? string.Empty;

                foreach (var resource in allResources)
                {
                    if (resource.ToLowerInvariant().StartsWith(searchPrefix))
                    {
                        result.Add(resource);
                    }
                }
            }
            catch { }

            return result;
        }

        #endregion

        #region 专用快捷方法

        /// <summary>
        /// 读取嵌入的JSON文件
        /// </summary>
        /// <param name="resourcePath">JSON资源路径</param>
        /// <returns>JSON字符串</returns>
        public static string GetJson(string resourcePath)
        {
            return GetText(resourcePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取嵌入的XML文件
        /// </summary>
        public static string GetXml(string resourcePath)
        {
            return GetText(resourcePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取嵌入的SQL脚本
        /// </summary>
        public static string GetSqlScript(string resourcePath)
        {
            return GetText(resourcePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取嵌入的配置文件（如appsettings.json）
        /// </summary>
        public static string GetConfigFile(string resourcePath)
        {
            return GetText(resourcePath, Encoding.UTF8);
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 获取完整的资源名称
        /// </summary>
        private static string GetFullResourceName(string resourcePath)
        {
            resourcePath = resourcePath.Trim().Replace('/', '.').Replace('\\', '.');

            // 如果已经是完整路径，直接返回
            if (resourcePath.StartsWith(AssemblyName + ".", StringComparison.OrdinalIgnoreCase))
                return resourcePath;

            // 补全程序集名
            return $"{AssemblyName}.{resourcePath}";
        }

        /// <summary>
        /// 调试时记录资源未找到的信息
        /// </summary>
        private static void DebugLogResourceNotFound(string requestedPath, string fullName)
        {
#if DEBUG
            var allResources = GetAllResourceNames();
            System.Diagnostics.Debug.WriteLine($"资源未找到: {requestedPath}");
            System.Diagnostics.Debug.WriteLine($"查找的全名: {fullName}");
            System.Diagnostics.Debug.WriteLine("可用资源列表:");
            foreach (var resource in allResources)
            {
                System.Diagnostics.Debug.WriteLine($"  - {resource}");
            }
#endif
        }

        #endregion
    }

    /// <summary>
    /// 嵌入资源操作扩展类（提供更简洁的API）
    /// </summary>
    public static class EmbeddedResourceExtensions
    {
        /// <summary>
        /// 从嵌入资源读取文本（扩展方法版本）
        /// </summary>
        public static string ReadEmbeddedText(this string resourcePath, Encoding encoding = null)
        {
            return ResourceHelper.GetText(resourcePath, encoding);
        }

        /// <summary>
        /// 从嵌入资源读取字节数组（扩展方法版本）
        /// </summary>
        public static byte[] ReadEmbeddedBytes(this string resourcePath)
        {
            return ResourceHelper.GetBytes(resourcePath);
        }

        /// <summary>
        /// 检查嵌入资源是否存在（扩展方法版本）
        /// </summary>
        public static bool IsEmbeddedResourceExists(this string resourcePath)
        {
            return ResourceHelper.ResourceExists(resourcePath);
        }
    }
}


/*
 
// 1. 基础文本读取
string configJson = ResourceHelper.GetText("Resources.Config.json");
string helpText = ResourceHelper.GetText("Docs.Help.txt", Encoding.UTF8);

// 2. 二进制文件读取（如图片、字体）
byte[] iconBytes = ResourceHelper.GetBytes("Images.AppIcon.ico");
byte[] fontBytes = ResourceHelper.GetBytes("Fonts.CustomFont.ttf");

// 3. 使用扩展方法（更简洁）
string sqlScript = "Scripts.Initialize.sql".ReadEmbeddedText();
bool exists = "Resources.License.txt".IsEmbeddedResourceExists();

// 4. 检查资源存在性
if (ResourceHelper.ResourceExists("Resources.Settings.xml"))
{
    string settings = ResourceHelper.GetText("Resources.Settings.xml");
}

// 5. 获取所有资源（调试用）
var allResources = ResourceHelper.GetAllResourceNames();
foreach (var resource in allResources)
{
    Console.WriteLine(resource);
}

// 6. 获取指定类型的资源
var jsonFiles = ResourceHelper.GetResourcesByPrefix("SmartMedia.Resources.Json.");
var imageFiles = ResourceHelper.GetResourcesByPrefix("SmartMedia.Resources.Images.");
 
 */
