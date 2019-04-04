using System;
using Utility;

namespace LightingModel
{
	// Token: 0x0200003F RID: 63
	public static class WKDColor
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0001BB9C File Offset: 0x00019D9C
		static WKDColor()
		{
			object value = 0;
			WMIEC.WMIReadECRAM(1858UL, ref value);
			if ((Convert.ToUInt64(value) & 4UL) == 4UL)
			{
				WKDColor.m_LED_Vender = 4;
				return;
			}
			if ((Convert.ToUInt64(value) & 8UL) == 8UL)
			{
				WKDColor.m_LED_Vender = 8;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0001BBE8 File Offset: 0x00019DE8
		public static RGB_S cheatRGB_2ndME(byte R, byte G, byte B)
		{
			RGB_S result = default(RGB_S);
			if (WKDColor.m_LED_Vender == 4)
			{
				if (R == 255 && G == 255 && B == 255)
				{
					result.R = byte.MaxValue;
					result.G = 50;
					result.B = 170;
				}
				else if (R == 243 && G == 152 && B == 0)
				{
					result.R = byte.MaxValue;
					result.G = 19;
					result.B = 0;
				}
				else if (R == 255 && G == 241 && B == 0)
				{
					result.R = byte.MaxValue;
					result.G = 50;
					result.B = 0;
				}
				else if (R == 0 && G == 255 && B == 255)
				{
					result.R = 0;
					result.G = 50;
					result.B = 170;
				}
				else if (R == 138 && G == 0 && B == 255)
				{
					result.R = 138;
					result.G = 0;
					result.B = 170;
				}
				else if (R > 249 && G < 20 && B < 128)
				{
					if (B < 100)
					{
						result.R = R;
						result.G = G;
						result.B = B / 10;
					}
					else
					{
						result.R = R;
						result.G = G;
						result.B = B - 10;
					}
				}
				else if (R < 20 && B < G)
				{
					result.R = R;
					result.G = G;
					result.B = 170 * B / byte.MaxValue;
				}
				else if (G < 20 && R < B)
				{
					result.R = R;
					result.G = G;
					result.B = B;
				}
				else
				{
					result.R = R;
					result.G = 50 * G / byte.MaxValue;
					result.B = 170 * B / byte.MaxValue;
				}
			}
			else if (WKDColor.m_LED_Vender == 8)
			{
				if (R == 255 && G == 255 && B == 255)
				{
					result.R = byte.MaxValue;
					result.G = 70;
					result.B = 80;
				}
				else if (R == 243 && G == 152 && B == 0)
				{
					result.R = byte.MaxValue;
					result.G = 25;
					result.B = 0;
				}
				else if (R == 241 && G == 90 && B == 36)
				{
					result.R = byte.MaxValue;
					result.G = 5;
					result.B = 0;
				}
				else if (R == 247 && G == 147 && B == 30)
				{
					result.R = byte.MaxValue;
					result.G = 15;
					result.B = 0;
				}
				else if (R == 255 && G == 241 && B == 0)
				{
					result.R = byte.MaxValue;
					result.G = 70;
					result.B = 0;
				}
				else if (R == 0 && G == 255 && B == 255)
				{
					result.R = 0;
					result.G = 70;
					result.B = 80;
				}
				else if (R == 138 && G == 0 && B == 255)
				{
					result.R = 138;
					result.G = 0;
					result.B = 80;
				}
				else if (R > 249 && G < 20 && B < 128)
				{
					if (B < 100)
					{
						result.R = R;
						result.G = G;
						result.B = B / 10;
					}
					else
					{
						result.R = R;
						result.G = G;
						result.B = B - 10;
					}
				}
				else if (R < 20 && B < G)
				{
					result.R = R;
					result.G = G;
					result.B = 80 * B / byte.MaxValue;
				}
				else if (G < 20 && R < B)
				{
					result.R = R;
					result.G = G;
					result.B = B;
				}
				else
				{
					result.R = R;
					result.G = 70 * G / byte.MaxValue;
					result.B = 80 * B / byte.MaxValue;
				}
			}
			else if (R == 255 && G == 255 && B == 255)
			{
				result.R = byte.MaxValue;
				result.G = 180;
				result.B = 200;
			}
			else if (R == 243 && G == 152 && B == 0)
			{
				result.R = byte.MaxValue;
				result.G = 42;
				result.B = 0;
			}
			else if (R == 241 && G == 90 && B == 36)
			{
				result.R = byte.MaxValue;
				result.G = 25;
				result.B = 0;
			}
			else if (R == 247 && G == 147 && B == 30)
			{
				result.R = byte.MaxValue;
				result.G = 42;
				result.B = 0;
			}
			else if (R == 255 && G == 241 && B == 0)
			{
				result.R = byte.MaxValue;
				result.G = 180;
				result.B = 0;
			}
			else if (R == 0 && G == 255 && B == 255)
			{
				result.R = 0;
				result.G = 180;
				result.B = 200;
			}
			else if (R == 138 && G == 0 && B == 255)
			{
				result.R = 138;
				result.G = 0;
				result.B = 200;
			}
			else if (R > 249 && G < 20 && B < 128)
			{
				if (B < 100)
				{
					result.R = R;
					result.G = G;
					result.B = B / 10;
				}
				else
				{
					result.R = R;
					result.G = G;
					result.B = B - 10;
				}
			}
			else if (R < 20 && B < G)
			{
				result.R = R;
				result.G = G;
				result.B = 200 * B / byte.MaxValue;
			}
			else if (G < 20 && R < B)
			{
				result.R = R;
				result.G = G;
				result.B = B;
			}
			else
			{
				result.R = R;
				result.G = 180 * G / byte.MaxValue;
				result.B = 200 * B / byte.MaxValue;
			}
			return result;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0001C2D8 File Offset: 0x0001A4D8
		public static RGB_S cheatRGB_4Zone(byte R, byte G, byte B)
		{
			RGB_S result = default(RGB_S);
			if (R == 243 && G == 152 && B == 0)
			{
				result.R = byte.MaxValue;
				result.G = 60;
				result.B = 0;
			}
			else if (R == 255 && G == 241 && B == 0)
			{
				result.R = byte.MaxValue;
				result.G = 180;
				result.B = 0;
			}
			else
			{
				result.R = R;
				result.G = 180 * G / byte.MaxValue;
				result.B = B;
			}
			return result;
		}

		// Token: 0x04000419 RID: 1049
		private const ulong m_SupportByte5 = 1858UL;

		// Token: 0x0400041A RID: 1050
		private const byte m_bitLiteon = 4;

		// Token: 0x0400041B RID: 1051
		private const byte m_bitEverlight = 8;

		// Token: 0x0400041C RID: 1052
		private static byte m_LED_Vender;
	}
}
