using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using OemServiceModel;
using UsbHidModel;
using Utility;

namespace LightingModel
{
	// Token: 0x02000038 RID: 56
	internal class LM_ITE_RGB : ITE_SPEC, ILM_RGBKB
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000141 RID: 321 RVA: 0x000160B0 File Offset: 0x000142B0
		// (remove) Token: 0x06000142 RID: 322 RVA: 0x000160E4 File Offset: 0x000142E4
		private static event RGBKB_Event_Handler m_Layout_Event_handler;

		// Token: 0x06000143 RID: 323 RVA: 0x00016118 File Offset: 0x00014318
		private void ScanCode_Hnadler(int scancode)
		{
			if (scancode == 240)
			{
				this.m_light_lock = true;
				if (this.m_effect_type == 3)
				{
					this.DLL_SetMusicMode(false, 0);
				}
				RGBKB_Event_Data rgbkb_Event_Data = default(RGBKB_Event_Data);
				byte b = 0;
				this.Get_ITE_Light_Value(ref b);
				rgbkb_Event_Data.event_id = RGBKB_EventID.Brightness_update;
				rgbkb_Event_Data.envet_data_len = 1u;
				rgbkb_Event_Data.event_data = new byte[1];
				rgbkb_Event_Data.event_data[0] = (byte)this.Translate_Layout_LightValue(b);
				LM_ITE_RGB.m_Layout_Event_handler(rgbkb_Event_Data);
				if (this.m_effect_type == 3)
				{
					this.DLL_SetMusicMode(true, b);
				}
				else if (this.m_effect_type == 4)
				{
					this.m_ap_effect_data.save_light = b;
				}
				else
				{
					LM_ITE_RGB.m_save_lighting_data.save_light = b;
				}
				this.m_light_lock = false;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000161D4 File Offset: 0x000143D4
		private void Light_Lock()
		{
			if (this.m_light_lock)
			{
				uint num = 10u;
				while (this.m_light_lock && num > 0u)
				{
					Thread.Sleep(5);
					num -= 1u;
				}
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00016204 File Offset: 0x00014404
		private void AP_Effect_Task()
		{
			RGB_S[] array = new RGB_S[3];
			RGB_S[] array2 = new RGB_S[126];
			byte save_light = this.m_ap_effect_data.save_light;
			bool bRefresh = true;
			uint num = 0u;
			while (!this.m_ap_effect_task_stop)
			{
				if (save_light != this.m_ap_effect_data.save_light)
				{
					save_light = this.m_ap_effect_data.save_light;
					bRefresh = true;
				}
				if (this.m_ap_effect_data.save_effect == 15)
				{
					if (num == 7u)
					{
						num = 0u;
					}
					array[0].R = this.m_ap_effect_data.save_layout_color.ColorBuffer[(int)num].R;
					array[0].G = this.m_ap_effect_data.save_layout_color.ColorBuffer[(int)num].G;
					array[0].B = this.m_ap_effect_data.save_layout_color.ColorBuffer[(int)num].B;
					this.Set_ITE_Effect_Type_StaticMode(this.m_ap_effect_data.save_light, 0, array, bRefresh);
					Thread.Sleep((int)(50 * this.m_ap_effect_data.save_speed));
					num += 1u;
					if (this.m_ap_effect_data.save_effect != 15)
					{
						num = 0u;
					}
				}
				else if (this.m_ap_effect_data.save_effect == 13)
				{
					array[0].R = this.m_ap_effect_data.save_layout_color.ColorBuffer[0].R;
					array[0].G = this.m_ap_effect_data.save_layout_color.ColorBuffer[0].G;
					array[0].B = this.m_ap_effect_data.save_layout_color.ColorBuffer[0].B;
					array[1].R = this.m_ap_effect_data.save_layout_color.ColorBuffer[1].R;
					array[1].G = this.m_ap_effect_data.save_layout_color.ColorBuffer[1].G;
					array[1].B = this.m_ap_effect_data.save_layout_color.ColorBuffer[1].B;
					array[2].R = this.m_ap_effect_data.save_layout_color.ColorBuffer[2].R;
					array[2].G = this.m_ap_effect_data.save_layout_color.ColorBuffer[2].G;
					array[2].B = this.m_ap_effect_data.save_layout_color.ColorBuffer[2].B;
					if (num == 20u)
					{
						num = 0u;
					}
					for (uint num2 = 0u; num2 < 126u; num2 += 1u)
					{
						array2[(int)num2].ID = num2;
						array2[(int)num2].R = 0;
						array2[(int)num2].G = 0;
						array2[(int)num2].B = 0;
					}
					if (num < 10u)
					{
						array2[(int)num].R = array[0].R;
						array2[(int)num].G = array[0].G;
						array2[(int)num].B = array[0].B;
						array2[(int)(1u + num)].R = array[0].R;
						array2[(int)(1u + num)].G = array[0].G;
						array2[(int)(1u + num)].B = array[0].B;
						array2[(int)(21u + num)].R = array[0].R;
						array2[(int)(21u + num)].G = array[0].G;
						array2[(int)(21u + num)].B = array[0].B;
						array2[(int)(22u + num)].R = array[0].R;
						array2[(int)(22u + num)].G = array[0].G;
						array2[(int)(22u + num)].B = array[0].B;
						array2[(int)(42u + num)].R = array[0].R;
						array2[(int)(42u + num)].G = array[0].G;
						array2[(int)(42u + num)].B = array[0].B;
						array2[(int)(43u + num)].R = array[0].R;
						array2[(int)(43u + num)].G = array[0].G;
						array2[(int)(43u + num)].B = array[0].B;
						array2[(int)(63u + num)].R = array[0].R;
						array2[(int)(63u + num)].G = array[0].G;
						array2[(int)(63u + num)].B = array[0].B;
						array2[(int)(64u + num)].R = array[0].R;
						array2[(int)(64u + num)].G = array[0].G;
						array2[(int)(64u + num)].B = array[0].B;
						array2[(int)(84u + num)].R = array[0].R;
						array2[(int)(84u + num)].G = array[0].G;
						array2[(int)(84u + num)].B = array[0].B;
						array2[(int)(85u + num)].R = array[0].R;
						array2[(int)(85u + num)].G = array[0].G;
						array2[(int)(85u + num)].B = array[0].B;
						array2[(int)(105u + num)].R = array[0].R;
						array2[(int)(105u + num)].G = array[0].G;
						array2[(int)(105u + num)].B = array[0].B;
						array2[(int)(106u + num)].R = array[0].R;
						array2[(int)(106u + num)].G = array[0].G;
						array2[(int)(106u + num)].B = array[0].B;
						array2[(int)(20u - num)].R = array[1].R;
						array2[(int)(20u - num)].G = array[1].G;
						array2[(int)(20u - num)].B = array[1].B;
						array2[(int)(19u - num)].R = array[1].R;
						array2[(int)(19u - num)].G = array[1].G;
						array2[(int)(19u - num)].B = array[1].B;
						array2[(int)(41u - num)].R = array[1].R;
						array2[(int)(41u - num)].G = array[1].G;
						array2[(int)(41u - num)].B = array[1].B;
						array2[(int)(40u - num)].R = array[1].R;
						array2[(int)(40u - num)].G = array[1].G;
						array2[(int)(40u - num)].B = array[1].B;
						array2[(int)(62u - num)].R = array[1].R;
						array2[(int)(62u - num)].G = array[1].G;
						array2[(int)(62u - num)].B = array[1].B;
						array2[(int)(61u - num)].R = array[1].R;
						array2[(int)(61u - num)].G = array[1].G;
						array2[(int)(61u - num)].B = array[1].B;
						array2[(int)(83u - num)].R = array[1].R;
						array2[(int)(83u - num)].G = array[1].G;
						array2[(int)(83u - num)].B = array[1].B;
						array2[(int)(82u - num)].R = array[1].R;
						array2[(int)(82u - num)].G = array[1].G;
						array2[(int)(82u - num)].B = array[1].B;
						array2[(int)(104u - num)].R = array[1].R;
						array2[(int)(104u - num)].G = array[1].G;
						array2[(int)(104u - num)].B = array[1].B;
						array2[(int)(103u - num)].R = array[1].R;
						array2[(int)(103u - num)].G = array[1].G;
						array2[(int)(103u - num)].B = array[1].B;
						array2[(int)(125u - num)].R = array[1].R;
						array2[(int)(125u - num)].G = array[1].G;
						array2[(int)(125u - num)].B = array[1].B;
						array2[(int)(124u - num)].R = array[1].R;
						array2[(int)(124u - num)].G = array[1].G;
						array2[(int)(124u - num)].B = array[1].B;
					}
					else
					{
						uint num3 = num - 10u;
						array2[(int)(10u - num3)].R = array[2].R;
						array2[(int)(10u - num3)].G = array[2].G;
						array2[(int)(10u - num3)].B = array[2].B;
						array2[(int)(9u - num3)].R = array[2].R;
						array2[(int)(9u - num3)].G = array[2].G;
						array2[(int)(9u - num3)].B = array[2].B;
						array2[(int)(31u - num3)].R = array[2].R;
						array2[(int)(31u - num3)].G = array[2].G;
						array2[(int)(31u - num3)].B = array[2].B;
						array2[(int)(30u - num3)].R = array[2].R;
						array2[(int)(30u - num3)].G = array[2].G;
						array2[(int)(30u - num3)].B = array[2].B;
						array2[(int)(52u - num3)].R = array[2].R;
						array2[(int)(52u - num3)].G = array[2].G;
						array2[(int)(52u - num3)].B = array[2].B;
						array2[(int)(51u - num3)].R = array[2].R;
						array2[(int)(51u - num3)].G = array[2].G;
						array2[(int)(51u - num3)].B = array[2].B;
						array2[(int)(73u - num3)].R = array[2].R;
						array2[(int)(73u - num3)].G = array[2].G;
						array2[(int)(73u - num3)].B = array[2].B;
						array2[(int)(72u - num3)].R = array[2].R;
						array2[(int)(72u - num3)].G = array[2].G;
						array2[(int)(72u - num3)].B = array[2].B;
						array2[(int)(94u - num3)].R = array[2].R;
						array2[(int)(94u - num3)].G = array[2].G;
						array2[(int)(94u - num3)].B = array[2].B;
						array2[(int)(93u - num3)].R = array[2].R;
						array2[(int)(93u - num3)].G = array[2].G;
						array2[(int)(93u - num3)].B = array[2].B;
						array2[(int)(115u - num3)].R = array[2].R;
						array2[(int)(115u - num3)].G = array[2].G;
						array2[(int)(115u - num3)].B = array[2].B;
						array2[(int)(114u - num3)].R = array[2].R;
						array2[(int)(114u - num3)].G = array[2].G;
						array2[(int)(114u - num3)].B = array[2].B;
						array2[(int)(10u + num3)].R = array[2].R;
						array2[(int)(10u + num3)].G = array[2].G;
						array2[(int)(10u + num3)].B = array[2].B;
						array2[(int)(11u + num3)].R = array[2].R;
						array2[(int)(9u + num3)].G = array[2].G;
						array2[(int)(9u + num3)].B = array[2].B;
						array2[(int)(31u + num3)].R = array[2].R;
						array2[(int)(31u + num3)].G = array[2].G;
						array2[(int)(31u + num3)].B = array[2].B;
						array2[(int)(30u + num3)].R = array[2].R;
						array2[(int)(30u + num3)].G = array[2].G;
						array2[(int)(30u + num3)].B = array[2].B;
						array2[(int)(52u + num3)].R = array[2].R;
						array2[(int)(52u + num3)].G = array[2].G;
						array2[(int)(52u + num3)].B = array[2].B;
						array2[(int)(51u + num3)].R = array[2].R;
						array2[(int)(51u + num3)].G = array[2].G;
						array2[(int)(51u + num3)].B = array[2].B;
						array2[(int)(73u + num3)].R = array[2].R;
						array2[(int)(73u + num3)].G = array[2].G;
						array2[(int)(73u + num3)].B = array[2].B;
						array2[(int)(72u + num3)].R = array[2].R;
						array2[(int)(72u + num3)].G = array[2].G;
						array2[(int)(72u + num3)].B = array[2].B;
						array2[(int)(94u + num3)].R = array[2].R;
						array2[(int)(94u + num3)].G = array[2].G;
						array2[(int)(94u + num3)].B = array[2].B;
						array2[(int)(93u + num3)].R = array[2].R;
						array2[(int)(93u + num3)].G = array[2].G;
						array2[(int)(93u + num3)].B = array[2].B;
						array2[(int)(115u + num3)].R = array[2].R;
						array2[(int)(115u + num3)].G = array[2].G;
						array2[(int)(115u + num3)].B = array[2].B;
						array2[(int)(114u + num3)].R = array[2].R;
						array2[(int)(114u + num3)].G = array[2].G;
						array2[(int)(114u + num3)].B = array[2].B;
					}
					this.Set_ITE_Effect_Type_UserMode(this.m_ap_effect_data.save_light, 0, array2, bRefresh);
					Thread.Sleep((int)(20 * this.m_ap_effect_data.save_speed));
					num += 1u;
					if (this.m_ap_effect_data.save_effect != 13)
					{
						num = 0u;
					}
				}
				else if (this.m_ap_effect_data.save_effect == 12)
				{
					array[0].R = this.m_ap_effect_data.save_layout_color.ColorBuffer[0].R;
					array[0].G = this.m_ap_effect_data.save_layout_color.ColorBuffer[0].G;
					array[0].B = this.m_ap_effect_data.save_layout_color.ColorBuffer[0].B;
					array[1].R = this.m_ap_effect_data.save_layout_color.ColorBuffer[1].R;
					array[1].G = this.m_ap_effect_data.save_layout_color.ColorBuffer[1].G;
					array[1].B = this.m_ap_effect_data.save_layout_color.ColorBuffer[1].B;
					array[2].R = this.m_ap_effect_data.save_layout_color.ColorBuffer[2].R;
					array[2].G = this.m_ap_effect_data.save_layout_color.ColorBuffer[2].G;
					array[2].B = this.m_ap_effect_data.save_layout_color.ColorBuffer[2].B;
					for (uint num4 = 0u; num4 < 126u; num4 += 1u)
					{
						array2[(int)num4].ID = num4;
						array2[(int)num4].R = 0;
						array2[(int)num4].G = 0;
						array2[(int)num4].B = 0;
					}
					uint num5 = num % 15u;
					uint num6 = num / 15u;
					byte r = array[(int)num6].R;
					byte g = array[(int)num6].G;
					byte b = array[(int)num6].B;
					array2[52].R = r;
					array2[52].G = g;
					array2[52].B = b;
					switch (num5)
					{
					case 0u:
						array2[0].R = (array2[105].R = r);
						array2[0].G = (array2[105].G = g);
						array2[0].B = (array2[105].B = b);
						array2[20].R = (array2[125].R = r);
						array2[20].G = (array2[125].G = g);
						array2[20].B = (array2[125].B = b);
						break;
					case 1u:
						array2[1].R = (array2[21].R = (array2[84].R = (array2[106].R = r)));
						array2[1].G = (array2[21].G = (array2[84].G = (array2[106].G = g)));
						array2[1].B = (array2[21].B = (array2[84].B = (array2[106].B = b)));
						array2[19].R = (array2[41].R = (array2[104].R = (array2[124].R = r)));
						array2[19].G = (array2[41].G = (array2[104].G = (array2[124].G = g)));
						array2[19].B = (array2[41].B = (array2[104].B = (array2[124].B = b)));
						break;
					case 2u:
						array2[2].R = (array2[21].R = (array2[22].R = (array2[63].R = (array2[84].R = (array2[85].R = (array2[107].R = r))))));
						array2[2].G = (array2[21].G = (array2[22].G = (array2[63].G = (array2[84].G = (array2[85].G = (array2[107].G = g))))));
						array2[2].B = (array2[21].B = (array2[22].B = (array2[63].B = (array2[84].B = (array2[85].B = (array2[107].B = b))))));
						array2[18].R = (array2[40].R = (array2[41].R = (array2[83].R = (array2[103].R = (array2[104].R = (array2[123].R = r))))));
						array2[18].G = (array2[40].G = (array2[41].G = (array2[83].G = (array2[103].G = (array2[104].G = (array2[123].G = g))))));
						array2[18].B = (array2[40].B = (array2[41].B = (array2[83].B = (array2[103].B = (array2[104].B = (array2[123].B = b))))));
						break;
					case 3u:
						array2[3].R = (array2[22].R = (array2[23].R = (array2[42].R = (array2[63].R = (array2[64].R = (array2[85].R = (array2[86].R = (array2[108].R = r))))))));
						array2[3].G = (array2[22].G = (array2[23].G = (array2[42].G = (array2[63].G = (array2[64].G = (array2[85].G = (array2[86].G = (array2[108].G = g))))))));
						array2[3].B = (array2[22].B = (array2[23].B = (array2[42].B = (array2[63].B = (array2[64].B = (array2[85].B = (array2[86].B = (array2[108].B = b))))))));
						array2[17].R = (array2[38].R = (array2[39].R = (array2[62].R = (array2[82].R = (array2[83].R = (array2[102].R = (array2[103].R = (array2[122].R = r))))))));
						array2[17].G = (array2[38].G = (array2[39].G = (array2[62].G = (array2[82].G = (array2[83].G = (array2[102].G = (array2[103].G = (array2[122].G = g))))))));
						array2[17].B = (array2[38].B = (array2[39].B = (array2[62].B = (array2[82].B = (array2[83].B = (array2[102].B = (array2[103].B = (array2[122].B = b))))))));
						break;
					case 4u:
						array2[4].R = (array2[23].R = (array2[24].R = (array2[42].R = (array2[43].R = (array2[64].R = (array2[65].R = (array2[86].R = (array2[87].R = (array2[109].R = r)))))))));
						array2[4].G = (array2[23].G = (array2[24].G = (array2[42].G = (array2[43].G = (array2[64].G = (array2[65].G = (array2[86].G = (array2[87].G = (array2[109].G = g)))))))));
						array2[4].B = (array2[23].B = (array2[24].B = (array2[42].B = (array2[43].B = (array2[64].B = (array2[65].B = (array2[86].B = (array2[87].B = (array2[109].B = b)))))))));
						array2[16].R = (array2[37].R = (array2[38].R = (array2[61].R = (array2[62].R = (array2[81].R = (array2[82].R = (array2[101].R = (array2[102].R = (array2[121].R = r)))))))));
						array2[16].G = (array2[37].G = (array2[38].G = (array2[61].G = (array2[62].G = (array2[81].G = (array2[82].G = (array2[101].G = (array2[102].G = (array2[121].G = g)))))))));
						array2[16].B = (array2[37].B = (array2[38].B = (array2[61].B = (array2[62].B = (array2[81].B = (array2[82].B = (array2[101].B = (array2[102].B = (array2[121].B = b)))))))));
						break;
					case 5u:
						array2[5].R = (array2[24].R = (array2[25].R = (array2[43].R = (array2[44].R = (array2[65].R = (array2[66].R = (array2[87].R = (array2[88].R = (array2[110].R = r)))))))));
						array2[5].G = (array2[24].G = (array2[25].G = (array2[43].G = (array2[44].G = (array2[65].G = (array2[66].G = (array2[87].G = (array2[88].G = (array2[110].G = g)))))))));
						array2[5].B = (array2[24].B = (array2[25].B = (array2[43].B = (array2[44].B = (array2[65].B = (array2[66].B = (array2[87].B = (array2[88].B = (array2[110].B = b)))))))));
						array2[15].R = (array2[36].R = (array2[37].R = (array2[60].R = (array2[61].R = (array2[80].R = (array2[81].R = (array2[100].R = (array2[101].R = (array2[120].R = r)))))))));
						array2[15].G = (array2[36].G = (array2[37].G = (array2[60].G = (array2[61].G = (array2[80].G = (array2[81].G = (array2[100].G = (array2[101].G = (array2[120].G = g)))))))));
						array2[15].B = (array2[36].B = (array2[37].B = (array2[60].B = (array2[61].B = (array2[80].B = (array2[81].B = (array2[100].B = (array2[101].B = (array2[120].B = b)))))))));
						break;
					case 6u:
						array2[6].R = (array2[25].R = (array2[26].R = (array2[44].R = (array2[45].R = (array2[66].R = (array2[67].R = (array2[88].R = (array2[89].R = (array2[111].R = r)))))))));
						array2[6].G = (array2[25].G = (array2[26].G = (array2[44].G = (array2[45].G = (array2[66].G = (array2[67].G = (array2[88].G = (array2[89].G = (array2[111].G = g)))))))));
						array2[6].B = (array2[25].B = (array2[26].B = (array2[44].B = (array2[45].B = (array2[66].B = (array2[67].B = (array2[88].B = (array2[89].B = (array2[111].B = b)))))))));
						array2[14].R = (array2[35].R = (array2[36].R = (array2[59].R = (array2[60].R = (array2[79].R = (array2[80].R = (array2[99].R = (array2[100].R = (array2[119].R = r)))))))));
						array2[14].G = (array2[35].G = (array2[36].G = (array2[59].G = (array2[60].G = (array2[79].G = (array2[80].G = (array2[99].G = (array2[100].G = (array2[119].G = g)))))))));
						array2[14].B = (array2[35].B = (array2[36].B = (array2[59].B = (array2[60].B = (array2[79].B = (array2[80].B = (array2[99].B = (array2[100].B = (array2[119].B = b)))))))));
						break;
					case 7u:
						array2[7].R = (array2[26].R = (array2[27].R = (array2[45].R = (array2[46].R = (array2[67].R = (array2[68].R = (array2[89].R = (array2[90].R = (array2[112].R = r)))))))));
						array2[7].G = (array2[26].G = (array2[27].G = (array2[45].G = (array2[46].G = (array2[67].G = (array2[68].G = (array2[89].G = (array2[90].G = (array2[112].G = g)))))))));
						array2[7].B = (array2[26].B = (array2[27].B = (array2[45].B = (array2[46].B = (array2[67].B = (array2[68].B = (array2[89].B = (array2[90].B = (array2[112].B = b)))))))));
						array2[13].R = (array2[34].R = (array2[35].R = (array2[58].R = (array2[59].R = (array2[78].R = (array2[79].R = (array2[98].R = (array2[99].R = (array2[118].R = r)))))))));
						array2[13].G = (array2[34].G = (array2[35].G = (array2[58].G = (array2[59].G = (array2[78].G = (array2[79].G = (array2[98].G = (array2[99].G = (array2[118].G = g)))))))));
						array2[13].B = (array2[34].B = (array2[35].B = (array2[58].B = (array2[59].B = (array2[78].B = (array2[79].B = (array2[98].B = (array2[99].B = (array2[118].B = b)))))))));
						break;
					case 8u:
						array2[8].R = (array2[27].R = (array2[28].R = (array2[46].R = (array2[47].R = (array2[68].R = (array2[69].R = (array2[90].R = (array2[91].R = (array2[113].R = r)))))))));
						array2[8].G = (array2[27].G = (array2[28].G = (array2[46].G = (array2[47].G = (array2[68].G = (array2[69].G = (array2[90].G = (array2[91].G = (array2[113].G = g)))))))));
						array2[8].B = (array2[27].B = (array2[28].B = (array2[46].B = (array2[47].B = (array2[68].B = (array2[69].B = (array2[90].B = (array2[91].B = (array2[113].B = b)))))))));
						array2[12].R = (array2[33].R = (array2[34].R = (array2[57].R = (array2[58].R = (array2[77].R = (array2[78].R = (array2[97].R = (array2[98].R = (array2[117].R = r)))))))));
						array2[12].G = (array2[33].G = (array2[34].G = (array2[57].G = (array2[58].G = (array2[77].G = (array2[78].G = (array2[97].G = (array2[98].G = (array2[117].G = g)))))))));
						array2[12].B = (array2[33].B = (array2[34].B = (array2[57].B = (array2[58].B = (array2[77].B = (array2[78].B = (array2[97].B = (array2[98].B = (array2[117].B = b)))))))));
						break;
					case 9u:
						array2[9].R = (array2[28].R = (array2[29].R = (array2[47].R = (array2[48].R = (array2[69].R = (array2[70].R = (array2[91].R = (array2[92].R = (array2[114].R = r)))))))));
						array2[9].G = (array2[28].G = (array2[29].G = (array2[47].G = (array2[48].G = (array2[69].G = (array2[70].G = (array2[91].G = (array2[92].G = (array2[114].G = g)))))))));
						array2[9].B = (array2[28].B = (array2[29].B = (array2[47].B = (array2[48].B = (array2[69].B = (array2[70].B = (array2[91].B = (array2[92].B = (array2[114].B = b)))))))));
						array2[11].R = (array2[32].R = (array2[33].R = (array2[56].R = (array2[57].R = (array2[76].R = (array2[77].R = (array2[96].R = (array2[97].R = (array2[116].R = r)))))))));
						array2[11].G = (array2[32].G = (array2[33].G = (array2[56].G = (array2[57].G = (array2[76].G = (array2[77].G = (array2[96].G = (array2[97].G = (array2[116].G = g)))))))));
						array2[11].B = (array2[32].B = (array2[33].B = (array2[56].B = (array2[57].B = (array2[76].B = (array2[77].B = (array2[96].B = (array2[97].B = (array2[116].B = b)))))))));
						break;
					case 10u:
						array2[10].R = (array2[29].R = (array2[30].R = (array2[48].R = (array2[49].R = (array2[70].R = (array2[71].R = (array2[92].R = (array2[93].R = r))))))));
						array2[10].G = (array2[29].G = (array2[30].G = (array2[48].G = (array2[49].G = (array2[70].G = (array2[71].G = (array2[92].G = (array2[93].G = g))))))));
						array2[10].B = (array2[29].B = (array2[30].B = (array2[48].B = (array2[49].B = (array2[70].B = (array2[71].B = (array2[92].B = (array2[93].B = b))))))));
						array2[31].R = (array2[32].R = (array2[55].R = (array2[56].R = (array2[75].R = (array2[76].R = (array2[95].R = (array2[96].R = r)))))));
						array2[31].G = (array2[32].G = (array2[55].G = (array2[56].G = (array2[75].G = (array2[76].G = (array2[95].G = (array2[96].G = g)))))));
						array2[31].B = (array2[32].B = (array2[55].B = (array2[56].B = (array2[75].B = (array2[76].B = (array2[95].B = (array2[96].B = b)))))));
						break;
					case 11u:
						array2[30].R = (array2[49].R = (array2[50].R = (array2[71].R = (array2[72].R = (array2[93].R = r)))));
						array2[30].G = (array2[49].G = (array2[50].G = (array2[71].G = (array2[72].G = (array2[93].G = g)))));
						array2[30].B = (array2[49].B = (array2[50].B = (array2[71].B = (array2[72].B = (array2[93].B = b)))));
						array2[31].R = (array2[54].R = (array2[55].R = (array2[74].R = (array2[75].R = (array2[94].R = r)))));
						array2[31].G = (array2[54].G = (array2[55].G = (array2[74].G = (array2[75].G = (array2[94].G = g)))));
						array2[31].B = (array2[54].B = (array2[55].B = (array2[74].B = (array2[75].B = (array2[94].B = b)))));
						break;
					case 12u:
						array2[50].R = (array2[51].R = (array2[72].R = r));
						array2[50].G = (array2[51].G = (array2[72].G = g));
						array2[50].B = (array2[51].B = (array2[72].B = b));
						array2[53].R = (array2[54].R = (array2[73].R = r));
						array2[53].G = (array2[54].G = (array2[73].G = g));
						array2[53].B = (array2[54].B = (array2[73].B = b));
						break;
					case 13u:
						array2[51].R = (array2[52].R = r);
						array2[51].G = (array2[52].G = g);
						array2[51].B = (array2[52].B = b);
						array2[53].R = r;
						array2[53].G = g;
						array2[53].B = b;
						break;
					case 14u:
						array2[52].R = r;
						array2[52].G = g;
						array2[52].B = b;
						break;
					}
					this.Set_ITE_Effect_Type_UserMode(this.m_ap_effect_data.save_light, 0, array2, bRefresh);
					Thread.Sleep((int)(20 * this.m_ap_effect_data.save_speed));
					num += 1u;
					if (num == 45u)
					{
						num = 0u;
					}
					if (this.m_ap_effect_data.save_effect != 12)
					{
						num = 0u;
					}
				}
				bRefresh = false;
				if (this.m_effect_type != 4)
				{
					break;
				}
			}
			this.m_ap_effect_task = null;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0001A1C8 File Offset: 0x000183C8
		private bool HID_Set_Effect_Type_08H(byte Control, byte Effect, byte Speed, byte Light, byte ColorIndex, byte Direction, byte Save)
		{
			byte[] buffer = new byte[]
			{
				0,
				8,
				Control,
				Effect,
				Speed,
				Light,
				ColorIndex,
				Direction,
				Save
			};
			this.m_HIDManager.WriteFeature(buffer);
			Thread.Sleep(1);
			return true;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0001A218 File Offset: 0x00018418
		private bool HID_Get_Effect_Type_88H(ref byte Control, ref byte Effect, ref byte Speed, ref byte Light, ref byte ColorIndex, ref byte Direction)
		{
			byte[] array = new byte[9];
			array[1] = 136;
			byte[] buffer = array;
			this.m_HIDManager.WriteFeature(buffer);
			Thread.Sleep(1);
			byte[] array2 = new byte[9];
			this.m_HIDManager.GetFeature(array2);
			Thread.Sleep(1);
			Control = array2[2];
			Effect = array2[3];
			Speed = array2[4];
			Light = array2[5];
			ColorIndex = array2[6];
			Direction = array2[7];
			return true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0001A288 File Offset: 0x00018488
		private bool HID_Set_Picture_12H(byte Saved)
		{
			byte[] array = new byte[9];
			array[1] = 18;
			array[4] = 8;
			array[5] = Saved;
			byte[] buffer = array;
			this.m_HIDManager.WriteFeature(buffer);
			Thread.Sleep(1);
			return true;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0001A2C0 File Offset: 0x000184C0
		private bool HID_Set_Color_14H(byte Index, byte R, byte G, byte B)
		{
			byte[] buffer;
			if (this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102)
			{
				RGB_S rgb_S = WKDColor.cheatRGB_2ndME(R, G, B);
				byte[] array = new byte[9];
				array[1] = 20;
				array[3] = Index;
				array[4] = rgb_S.R;
				array[5] = rgb_S.G;
				array[6] = rgb_S.B;
				buffer = array;
			}
			else if (this.m_ITE_KB_Type == RGBKB_Type.FourZone && (this.m_Project_ID == 6 || this.m_Project_ID == 7))
			{
				RGB_S rgb_S2 = WKDColor.cheatRGB_4Zone(R, G, B);
				byte[] array2 = new byte[9];
				array2[1] = 20;
				array2[3] = Index;
				array2[4] = rgb_S2.R;
				array2[5] = rgb_S2.G;
				array2[6] = rgb_S2.B;
				buffer = array2;
			}
			else
			{
				byte[] array3 = new byte[9];
				array3[1] = 20;
				array3[3] = Index;
				array3[4] = R;
				array3[5] = G;
				array3[6] = B;
				buffer = array3;
			}
			this.m_HIDManager.WriteFeature(buffer);
			Thread.Sleep(1);
			return true;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0001A39C File Offset: 0x0001859C
		private bool HID_Set_RowIndex_16H(byte RowIndex)
		{
			byte[] array = new byte[9];
			array[1] = 22;
			array[3] = RowIndex;
			byte[] buffer = array;
			this.m_HIDManager.WriteFeature(buffer);
			Thread.Sleep(1);
			return true;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0001A3D0 File Offset: 0x000185D0
		private bool DLL_SetMusicMode(bool enable, byte light_level)
		{
			try
			{
				if (enable)
				{
					this.HID_Set_Effect_Type_08H(2, 34, 0, light_level, 0, 0, 0);
					MusicMode.m_delStartMonitorAudio(0, light_level);
				}
				else
				{
					MusicMode.m_delStopMonitorAudio();
				}
			}
			catch (Exception ex)
			{
				Log.s(LOG_LEVEL.ERROR, string.Format("DLL_SetMusicMode failed,enable ={0} e={1}", enable, ex.ToString()));
			}
			return true;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0001A43C File Offset: 0x0001863C
		private bool Disable_EC_OnkeyPressed()
		{
			if (this.m_enableOnkeyPressed)
			{
				this.m_enableOnkeyPressed = false;
			}
			return true;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0001A450 File Offset: 0x00018650
		private bool Enable_EC_OnkeyPressed(byte effect, byte direction)
		{
			if (this.m_enableOnkeyPressed)
			{
				return true;
			}
			if ((effect == 6 || effect == 14 || effect == 17 || effect == 4) && direction == 1)
			{
				object value = 0;
				WMIEC.WMIReadECRAM(1857UL, ref value);
				byte b = (byte)Convert.ToUInt64(value);
				b |= 8;
				WMIEC.WMIWriteECRAM(1857UL, (ulong)b);
				this.m_enableOnkeyPressed = true;
			}
			return true;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0001A4B4 File Offset: 0x000186B4
		private bool Set_ITE_Effect_Type_UserMode(byte Light, byte Save, RGB_S[] colorBuffer, bool bRefresh)
		{
			Log.s(LOG_LEVEL.TRACE, string.Format("RGBKeyboard_ITE | Set_ITE_Effect_Type_UserMode light ={0}", Light));
			if (bRefresh)
			{
				this.Light_Lock();
				this.HID_Set_Effect_Type_08H(2, 51, 0, Light, 0, 0, Save);
			}
			byte[] array = new byte[65];
			array[0] = 0;
			array[1] = 0;
			for (byte b = 0; b < 6; b += 1)
			{
				for (byte b2 = 0; b2 < 21; b2 += 1)
				{
					int num = (int)(5 - b);
					int num2 = (int)b2 + num * 21;
					if (this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102)
					{
						RGB_S rgb_S = WKDColor.cheatRGB_2ndME(colorBuffer[num2].R, colorBuffer[num2].G, colorBuffer[num2].B);
						array[(int)(2 + b2)] = rgb_S.B;
						array[(int)(23 + b2)] = rgb_S.G;
						array[(int)(44 + b2)] = rgb_S.R;
					}
					else if (this.m_ITE_KB_Type == RGBKB_Type.FourZone && (this.m_Project_ID == 6 || this.m_Project_ID == 7))
					{
						RGB_S rgb_S2 = WKDColor.cheatRGB_4Zone(colorBuffer[num2].R, colorBuffer[num2].G, colorBuffer[num2].B);
						array[(int)(2 + b2)] = rgb_S2.B;
						array[(int)(23 + b2)] = rgb_S2.G;
						array[(int)(44 + b2)] = rgb_S2.R;
					}
					else
					{
						array[(int)(2 + b2)] = colorBuffer[num2].B;
						array[(int)(23 + b2)] = colorBuffer[num2].G;
						array[(int)(44 + b2)] = colorBuffer[num2].R;
					}
				}
				this.Light_Lock();
				this.HID_Set_RowIndex_16H(b);
				this.Light_Lock();
				this.m_HIDDevice.Write(array, 0, array.Length);
				Thread.Sleep(1);
			}
			return true;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0001A674 File Offset: 0x00018874
		private bool Set_ITE_Effect_Type_StaticMode(byte Light, byte Save, RGB_S[] colorBuffer, bool bRefresh)
		{
			Log.s(LOG_LEVEL.TRACE, string.Format("RGBKeyboard_ITE | Set_ITE_Effect_Type_StaticMode light ={0}", Light));
			if (bRefresh)
			{
				this.Light_Lock();
				this.HID_Set_Effect_Type_08H(2, 51, 0, Light, 0, 0, Save);
			}
			byte[] array = new byte[65];
			array[0] = 0;
			RGB_S rgb_S = default(RGB_S);
			if (this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102)
			{
				rgb_S = WKDColor.cheatRGB_2ndME(colorBuffer[0].R, colorBuffer[0].G, colorBuffer[0].B);
			}
			else if (this.m_ITE_KB_Type == RGBKB_Type.FourZone && (this.m_Project_ID == 6 || this.m_Project_ID == 7))
			{
				rgb_S = WKDColor.cheatRGB_4Zone(colorBuffer[0].R, colorBuffer[0].G, colorBuffer[0].B);
			}
			for (byte b = 1; b < 65; b += 4)
			{
				array[(int)b] = 0;
				if (this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102)
				{
					array[(int)(b + 1)] = rgb_S.R;
					array[(int)(b + 2)] = rgb_S.G;
					array[(int)(b + 3)] = rgb_S.B;
				}
				else
				{
					array[(int)(b + 1)] = colorBuffer[0].R;
					array[(int)(b + 2)] = colorBuffer[0].G;
					array[(int)(b + 3)] = colorBuffer[0].B;
				}
			}
			this.Light_Lock();
			this.HID_Set_Picture_12H(Save);
			for (byte b2 = 0; b2 < 8; b2 += 1)
			{
				this.Light_Lock();
				this.m_HIDDevice.Write(array, 0, array.Length);
				Thread.Sleep(1);
			}
			return true;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0001A7FC File Offset: 0x000189FC
		private bool Set_ITE_Effect_Type_FwMode(byte effect, byte light, byte speed, byte direction, byte save, RGBKB_Color layout_color)
		{
			Log.s(LOG_LEVEL.TRACE, string.Format("RGBKeyboard_ITE | Set_ITE_Effect_Type_FwMode effect={0} light ={1}", effect, light));
			byte b;
			if (layout_color.ColorBlocks == 0u)
			{
				b = 0;
			}
			else if (layout_color.ColorBlocks > 0u && layout_color.isCircular)
			{
				b = 8;
				for (uint num = 0u; num < layout_color.ColorBlocks; num += 1u)
				{
					this.HID_Set_Color_14H((byte)(num + 1u), layout_color.ColorBuffer[(int)num].R, layout_color.ColorBuffer[(int)num].G, layout_color.ColorBuffer[(int)num].B);
					Thread.Sleep(1);
				}
			}
			else
			{
				b = 1;
				byte r = layout_color.ColorBuffer[0].R;
				byte g = layout_color.ColorBuffer[0].G;
				byte b2 = layout_color.ColorBuffer[0].B;
				this.HID_Set_Color_14H(b, r, g, b2);
			}
			return this.HID_Set_Effect_Type_08H(2, effect, speed, light, b, direction, save);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0001A900 File Offset: 0x00018B00
		private bool Set_ITE_Effect_Type_ApMode(byte effect, byte light, byte speed, byte direction, byte save, RGBKB_Color layout_color)
		{
			this.m_ap_effect_data.save_effect = effect;
			this.m_ap_effect_data.save_light = light;
			this.m_ap_effect_data.save_speed = speed;
			this.m_ap_effect_data.save_direction = direction;
			this.m_ap_effect_data.save_layout_color = layout_color;
			if (this.m_ap_effect_task == null)
			{
				this.m_ap_effect_task = new Task(new Action(this.AP_Effect_Task));
			}
			this.m_ap_effect_task_stop = false;
			if (this.m_ap_effect_task.Status != TaskStatus.Running)
			{
				this.m_ap_effect_task.Start();
			}
			return true;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0001A98C File Offset: 0x00018B8C
		private bool Set_ITE_Effect_Type_ApMode_Stop()
		{
			this.m_ap_effect_task_stop = true;
			uint num = 3000u;
			while (this.m_ap_effect_task != null || num == 0u)
			{
				Thread.Sleep(1);
				num -= 1u;
			}
			return true;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0001A9C0 File Offset: 0x00018BC0
		private void Save_Lighting_Effect_Data(byte save_effect, byte save_light, byte save_speed, byte save_direction, RGBKB_Color save_layout_color)
		{
			LM_ITE_RGB.m_save_lighting_data.bSaved = true;
			LM_ITE_RGB.m_save_lighting_data.save_effect = save_effect;
			LM_ITE_RGB.m_save_lighting_data.save_light = save_light;
			LM_ITE_RGB.m_save_lighting_data.save_speed = save_speed;
			LM_ITE_RGB.m_save_lighting_data.save_direction = save_direction;
			LM_ITE_RGB.m_save_lighting_data.save_layout_color = save_layout_color;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0001AA14 File Offset: 0x00018C14
		private void Save_Lighting_Color_Data(RGBKB_Color save_layout_color)
		{
			if (save_layout_color.ColorBlocks < 1u)
			{
				return;
			}
			int num = 0;
			while ((long)num < (long)((ulong)LM_ITE_RGB.m_save_lighting_data.save_layout_color.ColorBlocks))
			{
				if (LM_ITE_RGB.m_save_lighting_data.save_layout_color.ColorBuffer[num].ID == save_layout_color.ColorBuffer[0].ID)
				{
					LM_ITE_RGB.m_save_lighting_data.save_layout_color.ColorBuffer[num].R = save_layout_color.ColorBuffer[0].R;
					LM_ITE_RGB.m_save_lighting_data.save_layout_color.ColorBuffer[num].G = save_layout_color.ColorBuffer[0].G;
					LM_ITE_RGB.m_save_lighting_data.save_layout_color.ColorBuffer[num].B = save_layout_color.ColorBuffer[0].B;
					return;
				}
				num++;
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0001AB00 File Offset: 0x00018D00
		private bool Set_Lighting_Effect(byte control, byte effect, byte light, byte speed, byte direction, byte save, RGBKB_Color layout_color)
		{
			Log.s(LOG_LEVEL.TRACE, string.Format("RGBKeyboard_ITE | Set_Lighting_Effect effect={0} light ={1}", effect, light));
			if (this.m_effect_type == 3)
			{
				this.DLL_SetMusicMode(false, 0);
			}
			else if (this.m_effect_type == 4 && effect != 15 && effect != 12 && effect != 13)
			{
				this.Set_ITE_Effect_Type_ApMode_Stop();
			}
			if (effect == 255)
			{
				return false;
			}
			if (effect == 34)
			{
				if (this.m_effect_type == 1 || this.m_effect_type == 2 || this.m_effect_type == 4)
				{
					this.HID_Set_Effect_Type_08H(1, 0, 0, 0, 0, 0, 0);
				}
				this.m_effect_type = 3;
				this.DLL_SetMusicMode(true, light);
			}
			else if (effect == 1)
			{
				if (this.m_ITE_KB_Type == RGBKB_Type.FourZone)
				{
					this.m_effect_type = 0;
					this.Set_ITE_Effect_Type_FwMode(effect, light, speed, direction, save, layout_color);
				}
				else
				{
					this.m_effect_type = 2;
					RGB_S[] colorBuffer = layout_color.ColorBuffer;
					this.Set_ITE_Effect_Type_StaticMode(light, save, colorBuffer, true);
				}
			}
			else if (effect == 51)
			{
				this.m_effect_type = 1;
				RGB_S[] colorBuffer2 = layout_color.ColorBuffer;
				this.Set_ITE_Effect_Type_UserMode(light, save, colorBuffer2, true);
			}
			else if (effect == 15 || effect == 12 || effect == 13)
			{
				this.m_effect_type = 4;
				this.Set_ITE_Effect_Type_ApMode(effect, light, speed, direction, save, layout_color);
			}
			else if (effect == 5 && this.m_ITE_KB_Type == RGBKB_Type.FourZone)
			{
				this.m_effect_type = 0;
				this.HID_Set_Color_14H(1, byte.MaxValue, 0, 0);
				Thread.Sleep(1);
				this.HID_Set_Color_14H(2, byte.MaxValue, byte.MaxValue, 0);
				Thread.Sleep(1);
				this.HID_Set_Color_14H(3, 0, 0, byte.MaxValue);
				Thread.Sleep(1);
				this.HID_Set_Color_14H(4, byte.MaxValue, 0, byte.MaxValue);
				Thread.Sleep(1);
				this.Set_ITE_Effect_Type_FwMode(effect, light, speed, direction, save, layout_color);
			}
			else
			{
				this.m_effect_type = 0;
				this.Set_ITE_Effect_Type_FwMode(effect, light, speed, direction, save, layout_color);
			}
			this.Enable_EC_OnkeyPressed(effect, direction);
			return true;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0001ACEC File Offset: 0x00018EEC
		private bool Get_ITE_Light_Value(ref byte light)
		{
			byte b = 0;
			byte b2 = 0;
			byte b3 = 0;
			byte b4 = 0;
			byte b5 = 0;
			this.HID_Get_Effect_Type_88H(ref b, ref b2, ref b3, ref light, ref b4, ref b5);
			return true;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0001AD18 File Offset: 0x00018F18
		private bool Set_Welcome_Effect_Enable(bool Enable, byte timeoutEffect, byte timeout)
		{
			byte b = Enable ? 1 : 0;
			try
			{
				if (!WMIEC.WMIWriteBiosRom(1873497444986126336UL + ((ulong)timeoutEffect << 48) + ((ulong)b << 40) + ((ulong)timeout << 32)))
				{
					OemService.Write(string.Format("ledkb /setdata 0x1A {0} {1} {2} 0x00 0x00 0x00 0x00", timeoutEffect.ToString(), b.ToString(), timeout.ToString()));
				}
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0001AD8C File Offset: 0x00018F8C
		private bool Set_Welcome_Effect(byte control, byte effect, byte light, byte speed, byte direction, byte save, RGBKB_Color layout_color)
		{
			if (effect == 255)
			{
				return false;
			}
			if (effect == 1 && (this.m_ITE_KB_Type == RGBKB_Type.MEZone_1st || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102))
			{
				if (layout_color.ColorBlocks == 0u)
				{
					return false;
				}
				byte b = 8;
				uint num = 7u;
				byte r = layout_color.ColorBuffer[0].R;
				byte g = layout_color.ColorBuffer[0].G;
				byte b2 = layout_color.ColorBuffer[0].B;
				for (uint num2 = 0u; num2 < num; num2 += 1u)
				{
					this.HID_Set_Color_14H((byte)(num2 + 9u), r, g, b2);
				}
				effect = 3;
				if (!WMIEC.WMIWriteBiosRom(576460752303423488UL + ((ulong)control << 48) + ((ulong)effect << 40) + ((ulong)speed << 32) + ((ulong)light << 24) + ((ulong)b << 16) + ((ulong)direction << 8)))
				{
					OemService.Write(string.Format("ledkb /setdata 0x08 {0} {1} {2} {3} {4} {5} 0x00", new object[]
					{
						control.ToString(),
						effect.ToString(),
						speed.ToString(),
						light.ToString(),
						b.ToString(),
						direction.ToString()
					}));
				}
			}
			else if (effect == 5 && this.m_ITE_KB_Type == RGBKB_Type.FourZone)
			{
				byte b3 = 8;
				this.HID_Set_Color_14H(9, byte.MaxValue, 0, 0);
				Thread.Sleep(1);
				this.HID_Set_Color_14H(10, byte.MaxValue, byte.MaxValue, 0);
				Thread.Sleep(1);
				this.HID_Set_Color_14H(11, 0, 0, byte.MaxValue);
				Thread.Sleep(1);
				this.HID_Set_Color_14H(12, byte.MaxValue, 0, byte.MaxValue);
				Thread.Sleep(1);
				if (!WMIEC.WMIWriteBiosRom(576460752303423488UL + ((ulong)control << 48) + ((ulong)effect << 40) + ((ulong)speed << 32) + ((ulong)light << 24) + ((ulong)b3 << 16) + ((ulong)direction << 8)))
				{
					OemService.Write(string.Format("ledkb /setdata 0x08 {0} {1} {2} {3} {4} {5} 0x00", new object[]
					{
						control.ToString(),
						effect.ToString(),
						speed.ToString(),
						light.ToString(),
						b3.ToString(),
						direction.ToString()
					}));
				}
			}
			else
			{
				byte b4;
				if (layout_color.ColorBlocks == 0u)
				{
					b4 = 0;
				}
				else if (layout_color.ColorBlocks > 0u && layout_color.isCircular)
				{
					b4 = 8;
					for (uint num3 = 0u; num3 < layout_color.ColorBlocks; num3 += 1u)
					{
						this.HID_Set_Color_14H((byte)(num3 + 9u), layout_color.ColorBuffer[(int)num3].R, layout_color.ColorBuffer[(int)num3].G, layout_color.ColorBuffer[(int)num3].B);
					}
				}
				else
				{
					b4 = 9;
					for (byte b5 = 9; b5 <= 15; b5 += 1)
					{
						this.HID_Set_Color_14H(b5, layout_color.ColorBuffer[0].R, layout_color.ColorBuffer[0].G, layout_color.ColorBuffer[0].B);
					}
				}
				if (!WMIEC.WMIWriteBiosRom(576460752303423488UL + ((ulong)control << 48) + ((ulong)effect << 40) + ((ulong)speed << 32) + ((ulong)light << 24) + ((ulong)b4 << 16) + ((ulong)direction << 8)))
				{
					OemService.Write(string.Format("ledkb /setdata 0x08 {0} {1} {2} {3} {4} {5} 0x00", new object[]
					{
						control.ToString(),
						effect.ToString(),
						speed.ToString(),
						light.ToString(),
						b4.ToString(),
						direction.ToString()
					}));
				}
			}
			if (LM_ITE_RGB.m_save_lighting_data.bSaved)
			{
				this.Set_Lighting_Effect(2, LM_ITE_RGB.m_save_lighting_data.save_effect, LM_ITE_RGB.m_save_lighting_data.save_light, LM_ITE_RGB.m_save_lighting_data.save_speed, LM_ITE_RGB.m_save_lighting_data.save_direction, 1, LM_ITE_RGB.m_save_lighting_data.save_layout_color);
			}
			return true;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0001B178 File Offset: 0x00019378
		private RGBKB_Effect Translate_LM_EffectIndex(byte fw_effect_id)
		{
			if (fw_effect_id <= 22)
			{
				switch (fw_effect_id)
				{
				case 1:
					return RGBKB_Effect.Single;
				case 2:
					return RGBKB_Effect.Breathing;
				case 3:
					return RGBKB_Effect.Wave;
				case 4:
				case 7:
				case 8:
				case 11:
				case 12:
				case 13:
				case 15:
				case 16:
					break;
				case 5:
					return RGBKB_Effect.Rainbow;
				case 6:
					return RGBKB_Effect.Ripple;
				case 9:
					return RGBKB_Effect.Marquee;
				case 10:
					return RGBKB_Effect.Raindrop;
				case 14:
					return RGBKB_Effect.Aurora;
				case 17:
					return RGBKB_Effect.Spark;
				default:
					if (fw_effect_id == 22)
					{
						return RGBKB_Effect.RippleO;
					}
					break;
				}
			}
			else
			{
				if (fw_effect_id == 34)
				{
					return RGBKB_Effect.Music;
				}
				if (fw_effect_id == 51)
				{
					return RGBKB_Effect.UserMode;
				}
			}
			return RGBKB_Effect.UnKnown;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0001B208 File Offset: 0x00019408
		private byte Translate_ITE_EffectIndex(RGBKB_Effect layoutEffect)
		{
			switch (layoutEffect)
			{
			case RGBKB_Effect.Single:
				return 1;
			case RGBKB_Effect.Breathing:
				return 2;
			case RGBKB_Effect.Wave:
				return 3;
			case RGBKB_Effect.Reactive:
				return 4;
			case RGBKB_Effect.Rainbow:
				return 5;
			case RGBKB_Effect.Ripple:
				return 6;
			case RGBKB_Effect.Raindrop:
				return 10;
			case RGBKB_Effect.Neon:
				return 15;
			case RGBKB_Effect.Marquee:
				return 9;
			case RGBKB_Effect.Stack:
				return 12;
			case RGBKB_Effect.Impact:
				return 13;
			case RGBKB_Effect.Spark:
				return 17;
			case RGBKB_Effect.Aurora:
				return 14;
			case RGBKB_Effect.Music:
				return 34;
			case RGBKB_Effect.UserMode:
				return 51;
			case RGBKB_Effect.Flash:
				return 18;
			case RGBKB_Effect.Mix:
				return 19;
			case RGBKB_Effect.RippleO:
				return 22;
			}
			return byte.MaxValue;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0001B29E File Offset: 0x0001949E
		private byte Translate_ITE_LightValue(uint layoutLight)
		{
			switch (layoutLight)
			{
			case 0u:
				return 0;
			case 1u:
				return 8;
			case 2u:
				return 22;
			case 3u:
				return 36;
			case 4u:
				return 50;
			default:
				return 0;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0001B2CA File Offset: 0x000194CA
		private byte Translate_ITE_SpeedValue(uint layoutSpeed)
		{
			switch (layoutSpeed)
			{
			case 0u:
				return 10;
			case 1u:
				return 7;
			case 2u:
				return 5;
			case 3u:
				return 3;
			case 4u:
				return 1;
			default:
				return 1;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0001B2F4 File Offset: 0x000194F4
		private byte Translate_ITE_DirectionValue(RGBKB_Direction layoutDirection)
		{
			switch (layoutDirection)
			{
			case RGBKB_Direction.None:
				return 0;
			case RGBKB_Direction.LeftRight:
				return 1;
			case RGBKB_Direction.RightLeft:
				return 2;
			case RGBKB_Direction.DownUp:
				return 3;
			case RGBKB_Direction.UpDown:
				return 4;
			case RGBKB_Direction.OnKeyPressed:
				return 1;
			case RGBKB_Direction.Sync:
				return 3;
			default:
				return 1;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0001B32B File Offset: 0x0001952B
		private uint Translate_Layout_LightValue(byte ite_light)
		{
			if (ite_light <= 8)
			{
				if (ite_light == 0)
				{
					return 0u;
				}
				if (ite_light == 8)
				{
					return 1u;
				}
			}
			else
			{
				if (ite_light == 22)
				{
					return 2u;
				}
				if (ite_light == 36)
				{
					return 3u;
				}
				if (ite_light == 50)
				{
					return 4u;
				}
			}
			return 0u;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0001B356 File Offset: 0x00019556
		public LM_ITE_RGB()
		{
			LM_ITE_RGB.m_save_lighting_data.bSaved = false;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0001B36C File Offset: 0x0001956C
		public bool ILM_RGBKB_Init(RGBKB_Event_Handler event_handler)
		{
			object value = 0;
			WMIEC.WMIReadECRAM(1856UL, ref value);
			this.m_Project_ID = (byte)Convert.ToUInt64(value);
			this.m_HIDManager = new HIDManager();
			if (this.m_HIDManager.Init(1165, 52736, 1))
			{
				ushort usagePage = this.m_HIDManager.GetUsagePage();
				if (usagePage != 65282)
				{
					if (usagePage != 65283)
					{
						if (usagePage == 65298)
						{
							this.m_ITE_KB_Type = RGBKB_Type.FourZone;
							goto IL_E1;
						}
					}
					else
					{
						try
						{
							byte b = Convert.ToByte(URegistry.RegistryValueRead("", "KBTypeID", 25));
							if (b == 25 || b == 41)
							{
								this.m_ITE_KB_Type = RGBKB_Type.MEZone_2nd_101;
							}
							else if (b == 17 || b == 33)
							{
								this.m_ITE_KB_Type = RGBKB_Type.MEZone_2nd_102;
							}
							else
							{
								this.m_ITE_KB_Type = RGBKB_Type.MEZone_2nd_101;
							}
							goto IL_E1;
						}
						catch
						{
							this.m_ITE_KB_Type = RGBKB_Type.MEZone_2nd_101;
							Log.s(LOG_LEVEL.ERROR, "RGBKeyboard_ITE|ILM_RGBKB_Init : USAGE_PAGE_ME_2ND, query KBID failed");
							goto IL_E1;
						}
					}
					return false;
				}
				this.m_ITE_KB_Type = RGBKB_Type.MEZone_1st;
				IL_E1:
				this.m_HIDDevice = new FileStream(new SafeFileHandle(this.m_HIDManager.m_Handle, false), FileAccess.ReadWrite, 65, true);
				LM_ITE_RGB.m_Layout_Event_handler += event_handler;
				if (WMIEC.LMScanCodeEvent == null)
				{
					WMIEC.LMScanCodeEvent = (WMIEC.LM_ScanCode_EventHander)Delegate.Combine(WMIEC.LMScanCodeEvent, new WMIEC.LM_ScanCode_EventHander(this.ScanCode_Hnadler));
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0001B4BC File Offset: 0x000196BC
		public bool ILM_RGBKB_SetPower(RGBKB_PowerStatus PowerStatus)
		{
			Log.s(LOG_LEVEL.TRACE, string.Format("RGBKeyboard_ITE | ILM_RGBKB_SetPower powerstatus = {0}", PowerStatus));
			bool result = true;
			if (PowerStatus == RGBKB_PowerStatus.Off || PowerStatus == RGBKB_PowerStatus.Lighting_off)
			{
				this.DLL_SetMusicMode(false, 0);
				this.Disable_EC_OnkeyPressed();
				Log.s(LOG_LEVEL.TRACE, "RGBKeyboard_ITE | ILM_RGBKB_SetPower HID_Set_Effect_Type_08H LED_OFF");
				if (this.m_effect_type == 4)
				{
					this.Set_ITE_Effect_Type_ApMode_Stop();
				}
				this.HID_Set_Effect_Type_08H(1, 0, 0, 0, 0, 0, 0);
			}
			else if (PowerStatus == RGBKB_PowerStatus.On || PowerStatus == RGBKB_PowerStatus.Lighting_on)
			{
				if (this.m_effect_type == 3)
				{
					this.DLL_SetMusicMode(true, LM_ITE_RGB.m_save_lighting_data.save_light);
				}
				else if (LM_ITE_RGB.m_save_lighting_data.bSaved)
				{
					Log.s(LOG_LEVEL.TRACE, string.Format("RGBKeyboard_ITE | ILM_RGBKB_SetPower Set_Lighting_Effect effect={0} light ={1}", LM_ITE_RGB.m_save_lighting_data.save_effect, LM_ITE_RGB.m_save_lighting_data.save_light));
					this.Set_Lighting_Effect(2, LM_ITE_RGB.m_save_lighting_data.save_effect, LM_ITE_RGB.m_save_lighting_data.save_light, LM_ITE_RGB.m_save_lighting_data.save_speed, LM_ITE_RGB.m_save_lighting_data.save_direction, 0, LM_ITE_RGB.m_save_lighting_data.save_layout_color);
				}
				else
				{
					Log.s(LOG_LEVEL.TRACE, "RGBKeyboard_ITE | ILM_RGBKB_SetPower becuse ITE FW not support keep status, so return false to AP re-set again");
					result = false;
				}
			}
			if (PowerStatus == RGBKB_PowerStatus.Off || PowerStatus == RGBKB_PowerStatus.Welcome_off)
			{
				this.Set_Welcome_Effect_Enable(false, 5, 0);
			}
			else if (PowerStatus == RGBKB_PowerStatus.On || PowerStatus == RGBKB_PowerStatus.Welcome_on)
			{
				this.Set_Welcome_Effect_Enable(true, 5, 20);
			}
			return result;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0001609C File Offset: 0x0001429C
		public bool ILM_RGBKB_GetPower(ref RGBKB_PowerStatus PowerStatus)
		{
			return false;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0001B5FC File Offset: 0x000197FC
		public RGBKB_Type ILM_RGBKB_GetRGBKeyboardType()
		{
			return this.m_ITE_KB_Type;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0001B604 File Offset: 0x00019804
		public string ILM_RGBKB_GetFirmwareVersion()
		{
			byte[] array = new byte[9];
			array[1] = 128;
			if (!this.m_HIDManager.WriteFeature(array))
			{
				Log.s(LOG_LEVEL.ERROR, "RGBKeyboard_ITE|ILM_RGBKB_GetFirmwareVersion : WriteFeature failed");
			}
			else
			{
				Thread.Sleep(1);
				byte[] array2 = new byte[9];
				if (this.m_HIDManager.GetFeature(array2))
				{
					Thread.Sleep(1);
					return string.Format("{0:X}.{1:X}.{2:X}.{3:X}", new object[]
					{
						array2[2],
						array2[3],
						array2[4],
						array2[5]
					});
				}
				Log.s(LOG_LEVEL.ERROR, "RGBKeyboard_ITE|ILM_RGBKB_GetFirmwareVersion : GetFeature failed");
			}
			return "";
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0001B6B0 File Offset: 0x000198B0
		public bool ILM_RGBKB_SetEffectALL(RGBKB_Mode layout_mode, RGBKB_Effect layout_effect, uint layout_light, uint layout_speed, RGBKB_Direction layout_direction, RGBKB_Color layout_color, RGBKB_NV_SAVE layout_save)
		{
			byte b = this.Translate_ITE_EffectIndex(layout_effect);
			byte b2 = this.Translate_ITE_LightValue(layout_light);
			byte b3 = this.Translate_ITE_SpeedValue(layout_speed);
			byte b4 = this.Translate_ITE_DirectionValue(layout_direction);
			byte save = (byte)layout_save;
			if (layout_mode == RGBKB_Mode.Lighting)
			{
				this.Save_Lighting_Effect_Data(b, b2, b3, b4, layout_color);
				this.Set_Lighting_Effect(2, b, b2, b3, b4, save, layout_color);
			}
			else if (layout_mode == RGBKB_Mode.Welcome)
			{
				this.Set_Welcome_Effect(3, b, b2, b3, b4, save, layout_color);
			}
			return false;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0001609C File Offset: 0x0001429C
		public bool ILM_RGBKB_GetEffectALL(RGBKB_Mode layout_mode, ref RGBKB_Effect layout_effect, ref uint layout_light, ref uint layout_speed, ref RGBKB_Direction layout_direction, ref RGBKB_Color layout_color)
		{
			return false;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0001609C File Offset: 0x0001429C
		public bool ILM_RGBKB_SetEffect(RGBKB_Mode layout_mode, RGBKB_Effect layout_effect)
		{
			return false;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0001B728 File Offset: 0x00019928
		public bool ILM_RGBKB_GetEffect(RGBKB_Mode layout_mode, ref RGBKB_Effect layout_effect)
		{
			if (layout_mode == RGBKB_Mode.Lighting)
			{
				return false;
			}
			if (layout_mode == RGBKB_Mode.Welcome)
			{
				string[] array = new string[]
				{
					"OemServiceWinApp.exe",
					"ledkb",
					"/getstatus"
				};
				int lenRead = 512;
				byte[] array2 = new byte[512];
				OemService.Exec(array.Length, array, lenRead, array2);
				byte fw_effect_id = Convert.ToByte(string.Format("{0}{1}", (char)array2[6], (char)array2[7]), 16);
				layout_effect = this.Translate_LM_EffectIndex(fw_effect_id);
			}
			return false;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0001609C File Offset: 0x0001429C
		public bool ILM_RGBKB_SetBrighntess(uint layout_brightness)
		{
			return false;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0001B7A8 File Offset: 0x000199A8
		public bool ILM_RGBKB_GetBrighntess(ref uint layout_brightness)
		{
			byte ite_light = 0;
			if (this.Get_ITE_Light_Value(ref ite_light))
			{
				layout_brightness = (uint)((byte)this.Translate_Layout_LightValue(ite_light));
				return true;
			}
			return false;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0001609C File Offset: 0x0001429C
		public bool ILM_RGBKB_SetSpeed(uint layout_speed)
		{
			return false;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0001B7D0 File Offset: 0x000199D0
		public bool ILM_RGBKB_SetColor(RGBKB_Mode layout_mode, RGBKB_Effect layout_effect, RGBKB_Color layout_color)
		{
			if (layout_color.ColorBlocks == 0u)
			{
				return false;
			}
			if (layout_mode == RGBKB_Mode.Lighting)
			{
				this.Save_Lighting_Color_Data(layout_color);
				if (layout_effect == RGBKB_Effect.Single)
				{
					if (this.m_ITE_KB_Type == RGBKB_Type.FourZone)
					{
						byte index = (byte)(layout_color.ColorBuffer[0].ID + 1u);
						this.HID_Set_Color_14H(index, layout_color.ColorBuffer[0].R, layout_color.ColorBuffer[0].G, layout_color.ColorBuffer[0].B);
						return true;
					}
					byte[] array = new byte[65];
					array[0] = 0;
					RGB_S rgb_S = default(RGB_S);
					if (this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102)
					{
						rgb_S = WKDColor.cheatRGB_2ndME(layout_color.ColorBuffer[0].R, layout_color.ColorBuffer[0].G, layout_color.ColorBuffer[0].B);
					}
					else if (this.m_ITE_KB_Type == RGBKB_Type.FourZone && (this.m_Project_ID == 6 || this.m_Project_ID == 7))
					{
						rgb_S = WKDColor.cheatRGB_4Zone(layout_color.ColorBuffer[0].R, layout_color.ColorBuffer[0].G, layout_color.ColorBuffer[0].B);
					}
					for (byte b = 1; b < 65; b += 4)
					{
						array[(int)b] = 0;
						if (this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102)
						{
							array[(int)(b + 1)] = rgb_S.R;
							array[(int)(b + 2)] = rgb_S.G;
							array[(int)(b + 3)] = rgb_S.B;
						}
						else
						{
							array[(int)(b + 1)] = layout_color.ColorBuffer[0].R;
							array[(int)(b + 2)] = layout_color.ColorBuffer[0].G;
							array[(int)(b + 3)] = layout_color.ColorBuffer[0].B;
						}
					}
					this.HID_Set_Picture_12H(0);
					for (byte b2 = 0; b2 < 8; b2 += 1)
					{
						this.m_HIDDevice.Write(array, 0, array.Length);
						Thread.Sleep(1);
					}
					return true;
				}
				else
				{
					byte index2 = (byte)(layout_color.ColorBuffer[0].ID + 1u);
					this.HID_Set_Color_14H(index2, layout_color.ColorBuffer[0].R, layout_color.ColorBuffer[0].G, layout_color.ColorBuffer[0].B);
				}
			}
			return true;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0001BA24 File Offset: 0x00019C24
		public bool ILM_RGBKB_SaveLightingLevel(uint layout_light)
		{
			byte save_light = this.Translate_ITE_LightValue(layout_light);
			LM_ITE_RGB.m_save_lighting_data.save_light = save_light;
			return true;
		}

		// Token: 0x04000405 RID: 1029
		private HIDManager m_HIDManager;

		// Token: 0x04000406 RID: 1030
		private FileStream m_HIDDevice;

		// Token: 0x04000407 RID: 1031
		private RGBKB_Type m_ITE_KB_Type;

		// Token: 0x04000408 RID: 1032
		private byte m_Project_ID;

		// Token: 0x0400040A RID: 1034
		private byte m_effect_type;

		// Token: 0x0400040B RID: 1035
		private static SAVE_LIGHTING_EFFECT_DATA m_save_lighting_data;

		// Token: 0x0400040C RID: 1036
		private bool m_enableOnkeyPressed;

		// Token: 0x0400040D RID: 1037
		private bool m_light_lock;

		// Token: 0x0400040E RID: 1038
		private Task m_ap_effect_task;

		// Token: 0x0400040F RID: 1039
		private bool m_ap_effect_task_stop;

		// Token: 0x04000410 RID: 1040
		private SAVE_LIGHTING_EFFECT_DATA m_ap_effect_data;
	}
}
