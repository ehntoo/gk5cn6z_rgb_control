extern crate hidapi;

use hidapi::HidApi;
use std::fmt;

struct Array<T> {
    data: [T; 65]
}

impl<T: fmt::Debug> fmt::Debug for Array<T> {
    fn fmt(&self, formatter: &mut fmt::Formatter) -> fmt::Result {
        self.data[..].fmt(formatter)
    }
}

fn main() {
    let api = HidApi::new().expect("Failed to create API instance");

    let kb = api.open(1165, 52736).expect("Failed to open device");

    // private bool HID_Set_Effect_Type_08H(byte Control, byte Effect, byte Speed, byte Light, byte ColorIndex, byte Direction, byte Save)
    // {
    //     byte[] buffer = new byte[]
    //     {
    //         0,
    //         8,
    //         Control,
    //         Effect,
    //         Speed,
    //         Light,
    //         ColorIndex,
    //         Direction,
    //         Save
    //     };
    //     this.m_HIDManager.WriteFeature(buffer);
    //     Thread.Sleep(1);
    //     return true;
    // }

    // let mut report: [u8; 9] = [0; 9];
    // kb.get_feature_report(&mut report).expect("failed to get feature report");
    // println!("{:x?}", report);

    let disable_report = [0u8, 8, 1, 0, 0, 0, 0, 0, 0];
    kb.send_feature_report(&disable_report).expect("Failed to send feature report");

    // kb.get_feature_report(&mut report).expect("failed to get feature report");
    // println!("{:x?}", report);

    // [0, 12, 1, 0, 3, 1, 0, 8, 0]
    // [0, 8, 1, 0, 3, 1, 0, 8, 0]

    // let disable_report = [0u8, 12, 1, 0, 0, 0, 0, 0, 0];
    // kb.send_feature_report(&disable_report).expect("Failed to send feature report");

    // this.Set_Lighting_Effect(2, LM_ITE_RGB.m_save_lighting_data.save_effect, LM_ITE_RGB.m_save_lighting_data.save_light, LM_ITE_RGB.m_save_lighting_data.save_speed, LM_ITE_RGB.m_save_lighting_data.save_direction, 0, LM_ITE_RGB.m_save_lighting_data.save_layout_color);

    // kb.send_feature_report(data: &[u8])

    // loop {
    //     let mut buf = [0u8; 256];
    //     let res = joystick.read(&mut buf[..]).unwrap();

    //     let mut data_string = String::new();

    //     for u in &buf[..res] {
    //         data_string.push_str(&(u.to_string() + "\t"));
    //     }

    //     println!("{}", data_string);
    // }

    // private bool HID_Get_Effect_Type_88H(ref byte Control, ref byte Effect, ref byte Speed, ref byte Light, ref byte ColorIndex, ref byte Direction)
    // {
    //     byte[] array = new byte[9];
    //     array[1] = 136;
    //     byte[] buffer = array;
    //     this.m_HIDManager.WriteFeature(buffer);
    //     Thread.Sleep(1);
    //     byte[] array2 = new byte[9];
    //     this.m_HIDManager.GetFeature(array2);
    //     Thread.Sleep(1);
    //     Control = array2[2];
    //     Effect = array2[3];
    //     Speed = array2[4];
    //     Light = array2[5];
    //     ColorIndex = array2[6];
    //     Direction = array2[7];
    //     return true;
    // }
    // let first_report = [0u8, 136, 0, 0, 0, 0, 0, 0, 0];
    // kb.send_feature_report(&first_report).expect("Failed to send feature report");

    std::thread::sleep(std::time::Duration::from_secs(1));

    // let mut report: [u8; 9] = [0; 9];
    // kb.get_feature_report(&mut report).expect("failed to get feature report");
    // println!("{:?}", report);

    // let disable_report = [0u8, 8, 1, 0, 0, 0, 0, 0, 0];
    // kb.send_feature_report(&disable_report).expect("failed to send disabling report");

    // std::thread::sleep(std::time::Duration::from_secs(10));

    // let second_report = [0u8, 8, 2, 51, 0, report[5], 0, 0, 0];
    // kb.send_feature_report(&second_report).expect("failed to send re-enabling report");

    // [0, 88, 2, 33, 0, 8, 0, 0, 0]
    // Control = array2[2];
    // Effect = array2[3];
    // Speed = array2[4];
    // Light = array2[5];
    // ColorIndex = array2[6];
    // Direction = array2[7];


    // [TRACE]RGBKeyboardView|PowerOn powerOnOff = False userTrigger = False
    // [TRACE]RGBKeyboard_ITE | ILM_RGBKB_SetPower powerstatus = Lighting_off
    // [TRACE]RGBKeyboard_ITE | ILM_RGBKB_SetPower HID_Set_Effect_Type_08H LED_OFF
    // [TRACE]RGBKeyboardView|PowerOn powerOnOff = True userTrigger = False
    // [TRACE]RGBKeyboard_ITE | ILM_RGBKB_SetPower powerstatus = Lighting_on
    // [TRACE]RGBKeyboard_ITE | ILM_RGBKB_SetPower Set_Lighting_Effect effect=1 light =36
    // this.Set_ITE_Effect_Type_StaticMode(light, save, colorBuffer, true);
    //      this.HID_Set_Effect_Type_08H(2, 51, 0, Light, 0, 0, Save);
    //      rgb_S = WKDColor.cheatRGB_2ndME(colorBuffer[0].R, colorBuffer[0].G, colorBuffer[0].B);
            // for (byte b = 1; b < 65; b += 4)
			// 	array[(int)b] = 0;
			// 	if (this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_101 || this.m_ITE_KB_Type == RGBKB_Type.MEZone_2nd_102)
			// 	{
			// 		array[(int)(b + 1)] = rgb_S.R;
			// 		array[(int)(b + 2)] = rgb_S.G;
			// 		array[(int)(b + 3)] = rgb_S.B;
			// 	}
    //		this.HID_Set_Picture_12H(Save);
            // for (byte b2 = 0; b2 < 8; b2 += 1)
			// {
			// 	this.Light_Lock();
			// 	this.m_HIDDevice.Write(array, 0, array.Length);
			// 	Thread.Sleep(1);
			// }
    // let disable_report = [0u8, 8, 2, 0, 0, 0, 0, 0, 0];
    // kb.send_feature_report(&disable_report).expect("Failed to send feature report");


    // this.HID_Set_Effect_Type_08H(2, 51, 0, Light, 0, 0, Save); light is 22, save is 0
    // private bool HID_Set_Picture_12H(byte Saved) < 
            // byte[] array = new byte[9];
			// array[1] = 18;
			// array[4] = 8;
			// array[5] = Saved;
			// byte[] buffer = array;
			// this.m_HIDManager.WriteFeature(buffer);
			// Thread.Sleep(1);
	// 		// return true;
    // byte[] array = new byte[65];
    // array starts: 0x00 0x00 0xFF 0x2A 0x00 (repeat) 0x00 0xff 0x2a 0x00
    // for (byte b2 = 0; b2 < 8; b2 += 1)
    //     this.m_HIDDevice.Write(array, 0, array.Length);

    let brightness = 22u8;
    // let brightness = 10u8;
    let refresh_report = [0u8, 8, 2, 51, 0, brightness, 0, 0, 0];
    kb.send_feature_report(&refresh_report).expect("Failed to send refresh report");

    let set_picture_report = [0u8, 0x12, 0, 0, 8, 0, 0, 0, 0];
    kb.send_feature_report(&set_picture_report).expect("Failed to send set picture report");

    let mut key_array = [0u8; 65];
    for i in (1..65).step_by(4) {
        key_array[i+1] = 0xff;
        key_array[i+2] = 0x2a;
        // key_array[i+1] = 0xff; R
        // key_array[i+2] = 0xff; G
        // key_array[i+3] = 0xff; B
    }

    let array = Array { data: key_array };
    println!("{:x?}", array);
    for _ in 0..8 {
        kb.write(&key_array).expect("Failed to write key data");
    }
}