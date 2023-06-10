using System.Drawing;
using System.Drawing.Text;

namespace SkiaAndGdi.Gdi
{
	internal class GdiRender : IDisposable
	{
		readonly AppUserInfo userInfo;
		static readonly PrivateFontCollection fontCollection = new();
		static readonly Image backgroundImage = Image.FromFile(AppRes.BackgroundImage);

		internal GdiRender(AppUserInfo userInfo)
		{
			this.userInfo = userInfo;
			fontCollection.AddFontFile(AppRes.FontFileName);
		}

		internal void Render()
		{
			using var graphics = Graphics.FromImage(backgroundImage);
			graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			graphics.SetClip(new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height));
			DrawAccountName(graphics);
			DrawBalance(graphics);
			DrawAddress(graphics);

			SaveRenderedImage();
		}

		void DrawAccountName(Graphics graphics)
		{
			var font = new Font(fontCollection.Families[0], AppRes.AccountNameFontSize, FontStyle.Regular, GraphicsUnit.Pixel);

			var brush = new SolidBrush(ColorTranslator.FromHtml(AppRes.AccountNameColor));
			var bounds = graphics.ClipBounds;
			var content = userInfo.Name;
			var x = (bounds.Width - MeasureStringWidth(graphics, content, font)) / 2 - 14;
			DrawString(graphics, content, font, brush, x, 1260, -53);
		}

		void DrawBalance(Graphics graphics)
		{
			var font = new Font(fontCollection.Families[0], AppRes.AccountBalanceFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
			var brush = new SolidBrush(ColorTranslator.FromHtml(AppRes.AccountBalanceColor));
			var bounds = graphics.ClipBounds;
			var content = userInfo.Balance;
			var x = (bounds.Width - MeasureStringWidth(graphics, content, font)) / 2 - 16;
			graphics.DrawString(content, font, brush, x, 1512);
		}

		void DrawAddress(Graphics graphics)
		{
			var font = new Font(fontCollection.Families[0], AppRes.AccountAddressFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
			var brush = new SolidBrush(ColorTranslator.FromHtml(AppRes.AccountAddressColor));
			var bounds = graphics.ClipBounds;
			var content = userInfo.Address;
			var x = (bounds.Width - graphics.MeasureString(content, font).Width) / 2 - 18;
			DrawString(graphics, content, font, brush, x, 1799, -22);
		}

		static float MeasureStringWidth(Graphics graphics, string content, Font font)
		{
			var format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			CharacterRange[] ranges = { new CharacterRange(0, content.Length) };

			format.SetMeasurableCharacterRanges(ranges);

			var regions = graphics.MeasureCharacterRanges(content, font, new RectangleF(0, 0, 9999, 9999), format);

			return regions[0].GetBounds(graphics).Width;
		}

		static void DrawString(Graphics graphics, string content, Font font, Brush brush, float x, float y, float spacingOffset)
		{
			for (var i = 0; i < content.Length; i++)
			{
				if (i > 0 && content[i].ToString() == "." && content[i - 1].ToString() != ".")
				{
					x -= 8;
				}

				graphics.DrawString(content[i].ToString(), font, brush, x, y);
				
				if (content[i].ToString() == " ")
				{
					x += 42;
					continue;
				}

				if (content[i].ToString() == ".")
				{
					x += 20;
					continue;
				}

				x += graphics.MeasureString(content[i].ToString(), font).Width;
				x += spacingOffset;
			}
		}

		static void SaveRenderedImage()
		{
			backgroundImage.Save(@"Output\gdi.png");
		}

		public void Dispose()
		{
			backgroundImage.Dispose();
			fontCollection.Dispose();
		}
	}
}
