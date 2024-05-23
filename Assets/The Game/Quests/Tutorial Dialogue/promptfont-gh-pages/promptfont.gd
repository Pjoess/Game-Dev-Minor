# PromptFont by Yukari "Shinmera" Hafner, accessible at https://shinmera.com/promptfont
class_name PromptFont
extends Resource
const ASCII_BANG: StringName = &"!";
const ASCII_BANG_INT: int = 33;
const ASCII_DOUBLEQUOTE: StringName = &"\"";
const ASCII_DOUBLEQUOTE_INT: int = 34;
const ASCII_HASH: StringName = &"#";
const ASCII_HASH_INT: int = 35;
const ASCII_DOLLAR: StringName = &"$";
const ASCII_DOLLAR_INT: int = 36;
const ASCII_PERCENT: StringName = &"%";
const ASCII_PERCENT_INT: int = 37;
const ASCII_AMPERSAND: StringName = &"&";
const ASCII_AMPERSAND_INT: int = 38;
const ASCII_QUOTE: StringName = &"'";
const ASCII_QUOTE_INT: int = 39;
const ASCII_OPEN_PAREN: StringName = &"(";
const ASCII_OPEN_PAREN_INT: int = 40;
const ASCII_CLOSE_PAREN: StringName = &")";
const ASCII_CLOSE_PAREN_INT: int = 41;
const ASCII_STAR: StringName = &"*";
const ASCII_STAR_INT: int = 42;
const ASCII_PLUS: StringName = &"+";
const ASCII_PLUS_INT: int = 43;
const ASCII_COMMA: StringName = &",";
const ASCII_COMMA_INT: int = 44;
const ASCII_DASH: StringName = &"-";
const ASCII_DASH_INT: int = 45;
const ASCII_PERIOD: StringName = &".";
const ASCII_PERIOD_INT: int = 46;
const ASCII_SLASH: StringName = &"/";
const ASCII_SLASH_INT: int = 47;
const ASCII_0: StringName = &"0";
const ASCII_0_INT: int = 48;
const ASCII_1: StringName = &"1";
const ASCII_1_INT: int = 49;
const ASCII_2: StringName = &"2";
const ASCII_2_INT: int = 50;
const ASCII_3: StringName = &"3";
const ASCII_3_INT: int = 51;
const ASCII_4: StringName = &"4";
const ASCII_4_INT: int = 52;
const ASCII_5: StringName = &"5";
const ASCII_5_INT: int = 53;
const ASCII_6: StringName = &"6";
const ASCII_6_INT: int = 54;
const ASCII_7: StringName = &"7";
const ASCII_7_INT: int = 55;
const ASCII_8: StringName = &"8";
const ASCII_8_INT: int = 56;
const ASCII_9: StringName = &"9";
const ASCII_9_INT: int = 57;
const ASCII_COLON: StringName = &":";
const ASCII_COLON_INT: int = 58;
const ASCII_SEMICOLON: StringName = &";";
const ASCII_SEMICOLON_INT: int = 59;
const ASCII_OPEN_CARET: StringName = &"<";
const ASCII_OPEN_CARET_INT: int = 60;
const ASCII_EQUALS: StringName = &"=";
const ASCII_EQUALS_INT: int = 61;
const ASCII_CLOSE_CARET: StringName = &">";
const ASCII_CLOSE_CARET_INT: int = 62;
const ASCII_QUESTION: StringName = &"?";
const ASCII_QUESTION_INT: int = 63;
const ASCII_AT: StringName = &"@";
const ASCII_AT_INT: int = 64;
const ASCII_UPPER_A: StringName = &"A";
const ASCII_UPPER_A_INT: int = 65;
const ASCII_UPPER_B: StringName = &"B";
const ASCII_UPPER_B_INT: int = 66;
const ASCII_UPPER_C: StringName = &"C";
const ASCII_UPPER_C_INT: int = 67;
const ASCII_UPPER_D: StringName = &"D";
const ASCII_UPPER_D_INT: int = 68;
const ASCII_UPPER_E: StringName = &"E";
const ASCII_UPPER_E_INT: int = 69;
const ASCII_UPPER_F: StringName = &"F";
const ASCII_UPPER_F_INT: int = 70;
const ASCII_UPPER_G: StringName = &"G";
const ASCII_UPPER_G_INT: int = 71;
const ASCII_UPPER_H: StringName = &"H";
const ASCII_UPPER_H_INT: int = 72;
const ASCII_UPPER_I: StringName = &"I";
const ASCII_UPPER_I_INT: int = 73;
const ASCII_UPPER_J: StringName = &"J";
const ASCII_UPPER_J_INT: int = 74;
const ASCII_UPPER_K: StringName = &"K";
const ASCII_UPPER_K_INT: int = 75;
const ASCII_UPPER_L: StringName = &"L";
const ASCII_UPPER_L_INT: int = 76;
const ASCII_UPPER_M: StringName = &"M";
const ASCII_UPPER_M_INT: int = 77;
const ASCII_UPPER_N: StringName = &"N";
const ASCII_UPPER_N_INT: int = 78;
const ASCII_UPPER_O: StringName = &"O";
const ASCII_UPPER_O_INT: int = 79;
const ASCII_UPPER_P: StringName = &"P";
const ASCII_UPPER_P_INT: int = 80;
const ASCII_UPPER_Q: StringName = &"Q";
const ASCII_UPPER_Q_INT: int = 81;
const ASCII_UPPER_R: StringName = &"R";
const ASCII_UPPER_R_INT: int = 82;
const ASCII_UPPER_S: StringName = &"S";
const ASCII_UPPER_S_INT: int = 83;
const ASCII_UPPER_T: StringName = &"T";
const ASCII_UPPER_T_INT: int = 84;
const ASCII_UPPER_U: StringName = &"U";
const ASCII_UPPER_U_INT: int = 85;
const ASCII_UPPER_V: StringName = &"V";
const ASCII_UPPER_V_INT: int = 86;
const ASCII_UPPER_W: StringName = &"W";
const ASCII_UPPER_W_INT: int = 87;
const ASCII_UPPER_X: StringName = &"X";
const ASCII_UPPER_X_INT: int = 88;
const ASCII_UPPER_Y: StringName = &"Y";
const ASCII_UPPER_Y_INT: int = 89;
const ASCII_UPPER_Z: StringName = &"Z";
const ASCII_UPPER_Z_INT: int = 90;
const ASCII_OPEN_BRACKET: StringName = &"[";
const ASCII_OPEN_BRACKET_INT: int = 91;
const ASCII_BACKSLASH: StringName = &"\\";
const ASCII_BACKSLASH_INT: int = 92;
const ASCII_CLOSE_BRACKET: StringName = &"]";
const ASCII_CLOSE_BRACKET_INT: int = 93;
const ASCII_CARET: StringName = &"^";
const ASCII_CARET_INT: int = 94;
const ASCII_UNDERSCORE: StringName = &"_";
const ASCII_UNDERSCORE_INT: int = 95;
const ASCII_BACKTICK: StringName = &"`";
const ASCII_BACKTICK_INT: int = 96;
const ASCII_LOWER_A: StringName = &"a";
const ASCII_LOWER_A_INT: int = 97;
const ASCII_LOWER_B: StringName = &"b";
const ASCII_LOWER_B_INT: int = 98;
const ASCII_LOWER_C: StringName = &"c";
const ASCII_LOWER_C_INT: int = 99;
const ASCII_LOWER_D: StringName = &"d";
const ASCII_LOWER_D_INT: int = 100;
const ASCII_LOWER_E: StringName = &"e";
const ASCII_LOWER_E_INT: int = 101;
const ASCII_LOWER_F: StringName = &"f";
const ASCII_LOWER_F_INT: int = 102;
const ASCII_LOWER_G: StringName = &"g";
const ASCII_LOWER_G_INT: int = 103;
const ASCII_LOWER_H: StringName = &"h";
const ASCII_LOWER_H_INT: int = 104;
const ASCII_LOWER_I: StringName = &"i";
const ASCII_LOWER_I_INT: int = 105;
const ASCII_LOWER_J: StringName = &"j";
const ASCII_LOWER_J_INT: int = 106;
const ASCII_LOWER_K: StringName = &"k";
const ASCII_LOWER_K_INT: int = 107;
const ASCII_LOWER_L: StringName = &"l";
const ASCII_LOWER_L_INT: int = 108;
const ASCII_LOWER_M: StringName = &"m";
const ASCII_LOWER_M_INT: int = 109;
const ASCII_LOWER_N: StringName = &"n";
const ASCII_LOWER_N_INT: int = 110;
const ASCII_LOWER_O: StringName = &"o";
const ASCII_LOWER_O_INT: int = 111;
const ASCII_LOWER_P: StringName = &"p";
const ASCII_LOWER_P_INT: int = 112;
const ASCII_LOWER_Q: StringName = &"q";
const ASCII_LOWER_Q_INT: int = 113;
const ASCII_LOWER_R: StringName = &"r";
const ASCII_LOWER_R_INT: int = 114;
const ASCII_LOWER_S: StringName = &"s";
const ASCII_LOWER_S_INT: int = 115;
const ASCII_LOWER_T: StringName = &"t";
const ASCII_LOWER_T_INT: int = 116;
const ASCII_LOWER_U: StringName = &"u";
const ASCII_LOWER_U_INT: int = 117;
const ASCII_LOWER_V: StringName = &"v";
const ASCII_LOWER_V_INT: int = 118;
const ASCII_LOWER_W: StringName = &"w";
const ASCII_LOWER_W_INT: int = 119;
const ASCII_LOWER_X: StringName = &"x";
const ASCII_LOWER_X_INT: int = 120;
const ASCII_LOWER_Y: StringName = &"y";
const ASCII_LOWER_Y_INT: int = 121;
const ASCII_LOWER_Z: StringName = &"z";
const ASCII_LOWER_Z_INT: int = 122;
const ASCII_OPEN_BRACE: StringName = &"{";
const ASCII_OPEN_BRACE_INT: int = 123;
const ASCII_BAR: StringName = &"|";
const ASCII_BAR_INT: int = 124;
const ASCII_CLOSE_BRACE: StringName = &"}";
const ASCII_CLOSE_BRACE_INT: int = 125;
const ASCII_TILDE: StringName = &"~";
const ASCII_TILDE_INT: int = 126;
const ICON_EXCHANGE: StringName = &"↔";
const ICON_EXCHANGE_INT: int = 8596;
const ICON_REVERSE: StringName = &"↕";
const ICON_REVERSE_INT: int = 8597;
const XBOX_LEFT_TRIGGER: StringName = &"↖";
const XBOX_LEFT_TRIGGER_INT: int = 8598;
const XBOX_RIGHT_TRIGGER: StringName = &"↗";
const XBOX_RIGHT_TRIGGER_INT: int = 8599;
const XBOX_LEFT_SHOULDER: StringName = &"↘";
const XBOX_LEFT_SHOULDER_INT: int = 8600;
const XBOX_RIGHT_SHOULDER: StringName = &"↙";
const XBOX_RIGHT_SHOULDER_INT: int = 8601;
const NINTENDO_LEFT_TRIGGER: StringName = &"↚";
const NINTENDO_LEFT_TRIGGER_INT: int = 8602;
const NINTENDO_RIGHT_TRIGGER: StringName = &"↛";
const NINTENDO_RIGHT_TRIGGER_INT: int = 8603;
const NINTENDO_LEFT_SHOULDER: StringName = &"↜";
const NINTENDO_LEFT_SHOULDER_INT: int = 8604;
const NINTENDO_RIGHT_SHOULDER: StringName = &"↝";
const NINTENDO_RIGHT_SHOULDER_INT: int = 8605;
const DPAD_LEFT: StringName = &"↞";
const DPAD_LEFT_INT: int = 8606;
const DPAD_UP: StringName = &"↟";
const DPAD_UP_INT: int = 8607;
const DPAD_RIGHT: StringName = &"↠";
const DPAD_RIGHT_INT: int = 8608;
const DPAD_DOWN: StringName = &"↡";
const DPAD_DOWN_INT: int = 8609;
const DPAD_LEFT_RIGHT: StringName = &"↢";
const DPAD_LEFT_RIGHT_INT: int = 8610;
const DPAD_UP_DOWN: StringName = &"↣";
const DPAD_UP_DOWN_INT: int = 8611;
const GAMEPAD_X: StringName = &"↤";
const GAMEPAD_X_INT: int = 8612;
const GAMEPAD_Y: StringName = &"↥";
const GAMEPAD_Y_INT: int = 8613;
const GAMEPAD_B: StringName = &"↦";
const GAMEPAD_B_INT: int = 8614;
const GAMEPAD_A: StringName = &"↧";
const GAMEPAD_A_INT: int = 8615;
const ANALOG_L_CLOCKWISE: StringName = &"↩";
const ANALOG_L_CLOCKWISE_INT: int = 8617;
const ANALOG_L_COUNTER: StringName = &"↪";
const ANALOG_L_COUNTER_INT: int = 8618;
const ANALOG_R_CLOCKWISE: StringName = &"↫";
const ANALOG_R_CLOCKWISE_INT: int = 8619;
const ANALOG_R_COUNTER: StringName = &"↬";
const ANALOG_R_COUNTER_INT: int = 8620;
const ANALOG_LR_BLOCKWISE: StringName = &"↭";
const ANALOG_LR_BLOCKWISE_INT: int = 8621;
const ANALOG_LR_COUNTER: StringName = &"↮";
const ANALOG_LR_COUNTER_INT: int = 8622;
const SONY_LEFT_SHOULDER: StringName = &"↰";
const SONY_LEFT_SHOULDER_INT: int = 8624;
const SONY_RIGHT_SHOULDER: StringName = &"↱";
const SONY_RIGHT_SHOULDER_INT: int = 8625;
const SONY_LEFT_TRIGGER: StringName = &"↲";
const SONY_LEFT_TRIGGER_INT: int = 8626;
const SONY_RIGHT_TRIGGER: StringName = &"↳";
const SONY_RIGHT_TRIGGER_INT: int = 8627;
const DPAD_LEFT_DOWN: StringName = &"↴";
const DPAD_LEFT_DOWN_INT: int = 8628;
const GAMEPAD_UP_RIGHT: StringName = &"↵";
const GAMEPAD_UP_RIGHT_INT: int = 8629;
const ANALOG_CLOCKWISE: StringName = &"↶";
const ANALOG_CLOCKWISE_INT: int = 8630;
const ANALOG_COUNTER: StringName = &"↷";
const ANALOG_COUNTER_INT: int = 8631;
const ANALOG_CLICK: StringName = &"↹";
const ANALOG_CLICK_INT: int = 8633;
const ANALOG_L_CLICK: StringName = &"↺";
const ANALOG_L_CLICK_INT: int = 8634;
const ANALOG_R_CLICK: StringName = &"↻";
const ANALOG_R_CLICK_INT: int = 8635;
const ANALOG_L_LEFT: StringName = &"↼";
const ANALOG_L_LEFT_INT: int = 8636;
const ANALOG_R_LEFT: StringName = &"↽";
const ANALOG_R_LEFT_INT: int = 8637;
const ANALOG_L_UP: StringName = &"↾";
const ANALOG_L_UP_INT: int = 8638;
const ANALOG_R_UP: StringName = &"↿";
const ANALOG_R_UP_INT: int = 8639;
const ANALOG_L_RIGHT: StringName = &"⇀";
const ANALOG_L_RIGHT_INT: int = 8640;
const ANALOG_R_RIGHT: StringName = &"⇁";
const ANALOG_R_RIGHT_INT: int = 8641;
const ANALOG_L_DOWN: StringName = &"⇂";
const ANALOG_L_DOWN_INT: int = 8642;
const ANALOG_R_DOWN: StringName = &"⇃";
const ANALOG_R_DOWN_INT: int = 8643;
const ANALOG_L_LEFT_RIGHT: StringName = &"⇄";
const ANALOG_L_LEFT_RIGHT_INT: int = 8644;
const ANALOG_L_UP_DOWN: StringName = &"⇅";
const ANALOG_L_UP_DOWN_INT: int = 8645;
const ANALOG_R_LEFT_RIGHT: StringName = &"⇆";
const ANALOG_R_LEFT_RIGHT_INT: int = 8646;
const ANALOG_LEFT: StringName = &"⇇";
const ANALOG_LEFT_INT: int = 8647;
const ANALOG_UP: StringName = &"⇈";
const ANALOG_UP_INT: int = 8648;
const ANALOG_RIGHT: StringName = &"⇉";
const ANALOG_RIGHT_INT: int = 8649;
const ANALOG_DOWN: StringName = &"⇊";
const ANALOG_DOWN_INT: int = 8650;
const ANALOG_L: StringName = &"⇋";
const ANALOG_L_INT: int = 8651;
const ANALOG_R: StringName = &"⇌";
const ANALOG_R_INT: int = 8652;
const DPAD: StringName = &"⇎";
const DPAD_INT: int = 8654;
const XBOX_X: StringName = &"⇐";
const XBOX_X_INT: int = 8656;
const XBOX_Y: StringName = &"⇑";
const XBOX_Y_INT: int = 8657;
const XBOX_B: StringName = &"⇒";
const XBOX_B_INT: int = 8658;
const XBOX_A: StringName = &"⇓";
const XBOX_A_INT: int = 8659;
const ANALOG_LEFT_RIGHT: StringName = &"⇔";
const ANALOG_LEFT_RIGHT_INT: int = 8660;
const ANALOG_UP_DOWN: StringName = &"⇕";
const ANALOG_UP_DOWN_INT: int = 8661;
const ANALOG_UP_LEFT: StringName = &"⇖";
const ANALOG_UP_LEFT_INT: int = 8662;
const ANALOG_UP_RIGHT: StringName = &"⇗";
const ANALOG_UP_RIGHT_INT: int = 8663;
const ANALOG_DOWN_RIGHT: StringName = &"⇘";
const ANALOG_DOWN_RIGHT_INT: int = 8664;
const ANALOG_DOWN_LEFT: StringName = &"⇙";
const ANALOG_DOWN_LEFT_INT: int = 8665;
const ANALOG_L_TOUCH: StringName = &"⇚";
const ANALOG_L_TOUCH_INT: int = 8666;
const ANALOG_R_TOUCH: StringName = &"⇛";
const ANALOG_R_TOUCH_INT: int = 8667;
const XBOX_LEFT_TRIGGER_PULL: StringName = &"⇜";
const XBOX_LEFT_TRIGGER_PULL_INT: int = 8668;
const XBOX_RIGHT_TRIGGER_PULL: StringName = &"⇝";
const XBOX_RIGHT_TRIGGER_PULL_INT: int = 8669;
const DPAD_RIGHT_DOWN: StringName = &"⇞";
const DPAD_RIGHT_DOWN_INT: int = 8670;
const DUPAD_LEFT_UP: StringName = &"⇟";
const DUPAD_LEFT_UP_INT: int = 8671;
const SONY_X: StringName = &"⇠";
const SONY_X_INT: int = 8672;
const SONY_Y: StringName = &"⇡";
const SONY_Y_INT: int = 8673;
const SONY_B: StringName = &"⇢";
const SONY_B_INT: int = 8674;
const SONY_A: StringName = &"⇣";
const SONY_A_INT: int = 8675;
const STEAM_MENU: StringName = &"⇤";
const STEAM_MENU_INT: int = 8676;
const STEAM_OPTIONS: StringName = &"⇥";
const STEAM_OPTIONS_INT: int = 8677;
const SONY_SHARE: StringName = &"⇦";
const SONY_SHARE_INT: int = 8678;
const SONY_TOUCHPAD: StringName = &"⇧";
const SONY_TOUCHPAD_INT: int = 8679;
const SONY_OPTIONS: StringName = &"⇨";
const SONY_OPTIONS_INT: int = 8680;
const NINTENDO_BUTTON_Z: StringName = &"⇩";
const NINTENDO_BUTTON_Z_INT: int = 8681;
const NINTENDO_TRIGGER_Z: StringName = &"⇪";
const NINTENDO_TRIGGER_Z_INT: int = 8682;
const NINTENDO_C: StringName = &"⇫";
const NINTENDO_C_INT: int = 8683;
const NINTENDO_Z: StringName = &"⇬";
const NINTENDO_Z_INT: int = 8684;
const NINTENDO_1: StringName = &"⇭";
const NINTENDO_1_INT: int = 8685;
const NINTENDO_2: StringName = &"⇮";
const NINTENDO_2_INT: int = 8686;
const DPAD_RIGHT_UP: StringName = &"⇯";
const DPAD_RIGHT_UP_INT: int = 8687;
const DPAD_LEFT_DOWN: StringName = &"⇰";
const DPAD_LEFT_DOWN_INT: int = 8688;
const ANALOG_L_ANY: StringName = &"⇱";
const ANALOG_L_ANY_INT: int = 8689;
const ANALOG_R_ANY: StringName = &"⇲";
const ANALOG_R_ANY_INT: int = 8690;
const ANALOG_ANY: StringName = &"⇳";
const ANALOG_ANY_INT: int = 8691;
const ANALOG_R_UP_DOWN: StringName = &"⇵";
const ANALOG_R_UP_DOWN_INT: int = 8693;
const GAMEPAD_SELECT: StringName = &"⇷";
const GAMEPAD_SELECT_INT: int = 8695;
const GAMEPAD_START: StringName = &"⇸";
const GAMEPAD_START_INT: int = 8696;
const GAMEPAD_HOME: StringName = &"⇹";
const GAMEPAD_HOME_INT: int = 8697;
const XBOX_VIEW: StringName = &"⇺";
const XBOX_VIEW_INT: int = 8698;
const XBOX_MENU: StringName = &"⇻";
const XBOX_MENU_INT: int = 8699;
const NINTENDO_MINUS: StringName = &"⇽";
const NINTENDO_MINUS_INT: int = 8701;
const NINTENDO_PLUS: StringName = &"⇾";
const NINTENDO_PLUS_INT: int = 8702;
const NINTENDO_DPAD_LEFT: StringName = &"⇿";
const NINTENDO_DPAD_LEFT_INT: int = 8703;
const NINTENDO_DPAD_UP: StringName = &"∀";
const NINTENDO_DPAD_UP_INT: int = 8704;
const NINTENDO_DPAD_RIGHT: StringName = &"∁";
const NINTENDO_DPAD_RIGHT_INT: int = 8705;
const NINTENDO_DPAD_DOWN: StringName = &"∂";
const NINTENDO_DPAD_DOWN_INT: int = 8706;
const NINTENDO_JOYCON_SL: StringName = &"∃";
const NINTENDO_JOYCON_SL_INT: int = 8707;
const NINTENDO_JOYCON_SR: StringName = &"∄";
const NINTENDO_JOYCON_SR_INT: int = 8708;
const LEGION_SETTINGS: StringName = &"∅";
const LEGION_SETTINGS_INT: int = 8709;
const SONY_DUALSENSE_SHARE: StringName = &"∆";
const SONY_DUALSENSE_SHARE_INT: int = 8710;
const SONY_DUALSENSE_TOUCHPAD: StringName = &"∇";
const SONY_DUALSENSE_TOUCHPAD_INT: int = 8711;
const SONY_DUALSENSE_OPTIONS: StringName = &"∈";
const SONY_DUALSENSE_OPTIONS_INT: int = 8712;
const AYANEO_LC: StringName = &"∉";
const AYANEO_LC_INT: int = 8713;
const AYANEO_RC: StringName = &"∊";
const AYANEO_RC_INT: int = 8714;
const AYANEO_WAVE: StringName = &"∋";
const AYANEO_WAVE_INT: int = 8715;
const AYANEO_HOME: StringName = &"∌";
const AYANEO_HOME_INT: int = 8716;
const AYANEO_LCC: StringName = &"∍";
const AYANEO_LCC_INT: int = 8717;
const GPD_C1: StringName = &"∎";
const GPD_C1_INT: int = 8718;
const GPD_C2: StringName = &"∏";
const GPD_C2_INT: int = 8719;
const ONEXPLAYER_KEYBOARD: StringName = &"∐";
const ONEXPLAYER_KEYBOARD_INT: int = 8720;
const ONEXPLAYER_TURBO: StringName = &"∑";
const ONEXPLAYER_TURBO_INT: int = 8721;
const GAMEPAD_M1: StringName = &"−";
const GAMEPAD_M1_INT: int = 8722;
const GAMEPAD_M2: StringName = &"∓";
const GAMEPAD_M2_INT: int = 8723;
const GAMEPAD_M3: StringName = &"∔";
const GAMEPAD_M3_INT: int = 8724;
const GAMEPAD_Y1: StringName = &"∕";
const GAMEPAD_Y1_INT: int = 8725;
const GAMEPAD_Y2: StringName = &"∖";
const GAMEPAD_Y2_INT: int = 8726;
const GAMEPAD_Y3: StringName = &"∗";
const GAMEPAD_Y3_INT: int = 8727;
const ONEXPLAYER_FUNCTION: StringName = &"∘";
const ONEXPLAYER_FUNCTION_INT: int = 8728;
const ONEXPLAYER_HOME: StringName = &"∙";
const ONEXPLAYER_HOME_INT: int = 8729;
const GAMEPAD_X_B: StringName = &"∥";
const GAMEPAD_X_B_INT: int = 8741;
const GAMEPAD_A_Y: StringName = &"∦";
const GAMEPAD_A_Y_INT: int = 8742;
const GAMEPAD_X_Y: StringName = &"∧";
const GAMEPAD_X_Y_INT: int = 8743;
const GAMEPAD_B_Y: StringName = &"∨";
const GAMEPAD_B_Y_INT: int = 8744;
const GAMEPAD_A_B: StringName = &"∩";
const GAMEPAD_A_B_INT: int = 8745;
const GAMEPAD_X_A: StringName = &"∪";
const GAMEPAD_X_A_INT: int = 8746;
const TRACKPAD_L_ANY: StringName = &"≤";
const TRACKPAD_L_ANY_INT: int = 8804;
const TRACKPAD_R_ANY: StringName = &"≥";
const TRACKPAD_R_ANY_INT: int = 8805;
const TRACKPAD_L_CLICK: StringName = &"≦";
const TRACKPAD_L_CLICK_INT: int = 8806;
const TRACKPAD_R_CLICK: StringName = &"≧";
const TRACKPAD_R_CLICK_INT: int = 8807;
const TRACKPAD_L_TOUCH: StringName = &"≨";
const TRACKPAD_L_TOUCH_INT: int = 8808;
const TRACKPAD_R_TOUCH: StringName = &"≩";
const TRACKPAD_R_TOUCH_INT: int = 8809;
const TRACKPAD_L_LEFT: StringName = &"≮";
const TRACKPAD_L_LEFT_INT: int = 8814;
const TRACKPAD_R_LEFT: StringName = &"≯";
const TRACKPAD_R_LEFT_INT: int = 8815;
const TRACKPAD_L_UP: StringName = &"≰";
const TRACKPAD_L_UP_INT: int = 8816;
const TRACKPAD_R_UP: StringName = &"≱";
const TRACKPAD_R_UP_INT: int = 8817;
const TRACKPAD_L_RIGHT: StringName = &"≲";
const TRACKPAD_L_RIGHT_INT: int = 8818;
const TRACKPAD_R_RIGHT: StringName = &"≳";
const TRACKPAD_R_RIGHT_INT: int = 8819;
const TRACKPAD_L_DOWN: StringName = &"≴";
const TRACKPAD_L_DOWN_INT: int = 8820;
const TRACKPAD_R_DOWN: StringName = &"≵";
const TRACKPAD_R_DOWN_INT: int = 8821;
const GAMEPAD_L4: StringName = &"≶";
const GAMEPAD_L4_INT: int = 8822;
const GAMEPAD_R4: StringName = &"≷";
const GAMEPAD_R4_INT: int = 8823;
const GAMEPAD_L5: StringName = &"≸";
const GAMEPAD_L5_INT: int = 8824;
const GAMEPAD_R5: StringName = &"≹";
const GAMEPAD_R5_INT: int = 8825;
const XBOX_DPAD_LEFT: StringName = &"≺";
const XBOX_DPAD_LEFT_INT: int = 8826;
const XBOX_DPAD_UP: StringName = &"≻";
const XBOX_DPAD_UP_INT: int = 8827;
const XBOX_DPAD_RIGHT: StringName = &"≼";
const XBOX_DPAD_RIGHT_INT: int = 8828;
const XBOX_DPAD_DOWN: StringName = &"≽";
const XBOX_DPAD_DOWN_INT: int = 8829;
const XBOX_DEPAD_LEFT_RIGHT: StringName = &"≾";
const XBOX_DEPAD_LEFT_RIGHT_INT: int = 8830;
const XBOX_DPAD_UP_DOWN: StringName = &"≿";
const XBOX_DPAD_UP_DOWN_INT: int = 8831;
const XBOX_DPAD_LEFT_UP: StringName = &"⊀";
const XBOX_DPAD_LEFT_UP_INT: int = 8832;
const XBOX_DPAD_RIGHT_UP: StringName = &"⊁";
const XBOX_DPAD_RIGHT_UP_INT: int = 8833;
const XBOX_DPAD_LEFT_DOWN: StringName = &"⊂";
const XBOX_DPAD_LEFT_DOWN_INT: int = 8834;
const XBOX_DPAD_RIGHT_DOWN: StringName = &"⊃";
const XBOX_DPAD_RIGHT_DOWN_INT: int = 8835;
const XBOX_DPAD: StringName = &"⊄";
const XBOX_DPAD_INT: int = 8836;
const ICON_PIN: StringName = &"⌖";
const ICON_PIN_INT: int = 8982;
const ANDROID_TABS: StringName = &"⏍";
const ANDROID_TABS_INT: int = 9165;
const ANDROID_BACK: StringName = &"⏎";
const ANDROID_BACK_INT: int = 9166;
const ANDROID_HOME: StringName = &"⏏";
const ANDROID_HOME_INT: int = 9167;
const ANDROID_HORIZONTAL_DOTS: StringName = &"⏐";
const ANDROID_HORIZONTAL_DOTS_INT: int = 9168;
const ANDROID_VERTICAL_DOTS: StringName = &"⏑";
const ANDROID_VERTICAL_DOTS_INT: int = 9169;
const ANDROID_HAMBURGER_MENU: StringName = &"⏒";
const ANDROID_HAMBURGER_MENU_INT: int = 9170;
const KEYBOARD_LEFT: StringName = &"⏴";
const KEYBOARD_LEFT_INT: int = 9204;
const KEYBOARD_UP: StringName = &"⏵";
const KEYBOARD_UP_INT: int = 9205;
const KEYBOARD_RIGHT: StringName = &"⏶";
const KEYBOARD_RIGHT_INT: int = 9206;
const KEYBOARD_DOWN: StringName = &"⏷";
const KEYBOARD_DOWN_INT: int = 9207;
const KEYBOARD_WASD: StringName = &"␣";
const KEYBOARD_WASD_INT: int = 9251;
const KEYBOARD_ARROWS: StringName = &"␤";
const KEYBOARD_ARROWS_INT: int = 9252;
const KEYBOARD_IJKL: StringName = &"␥";
const KEYBOARD_IJKL_INT: int = 9253;
const KEYBOARD_FN: StringName = &"␦";
const KEYBOARD_FN_INT: int = 9254;
const KEYBOARD_CONTROL: StringName = &"␧";
const KEYBOARD_CONTROL_INT: int = 9255;
const KEYBOARD_ALT: StringName = &"␨";
const KEYBOARD_ALT_INT: int = 9256;
const KEYBOARD_SHIFT: StringName = &"␩";
const KEYBOARD_SHIFT_INT: int = 9257;
const KEYBOARD_SUPER: StringName = &"␪";
const KEYBOARD_SUPER_INT: int = 9258;
const KEYBOARD_TAB: StringName = &"␫";
const KEYBOARD_TAB_INT: int = 9259;
const KEYBOARD_CAPS: StringName = &"␬";
const KEYBOARD_CAPS_INT: int = 9260;
const KEYBOARD_BACKSPACE: StringName = &"␭";
const KEYBOARD_BACKSPACE_INT: int = 9261;
const KEYBOARD_ENTER: StringName = &"␮";
const KEYBOARD_ENTER_INT: int = 9262;
const KEYBOARD_ESCAPE: StringName = &"␯";
const KEYBOARD_ESCAPE_INT: int = 9263;
const KEYBOARD_PRINT_SCREEN: StringName = &"␰";
const KEYBOARD_PRINT_SCREEN_INT: int = 9264;
const KEYBOARD_SCROLL_LOCK: StringName = &"␱";
const KEYBOARD_SCROLL_LOCK_INT: int = 9265;
const KEYBOARD_PAUSE: StringName = &"␲";
const KEYBOARD_PAUSE_INT: int = 9266;
const KEYBOARD_NUM_LOCK: StringName = &"␳";
const KEYBOARD_NUM_LOCK_INT: int = 9267;
const KEYBOARD_INSERT: StringName = &"␴";
const KEYBOARD_INSERT_INT: int = 9268;
const KEYBOARD_HOME: StringName = &"␵";
const KEYBOARD_HOME_INT: int = 9269;
const KEYBOARD_PAGE_UP: StringName = &"␶";
const KEYBOARD_PAGE_UP_INT: int = 9270;
const KEYBOARD_DELETE: StringName = &"␷";
const KEYBOARD_DELETE_INT: int = 9271;
const KEYBOARD_END: StringName = &"␸";
const KEYBOARD_END_INT: int = 9272;
const KEYBOARD_PAGE_DOWN: StringName = &"␹";
const KEYBOARD_PAGE_DOWN_INT: int = 9273;
const KEYBOARD_SPACE: StringName = &"␺";
const KEYBOARD_SPACE_INT: int = 9274;
const DEVICE_GAMEPAD: StringName = &"␼";
const DEVICE_GAMEPAD_INT: int = 9276;
const DEVICE_KEYBOARD: StringName = &"␽";
const DEVICE_KEYBOARD_INT: int = 9277;
const DEVICE_MOUSE: StringName = &"␾";
const DEVICE_MOUSE_INT: int = 9278;
const DEVICE_MOUSE_KEYBOARD: StringName = &"␿";
const DEVICE_MOUSE_KEYBOARD_INT: int = 9279;
const KEYBOARD_F1: StringName = &"①";
const KEYBOARD_F1_INT: int = 9312;
const KEYBOARD_F2: StringName = &"②";
const KEYBOARD_F2_INT: int = 9313;
const KEYBOARD_F3: StringName = &"③";
const KEYBOARD_F3_INT: int = 9314;
const KEYBOARD_F4: StringName = &"④";
const KEYBOARD_F4_INT: int = 9315;
const KEYBOARD_F5: StringName = &"⑤";
const KEYBOARD_F5_INT: int = 9316;
const KEYBOARD_F6: StringName = &"⑥";
const KEYBOARD_F6_INT: int = 9317;
const KEYBOARD_F7: StringName = &"⑦";
const KEYBOARD_F7_INT: int = 9318;
const KEYBOARD_F8: StringName = &"⑧";
const KEYBOARD_F8_INT: int = 9319;
const KEYBOARD_F9: StringName = &"⑨";
const KEYBOARD_F9_INT: int = 9320;
const KEYBOARD_F10: StringName = &"⑩";
const KEYBOARD_F10_INT: int = 9321;
const KEYBOARD_F11: StringName = &"⑪";
const KEYBOARD_F11_INT: int = 9322;
const KEYBOARD_F12: StringName = &"⑫";
const KEYBOARD_F12_INT: int = 9323;
const KEYBOARD_KEY: StringName = &"⒏";
const KEYBOARD_KEY_INT: int = 9359;
const ICON_1: StringName = &"⓵";
const ICON_1_INT: int = 9461;
const ICON_2: StringName = &"⓶";
const ICON_2_INT: int = 9462;
const ICON_3: StringName = &"⓷";
const ICON_3_INT: int = 9463;
const ICON_4: StringName = &"⓸";
const ICON_4_INT: int = 9464;
const ICON_5: StringName = &"⓹";
const ICON_5_INT: int = 9465;
const ICON_6: StringName = &"⓺";
const ICON_6_INT: int = 9466;
const ICON_7: StringName = &"⓻";
const ICON_7_INT: int = 9467;
const ICON_8: StringName = &"⓼";
const ICON_8_INT: int = 9468;
const ICON_9: StringName = &"⓽";
const ICON_9_INT: int = 9469;
const ICON_0: StringName = &"⓿";
const ICON_0_INT: int = 9471;
const ICON_STAR: StringName = &"★";
const ICON_STAR_INT: int = 9733;
const ICON_EMPTY_STAR: StringName = &"☆";
const ICON_EMPTY_STAR_INT: int = 9734;
const ICON_SKULL: StringName = &"☠";
const ICON_SKULL_INT: int = 9760;
const ICON_FROWN: StringName = &"☹";
const ICON_FROWN_INT: int = 9785;
const ICON_SMILE: StringName = &"☺";
const ICON_SMILE_INT: int = 9786;
const ICON_FULL_SPADE: StringName = &"♠";
const ICON_FULL_SPADE_INT: int = 9824;
const ICON_EMPTY_HEART: StringName = &"♡";
const ICON_EMPTY_HEART_INT: int = 9825;
const ICON_EMPTY_DIAMOND: StringName = &"♢";
const ICON_EMPTY_DIAMOND_INT: int = 9826;
const ICON_FULL_CLUB: StringName = &"♣";
const ICON_FULL_CLUB_INT: int = 9827;
const ICON_EMPTY_SPADE: StringName = &"♤";
const ICON_EMPTY_SPADE_INT: int = 9828;
const ICON_FULL_HEART: StringName = &"♥";
const ICON_FULL_HEART_INT: int = 9829;
const ICON_FULL_DIAMOND: StringName = &"♦";
const ICON_FULL_DIAMOND_INT: int = 9830;
const ICON_EMPTY_CLUB: StringName = &"♧";
const ICON_EMPTY_CLUB_INT: int = 9831;
const ICON_D4: StringName = &"♳";
const ICON_D4_INT: int = 9843;
const ICON_D6: StringName = &"♴";
const ICON_D6_INT: int = 9844;
const ICON_D8: StringName = &"♵";
const ICON_D8_INT: int = 9845;
const ICON_D10: StringName = &"♶";
const ICON_D10_INT: int = 9846;
const ICON_D12: StringName = &"♷";
const ICON_D12_INT: int = 9847;
const ICON_D20: StringName = &"♸";
const ICON_D20_INT: int = 9848;
const ICON_D6_1: StringName = &"⚀";
const ICON_D6_1_INT: int = 9856;
const ICON_D6_2: StringName = &"⚁";
const ICON_D6_2_INT: int = 9857;
const ICON_D6_3: StringName = &"⚂";
const ICON_D6_3_INT: int = 9858;
const ICON_D6_4: StringName = &"⚃";
const ICON_D6_4_INT: int = 9859;
const ICON_D6_5: StringName = &"⚄";
const ICON_D6_5_INT: int = 9860;
const ICON_D6_6: StringName = &"⚅";
const ICON_D6_6_INT: int = 9861;
const ICON_FLAG: StringName = &"⚑";
const ICON_FLAG_INT: int = 9873;
const ICON_GEARS: StringName = &"⚙";
const ICON_GEARS_INT: int = 9881;
const ICON_CROSS: StringName = &"✗";
const ICON_CROSS_INT: int = 10007;
const ICON_QUESTION: StringName = &"❓";
const ICON_QUESTION_INT: int = 10067;
const ICON_BANG: StringName = &"❗";
const ICON_BANG_INT: int = 10071;
const MOUSE_1: StringName = &"➊";
const MOUSE_1_INT: int = 10122;
const MOUSE_2: StringName = &"➋";
const MOUSE_2_INT: int = 10123;
const MOUSE_3: StringName = &"➌";
const MOUSE_3_INT: int = 10124;
const MOUSE_4: StringName = &"➍";
const MOUSE_4_INT: int = 10125;
const MOUSE_5: StringName = &"➎";
const MOUSE_5_INT: int = 10126;
const MOUSE_6: StringName = &"➏";
const MOUSE_6_INT: int = 10127;
const MOUSE_7: StringName = &"➐";
const MOUSE_7_INT: int = 10128;
const MOUSE_8: StringName = &"➑";
const MOUSE_8_INT: int = 10129;
const MOUSE_SCROLL_UP: StringName = &"⟰";
const MOUSE_SCROLL_UP_INT: int = 10224;
const MOUSE_SCROLL_DOWN: StringName = &"⟱";
const MOUSE_SCROLL_DOWN_INT: int = 10225;
const MOUSE_LEFT: StringName = &"⟵";
const MOUSE_LEFT_INT: int = 10229;
const MOUSE_RIGHT: StringName = &"⟶";
const MOUSE_RIGHT_INT: int = 10230;
const MOUSE_MIDDLE: StringName = &"⟷";
const MOUSE_MIDDLE_INT: int = 10231;
const MOUSE_LEFT_RIGHT: StringName = &"⟺";
const MOUSE_LEFT_RIGHT_INT: int = 10234;
const MOUSE_UP_DOWN: StringName = &"⟻";
const MOUSE_UP_DOWN_INT: int = 10235;
const MOUSE_ANY: StringName = &"⟼";
const MOUSE_ANY_INT: int = 10236;
const ICON_BOX: StringName = &"⬛";
const ICON_BOX_INT: int = 11035;
const ICON_PLAYSTATION: StringName = &"";
const ICON_PLAYSTATION_INT: int = 57344;
const ICON_XBOX: StringName = &"";
const ICON_XBOX_INT: int = 57345;
const ICON_NINTENDO_SWITCH: StringName = &"";
const ICON_NINTENDO_SWITCH_INT: int = 57346;
const ICON_AYANEO: StringName = &"";
const ICON_AYANEO_INT: int = 57347;
const ICON_LENOVO_LEGION: StringName = &"";
const ICON_LENOVO_LEGION_INT: int = 57348;
const ROG_ALLY_ARMOURY: StringName = &"";
const ROG_ALLY_ARMOURY_INT: int = 57349;
const ROG_ALLY_COMMAND: StringName = &"";
const ROG_ALLY_COMMAND_INT: int = 57350;
const ICON_MAC: StringName = &"";
const ICON_MAC_INT: int = 57351;
const ICON_WINDOWS: StringName = &"";
const ICON_WINDOWS_INT: int = 57352;
const ICON_LINUX: StringName = &"";
const ICON_LINUX_INT: int = 57353;
const ICON_BSD: StringName = &"";
const ICON_BSD_INT: int = 57354;
const ICON_STEAM: StringName = &"";
const ICON_STEAM_INT: int = 57355;
const ICON_ITCH_IO: StringName = &"";
const ICON_ITCH_IO_INT: int = 57356;
const ICON_HUMBLE: StringName = &"";
const ICON_HUMBLE_INT: int = 57357;
const ICON_EPIC_GAME_STORE: StringName = &"";
const ICON_EPIC_GAME_STORE_INT: int = 57358;
const ICON_GOOD_OLD_GAMES: StringName = &"";
const ICON_GOOD_OLD_GAMES_INT: int = 57359;
const MSI_CLAW_CENTER: StringName = &"";
const MSI_CLAW_CENTER_INT: int = 57360;
const MSI_CLAW_QUICK: StringName = &"";
const MSI_CLAW_QUICK_INT: int = 57361;
const KEYBOARD_0: StringName = &"０";
const KEYBOARD_0_INT: int = 65296;
const KEYBOARD_1: StringName = &"１";
const KEYBOARD_1_INT: int = 65297;
const KEYBOARD_2: StringName = &"２";
const KEYBOARD_2_INT: int = 65298;
const KEYBOARD_3: StringName = &"３";
const KEYBOARD_3_INT: int = 65299;
const KEYBOARD_4: StringName = &"４";
const KEYBOARD_4_INT: int = 65300;
const KEYBOARD_5: StringName = &"５";
const KEYBOARD_5_INT: int = 65301;
const KEYBOARD_6: StringName = &"６";
const KEYBOARD_6_INT: int = 65302;
const KEYBOARD_7: StringName = &"７";
const KEYBOARD_7_INT: int = 65303;
const KEYBOARD_8: StringName = &"８";
const KEYBOARD_8_INT: int = 65304;
const KEYBOARD_9: StringName = &"９";
const KEYBOARD_9_INT: int = 65305;
const KEYBOARD_A: StringName = &"Ａ";
const KEYBOARD_A_INT: int = 65313;
const KEYBOARD_B: StringName = &"Ｂ";
const KEYBOARD_B_INT: int = 65314;
const KEYBOARD_C: StringName = &"Ｃ";
const KEYBOARD_C_INT: int = 65315;
const KEYBOARD_D: StringName = &"Ｄ";
const KEYBOARD_D_INT: int = 65316;
const KEYBOARD_E: StringName = &"Ｅ";
const KEYBOARD_E_INT: int = 65317;
const KEYBOARD_F: StringName = &"Ｆ";
const KEYBOARD_F_INT: int = 65318;
const KEYBOARD_G: StringName = &"Ｇ";
const KEYBOARD_G_INT: int = 65319;
const KEYBOARD_H: StringName = &"Ｈ";
const KEYBOARD_H_INT: int = 65320;
const KEYBOARD_I: StringName = &"Ｉ";
const KEYBOARD_I_INT: int = 65321;
const KEYBOARD_J: StringName = &"Ｊ";
const KEYBOARD_J_INT: int = 65322;
const KEYBOARD_K: StringName = &"Ｋ";
const KEYBOARD_K_INT: int = 65323;
const KEYBOARD_L: StringName = &"Ｌ";
const KEYBOARD_L_INT: int = 65324;
const KEYBOARD_M: StringName = &"Ｍ";
const KEYBOARD_M_INT: int = 65325;
const KEYBOARD_N: StringName = &"Ｎ";
const KEYBOARD_N_INT: int = 65326;
const KEYBOARD_O: StringName = &"Ｏ";
const KEYBOARD_O_INT: int = 65327;
const KEYBOARD_P: StringName = &"Ｐ";
const KEYBOARD_P_INT: int = 65328;
const KEYBOARD_Q: StringName = &"Ｑ";
const KEYBOARD_Q_INT: int = 65329;
const KEYBOARD_R: StringName = &"Ｒ";
const KEYBOARD_R_INT: int = 65330;
const KEYBOARD_S: StringName = &"Ｓ";
const KEYBOARD_S_INT: int = 65331;
const KEYBOARD_T: StringName = &"Ｔ";
const KEYBOARD_T_INT: int = 65332;
const KEYBOARD_U: StringName = &"Ｕ";
const KEYBOARD_U_INT: int = 65333;
const KEYBOARD_V: StringName = &"Ｖ";
const KEYBOARD_V_INT: int = 65334;
const KEYBOARD_W: StringName = &"Ｗ";
const KEYBOARD_W_INT: int = 65335;
const KEYBOARD_X: StringName = &"Ｘ";
const KEYBOARD_X_INT: int = 65336;
const KEYBOARD_Y: StringName = &"Ｙ";
const KEYBOARD_Y_INT: int = 65337;
const KEYBOARD_Z: StringName = &"Ｚ";
const KEYBOARD_Z_INT: int = 65338;
const ICON_HEADPHONES: StringName = &"🎧";
const ICON_HEADPHONES_INT: int = 127911;
const ICON_MUSIC: StringName = &"🎶";
const ICON_MUSIC_INT: int = 127926;
const ICON_FISH: StringName = &"🐟";
const ICON_FISH_INT: int = 128031;
const DEVICE_DANCE_PAD: StringName = &"💃";
const DEVICE_DANCE_PAD_INT: int = 128131;
const ICON_LAPTOP: StringName = &"💻";
const ICON_LAPTOP_INT: int = 128187;
const ICON_DISKETTE: StringName = &"💾";
const ICON_DISKETTE_INT: int = 128190;
const ICON_WRITE: StringName = &"📝";
const ICON_WRITE_INT: int = 128221;
const DEVICE_PHONE: StringName = &"📱";
const DEVICE_PHONE_INT: int = 128241;
const ICON_CAMERA: StringName = &"📷";
const ICON_CAMERA_INT: int = 128247;
const ICON_SPEAKER: StringName = &"🔈";
const ICON_SPEAKER_INT: int = 128264;
const DEVICE_LIGHT_GUN: StringName = &"🔫";
const DEVICE_LIGHT_GUN_INT: int = 128299;
const ICON_NOISE: StringName = &"🕬";
const ICON_NOISE_INT: int = 128364;
const DEVICE_STEERING_WHEEL: StringName = &"🕸";
const DEVICE_STEERING_WHEEL_INT: int = 128376;
const DEVICE_JOY_STICK: StringName = &"🕹";
const DEVICE_JOY_STICK_INT: int = 128377;
const DEVICE_VR_HEADSET: StringName = &"🕻";
const DEVICE_VR_HEADSET_INT: int = 128379;
const DEVICE_VR_CONTROLLER: StringName = &"🕼";
const DEVICE_VR_CONTROLLER_INT: int = 128380;
const DEVICE_FLIGHT_STICK: StringName = &"🕽";
const DEVICE_FLIGHT_STICK_INT: int = 128381;
const ICON_PROCESSOR: StringName = &"🖥";
const ICON_PROCESSOR_INT: int = 128421;
const ICON_INTERNET: StringName = &"🖧";
const ICON_INTERNET_INT: int = 128423;
const ICON_GRAPHICS_CARD: StringName = &"🖨";
const ICON_GRAPHICS_CARD_INT: int = 128424;
const ICON_MEMORY: StringName = &"🖪";
const ICON_MEMORY_INT: int = 128426;
const ICON_USB_STICK: StringName = &"🖫";
const ICON_USB_STICK_INT: int = 128427;
const ICON_DATABASE: StringName = &"🖬";
const ICON_DATABASE_INT: int = 128428;
const ICON_HARD_DISK: StringName = &"🖴";
const ICON_HARD_DISK_INT: int = 128436;
const ICON_SCREEN: StringName = &"🖵";
const ICON_SCREEN_INT: int = 128437;
const ICON_TEXT_ENTRY: StringName = &"🖹";
const ICON_TEXT_ENTRY_INT: int = 128441;
const ICON_SPEAK: StringName = &"🗣";
const ICON_SPEAK_INT: int = 128483;
const ICON_LANGUAGE: StringName = &"🗩";
const ICON_LANGUAGE_INT: int = 128489;
const ICON_EXIT: StringName = &"🚪";
const ICON_EXIT_INT: int = 128682;
const ICON_INFORMATION: StringName = &"🛈";
const ICON_INFORMATION_INT: int = 128712;
const ICON_SHOPPING_CART: StringName = &"🛒";
const ICON_SHOPPING_CART_INT: int = 128722;
