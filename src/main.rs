extern crate hidapi;

use hidapi::HidApi;

#[repr(u8)]
pub enum FeatureReportOpcode {
    SetEffectType = 0x08,
    GetEffectType = 0x88,
    SetPicture    = 0x12,
    SetColor      = 0x14,
    SetRowIndex   = 0x16,
}

#[repr(u8)]
pub enum EffectType {
    FW      = 0,
	Row     = 1,
	Picture = 2,
	Music   = 3,
	AP      = 4,
}

#[repr(u8)]
pub enum Effect {
    Static    = 1,
	Breathing = 2,
	Reactive  = 4,
	Wave      = 3,
	Rainbow   = 5,
	Ripple    = 6,
	Nomo      = 8,
	Marquee   = 9,
	Raindrop  = 10,
	Stack     = 12,
	Impact    = 13,
	Aurora    = 14,
	Neon      = 15,
	Spark     = 17,
	Flash     = 18,
	Mix       = 19,
	Rippleo   = 22,
	Music     = 34,
	Usermode  = 51,
	Unknown   = 255,
}

#[repr(u8)]
pub enum ControlLED {
    Off = 1,
    Default = 2,
    Welcome = 3,
}

pub struct Color {
    r: u8,
    g: u8,
    b: u8,
}

pub fn _adjust_color(color: Color) -> Color {
    match color {
        Color { r: 255, g: 255, b: 255} => Color { r: 255, g: 180, b: 200},
        Color { r: 243, g: 152, b: 0} => Color { r: 255, g: 42, b: 0},
        Color { r: 241, g: 90, b: 36} => Color { r: 255, g: 25, b: 0},
        Color { r: 247, g: 147, b: 30} => Color { r: 255, g: 42, b: 0},
        Color { r: 255, g: 241, b: 0} => Color { r: 255, g: 180, b: 0},
        Color { r: 0, g: 255, b: 255} => Color { r: 0, g: 180, b: 200},
        Color { r: 138, g: 0, b: 255} => Color { r: 138, g: 0, b: 200},
        Color { r: r@250...255, g: g@0...19, b: b@0...99} => Color { r, g, b: b/10},
        Color { r: r@250...255, g: g@0...19, b: b@100...127} => Color { r, g, b: b-10},
        Color { r: r@0...19, g, b} if b < g => Color { r: r, g, b: 200*b/255 },
        Color { r, g: g@0...19, b} if r < b => Color { r, g, b },
        Color { r, g, b } => Color { r, g: 180*g/255, b: 200*b/255},
    }
}

fn main() {
    let api = HidApi::new().expect("Failed to create API instance");

    let kb = api.open(1165, 52736).expect("Failed to open device");

    let disable_report = [0u8, FeatureReportOpcode::SetEffectType as u8, ControlLED::Off as u8, 0, 0, 0, 0, 0, 0];
    kb.send_feature_report(&disable_report).expect("Failed to send feature report");

    // std::thread::sleep(std::time::Duration::from_secs(1));

    let brightness = 22u8;
    let refresh_report = [
        0u8,
        FeatureReportOpcode::SetEffectType as u8,
        ControlLED::Default as u8,
        Effect::Usermode as u8,
        0, // speed
        brightness,
        0, // colorindex
        0, // direction
        0  // save
    ];
    kb.send_feature_report(&refresh_report).expect("Failed to send refresh report");

    let set_picture_report = [0u8, FeatureReportOpcode::SetPicture as u8, 0, 0, 8, 0, 0, 0, 0];
    kb.send_feature_report(&set_picture_report).expect("Failed to send set picture report");

    let display_color = Color { r: 0xff, g: 0x2a, b: 0 };
    // let color = Color { r: 255, g: 255, b: 255 };
    // let display_color = _adjust_color(color);

    let mut key_array = [0u8; 65];
    for i in (1..65).step_by(4) {
        key_array[i+1] = display_color.r;
        key_array[i+2] = display_color.g;
        key_array[i+3] = display_color.b;
        // key_array[i+1] = 0xff; R
        // key_array[i+2] = 0xff; G
        // key_array[i+3] = 0xff; B
    }
    for _ in 0..8 {
        kb.write(&key_array).expect("Failed to write key data");
    }
}