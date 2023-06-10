using SkiaAndGdi;
using SkiaAndGdi.Gdi;
using SkiaAndGdi.Skia;

namespace MyApp // Note: actual namespace depends on the project name.
{
	internal class SkiaAndGdi
	{

		/*
			Skia 与 Gdi 这两个渲染方式与文档中给出的数值均会存在一些偏差，需要手动微调。
			但是Skia与Gdi相比较还是具有一定优势的。
			Skia对字符串宽度测量更为精准，并且Skia使用文档中给定的XY轴坐标绘制出来的文字
			几乎没有偏差。
			但是Skia和Gdi一样，它不能控制文字之间的间距，需要手动绘制。
		 */

		static void Main(string[] args)
		{
			var userInfo = new AppUserInfo();

			using var skia = new SkiaRender(userInfo);
			skia.Render();
			Console.WriteLine(@"图片已保存至 Output\skia.png");

			using var gdi = new GdiRender(userInfo);
			gdi.Render();
			Console.WriteLine(@"图片已保存至 Output\gid.png");
		}
	}
}