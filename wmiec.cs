using System;
using System.Management;

namespace Utility
{
	// Token: 0x02000006 RID: 6
	public static class WMIEC
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000022E8 File Offset: 0x000004E8
		private static void WMIHandleEvent(object sender, EventArrivedEventArgs e)
		{
			try
			{
				int scancode = Convert.ToInt32(e.NewEvent.SystemProperties["ULong"].Value.ToString());
				WMIEC.ScanCode_EventHander scanCodeEvent = WMIEC.ScanCodeEvent;
				if (scanCodeEvent != null)
				{
					scanCodeEvent(scancode);
				}
				WMIEC.LM_ScanCode_EventHander lmscanCodeEvent = WMIEC.LMScanCodeEvent;
				if (lmscanCodeEvent != null)
				{
					lmscanCodeEvent(scancode);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002354 File Offset: 0x00000554
		private static void StartWMIReceiveEvent(EventArrivedEventHandler WMIHandleEvent)
		{
			try
			{
				WqlEventQuery query = new WqlEventQuery("SELECT * FROM AcpiTest_EventULong");
				WMIEC.watcher = new ManagementEventWatcher(new ManagementScope("\\\\.\\Root\\WMI"), query);
				WMIEC.watcher.EventArrived += WMIHandleEvent;
				WMIEC.watcher.Start();
			}
			catch (ManagementException ex)
			{
				Log.s(LOG_LEVEL.ERROR, string.Format("WMIEC|StartWMIReceiveEvent : An error occurred while trying to receive an event: " + ex.Message, Array.Empty<object>()));
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023CC File Offset: 0x000005CC
		private static void EndWMIRecieveEvent()
		{
			if (WMIEC.watcher != null)
			{
				WMIEC.watcher.Stop();
				WMIEC.watcher = null;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023E8 File Offset: 0x000005E8
		public static bool WMIReadECRAM(ulong Addr, ref object data)
		{
			bool result;
			try
			{
				ManagementObject managementObject = new ManagementObject("root\\WMI", "AcpiTest_MULong.InstanceName='ACPI\\PNP0C14\\1_1'", null);
				ManagementBaseObject methodParameters = managementObject.GetMethodParameters("GetSetULong");
				Addr = 1099511627776UL + Addr;
				methodParameters["Data"] = Addr;
				ManagementBaseObject managementBaseObject = managementObject.InvokeMethod("GetSetULong", methodParameters, null);
				data = managementBaseObject["Return"];
				result = true;
			}
			catch (ManagementException ex)
			{
				Log.s(LOG_LEVEL.ERROR, string.Format("WMIEC|WMIReadECRAM : Failed" + ex.Message, Array.Empty<object>()));
				result = false;
			}
			return result;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002484 File Offset: 0x00000684
		public static void WMIWriteECRAM(ulong Addr, ulong Value)
		{
			try
			{
				ManagementObject managementObject = new ManagementObject("root\\WMI", "AcpiTest_MULong.InstanceName='ACPI\\PNP0C14\\1_1'", null);
				ManagementBaseObject methodParameters = managementObject.GetMethodParameters("GetSetULong");
				Value <<= 16;
				Addr = Value + Addr;
				methodParameters["Data"] = Addr;
				managementObject.InvokeMethod("GetSetULong", methodParameters, null);
			}
			catch (ManagementException ex)
			{
				Log.s(LOG_LEVEL.ERROR, string.Format("WMIEC|WMIWriteECRAM : Failed" + ex.Message, Array.Empty<object>()));
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000250C File Offset: 0x0000070C
		public static bool WMIWriteBiosRom(ulong Value)
		{
			try
			{
				ManagementObject managementObject = new ManagementObject("root\\WMI", "AcpiODM_Demo.InstanceName='ACPI\\PNP0C14\\2_0'", null);
				ManagementBaseObject methodParameters = managementObject.GetMethodParameters("GetUlongEx7");
				methodParameters["Data"] = Value;
				managementObject.InvokeMethod("GetUlongEx7", methodParameters, null);
			}
			catch (ManagementException ex)
			{
				Log.s(LOG_LEVEL.ERROR, string.Format("WMIEC|WMIWriteECRAM : Failed" + ex.Message, Array.Empty<object>()));
				return false;
			}
			return true;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002590 File Offset: 0x00000790
		static WMIEC()
		{
			WMIEC.StartWMIReceiveEvent(new EventArrivedEventHandler(WMIEC.WMIHandleEvent));
		}

		// Token: 0x04000009 RID: 9
		public static WMIEC.ScanCode_EventHander ScanCodeEvent;

		// Token: 0x0400000A RID: 10
		public static WMIEC.LM_ScanCode_EventHander LMScanCodeEvent;

		// Token: 0x0400000B RID: 11
		private static ManagementEventWatcher watcher = null;

		// Token: 0x0400000C RID: 12
		private static readonly WMIEC.Destructor finalObj = new WMIEC.Destructor();

		// Token: 0x0200009D RID: 157
		// (Invoke) Token: 0x060005F2 RID: 1522
		public delegate void ScanCode_EventHander(int scancode);

		// Token: 0x0200009E RID: 158
		// (Invoke) Token: 0x060005F6 RID: 1526
		public delegate void LM_ScanCode_EventHander(int scancode);

		// Token: 0x0200009F RID: 159
		private class Destructor
		{
			// Token: 0x060005F9 RID: 1529 RVA: 0x000637C8 File Offset: 0x000619C8
			~Destructor()
			{
				WMIEC.EndWMIRecieveEvent();
			}
		}
	}
}
