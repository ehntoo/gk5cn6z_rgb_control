using System;

namespace LightingModel
{
	// Token: 0x02000036 RID: 54
	internal class ITE_SPEC
	{
		// Token: 0x040003C8 RID: 968
		internal const ushort PID = 0x48D;

		// Token: 0x040003C9 RID: 969
		internal const ushort VID = 0xCE00;

		// Token: 0x040003CA RID: 970
		internal const ushort USAGE = 1;

		// Token: 0x040003CB RID: 971
		internal const ushort USAGE_PAGE_4Zone = 0xFF12;

		// Token: 0x040003CC RID: 972
		internal const ushort USAGE_PAGE_ME_1ST = 0xFF02;

		// Token: 0x040003CD RID: 973
		internal const ushort USAGE_PAGE_ME_2ND = 0xFF03;

		// Token: 0x040003CE RID: 974
		internal const byte MS_RESERVED = 0;

		// Token: 0x040003CF RID: 975
		internal const uint RGB_LED_NUM = 126u;

		// Token: 0x040003D0 RID: 976
		internal const byte Contorl_LED_Off = 1;

		// Token: 0x040003D1 RID: 977
		internal const byte Contorl_LED_Default = 2;

		// Token: 0x040003D2 RID: 978
		internal const byte Contorl_LED_Welcome = 3;

		// Token: 0x040003D3 RID: 979
		internal const byte NV_SAVE = 1;

		// Token: 0x040003D4 RID: 980
		internal const byte NV_NOT_SAVE = 0;

		// Token: 0x040003D5 RID: 981
		internal const byte EFFECT_STATIC = 1;

		// Token: 0x040003D6 RID: 982
		internal const byte EFFECT_BREATHING = 2;

		// Token: 0x040003D7 RID: 983
		internal const byte EFFECT_REACTIVE = 4;

		// Token: 0x040003D8 RID: 984
		internal const byte EFFECT_WAVE = 3;

		// Token: 0x040003D9 RID: 985
		internal const byte EFFECT_RAINBOW = 5;

		// Token: 0x040003DA RID: 986
		internal const byte EFFECT_RIPPLE = 6;

		// Token: 0x040003DB RID: 987
		internal const byte EFFECT_NOMO = 8;

		// Token: 0x040003DC RID: 988
		internal const byte EFFECT_MARQUEE = 9;

		// Token: 0x040003DD RID: 989
		internal const byte EFFECT_RAINDROP = 10;

		// Token: 0x040003DE RID: 990
		internal const byte EFFECT_STACK = 12;

		// Token: 0x040003DF RID: 991
		internal const byte EFFECT_IMPACT = 13;

		// Token: 0x040003E0 RID: 992
		internal const byte EFFECT_AURORA = 14;

		// Token: 0x040003E1 RID: 993
		internal const byte EFFECT_NEON = 15;

		// Token: 0x040003E2 RID: 994
		internal const byte EFFECT_SPARK = 17;

		// Token: 0x040003E3 RID: 995
		internal const byte EFFECT_FLASH = 18;

		// Token: 0x040003E4 RID: 996
		internal const byte EFFECT_MIX = 19;

		// Token: 0x040003E5 RID: 997
		internal const byte EFFECT_RIPPLEO = 22;

		// Token: 0x040003E6 RID: 998
		internal const byte EFFECT_MUSIC = 34;

		// Token: 0x040003E7 RID: 999
		internal const byte EFFECT_USERMODE = 51;

		// Token: 0x040003E8 RID: 1000
		internal const byte EFFECT_UNKNOWN = 255;

		// Token: 0x040003E9 RID: 1001
		internal const byte EFFECT_TYPE_FW = 0;

		// Token: 0x040003EA RID: 1002
		internal const byte EFFECT_TYPE_ROW = 1;

		// Token: 0x040003EB RID: 1003
		internal const byte EFFECT_TYPE_PICTURE = 2;

		// Token: 0x040003EC RID: 1004
		internal const byte EFFECT_TYPE_MUSIC = 3;

		// Token: 0x040003ED RID: 1005
		internal const byte EFFECT_TYPE_AP = 4;

		// Token: 0x040003EE RID: 1006
		internal const ushort EC_RGBKBBKL_LEVEL_UPDATE = 240;

		// Token: 0x040003EF RID: 1007
		internal const ulong EC_PROJECT_ID_BYTE = 1856UL;

		// Token: 0x040003F0 RID: 1008
		internal const ulong EC_AP_OEM_BYTE = 1857UL;

		// Token: 0x040003F1 RID: 1009
		internal const byte EC_bitOnKeyPressOn = 8;

		// Token: 0x040003F2 RID: 1010
		internal const byte EC_PROJECT_LEGANCY = 0;

		// Token: 0x040003F3 RID: 1011
		internal const byte EC_PROJECT_GIxKN = 1;

		// Token: 0x040003F4 RID: 1012
		internal const byte EC_PROJECT_GJxKN = 2;

		// Token: 0x040003F5 RID: 1013
		internal const byte EC_PROJECT_GKxCN = 3;

		// Token: 0x040003F6 RID: 1014
		internal const byte EC_PROJECT_GIxCN = 4;

		// Token: 0x040003F7 RID: 1015
		internal const byte EC_PROJECT_GJxCN = 5;

		// Token: 0x040003F8 RID: 1016
		internal const byte EC_PROJECT_GK5CN_X = 6;

		// Token: 0x040003F9 RID: 1017
		internal const byte EC_PROJECT_GK7CN_S = 7;

		// Token: 0x040003FA RID: 1018
		internal const byte EC_PROJECT_GK7CPCS_GK5CQ7Z = 8;

		// Token: 0x040003FB RID: 1019
		internal const byte EC_KBID_101 = 25;

		// Token: 0x040003FC RID: 1020
		internal const byte EC_KBID_101M = 41;

		// Token: 0x040003FD RID: 1021
		internal const byte EC_KBID_102 = 17;

		// Token: 0x040003FE RID: 1022
		internal const byte EC_KBID_102M = 33;
	}
}
