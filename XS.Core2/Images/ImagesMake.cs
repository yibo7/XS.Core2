using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Http;

namespace XS.Core2
{
    public class ImagesMake
    {
        /// <summary>创建规定大小的图像    /// 源图像只能是JPG格式   
        /// </summary>   
        /// <param name="oPath">源图像绝对路径</param>   
        /// <param name="tPath">生成图像绝对路径</param>   
        /// <param name="width">生成图像的宽度</param>   
        /// <param name="height">生成图像的高度</param>   
        static public void CreateImage(string oPath, string tPath, int width, int height)
        {
            Bitmap originalBmp = new Bitmap(oPath);
            // 源图像在新图像中的位置   
            int left, top;


            if (originalBmp.Width <= width && originalBmp.Height <= height)
            {
                // 原图像的宽度和高度都小于生成的图片大小   
                left = (int)Math.Round((decimal)(width - originalBmp.Width) / 2);
                top = (int)Math.Round((decimal)(height - originalBmp.Height) / 2);


                // 最终生成的图像   
                Bitmap bmpOut = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(bmpOut))
                {
                    // 设置高质量插值法   
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 清空画布并以白色背景色填充   
                    graphics.Clear(Color.White);
                    // 把源图画到新的画布上   
                    graphics.DrawImage(originalBmp, left, top);
                }
                bmpOut.Save(tPath);
                bmpOut.Dispose();


                return;
            }


            // 新图片的宽度和高度，如400*200的图像，想要生成160*120的图且不变形，   
            // 那么生成的图像应该是160*80，然后再把160*80的图像画到160*120的画布上   
            int newWidth, newHeight;
            if (width * originalBmp.Height < height * originalBmp.Width)
            {
                newWidth = width;
                newHeight = (int)Math.Round((decimal)originalBmp.Height * width / originalBmp.Width);
                // 缩放成宽度跟预定义的宽度相同的，即left=0，计算top   
                left = 0;
                top = (int)Math.Round((decimal)(height - newHeight) / 2);
            }
            else
            {
                newWidth = (int)Math.Round((decimal)originalBmp.Width * height / originalBmp.Height);
                newHeight = height;
                // 缩放成高度跟预定义的高度相同的，即top=0，计算left   
                left = (int)Math.Round((decimal)(width - newWidth) / 2);
                top = 0;
            }


            // 生成按比例缩放的图，如：160*80的图   
            Bitmap bmpOut2 = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(bmpOut2))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);
            }
            // 再把该图画到预先定义的宽高的画布上，如160*120   
            Bitmap lastbmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(lastbmp))
            {
                // 设置高质量插值法   
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 清空画布并以白色背景色填充   
                graphics.Clear(Color.White);
                // 把源图画到新的画布上   
                graphics.DrawImage(bmpOut2, left, top);
            }
            lastbmp.Save(tPath);
            lastbmp.Dispose();
        }

        /// <summary> 
        /// 生成缩略图 静态方法 
        /// </summary> 
        /// <param name="pathImageFrom"> 源图的路径(含文件名及扩展名) </param> 
        /// <param name="pathImageTo"> 生成的缩略图所保存的路径(含文件名及扩展名) 
        /// 注意：扩展名一定要与生成的缩略图格式相对应 </param> 
        /// <param name="width"> 欲生成的缩略图 "画布" 的宽度(像素值) </param> 
        /// <param name="height"> 欲生成的缩略图 "画布" 的高度(像素值) </param> 
        public static bool GenThumbnail(string pathImageFrom, string pathImageTo, int width, int height)
        {
            Image imageFrom = null;
            bool isok = false;
            try
            {
                imageFrom = Image.FromFile(pathImageFrom);
            }
            catch
            {
                //throw; 
            }

            if (imageFrom == null)
            {
                return false;
            }

            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;

            // 生成的缩略图实际宽度及高度 
            int bitmapWidth = width;
            int bitmapHeight = height;

            // 生成的缩略图在上述"画布"上的位置 
            int X = 0;
            int Y = 0;

            // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置 
            if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
            {
                bitmapHeight = imageFromHeight * width / imageFromWidth;
                Y = (height - bitmapHeight) / 2;
            }
            else
            {
                bitmapWidth = imageFromWidth * height / imageFromHeight;
                X = (width - bitmapWidth) / 2;
            }

            // 创建画布 
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);

            // 用白色清空 
            g.Clear(Color.White);

            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
            g.DrawImage(imageFrom, new Rectangle(X, Y, bitmapWidth, bitmapHeight), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);

            try
            {
                //经测试 .jpg 格式缩略图大小与质量等最优 
                bmp.Save(pathImageTo, ImageFormat.Jpeg);
                isok = true;
            }
            catch
            {
                isok = false;
            }
            finally
            {
                //显示释放资源 
                imageFrom.Dispose();
                bmp.Dispose();
                g.Dispose();
            }
            return isok;
        }

        /// <summary>
        /// 从中间位置裁切图片
        /// </summary>
        /// <param name="sPath">源乐谱的相对路径</param>
        /// <returns></returns>
        public static string CutPictureCenter(string SourePath,string targetPath,int height = 300, int width = 0)
        {

            string fullPath = SourePath;
            //string targetPath = $"/scores/image/{Path.GetFileNameWithoutExtension(fullPath)}-small{Path.GetExtension(fullPath)}";
            Image img = Image.FromFile(fullPath);
            int y = img.Height / 2;
            int iWidth = width;
            if(width==0)
            {
                iWidth = img.Width;
            }
            img.Dispose();
            CutPicture(fullPath, targetPath, 0, y, iWidth, height);
            return targetPath;


        }

        /// 图片裁剪，生成新图
        /// </summary>
        /// <param name="picPath">要修改图片完整路径</param>
        /// <param name="x">修改起点x坐标</param>
        /// <param name="y">修改起点y坐标</param>
        /// <param name="width">新图宽度</param>
        /// <param name="height">新图高度</param>
        public static void CutPicture(string sourePath, string targetPath, int x, int y, int width, int height)
        {
            //图片路径
            //String oldPath = picPath;
            //新图片路径
            //String newPath = System.IO.Path.GetExtension(oldPath);
            ////计算新的文件名，在旧文件名后加_new
            //newPath = oldPath.Substring(0, oldPath.Length - newPath.Length) + "_small" + newPath;
            //定义截取矩形
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(x, y, width, height);
            //要截取的区域大小
            //加载图片
            System.Drawing.Image img = System.Drawing.Image.FromStream(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(sourePath)));
            //判断超出的位置否
            if ((img.Width < x + width) || img.Height < y + height)
            {
                img.Dispose();
                throw new Exception("裁剪尺寸超出原有尺寸！");
                //return;
            }
            //定义Bitmap对象
            System.Drawing.Bitmap bmpImage = new System.Drawing.Bitmap(img);
            //进行裁剪
            System.Drawing.Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            //保存成新文件
            bmpCrop.Save(targetPath);
            //释放对象
            img.Dispose();
            bmpCrop.Dispose();
        }


    }
}
