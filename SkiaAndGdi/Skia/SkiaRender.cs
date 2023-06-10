using SkiaSharp;

namespace SkiaAndGdi.Skia
{
	internal class SkiaRender : IDisposable
	{
		readonly AppUserInfo userInfo;
		static readonly SKTypeface font = SKTypeface.FromFile(AppRes.FontFileName);
		static readonly SKBitmap backgroundBitmap = SKBitmap.Decode(AppRes.BackgroundImage);

		internal SkiaRender(AppUserInfo userInfo)
		{
			this.userInfo = userInfo;
		}

		internal void Render()
		{
			using var canvas = new SKCanvas(backgroundBitmap);

			DrawAccountName(canvas);
			DrawBalance(canvas);
			DrawAddress(canvas);

			SaveRenderedBitmap();
		}

		void DrawAccountName(SKCanvas canvas)
		{
			using var textPaint = new SKPaint();
			textPaint.Typeface = font;
			textPaint.IsAntialias = true;
			textPaint.TextSize = AppRes.AccountNameFontSize;
			textPaint.Color = SKColor.Parse(AppRes.AccountNameColor);

			var x = CalculateHorizontalCenteringPositionX(userInfo.Name, backgroundBitmap.Width, textPaint);
			canvas.DrawText(userInfo.Name, x, ConvertFontY(1261, textPaint), textPaint);
		}

		void DrawBalance(SKCanvas canvas)
		{
			using var textPaint = new SKPaint();
			textPaint.Typeface = font;
			textPaint.IsAntialias = true;
			textPaint.TextSize = AppRes.AccountBalanceFontSize;
			textPaint.Color = SKColor.Parse(AppRes.AccountBalanceColor);

			var x = CalculateHorizontalCenteringPositionX(userInfo.Balance, backgroundBitmap.Width, textPaint);
			canvas.DrawText(userInfo.Balance, x, ConvertFontY(1512, textPaint), textPaint);
		}

		void DrawAddress(SKCanvas canvas)
		{
			using var textPaint = new SKPaint();
			textPaint.Typeface = font;
			textPaint.IsAntialias = true;
			textPaint.TextSize = AppRes.AccountAddressFontSize;
			textPaint.Color = SKColor.Parse(AppRes.AccountAddressColor);

			var x = CalculateHorizontalCenteringPositionX(userInfo.Address, backgroundBitmap.Width, textPaint);
			var additionCharSpacing = 5.5f;
			DrawTextWithCustomSpacing(canvas, textPaint, userInfo.Address, x, ConvertFontY(1799, textPaint), additionCharSpacing);
		}

		static void DrawTextWithCustomSpacing(SKCanvas canvas, SKPaint sKPaint,
										string text, float x, float y,
										float additionCharSpacing)
		{
			var dotOffset = 2;
			var offsetX = x - ((text.Replace(".", "").Length * additionCharSpacing) / 2);
			var dotIndex = 0;

			for (var i = 0; i < text.Length; i++)
			{

				if (i > 0 && text[i] != '.')
				{
					offsetX += additionCharSpacing;

					if (dotIndex == 2)
					{
						offsetX -= dotOffset;
						dotIndex = -1;
					}

				}
				else if (text[i] == '.')
				{
					if (dotIndex == 0)
					{
						offsetX -= dotOffset;
					}

					dotIndex += 1;
				}

				canvas.DrawText(text[i].ToString(), offsetX, y, sKPaint);
				offsetX += sKPaint.MeasureText(text[i].ToString());
			}
		}

		static void SaveRenderedBitmap()
		{
			using var output = File.OpenWrite(@"Output\skia.png");
			backgroundBitmap.Encode(SKEncodedImageFormat.Png, 100).SaveTo(output);
		}

		static float ConvertFontY(float originY, SKPaint sKpaint)
		{
			return originY - sKpaint.FontMetrics.Ascent;
		}

		static float CalculateHorizontalCenteringPositionX(string text, float parentWidth, SKPaint sKPaint)
		{
			var textWidth = sKPaint.MeasureText(text);
			return (parentWidth - textWidth) / 2;
		}

		public void Dispose()
		{
			font.Dispose();
			backgroundBitmap.Dispose();
		}
	}
}
