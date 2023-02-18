using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace XS.Core2
{
    /// <summary>
    /// 基于HtmlAgilityPackhtml文档解析辅助类库
    /// </summary>
    public class HtmlParse
    {
        private readonly HtmlDocument doc = new HtmlDocument();

        /// <summary>
        /// 构造函数 初始化文档并解析 默认utf-8模式
        /// </summary>
        /// <param name="htmlOrUrl">获取的html字符串或url链接</param>
        public HtmlParse(string htmlOrUrl)
        {
            InitDoc(htmlOrUrl);
        }

        /// <summary>
        /// 构造函数 初始化文档并解析 默认utf-8模式
        /// </summary>
        /// <param name="htmlOrUrl">获取的html字符串或url链接</param>
        /// <param name="encode">字符编码</param>
        public HtmlParse(string htmlOrUrl, bool IsUtf8)
        {
            InitDoc(htmlOrUrl, IsUtf8);
        }


        /// <summary>
        ///     根据url或html字符串获取文档并解析
        /// </summary>
        /// <param name="htmlOrUrl">html字符串或url</param>
        /// <param name="encode">网站编码</param>
        /// <returns></returns>
        public HtmlDocument InitDoc(string htmlOrUrl, bool IsUtf8=false)
        {
            if (htmlOrUrl.Trim().StartsWith("http")|| htmlOrUrl.Trim().StartsWith("https"))
            {
                if (IsUtf8)
                {
                    htmlOrUrl = WebUtils.LoadURLStringUTF8(htmlOrUrl);
                }
                else
                {
                    htmlOrUrl = WebUtils.GetHtml(htmlOrUrl);
                    // WebUtils.LoadURLString(htmlOrUrl);
                }
            }
            doc.LoadHtml(htmlOrUrl);
            return doc;
        }

        /// <summary>
        /// 获取节点集合
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public HtmlNodeCollection GetNodes(string xPath)
        {
            return doc.DocumentNode.SelectNodes(xPath);
        }


        /// <summary>
        /// 获取单个节点
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public HtmlNode GetNode(string xPath)
        {
            return doc.DocumentNode.SelectSingleNode(xPath);
        }

        /// <summary>
        /// 获取节点的属性值
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attrName">属性名称</param>
        /// <returns></returns>
        public string GetNodeAttr(HtmlNode node, string attrName)
        {
            if (node == null || node.Attributes[attrName] == null)
            {
                return string.Empty;
            }
            return node.Attributes[attrName].Value;
        }

        /// <summary>
        /// 获取节点的InnerText的值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string GetNodeText(HtmlNode node)
        {
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }

        /// <summary>
        /// 获取节点的InnerHtml或OuterHtml值
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="isOuter">是否要获取OuterHtml</param>
        /// <returns></returns>
        public string GetNodeHtml(HtmlNode node, bool isOuter = false)
        {
            if (node == null)
            {
                return string.Empty;
            }
            if (isOuter)
            {
                return node.OuterHtml;
            }
            return node.InnerHtml;
        }

        /// <summary>
        /// 根据Xpath和属性名称获取属性值
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public string GetNodeAttr(string xPath, string attrName)
        {
            var node = GetNode(xPath);
            return GetNodeAttr(node, attrName);
        }

        /// <summary>
        /// 根据XPath获取节点的InnerText
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public string GetNodeText(string xPath)
        {
            var node = GetNode(xPath);
            return GetNodeText(node);
        }

        /// <summary>
        /// 根据XPath获取节点的InnerHtml或OuterHtml值
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="isOuter"></param>
        /// <returns></returns>
        public string GetNodeHtml(string xPath, bool isOuter = false)
        {
            var node = GetNode(xPath);
            return GetNodeHtml(node);
        }
    }
}
