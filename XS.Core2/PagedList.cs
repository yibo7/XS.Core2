using System;
using System.Collections.Generic;
using System.Text;

namespace XS.Core2
{ 
    /// <summary>
    /// 将一个List列表进行分页
    /// </summary>
    /// <typeparam name="T">一个List数据集</typeparam>
    public class PagedList<T> : List<T>  
    {  
  
           /**//// <summary>  
        /// 分页编号  
        /// </summary>  
        public int PageIndex { get; set; }  
   
        /**//// <summary>  
        /// 分页大小  
       /// </summary>  
       public int PageSize { get; set; }  
  
       /**//// <summary>  
       /// 元素总共数目  
       /// </summary>  
       public int TotalCount { get; set; }  
  
       /**//// <summary>  
       /// 页数  
       /// </summary>  
       public int PageCount { get; set; }  
  
       /**//// <summary>  
       /// 构造函数  
       /// </summary>  
       /// <param name="list">链表</param>  
       /// <param name="intPageIndex">编号</param>  
       /// <param name="intPageSize">大小</param>  
       public PagedList(List<T> list, int intPageIndex, int intPageSize)  
       {  
           PageIndex = intPageIndex;  
           PageSize = intPageSize;  
  
           GetPagedList(list);  
       }  
  
       /**//// <summary>  
       /// 转为为分页  
       /// </summary>  
       /// <param name="list">链表</param>  
       private void GetPagedList(List<T> list)  
       {  
           int intStart = (PageIndex - 1) * PageSize;  
  
           for (int i = intStart; i < intStart + PageSize && i < list.Count; i++ )  
           {  
               this.Add(list[i]);  
           }  
  
           TotalCount = list.Count;  
           PageCount = TotalCount / PageSize + 1;  
       }  
  
    }  
  
    public static class PagedListExpansion  
    {  
        public static PagedList<T> ToPagedList<T>(List<T> list, int intPageIndex, int intPageSize)  
        {  
            return new PagedList<T>(list, intPageIndex, intPageSize);  
        }  
    }  
 

}
